#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GooglePlaySocialPlatform : SocialPlatform
{
	public override void Login(Action OnSuccess)
	{
		try
		{
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			Social.localUser.Authenticate((success) =>
			{
				if (success)
				{
					OnSuccess();
				}
			});
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
		}
	}

	public override void ShowAchievements()
	{
		if (IsAuthenticated)
		{
			Social.ShowAchievementsUI();
		}
	}

	public override void UnlockAchievement(string name, string ID)
	{
		if (IsAuthenticated)
		{
			Social.ReportProgress(ID, 100, (success) => { });
		}
	}

	public override bool IsAuthenticated
	{
		get { return Social.localUser.authenticated; }
	}
}
#endif