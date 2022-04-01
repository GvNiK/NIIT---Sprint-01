using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guards : MonoBehaviour     //Assignment - 03
{
    public GeneralData generalData;
    public WayPoints waypointsList;
    public VisionData visionData;
    public SuspicionData suspicionData;
    public Transform target;

    public Action<float, Transform> OnDamageTaken = delegate { };

    public void TakeDamage(float damageTaken, Transform instigator)
    {
        OnDamageTaken.Invoke(damageTaken, instigator);
    }

    [SerializeField]
    private Transform player;
    [SerializeField]
    private float damageAmount;

    [ContextMenu("Apply Damage")]
    public void DebugTakeDamage()
    {
        TakeDamage(damageAmount, player);
    }

}

[System.Serializable]   //We needed to add this bcoz it is a Sub-Class under an Existing Class
public class GeneralData
{
    public float maxHealth = 100;
    public float attackDamage = 25;
    public float patrolMoveSpeed = 0.5f;
    public float pursuitMoveSpeed = 1.0f;
    public float maxSpeed = 10.0f;
    public float attackRotateSpeed = 2;
}

[System.Serializable]
public class WayPoints
{
    public List<WayPointInfo> wayPoints;
}

[System.Serializable]
public class WayPointInfo
{
    public WayPointType wayPointType;
    public Transform goal;
    public float waitTime;
    public Vector3 targetRotation;
}

[System.Serializable]
public class VisionData
{
    public bool visualize;  //Default value = false
    public float radius = 8.0f;

    [Range(0, 180)]
    public float angle = 30.0f;
    public float eyeHeight;
    public float attackRange = 5;
    public float searchRange = 8;

    [Tooltip("Sets the amount of Raycast Lines.")]
    [Range(0, 2)]
    public int raycastLines;
    public int awarnessZone;  
    public LayerMask raycastMask;
    
}

[System.Serializable]
public class SuspicionData
{
    public float currentSuspicion;

    [Range(0.0f, 1.0f)]
    public float patrollingThresold = 0.1f; //Blue Bar in the Powerpoint document

    [Range(0.0f, 1.0f)]
    public float pursuingThresold = 0.6f;   //Red Bar in the Powerpoint document
    public float lookingThresold = 1.0f;

    public float suspicionBuildRate = 0.6f;
    public float suspicionDecayRate = 0.2f;

}

public enum WayPointType
{
    MoveTo,
    Wait,
    Rotate,
    LookAround
}