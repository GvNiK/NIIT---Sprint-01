using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private LoadingScreenController loadingScreenController;	//Assignment - 02
	private LevelController levelController;	//Assignment - 02 (Craeted for the Time.timeScale value)

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void CreateGameController()
	{
		GameObject gameController = new GameObject("GameController");
		GameObject.DontDestroyOnLoad(gameController);
		gameController.AddComponent<GameController>();
	}

	private SaveDataController saveDataController;
	private SaveData saveData;

	private void Awake()
	{
		saveDataController = new SaveDataController();
		saveData = saveDataController.Load();

		loadingScreenController = new LoadingScreenController();	//Assignment - 02 (Created Instance of the class/script)

		SceneManager.sceneLoaded += (scene, loadType) => OnNewSceneLoaded(scene);
	}

	private void OnNewSceneLoaded(Scene scene)
	{
		if (scene.name == "Menu")
		{
			OnLevelSelectLoaded(scene);
			return;
		}

		foreach (GameObject rootObj in scene.GetRootGameObjects())
		{
			levelController = rootObj.GetComponent<LevelController>();	//Assignment - 02 (LevelController)

			if (levelController != null)
			{
				OnLevelLoaded(scene);
				return;
			}
		}
	}

	private void OnLevelSelectLoaded(Scene scene)
	{
		MenuUIController menus = FindObjectOfType<MenuUIController>();

		LevelSelectController levelSelect = menus.levelSelectUI;
		levelSelect.UpdateButtons(CreateLevelData());
	}

	private LevelSelectData CreateLevelData()
	{
		LevelSelectData levelSelectData = new LevelSelectData();
		bool previousLevelCompleted = true;

		foreach (Levels.Data level in Levels.ALL)
		{
			LevelSelectButtonData buttonData = new LevelSelectButtonData();

			buttonData.name = level.name;

			SaveData.Level levelData = saveData.levels.FirstOrDefault(x => x.ID == level.ID);	

			if (levelData != null)
			{
				buttonData.score = levelData.score;
			}
			else if (previousLevelCompleted)
			{
				previousLevelCompleted = false;
			}
			if(buttonData.locked == false)		//Assignment - 02 (Unlcoks Levels in 'Menu')
			{
				buttonData.OnClicked += () => StartCoroutine(OnSceneLoadRequested(level));
			}
			else
			{
				buttonData.locked = true;
			}

			levelSelectData.buttons.Add(buttonData);
		}

		return levelSelectData;
	}

    IEnumerator OnSceneLoadRequested(Levels.Data levelData)	//Assignment - 02 (Loads a level using its 'Path') (Whenver using Coroutine, need to return something - 'yeild return')
    {
		loadingScreenController.Show();	
        AsyncOperation asynOP = SceneManager.LoadSceneAsync(levelData.scenePath);
		//asynOP.allowSceneActivation = false;

		/* asynOP.completed += (op) =>		//op - bcoz asyncOP does not take 0 argument
		{
			loadingScreenController.Hide();
		}; */

		while(!asynOP.isDone)	
		{
			//if(loadingScreenController != null)
			//{	
				float progress = Mathf.Clamp01(asynOP.progress / 0.9f); //Clamps values between 0 to 1
				loadingScreenController.slider.value = progress;
				loadingScreenController.progressText.text = progress * 100 + "%";
				Debug.Log(progress);	
				//asynOP.allowSceneActivation = true;
			//}	
			yield return null;
		} 

		loadingScreenController.Hide();

		levelController?.OnLevelLoadResume();	//Time.timeScale = 1;	//Assignment - 02
		//Time.timeScale = 1;
    }

	private void LoadMainMenu()		//Assignment - 02
	{
		SceneManager.LoadScene("Menu");
	}

    private void OnLevelLoaded(Scene scene)
	{
		LevelController levelController = FindObjectOfType<LevelController>();
		if (levelController != null)
		{
			levelController.OnLevelLoadRequest += (level) =>	//Assignment - 02 (Passes a parameter to Load a Level)
			{
				StartCoroutine(OnSceneLoadRequested(level));
			};

			levelController.OnExitRequest += () =>		//Assignment - 02 
			{
				LoadMainMenu();
			};

			Debug.Log("Level loaded");				
		}
	}
}
