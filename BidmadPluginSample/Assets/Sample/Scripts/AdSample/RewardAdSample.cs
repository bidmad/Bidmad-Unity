using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAdSample : MonoBehaviour
{
    static BidmadReward reward;

    // Optional UI hook: raised with a human-readable status message on each event.
    public System.Action<string> OnStatus;

    // Start is called before the first frame update
    void Start()
    {
        BidmadCommon.setIsDebug(true);
    }

    public void LoadRewardAd()
    {
        OnStatus?.Invoke("Requesting rewarded ad…");
#if UNITY_ANDROID
        if (reward == null)
            reward = new BidmadReward("7d9a2c9e-5755-4022-85f1-6d4fc79e4418");
#elif UNITY_IOS
        if (reward == null)
            reward = new BidmadReward("29e1ef67-98d2-47b3-9fa2-9192327dd75d");
#endif
        // Bidmad Rewarded Video Ads auto-reload after the Video ad is shown to the user.
        // You can disable the auto-reload feature by giving "false" for setAutoReload method.
        // reward.setAutoReload(false);

        // Bidmad Reward Ads can be set with Custom Unique ID with the following method.
        // BidmadCommon.setCuid("YOUR ENCRYPTED CUID TEXT");

        reward.load();

        reward.setRewardLoadCallback(OnRewardLoad);
        reward.setRewardShowCallback(OnRewardShow);
        reward.setRewardFailCallback(OnRewardLoadFail);
        reward.setRewardCompleteCallback(OnRewardComplete);
        reward.setRewardSkipCallback(OnRewardSkip);
        reward.setRewardCloseCallback(OnRewardClose);
    }

    public void ShowRewardAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if(reward == null)
        {
            return;
        }

        if (reward.isLoaded()) {
            reward.show();
        } else {
            OnStatus?.Invoke("Not ready yet — reloading…");
            reward.load();
        }
#endif
    }

    void OnRewardLoad()
    {
        Debug.Log("OnRewardLoad Deletgate Callback Complate!!!");
        OnStatus?.Invoke("Rewarded ad loaded");
    }

    void OnRewardShow()
    {
        Debug.Log("OnRewardShow Deletgate Callback Complate!!!");
        OnStatus?.Invoke("Rewarded ad shown");
    }

    void OnRewardLoadFail(string errorInfo)
    {
        Debug.Log("OnRewardLoadFail Deletgate Callback Complate!!! : " + errorInfo);
        OnStatus?.Invoke("Rewarded ad failed: " + errorInfo);
    }

    void OnRewardComplete()
    {
        Debug.Log("OnRewardComplete Deletgate Callback Complate!!!");
        OnStatus?.Invoke("Reward earned");
    }

    void OnRewardSkip()
    {
        Debug.Log("OnRewardSkip Deletgate Callback Complate!!!");
        OnStatus?.Invoke("Reward skipped");
    }

    void OnRewardClose()
    {
        Debug.Log("OnRewardClose Deletgate Callback Complate!!!");
        OnStatus?.Invoke("Rewarded ad closed");
    }

}
