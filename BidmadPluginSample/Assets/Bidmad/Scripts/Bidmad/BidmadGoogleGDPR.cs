using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class BidmadGoogleGDPR
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadSetDebug(bool isDebug);

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleNewInstance();

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleSetListener();

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleSetDebug(string testDeviceId, bool isTestEurope);

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleRequestConsentInfoUpdate();

    [DllImport("__Internal")]
    private static extern bool _bidmadGDPRforGoogleIsConsentFormAvailable();

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleLoadForm();

    [DllImport("__Internal")]
    private static extern int _bidmadGDPRforGoogleGetConsentStatus();

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleReset();

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleSetDelegate();

    [DllImport("__Internal")]
    private static extern void _bidmadGDPRforGoogleShowForm();

#elif UNITY_ANDROID
    private AndroidJavaObject activityContext = null;

    private AndroidJavaClass javaGoogleGDPRClass = null;
    private AndroidJavaObject javaGoogleGDPRClassInstance = null; 
#endif

    public BidmadGoogleGDPR()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadGDPRforGoogleNewInstance();
            _bidmadGDPRforGoogleSetListener();
            _bidmadGDPRforGoogleSetDelegate();
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
            javaGoogleGDPRClassInstance = new AndroidJavaObject("com.adop.sdk.userinfo.consent.GoogleGDPRConsent", activityContext);
            javaGoogleGDPRClassInstance.Call("setListenerForUnity");
        }
#endif
    }

	public void setDebug(string testDeviceId, bool isEEA)
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadGDPRforGoogleSetDebug(testDeviceId, isEEA);
        }
#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            javaGoogleGDPRClassInstance.Call("setDebug", testDeviceId, isEEA);
        }
#endif
	}

	public bool isConsentFormAvailable()
	{
        bool result = false;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = _bidmadGDPRforGoogleIsConsentFormAvailable();
        }
#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            result = javaGoogleGDPRClassInstance.Call<bool>("isConsentFormAvailable");
        }
#endif

        return result;
	}

    /*
        int UNKNOWN = 0;
        int REQUIRED = 1;
        int NOT_REQUIRED = 2;
        int OBTAINED = 3;
    */
	public int getConsentStatus()
	{
        int result = 0;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result = _bidmadGDPRforGoogleGetConsentStatus();
        }

#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            result = javaGoogleGDPRClassInstance.Call<int>("getConsentStatus");
        }
#endif
        return result;
	}

    public void reset()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadGDPRforGoogleReset();
        }

#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            javaGoogleGDPRClassInstance.Call("reset");
        }
#endif
    }

    public void requestConsentInfoUpdate()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadGDPRforGoogleRequestConsentInfoUpdate();
        }

#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            javaGoogleGDPRClassInstance.Call("requestConsentInfoUpdate");
        }
#endif
    }

    public void loadForm()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadGDPRforGoogleLoadForm();
        }

#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            javaGoogleGDPRClassInstance.Call("loadForm");
        }
#endif
    }

    public void showForm()
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadGDPRforGoogleShowForm();
        }

#elif UNITY_ANDROID
        if(javaGoogleGDPRClassInstance != null){
            javaGoogleGDPRClassInstance.Call("showForm");
        }
#endif
    }

    public void setConsentInfoUpdateSuccessCallback(Action callback)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            BidmadManager.consentInfoUpdateSuccess = callback;
        }

#elif UNITY_ANDROID
        BidmadManager.consentInfoUpdateSuccess = callback;
#endif
    }

    public void setConsentInfoUpdateFailureCallback(Action<string> callback)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            BidmadManager.consentInfoUpdateFailure = callback;
        }

#elif UNITY_ANDROID
        BidmadManager.consentInfoUpdateFailure = callback;
#endif
    }

    public void setConsentFormLoadSuccessCallback(Action callback)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            BidmadManager.consentFormLoadSuccess = callback;
        }

#elif UNITY_ANDROID
        BidmadManager.consentFormLoadSuccess = callback;
#endif
    }

    public void setConsentFormLoadFailureCallback(Action<string> callback)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            BidmadManager.consentFormLoadFailure = callback;
        }

#elif UNITY_ANDROID
        BidmadManager.consentFormLoadFailure = callback;
#endif
    }

    public void setConsentFormDismissedCallback(Action<string> callback)
    {
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            BidmadManager.consentFormDismissed = callback;
        }

#elif UNITY_ANDROID
        BidmadManager.consentFormDismissed = callback;
#endif
    }

}//END
