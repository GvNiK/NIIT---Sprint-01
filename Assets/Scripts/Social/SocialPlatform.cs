using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class SocialPlatform
{
	public abstract void Login(Action OnSuccess);
	public abstract void ShowAchievements();
	public abstract void UnlockAchievement(string name, string ID);
	public abstract bool IsAuthenticated { get; }
}
