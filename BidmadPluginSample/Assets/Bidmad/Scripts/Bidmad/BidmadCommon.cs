using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class BidmadCommon
{
    string UNITY_PLUGIN_VERSION = "2.5.1";
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _setIsDebug(bool isDebug);

    [DllImport("__Internal")]
    private static extern void _setTestMode(bool b);

    [DllImport("__Internal")]
    private static extern void _setGgTestDeviceid(string deviceId);

    [DllImport("__Internal")]
    private static extern void _setGdprConsent(bool consent, bool useArea);

    [DllImport("__Internal")]
    private static extern int  _getGdprConsent(bool useArea);


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
            _setIsDebug(isDebug);
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaCommonClass = new AndroidJavaClass("com.adop.sdk.Common");
            javaCommonClass.CallStatic("setDebugging", isDebug);
        }
#endif
	}

	public static void setRewardTestMode(bool b)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _setTestMode(b);

        }
#elif UNITY_ANDROID
#endif
	}

	public static void setGgTestDeviceid(string deviceId)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
           _setGgTestDeviceid(deviceId);
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
            _setGdprConsent(consent, useArea);
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
            result = _getGdprConsent(useArea);
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
}//END
