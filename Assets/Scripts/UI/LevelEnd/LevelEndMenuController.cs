using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndMenuController
{
	private LevelEndMenu data;
	public LevelStatsController levelStatsController;
	public Action<Levels.Data> OnLevelLoadRequested = delegate { };		//Assignment - 02 
	public Action OnRetryRequested = delegate { };		//Assignment - 02
	public Action OnExitRequested = delegate { };		//Assignment - 02

	public LevelEndMenuController(LevelEndMenu data, int currentLevelID, LevelStatsController levelStatsController)		//Constructor
	{
		this.data = data;
		this.levelStatsController = levelStatsController;

		Hide();

		GetNextLevel(currentLevelID);

		data.retryButton.onClick.AddListener( () =>		//Assignment - 02 (for 'RestartButton' - Restarts)
		{
			OnRetryRequested();	//Invokes whenever & wherever it is called ( OnRetryRequested.Invoke(); )
		});

		data.exitToLevelSelectButton.onClick.AddListener(() =>	//Assignment - 02 (for 'MainMenuButton' - MainMenu)
		{
			OnExitRequested();
		});

	}

	public void Show(string message, bool levelComplete)
	{
		data.gameObject.SetActive(true);

		data.message.text = message;
		data.score.text = levelStatsController.LevelTime.Value.ToString("0.00");
		data.enemiesKilled.text = "-";
		data.pickupsCollected.text = "-";

		if (!levelComplete)
		{
			data.scoreContainer.gameObject.SetActive(false);
			data.nextLevelButton.gameObject.SetActive(false);
		}
	}

	private void GetNextLevel(int currentLevelID)	//Assignment - 02 (NextLevel - Code Logic)
	{
		for(int i = 0; i < Levels.ALL.Count; i++)
		{
			if(Levels.ALL[i].ID == currentLevelID)
			{
				if((i + 1) < Levels.ALL.Count)
				{
					FoundNextLevel(Levels.ALL[i + 1]);
					return;
				}
			}
		}

		HideNextLevelButton();
	}

    public void FoundNextLevel(Levels.Data levelData)	//Assignment - 02
    {
        data.nextLevelButton.onClick.AddListener(() =>
		{
			OnLevelLoadRequested(levelData);
		});
    }

    private void HideNextLevelButton()	//Assignment - 02
    {
        data.nextLevelButton.gameObject.SetActive(false);
    }

    public void Hide()
	{
		data.gameObject.SetActive(false);
	}
}
