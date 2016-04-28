using UnityEngine;
using System.Runtime.InteropServices;

public class ShareApp
{

    public enum ShareAppProvider
    {
        FACEBOOK,
        TWITTER
    }

    static string FACEBOOK_PACKAGE_NAME = "com.facebook.katana";
    static string TWITTER_PACKAGE_NAME = "com.twitter.android";

    public static bool IsFacebookAppInstalled()
    {
        return IsAppInstalled(FACEBOOK_PACKAGE_NAME);
    }

    public static bool IsTwitterAppInstalled()
    {
        return IsAppInstalled(TWITTER_PACKAGE_NAME);
    }

    private static bool IsAppInstalled(string packagename)
    {
		#if UNITY_ANDROID
        bool IsAppInstalled = true;

        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

        try
        {
            AndroidJavaObject AppInfo = packageManager.Call<AndroidJavaObject>("getApplicationInfo", packagename, 0);
        }
        catch
        {
            IsAppInstalled = false;
        }

        return IsAppInstalled;
		#else
		return true;
		#endif
    }

    public static void Share(ShareAppProvider provider, string title, string message, string imagepath, string url)
    {
#if UNITY_EDITOR
        Debug.Log("It doesn't work on editor!");
#endif
#if UNITY_ANDROID
        string packagename = (provider == ShareAppProvider.FACEBOOK) ? FACEBOOK_PACKAGE_NAME : TWITTER_PACKAGE_NAME;

        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagepath);

        try
        {
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setPackage", packagename);

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), title);
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");

            ca.Call("startActivity", intentObject);
        }
        catch { }

#elif UNITY_IOS
		if (provider == ShareAppProvider.FACEBOOK)
		{
			ShareIOSNativeFacebook(title, message, imagepath, url);
		}
		else if (provider == ShareAppProvider.TWITTER)
		{
			ShareIOSNativeTwitter(title, message, imagepath, url);
		}
#endif
    }

#if UNITY_IOS

	[DllImport ("__Internal")] private static extern void _showAlertMessage(string title, string msg);
	[DllImport ("__Internal")] private static extern void _showShareFacebook(string subject, string msg, string imagePath, string url);
	[DllImport ("__Internal")] private static extern void _showShareTwitter(string subject, string msg, string imagePath, string url);

	public static void ShowAlertMessage(string title, string message)
	{
		_showAlertMessage (title, message);
	}

	public static void ShareIOSNativeFacebook(string subject, string msg, string img, string url)
	{
		_showShareFacebook (subject, msg, img, url);
	}

	public static void ShareIOSNativeTwitter(string subject, string msg, string img, string url)
	{
		_showShareTwitter (subject, msg, img, url);
	}
#endif
}
