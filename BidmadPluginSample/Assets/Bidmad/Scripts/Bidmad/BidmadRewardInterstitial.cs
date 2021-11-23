using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class BidmadRewardInterstitial
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadNewInstanceRewardInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadLoadRewardInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadShowRewardInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern bool _bidmadIsLoadedRewardInterstitial(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadSetAutoReloadRewardInterstitial(string zoneId, bool isAutoReload);

#elif UNITY_ANDROID
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;
#endif
    private string mZoneId = "";

    public BidmadRewardInterstitial(string zoneId)
    {
        mZoneId = zoneId;
#if UNITY_IOS
        _bidmadNewInstanceRewardInterstitial(zoneId);
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
            javaClassInstance.Call("makeRewardInterstitial");

            javaClassInstance.Call("setAdInfo", mZoneId);
        }
#endif
    }

    public void getInstance()
	{
#if UNITY_IOS
#elif UNITY_ANDROID
        using (javaClass = new AndroidJavaClass("ad.helper.openbidding.rewardinterstitial.UnityRewardInterstitial"))
        {
            if(javaClass != null)
            {
                javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance", mZoneId);
            }

        }
#endif
	}

    public void setUserId(string userId)
    {
        //Only Android Support
#if UNITY_IOS
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("setUserId", userId);
        }
#endif
    }

    public void setAutoReload(bool isAutoReload) {
#if UNITY_IOS
        _bidmadSetAutoReloadRewardInterstitial(mZoneId, isAutoReload);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("setAutoReload", isAutoReload);
        }
#endif
    }
    public void load()
	{
#if UNITY_IOS
        _bidmadLoadRewardInterstitial(mZoneId);
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
        _bidmadShowRewardInterstitial(mZoneId);
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
        result = _bidmadIsLoadedRewardInterstitial(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            result = javaClassInstance.Call<bool>("isLoaded");
        }
#endif
        return result;
    }

    public void setRewardInterstitialLoadCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardInterstitialLoad.ContainsKey(mZoneId)){
            BidmadManager.dicRewardInterstitialLoad.Remove(mZoneId);
        }
        BidmadManager.dicRewardInterstitialLoad.Add(mZoneId, callback);
#endif
    }

    public void setRewardInterstitialShowCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardInterstitialShow.ContainsKey(mZoneId)){
            BidmadManager.dicRewardInterstitialShow.Remove(mZoneId);
        }
        BidmadManager.dicRewardInterstitialShow.Add(mZoneId, callback);
#endif
    }

    public void setRewardInterstitialFailCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardInterstitialFail.ContainsKey(mZoneId)){
            BidmadManager.dicRewardInterstitialFail.Remove(mZoneId);
        }
        BidmadManager.dicRewardInterstitialFail.Add(mZoneId, callback);
#endif
    }

    public void setRewardInterstitialCompleteCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardInterstitialComplete.ContainsKey(mZoneId)){
            BidmadManager.dicRewardInterstitialComplete.Remove(mZoneId);
        }
        BidmadManager.dicRewardInterstitialComplete.Add(mZoneId, callback);
#endif
    }

    public void setRewardInterstitialSkipCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardInterstitialSkip.ContainsKey(mZoneId)){
            BidmadManager.dicRewardInterstitialSkip.Remove(mZoneId);
        }
        BidmadManager.dicRewardInterstitialSkip.Add(mZoneId, callback);
#endif
    }

    public void setRewardInterstitialCloseCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardInterstitialClose.ContainsKey(mZoneId)){
            BidmadManager.dicRewardInterstitialClose.Remove(mZoneId);
        }
        BidmadManager.dicRewardInterstitialClose.Add(mZoneId, callback);
#endif
    }

}
