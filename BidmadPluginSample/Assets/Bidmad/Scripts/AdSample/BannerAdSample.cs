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
    
        banner = new BidmadBanner("944fe870-fa3a-4d1b-9cc2-38e50b2aed43", (float)130);  // Auto Center alignment
        // banner = new BidmadBanner("944fe870-fa3a-4d1b-9cc2-38e50b2aed43", 0, 130); // If you need to set position x.
        // banner = new BidmadBanner("944fe870-fa3a-4d1b-9cc2-38e50b2aed43", AdPosition.Bottom); // If you need to set AdPosition.

#elif UNITY_IOS
        banner = new BidmadBanner("1c3e3085-333f-45af-8427-2810c26a72fc", 130);
#endif

        // Bidmad Banner Ads can be set with Custom Unique ID with the following method.
        // BidmadCommon.setCuid("YOUR ENCRYPTED CUID TEXT");
        
        banner.setRefreshInterval(80);
        
        banner.load();

        // Ads can be repositioned after loading.
        // banner.updateViewPosition(0, 130);
  
        banner.setBannerLoadCallback(OnBannerLoad);
        banner.setBannerFailCallback(OnBannerLoadFail);
        banner.setBannerClickCallback(OnBannerClick);
    }

    public void removeBanner()
    {
        Debug.Log("removeBanner!!!");
        if(banner != null)
            banner.removeBanner();
    }

    void OnBannerLoad()
    {
        Debug.Log("OnBannerLoad Deletgate Callback Complate!!!");
    }

    void OnBannerLoadFail(string errorInfo)
    {
        Debug.Log("OnBannerLoadFail Deletgate Callback Complate!!! : "+errorInfo);
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
            if(banner != null)
                banner.pauseBanner();
        }
        else
        {
            if(banner != null)
                banner.resumeBanner();
        }
    }
    #endif

}
