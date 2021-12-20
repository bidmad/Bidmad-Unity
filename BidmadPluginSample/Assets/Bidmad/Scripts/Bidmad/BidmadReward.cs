using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class BidmadReward
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadNewInstanceReward(string zoneId);
    
    [DllImport("__Internal")]
    private static extern void _bidmadLoadRewardVideo(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadShowRewardVideo(string zoneId);

    [DllImport("__Internal")]
    private static extern bool _bidmadIsLoadedReward(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadSetCUIDRewardVideo(string zoneId, string cuid);

    [DllImport("__Internal")]
    private static extern void _bidmadSetAutoReloadRewardVideo(string zoneId, bool isAutoReload);

#elif UNITY_ANDROID
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;
#endif
    private string mZoneId = "";

    public BidmadReward(string zoneId)
    {
        mZoneId = zoneId;
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadNewInstanceReward(zoneId);
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
            javaClassInstance.Call("makeReward");

            javaClassInstance.Call("setAdInfo", mZoneId);
        }
#endif
    }

    public void getInstance()
	{
#if UNITY_IOS
#elif UNITY_ANDROID
        using (javaClass = new AndroidJavaClass("ad.helper.openbidding.reward.UnityReward"))
        {
            if(javaClass != null)
            {
                javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance", mZoneId);
            }

        }
#endif
	}

    public void setCUID(string cuid) {
#if UNITY_IOS
        _bidmadSetCUIDRewardVideo(mZoneId, cuid);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("setCUID", cuid);
        }
#endif
    }

    public void setAutoReload(bool isAutoReload) {
#if UNITY_IOS
        _bidmadSetAutoReloadRewardVideo(mZoneId, isAutoReload);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("setAutoReload", isAutoReload);
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

    public void load()
	{
#if UNITY_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _bidmadLoadRewardVideo(mZoneId);
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
            _bidmadShowRewardVideo(mZoneId);
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
        result = _bidmadIsLoadedReward(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            result = javaClassInstance.Call<bool>("isLoaded");
        }
#endif
        return result;
    }

    public void setRewardLoadCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardLoad.ContainsKey(mZoneId)){
            BidmadManager.dicRewardLoad.Remove(mZoneId);
        }
        BidmadManager.dicRewardLoad.Add(mZoneId, callback);
#endif
    }

    public void setRewardShowCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardShow.ContainsKey(mZoneId)){
            BidmadManager.dicRewardShow.Remove(mZoneId);
        }
        BidmadManager.dicRewardShow.Add(mZoneId, callback);
#endif
    }

    public void setRewardFailCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardFail.ContainsKey(mZoneId)){
            BidmadManager.dicRewardFail.Remove(mZoneId);
        }
        BidmadManager.dicRewardFail.Add(mZoneId, callback);
#endif
    }

    public void setRewardCompleteCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardComplete.ContainsKey(mZoneId)){
            BidmadManager.dicRewardComplete.Remove(mZoneId);
        }
        BidmadManager.dicRewardComplete.Add(mZoneId, callback);
#endif
    }

    public void setRewardSkipCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardSkip.ContainsKey(mZoneId)){
            BidmadManager.dicRewardSkip.Remove(mZoneId);
        }
        BidmadManager.dicRewardSkip.Add(mZoneId, callback);
#endif
    }

    public void setRewardCloseCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicRewardClose.ContainsKey(mZoneId)){
            BidmadManager.dicRewardClose.Remove(mZoneId);
        }
        BidmadManager.dicRewardClose.Add(mZoneId, callback);
#endif
    }

}
