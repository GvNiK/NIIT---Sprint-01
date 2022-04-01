using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DummySocialPlatform : SocialPlatform
{
	public override bool IsAuthenticated { get { return true; } }

	public override void Login(Action OnSuccess)
	{
		OnSuccess();
	}

	public override void ShowAchievements() { }

	public override void UnlockAchievement(string name, string ID) { }
}
