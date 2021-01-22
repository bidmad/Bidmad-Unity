using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public class BidmadBanner
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _newInstanceBanner(string zoneId, float _x, float _y);

    [DllImport("__Internal")]
    private static extern void _setRefreshInterval(string zoneId, int time);

    [DllImport("__Internal")]
    private static extern void _loadBanner(string zoneId);

    [DllImport("__Internal")]
    private static extern void _removeBanner(string zoneId);

#elif UNITY_ANDROID
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;
#endif

    private string mZoneId = "";
    private float mX = 0;
    private float mY = 0;

    public BidmadBanner(string zoneId, float _y)
    {
        mZoneId = zoneId;
        mY = _y;
#if UNITY_IOS
        _newInstanceBanner(zoneId, 0, mY);
#elif UNITY_ANDROID
        using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
        }

        getInstance();
        if(javaClassInstance != null)
        {
            javaClassInstance.Call("setActivity", activityContext);
            javaClassInstance.Call("setContext", activityContext);
            javaClassInstance.Call("makeAdView");

            javaClassInstance.Call("setAdInfo", mZoneId);
        }
#endif
    }

    public void getInstance()
	{
#if UNITY_IOS

#elif UNITY_ANDROID
        using (javaClass = new AndroidJavaClass("com.adop.sdk.adview.UnityAdView"))
        {
            if(javaClass != null) 
            {
                javaClassInstance = javaClass.CallStatic<AndroidJavaObject>("getInstance", mZoneId);
            }
        }
#endif
	}

    public void setRefreshInterval(int time)
    {
#if UNITY_IOS
        _setRefreshInterval(mZoneId, time);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("setInterval", time);
        }
#endif
    }

	public void removeBanner()
	{
#if UNITY_IOS
        _removeBanner(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("removeBanner");
        }
#endif
    }

	public void load()
	{
#if UNITY_IOS
        _loadBanner(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("start", (int)mY);
        }
#endif
    }

    public void pauseBanner()
    {
#if UNITY_IOS

#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("onPause");
        }
#endif
    }

    public void resumeBanner()
    {
#if UNITY_IOS

#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("onResume");
        }
#endif
    }

    public void setBannerLoadCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicBannerLoad.ContainsKey(mZoneId))
        {
            BidmadManager.dicBannerLoad.Remove(mZoneId);
        }
        BidmadManager.dicBannerLoad.Add(mZoneId, callback);
#endif
    }

    public void setBannerFailCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicBannerFail.ContainsKey(mZoneId))
        {
            BidmadManager.dicBannerFail.Remove(mZoneId);
        }
        BidmadManager.dicBannerFail.Add(mZoneId, callback);
#endif
    }

    public void setBannerClickCallback(Action callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (BidmadManager.dicBannerClick.ContainsKey(mZoneId)){

            BidmadManager.dicBannerClick.Remove(mZoneId);
        }
        BidmadManager.dicBannerClick.Add(mZoneId, callback);
#endif
    }
}//END
