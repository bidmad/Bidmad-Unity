using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using AOT;

public enum AdPosition {
        Center = 0,
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
        TopLeft = 5,
        TopRight = 6,
        BottomLeft = 7,
        BottomRight = 8,
        None = 9
    }

public class BidmadBanner
{
#if UNITY_IOS
    [DllImport("__Internal")]
    private static extern void _bidmadNewInstanceBanner(string zoneId, float _x, float _y);

    [DllImport("__Internal")]
    private static extern void _bidmadNewInstanceBannerAutoCenter(string zoneId, float _y);

    [DllImport("__Internal")]
    private static extern void _bidmadNewInstanceBannerAdPosition(string zoneId, int position);

    [DllImport("__Internal")]
    private static extern void _bidmadSetRefreshInterval(string zoneId, int time);

    [DllImport("__Internal")]
    private static extern void _bidmadLoadBanner(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadRemoveBanner(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadHideBannerView(string zoneId);

    [DllImport("__Internal")]
    private static extern void _bidmadShowBannerView(string zoneId);

#elif UNITY_ANDROID
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass javaClass = null;
    private AndroidJavaObject javaClassInstance = null;
#endif

    private string mZoneId = "";
    private AdPosition mPosition = AdPosition.None;
    private bool setBannerPosition = false;

    public BidmadBanner(string zoneId, float _y)
    {
        mZoneId = zoneId;
#if UNITY_IOS
        _bidmadNewInstanceBannerAutoCenter(zoneId, _y);
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
            javaClassInstance.Call("setBottom", (int)_y);
            javaClassInstance.Call("makeAdView");
        }
#endif
    }

    public BidmadBanner(string zoneId, float _x, float _y)
    {
        mZoneId = zoneId;
        setBannerPosition = true;
#if UNITY_IOS
        _bidmadNewInstanceBanner(zoneId, _x, _y);
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
            javaClassInstance.Call("setBottom", (int)_y);
            javaClassInstance.Call("setLeft", (int)_x);
            javaClassInstance.Call("makeAdView");
        }
#endif
    }

    public BidmadBanner(string zoneId, AdPosition position)
    {
        mZoneId = zoneId;
        mPosition = position;

#if UNITY_IOS
        _bidmadNewInstanceBannerAdPosition(zoneId, (int)position);
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
            javaClassInstance.Call("setAdPosition", (int)position);
            javaClassInstance.Call("makeAdView");
        }
#endif
    }

    public void getInstance()
	{
#if UNITY_IOS

#elif UNITY_ANDROID
        using (javaClass = new AndroidJavaClass("ad.helper.openbidding.adview.UnityAdView"))
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
        _bidmadSetRefreshInterval(mZoneId, time);
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
        _bidmadRemoveBanner(mZoneId);
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
        _bidmadLoadBanner(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            if(mPosition != AdPosition.None){
                javaClassInstance.Call("loadWithAdPosition");
                return;
            }

            if(!setBannerPosition)
                javaClassInstance.Call("loadWithY");
            else
                javaClassInstance.Call("loadWithXY");
                
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

    public void hideBannerView()
    {
#if UNITY_IOS
        _bidmadHideBannerView(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("hideBannerView");
        }
#endif
    }

    public void showBannerView()
    {
#if UNITY_IOS
        _bidmadShowBannerView(mZoneId);
#elif UNITY_ANDROID
        if (javaClassInstance != null)
        {
            javaClassInstance.Call("showBannerView");
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

    public void setBannerFailCallback(Action<string> callback)
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
