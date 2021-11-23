using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAdSample : MonoBehaviour
{
    static BidmadReward reward;
    // Start is called before the first frame update
    void Start()
    {
        BidmadCommon.setIsDebug(true);
    }

    public void LoadRewardAd()
    {
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

        reward.load();

        reward.setRewardLoadCallback(OnRewardLoad);
        reward.setRewardShowCallback(OnRewardShow);
        reward.setRewardFailCallback(OnRewardFail);
        reward.setRewardCompleteCallback(OnRewardComplete);
        reward.setRewardSkipCallback(OnRewardSkip);
        reward.setRewardCloseCallback(OnRewardClose);
    }

    public void ShowRewardAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (reward.isLoaded())
        {
            reward.show();
        }
#endif
    }

    void OnRewardLoad()
    {
        Debug.Log("OnRewardLoad Deletgate Callback Complate!!!");
    }

    void OnRewardShow()
    {
        Debug.Log("OnRewardShow Deletgate Callback Complate!!!");
    }

    void OnRewardFail()
    {
        Debug.Log("OnRewardFail Deletgate Callback Complate!!!");
    }

    void OnRewardComplete()
    {
        Debug.Log("OnRewardComplete Deletgate Callback Complate!!!");
    }

    void OnRewardSkip()
    {
        Debug.Log("OnRewardSkip Deletgate Callback Complate!!!");
    }

    void OnRewardClose()
    {
        Debug.Log("OnRewardClose Deletgate Callback Complate!!!");
    }
}
