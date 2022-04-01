using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SocialPlatformLocator
{
	public static SocialPlatform Get()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return new GooglePlaySocialPlatform();

#else
		return new DummySocialPlatform();
#endif
	}
}
