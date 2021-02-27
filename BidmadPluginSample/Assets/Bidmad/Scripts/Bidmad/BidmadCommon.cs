using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public enum BidmadTrackingAuthorizationStatus
{
    BidmadAuthorizationStatusNotDetermined = 0,
    BidmadAuthorizationStatusRestricted,
    BidmadAuthorizationStatusDenied,
    BidmadAuthorizationStatusAuthorized,
    BidmadAuthorizationStatusLessThaniOS14
}

public class BidmadCommon
{
    string UNITY_PLUGIN_VERSION = "2.6.0";
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadSetDebug(bool isDebug);

    [DllImport("__Internal")]
    private static extern void _bidmadSetGgTestDeviceid(string deviceId);

    [DllImport("__Internal")]
    private static extern void _bidmadSetGdprConsent(bool consent, bool useArea);

    [DllImport("__Internal")]
    private static extern int _bidmadGetGdprConsent(bool useArea);

    [DllImport("__Internal")]
    private static extern void _bidmadReqAdTrackingAuthorization();

    [DllImport("__Internal")]
    private static extern void _bidmadSetAdvertiserTrackingEnabled(bool enable);

    [DllImport("__Internal")]
    private static extern bool _bidmadGetAdvertiserTrackingEnabled();


#elif UNITY_ANDROID
    private static AndroidJavaClass javaCommonClass = null;

    private static AndroidJavaClass javaConsentClass = null;
    private static AndroidJavaObject javaConsentClassInstance = null; 
#endif

	public static void setIsDebug(bool isDebug)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadSetDebug(isDebug);
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaCommonClass = new AndroidJavaClass("com.adop.sdk.Common");
            javaCommonClass.CallStatic("setDebugging", isDebug);
        }
#endif
	}

	public static void setGgTestDeviceid(string deviceId)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
           _bidmadSetGgTestDeviceid(deviceId);
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaCommonClass = new AndroidJavaClass("com.adop.sdk.Common");
            javaCommonClass.CallStatic("setGgTestDeviceid", deviceId);
        }
#endif
	}

	public static void setGdprConsent(bool consent, bool useArea)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadSetGdprConsent(consent, useArea);
        }

#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {

            javaConsentClass = new AndroidJavaClass("com.adop.sdk.userinfo.consent.Consent");
            if (javaConsentClass != null)
            {
                AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                javaConsentClassInstance = javaConsentClass.CallStatic<AndroidJavaObject>("unityInstatnce", context, useArea);
                javaConsentClassInstance.Call("setGdprConsent", consent);
            }
        }
#endif
	}

	public static int getGdprConsent(bool useArea)
	{
		int result = 999;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            /*
             * Return GDPR Status(int)
             * YES(1)
             * NO(0)
             * UNKWON(-1)
             * UNUSE(-2) 
             */
            result = _bidmadGetGdprConsent(useArea);
            return result;
        }
        else
        {
            return result;
        }

#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            /*
             * Return GDPR Status(int)
             * YES(1)
             * NO(0)
             * UNKWON(-1)
             * UNUSE(-2) 
             */
            javaConsentClass = new AndroidJavaClass("com.adop.sdk.userinfo.consent.Consent");
            if (javaConsentClass != null)
            {
                AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                javaConsentClassInstance = javaConsentClass.CallStatic<AndroidJavaObject>("unityInstatnce", context, useArea);
                result = javaConsentClassInstance.Call<int>("getGdprConsentForUnity");
            }

            return result;
        }
#endif
	}

	public static string getPRIVACYURL()
	{
		string result = "";
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return result;
        }
        else
        {
            return result;
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaConsentClass = new AndroidJavaClass("com.adop.sdk.userinfo.consent.Consent");
           
            if (javaConsentClass != null)
            {
                AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                javaConsentClassInstance = javaConsentClass.CallStatic<AndroidJavaObject>("unityInstatnce", context);
                result = javaConsentClassInstance.Call<string>("getPRIVACYURL");
            }

            return result;
        }
#endif
	}


    public static void reqAdTrackingAuthorization(Action<BidmadTrackingAuthorizationStatus> callback)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadReqAdTrackingAuthorization();

            BidmadManager.adTrackingAuthResponse = callback;
        }

#elif UNITY_ANDROID
#endif
    }

    public static void setAdvertiserTrackingEnabled(bool enable)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadSetAdvertiserTrackingEnabled(enable);
        }
#elif UNITY_ANDROID
#endif
    }


    public static bool getAdvertiserTrackingEnabled()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return _bidmadGetAdvertiserTrackingEnabled();
        }
        else
        {
            return false;
        }
#elif UNITY_ANDROID
#endif
    }

}//END
