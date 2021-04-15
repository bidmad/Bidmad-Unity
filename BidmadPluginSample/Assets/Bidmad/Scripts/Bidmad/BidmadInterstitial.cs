using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class BidmadInterstitial
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadNewInstanceInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadLoadInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadShowInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern bool _bidmadIsLoadedInterstitial(string zoneId);
#elif UNITY_ANDROID
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;
#endif
    private string mZoneId = "";

    public BidmadInterstitial(string zoneId)
    {
        mZoneId = zoneId;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadNewInstanceInterstitial(zoneId);
        }
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        getInstance();
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("setActivity", activityContext);
            javaClassInstance.Call("setContext", activityContext);
            javaClassInstance.Call("makeInterstitial");

            javaClassInstance.Call("setAdInfo", mZoneId);
        }
#endif
    }

    public void getInstance()
	{
#if UNITY_IOS

#elif UNITY_ANDROID
        using (javaClass = new AndroidJavaClass("com.adop.sdk.interstitial.UnityInterstitial"))
        {
            if(javaClass != null) 
            {
                javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance", mZoneId);
            }
        }
#endif
	}

	public void load()
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadLoadInterstitial(mZoneId);
        }
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("load");
        }
#endif
	}

	public void show()
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadShowInterstitial(mZoneId);
        }
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("show");
        }
#endif
	}

    public bool isLoaded()
    {
        bool result = false;
#if UNITY_IOS
        result = _bidmadIsLoadedInterstitial(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            result = javaClassInstance.Call<bool>("isLoaded");
            Debug.Log("isLoaded : " + result);
        }
#endif
        return result;
    }

        public void setInterstitialLoadCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicInterstitialLoad.ContainsKey(mZoneId))
        {
            BidmadManager.dicInterstitialLoad.Remove(mZoneId);
        }
        BidmadManager.dicInterstitialLoad.Add(mZoneId, callback);
#endif
    }

    public void setInterstitialShowCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicInterstitialShow.ContainsKey(mZoneId))
        {
            BidmadManager.dicInterstitialShow.Remove(mZoneId);
        }
        BidmadManager.dicInterstitialShow.Add(mZoneId, callback);
#endif
    }

    public void setInterstitialFailCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicInterstitialFail.ContainsKey(mZoneId))
        {
            BidmadManager.dicInterstitialFail.Remove(mZoneId);
        }
        BidmadManager.dicInterstitialFail.Add(mZoneId, callback);
#endif
    }

    public void setInterstitialCloseCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicInterstitialClose.ContainsKey(mZoneId))
        {
            BidmadManager.dicInterstitialClose.Remove(mZoneId);
        }
        BidmadManager.dicInterstitialClose.Add(mZoneId, callback);
#endif
    }
}//END
