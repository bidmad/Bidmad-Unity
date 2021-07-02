using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerAdSample : MonoBehaviour
{
    static BidmadBanner banner;
    // Start is called before the first frame update
    void Start()
    {
        BidmadCommon.setIsDebug(true);
    }

    public void LoadBannerAd()
    {
#if UNITY_ANDROID
        banner = new BidmadBanner("944fe870-fa3a-4d1b-9cc2-38e50b2aed43", 130);
#elif UNITY_IOS
        banner = new BidmadBanner("1c3e3085-333f-45af-8427-2810c26a72fc", 130);
#endif
        banner.setRefreshInterval(80);
        banner.load();

        banner.setBannerLoadCallback(OnBannerLoad);
        banner.setBannerFailCallback(OnBannerFail);
        banner.setBannerClickCallback(OnBannerClick);
    }

    public void removeBanner()
    {
        Debug.Log("removeBanner!!!");
        banner.removeBanner();
    }

    void OnBannerLoad()
    {
        Debug.Log("OnBannerLoad Deletgate Callback Complate!!!");
    }

    void OnBannerFail()
    {
        Debug.Log("OnBannerFail Deletgate Callback Complate!!!");
    }

    void OnBannerClick()
    {
        Debug.Log("OnBannerClick Deletgate Callback Complate!!!");
    }

    #if UNITY_ANDROID
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            banner.pauseBanner();
        }
        else
        {
            banner.resumeBanner();
        }
    }
    #endif
}
