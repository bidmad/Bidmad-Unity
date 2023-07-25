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
    string UNITY_PLUGIN_VERSION = "3.5.0";
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadSetDebug(bool isDebug);

    [DllImport("__Internal")]
    private static extern void _bidmadSetGgTestDeviceid(string deviceId);

    [DllImport("__Internal")]
    private static extern void _bidmadSetGDPRSetting(bool consent);

    [DllImport("__Internal")]
    private static extern void _bidmadSetUseArea(bool useArea);

    [DllImport("__Internal")]
    private static extern int _bidmadGetGdprConsent();

    [DllImport("__Internal")]
    private static extern void _bidmadReqAdTrackingAuthorization();

    [DllImport("__Internal")]
    private static extern void _bidmadSetAdvertiserTrackingEnabled(bool enable);

    [DllImport("__Internal")]
    private static extern bool _bidmadGetAdvertiserTrackingEnabled();

    [DllImport("__Internal")]
    private static extern string _bidmadGetPRIVACYURL();

    [DllImport("__Internal")]
    private static extern void _bidmadInitializeSdk(string appKey);

    [DllImport("__Internal")]
    private static extern void _bidmadInitializeSdkWithCallback(string appKey);

    [DllImport("__Internal")]
    private static extern void _bidmadSetCuid(string cuid);

    [DllImport("__Internal")]
    private static extern void _bidmadSetUseServerSideCallback(bool isServerSideCallback);
    
    [DllImport("__Internal")]
    private static extern void _bidmadSetAdFreeEventListener();
    
    [DllImport("__Internal")]
    private static extern bool _bidmadIsAdFree();

#elif UNITY_ANDROID
    private static AndroidJavaClass javaCommonClass = null;
    private static AndroidJavaObject javaCommonClassInstance = null;
    private static AndroidJavaClass javaConsentClass = null;
    private static AndroidJavaObject javaConsentClassInstance = null; 
    private static AndroidJavaClass javaAdOptionClass = null;
    private static AndroidJavaObject javaAdOptonClassInstance = null;
    private static AndroidJavaClass javaAdFreeClass = null;
    private static AndroidJavaObject javaAdFreeClassInstance = null;
#endif

    public static void initializeSdk(string appkey) 
    {
#if UNITY_IOS
        _bidmadInitializeSdk(appkey);

#elif UNITY_ANDROID
    using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        javaCommonClass = new AndroidJavaClass("ad.helper.openbidding.BidmadCommon");

        AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        javaCommonClass.CallStatic("initializeSdk", context, appkey);
    }

#endif
    }

    public static void initializeSdkWithCallback(string appkey, Action<bool> callback) {
        BidmadManager.onInitialized = callback;

#if UNITY_IOS
        _bidmadInitializeSdkWithCallback(appkey);

#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaCommonClass = new AndroidJavaClass("ad.helper.openbidding.BidmadCommon");

            AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            javaCommonClass.CallStatic("initializeSdkWithUnityListener", context, appkey);
        }
#endif
    }

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
            javaCommonClass = new AndroidJavaClass("ad.helper.openbidding.BidmadCommon");
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
            javaCommonClass = new AndroidJavaClass("ad.helper.openbidding.BidmadCommon");
            javaCommonClass.CallStatic("setGgTestDeviceid", deviceId);
        }
#endif
	}

        public static void setCuid(string cuid)
    {
#if UNITY_IOS
        _bidmadSetCuid(cuid);

#elif UNITY_ANDROID
        using (javaAdOptionClass = new AndroidJavaClass("com.adop.sdk.AdOption"))
        {
            
            if(javaAdOptionClass != null) 
            {
                Debug.Log("setCuid AdOption!!");
                javaAdOptonClassInstance = javaAdOptionClass.CallStatic<AndroidJavaObject>("getInstance");
                javaAdOptonClassInstance.Call("setCuid", cuid);
            }
        }
#endif
    }

    public static void setUseServerSideCallback(bool isServerSideCallback)
    {
#if UNITY_IOS   
        _bidmadSetUseServerSideCallback(isServerSideCallback);

#elif UNITY_ANDROID
        using(javaAdOptionClass = new AndroidJavaClass("com.adop.sdk.AdOption"))
        {
            
            if(javaAdOptionClass != null) 
            {
                Debug.Log("setUseServerSideCallback AdOption!!");
                javaAdOptonClassInstance = javaAdOptionClass.CallStatic<AndroidJavaObject>("getInstance");
                javaAdOptonClassInstance.Call("setUseServerSideCallback", isServerSideCallback);
            }
        }
#endif
    } 


	public static void setGdprConsent(bool consent, bool useArea)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadSetGDPRSetting(consent);
            _bidmadSetUseArea(useArea);
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
            result = _bidmadGetGdprConsent();
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
                result = javaConsentClassInstance.Call<int>("getGdprConsentForOtherPlatform");
            }

            return result;
        }
#endif
        return result;
	}

	public static string getPRIVACYURL()
	{
		string result = "";
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return _bidmadGetPRIVACYURL();
        }
        else
        {
            return _bidmadGetPRIVACYURL();
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
        return "";
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
        return false;
#endif
        return false;
    }

    public static void setAdFreeEventListener(Action<bool> callback)
    {
        BidmadManager.onAdFree = callback;
        
#if UNITY_IOS
        _bidmadSetAdFreeEventListener();
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaAdFreeClass = new AndroidJavaClass("ad.helper.openbidding.AdFreeInformation");

            AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            javaAdFreeClassInstance = javaAdFreeClass.CallStatic<AndroidJavaObject>("getInstance", context);
            javaAdFreeClassInstance.Call("setOnAdFreeWithUnityListener");
        }
#endif
    }
    
    public static bool isAdFree() {
#if UNITY_IOS
        return _bidmadIsAdFree();
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            javaAdFreeClass = new AndroidJavaClass("ad.helper.openbidding.AdFreeInformation");

            AndroidJavaObject context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            javaAdFreeClassInstance = javaAdFreeClass.CallStatic<AndroidJavaObject>("getInstance", context);
            int status = javaAdFreeClassInstance.Call<int>("getAdFreeStatus");
            
            return (status == 0)? true : false;
        }
        return false;
#endif
    }

}//END
