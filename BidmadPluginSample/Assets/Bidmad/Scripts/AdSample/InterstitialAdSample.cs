using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialAdSample : MonoBehaviour
{
    static BidmadInterstitial interstitial;
    // Start is called before the first frame update
    void Start()
    {
        BidmadCommon.setIsDebug(true);
    }

    public void LoadInterstitialAd()
    {
#if UNITY_ANDROID
        if (interstitial == null)
            interstitial = new BidmadInterstitial("e9acd7fc-a962-40e4-aaad-9feab1b4f821");
#elif UNITY_IOS || UNITY_IPHONE
        if (interstitial == null)
            interstitial = new BidmadInterstitial("228b95a9-6f42-46d8-a40d-60f17f751eb1");
#endif

        // Bidmad Interstitial Ads auto-reload after the interstitial ad is shown to the user.
        // You can disable the auto-reload feature by giving "false" for setAutoReload method.
        // interstitial.setAutoReload(false);

        // Bidmad Interstitial Ads can be set with Custom Unique ID with the following method.
        // interstitial.setCUID("YOUR ENCRYPTED CUID TEXT");

        interstitial.load();

        interstitial.setInterstitialLoadCallback(OnInterstitialLoad);
        interstitial.setInterstitialShowCallback(OnInterstitialShow);
        interstitial.setInterstitialFailCallback(OnInterstitialLoadFail);
        interstitial.setInterstitialCloseCallback(OnInterstitialClose);
    }

    public void ShowInterstitialAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if(interstitial == null)
            return;

        if (interstitial.isLoaded()){
            interstitial.show();
        } else {
            interstitial.load();
        }
#endif
    }

    void OnInterstitialLoad()
    {
        Debug.Log("OnInterstitialLoad Deletgate Callback Complate!!!");
    }

    void OnInterstitialShow()
    {
        Debug.Log("OnInterstitialShow Deletgate Callback Complate!!!");
    }

    void OnInterstitialLoadFail(string errorInfo) 
    {
        Debug.Log("OnInterstitialFail Deletgate Callback Complate!!! : "+errorInfo);
    }

    void OnInterstitialClose()
    {
        Debug.Log("OnInterstitialClose Deletgate Callback Complate!!!");
    }

}
