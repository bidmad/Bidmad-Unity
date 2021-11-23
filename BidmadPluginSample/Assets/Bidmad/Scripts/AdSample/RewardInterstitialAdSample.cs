using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardInterstitialAdSample : MonoBehaviour
{
    static BidmadRewardInterstitial rewardInterstitial;
    // Start is called before the first frame update
    void Start()
    {
        BidmadCommon.setIsDebug(true);
    }

    public void LoadRewardInterstitialAd()
    {
#if UNITY_ANDROID
        if (rewardInterstitial == null)
            rewardInterstitial = new BidmadRewardInterstitial("bcea5bf7-4082-4691-9401-aeb062edfcb0");
#elif UNITY_IOS
        if (rewardInterstitial == null)
            rewardInterstitial = new BidmadRewardInterstitial("ee6e601d-2232-421b-a429-2e7163a8b41f");
#endif
        // Bidmad Rewarded Interstitial Ads auto-reload after the ad is shown to the user.
        // You can disable the auto-reload feature by giving "false" for setAutoReload method.
        // rewardInterstitial.setAutoReload(false);

        rewardInterstitial.load();

        rewardInterstitial.setRewardInterstitialLoadCallback(OnRewardInterstitialLoad);
        rewardInterstitial.setRewardInterstitialShowCallback(OnRewardInterstitialShow);
        rewardInterstitial.setRewardInterstitialFailCallback(OnRewardInterstitialFail);
        rewardInterstitial.setRewardInterstitialCompleteCallback(OnRewardInterstitialComplete);
        rewardInterstitial.setRewardInterstitialSkipCallback(OnRewardInterstitialSkip);
        rewardInterstitial.setRewardInterstitialCloseCallback(OnRewardInterstitialClose);
    }

    public void ShowRewardInterstitialAd()
    {
        GameObject resource = Resources.Load<GameObject>("Prefabs/RewardInterstitialAdPopupSample"); 
        GameObject parent = GameObject.Find("Canvas"); 
        GameObject popup = Instantiate<GameObject>(resource, parent.transform, false); 
        popup.SetActive(true);

        RewardInterstitialAdPopupSample popupComponent = GameObject.Find("PopupSample").GetComponent<RewardInterstitialAdPopupSample>(); 

        popupComponent.SetPositiveCallBack(() => { 
            Debug.Log("PositiveCallBack"); 

            #if UNITY_ANDROID || UNITY_IOS
                if (rewardInterstitial.isLoaded()){
            
                    rewardInterstitial.show();
                }
            #endif

            Destroy(popup.gameObject); 
        });

        popupComponent.SetNagativeCallBack(() => { 
            Debug.Log("NagativeCallBack"); 
            Destroy(popup.gameObject); 
        });
    }

    void OnRewardInterstitialLoad()
    {
        Debug.Log("OnRewardInterstitialLoad Deletgate Callback Complate!!!");
    }

    void OnRewardInterstitialShow()
    {
        Debug.Log("OnRewardInterstitialShow Deletgate Callback Complate!!!");
    }

    void OnRewardInterstitialFail()
    {
        Debug.Log("OnRewardInterstitialFail Deletgate Callback Complate!!!");
    }

    void OnRewardInterstitialComplete()
    {
        Debug.Log("OnRewardInterstitialComplete Deletgate Callback Complate!!!");
    }

    void OnRewardInterstitialSkip()
    {
        Debug.Log("OnRewardInterstitialSkip Deletgate Callback Complate!!!");
    }

    void OnRewardInterstitialClose()
    {
        Debug.Log("OnRewardInterstitialClose Deletgate Callback Complate!!!");
    }
}
