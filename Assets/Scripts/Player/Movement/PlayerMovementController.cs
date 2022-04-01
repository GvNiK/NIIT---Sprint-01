using System;
using UnityEngine;
public class PlayerMovementController
{
	private Transform transform;
	//private PlayerInputCallbacks inputCallbacks;
	//private float sensitivityX = 0.05f;
	//private float sensitivityY = 0.05f;
	//private float movementSpeed = 5f;

	//S2 - Assignment 01
	private Animator animator;	
	private Target target;
	private float turnAmount;
	private float forwardAmount;
	private float runToIdelSpeed = 0.5f;
	private float idleToRunSpeed = 3f; 	

    public PlayerMovementController(Transform transform, Animator animator)	//S2 - Assignment 01
	{
		this.transform = transform;
		this.animator = animator;
		//this.inputCallbacks = inputCallbacks;

		/*inputCallbacks.OnPlayerLookFired += (lookDelta) =>
		{
			UpdateLookDirection(lookDelta);
		};*/
	}

	public void Update() 
	{
		if(target != null)
		{
			target.Update();	//Calls the "Abstract Update()". So all "Override Updates()" get called.
		}	
		else
		{
			turnAmount = 0f;
			forwardAmount = 0f;
			animator.SetFloat("Forward", 0f);
			animator.SetFloat("Turn", 0f);
		}
	}

	private bool IsFaceInProgress()	//Returns "True" if target is FaceTarget. Else returns False.
	{
		
		return target is FaceTarget;	
	}
	
	public void MoveTo(Vector3 movementVector)
	{
		//transform.position += movementVector * Time.deltaTime * movementSpeed;	//IMP: SPEED & SLIDE GLITCH

		//We check if the Face is in Progress, then simply return becoz we do not want the Player to Move when the Face Rotation is in process.
		if(IsFaceInProgress())
		{
			return;
		}

		//Move the Player
		SwitchTarget(new MoveTarget(transform, movementVector), null);
	}


	/*private void UpdateLookDirection(Vector2 lookDelta)
	{
		transform.Rotate(Vector3.up, lookDelta.x * sensitivityX, Space.World);
		transform.Rotate(Vector3.left, lookDelta.y * sensitivityY, Space.Self);
	}*/

	public void Face(Vector3 faceVector, Action OnComplete)	//Creates FaceTarget in SwitchTarget function. Face Rotation.
	{
		//transform.forward = faceVector;
		//OnComplete();
		
		//Bcoz we don't want to calculate the Y component, while Calculationg the Distances.
		faceVector.y = 0f; 	//S2 - Assignment 02

		//We Set newTarget here - Face Setup
		SwitchTarget(new FaceTarget(transform, faceVector), OnComplete);	//FaceTarget faceTarget = new FaceTarget(transform, faceVector);
	}

	/////////////---------	S2 - Assignment 01	------------/////////////

	private void SwitchTarget(Target newTarget, Action OnActionComplete)
	{
		target = newTarget;

		target.OnAnimatorPropertiesUpdated += (turn, forward, turnSpeed) =>
		{
			turnAmount = turn;	//How much to turn (Angle of Rotation)
			forwardAmount = forward;	//How many meter or unit to move forward.
			ApplyRotation(turnSpeed);	//Speed of ROtation.
			UpdateAnimator();
		};

		target.OnComplete += () =>
		{
			target = null;
			OnActionComplete?.Invoke();
		};

		target.Start();
	}
    private void ApplyRotation(float extraRotation)	//Actual Rotation of Player along Y-axis.
    {
        transform.Rotate(0, extraRotation, 0);
    }

    private void UpdateAnimator()	//Updates Animation States & its Variables.
    {
        float animForward = animator.GetFloat("Forward");
		//forwardAmount = Player's Movement Input.
		float transitionSpeed = animForward > forwardAmount ? runToIdelSpeed : idleToRunSpeed;

		animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime * transitionSpeed);
		animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);

		if(Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("Shoot");
			//transform.position += Vector3.zero * Time.deltaTime * 0;
		}

		if(Input.GetKeyDown(KeyCode.Return))
		{
			animator.SetTrigger("Swing");
			//transform.position += Vector3.zero * Time.deltaTime * 0;
		}		
    }


    private abstract class Target	//S2 - Assignment 01
	{
		public Action OnComplete = delegate { };
		public Action<float, float, float> OnAnimatorPropertiesUpdated = delegate { };

		public abstract void Start();
		public abstract void Update(); 
	}

    private class FaceTarget : Target	//S2 - Assignment 01
    {
		private Transform transform;
		private Vector3 targetDirection;

		public FaceTarget(Transform transform, Vector3 targetDirection)
		{
			this.transform = transform;
			this.targetDirection = targetDirection;
		}

        public override void Start()
        {
            Update();
        }

        public override void Update()
        {
			//Unit Vector = Normalized Magnitude of a Vector.
			//Normalized Value is always 1.
			//Magnitude can be any Integer value.
			//When you Transform a Vector with a Unit Vector (Normalized Vector), then the Unit Vector remains same, 
			//and the other Non-Unit Vector affects in terms or Rotation, Scale or Translation.
			//But if you Transform a Vector with another Vector, both the Vectors change their values.
           Vector3 faceDirection = targetDirection;
		   if(faceDirection.magnitude > 1f)
		   {
			   faceDirection.Normalize();	//Unit Vector
		   }

			//Transforms a direction from world space to local space. 
		   	faceDirection = transform.InverseTransformDirection(faceDirection);

		   	//Projects the Player onto the Plane. (Since in our case, we have Plan Surfaces for each levels, so this works)
		   	//We might would have want to use different logic or function when we are having a Non-Planar Terrain.
		   	faceDirection = Vector3.ProjectOnPlane(faceDirection, Vector3.zero);
			
			//Returns the angle in radians whose Tan is y/x.
			//Z = Player's Forward Direction where Player is Facing.
			//X = The Left(-ve) and Right(+ve) Direction of the Player, where he moves or turns Left or Right.
			//Y = Player's Vector Upwards Direction. This is used to check Player on Ground or to Physcially Rotate the Player in the Scene.
			//In this Case, we use X with Z becoz we want to rotate Left or Right with respective to the Player 
			//always Facing Forward Direction while rotating.
			float turnAmount = Mathf.Atan2(faceDirection.x, faceDirection.z);

			//Returns the signed angle in degrees between from and to.
			float angleToDirection = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

			float maxTurn = Math.Sign(angleToDirection) * 360f * Time.deltaTime;
			float turnSpeed = -Mathf.Min(maxTurn, angleToDirection);

			//This will set below Values in the "SwitchTarget" function. 
			OnAnimatorPropertiesUpdated(turnAmount, 0f, turnSpeed); 

			if(Vector3.Angle(targetDirection, transform.forward) < 1f)
			{
				OnComplete();
			}
        }
    }

	private class MoveTarget : Target
	{
		private Transform transform;
		private Vector3 movementVector;
		private float stationaryTurnSpeed = 720f;
		//private float movingTurnSpeed = 360f;

		public MoveTarget(Transform transform, Vector3 movementVector)
		{
			this.transform = transform;
			this.movementVector = movementVector;
		}

		public override void Start()
		{
			if(movementVector.magnitude > 1f)
			{
				movementVector.Normalize();
			}

			movementVector = transform.InverseTransformDirection(movementVector);
			movementVector = Vector3.ProjectOnPlane(movementVector, Vector3.zero);
			float turnAmount = Mathf.Atan2(movementVector.x, movementVector.z);

			float forwardAmount = 0f;
			float movementTarget = movementVector.z;

			if(movementTarget != 0f)
			{
				forwardAmount = movementVector.z;
			}
			else
			{
				forwardAmount = Mathf.Lerp(forwardAmount, movementTarget, Time.deltaTime);
			}

			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movementTarget, forwardAmount);	//Glitch - Player Sliding: turnAmount
			float extraRotation = turnAmount * turnSpeed * Time.deltaTime;

			OnAnimatorPropertiesUpdated(turnAmount, forwardAmount, extraRotation);
		}

		public override void Update()
		{
			OnComplete();
		}
	}
}
