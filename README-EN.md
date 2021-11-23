# BidmadPlugin

BidmadPlugin is a plugin for using Bidmad, a mobile app advertisement SDK, in Unity.<br>
You can use the plugin to serve banner/interstitial/reward ads in your Unity mobile app.<br>

## Getting started
- [Download the latest sample project](https://github.com/bidmad/Bidmad-Unity/archive/master.zip)
- [Download the latest plugin](https://github.com/bidmad/Bidmad-Unity/releases)

### 1. Add Plugin
#### 1.1 Android

1. Import the latest downloaded SDK to the project.<br>
2. Find the [apply plugin:'com.android.application'] code in the gradle file in the project and declare the path to the Bidmad Gradle file under it.<br>
(*mainTemplate.gradle for 2019.02 or less / launcherTemplate.gradle for 2019.03 or later)

```cpp
apply plugin: 'com.android.application'

apply from: "${getRootDir()}/../../Assets/Plugins/Android/bidmad/bidmad.gradle" //Path of Bidmad Gradle.
```
3. Apps that target children and are vetted by the PlayStore require additional setup to use certified ad networks.<br> 
If your app is targeting children, check out our [guide](https://github.com/bidmad/Bidmad-Unity/wiki/PlayStore-%EC%95%B1-%ED%83%80%EA%B2%9F%ED%8C%85-%EC%97%B0%EB%A0%B9%EC%97%90-%EB%94%B0%EB%A5%B8-%EC%B6%94%EA%B0%80-%EC%84%A4%EC%A0%95.) for further setup.<br>

*Bidmad uses the AndroidX library. If it is not an AndroidX project, please migrate to AndroidX.

#### 1.2 iOS

1. Please import the latest plugin.<br>
2. Please make adjustments to BidmadPostProcessBuild.cs file in Assets → Bidmad → Editor.<br>
    Make sure to change User Tracking Usage Description and Google App ID. 
    ( GADApplicationIdentifier can be found in the Google Admob website )<br>
    ![Bidmad-Guide-3](https://i.imgur.com/xPuJaSC.png)<br>
3. Please open the settings panel from Assets - External Dependency Manager - iOS Resolver - Settings.<br>
    ![Bidmad-Guide-4](https://i.imgur.com/8cvpZR0.png)<br>
    Please check and click the OK button on <strong>Link Frameworks Statically</strong> inside the settings panel.<br>
4. After building iOS Xcode Project, iOS Xcode Project folder will contain a project file with <strong>.xcworkspace</strong> extension. Please open it. <br>
5. Unity-iPhone Project Settings → Build Settings → UnityFramework Target → Set Enable Bitcode to "No".<br>
    ![Bidmad-Guide-4](https://i.imgur.com/cgCHNQA.png)<br>
6. Follow the [guide](https://github.com/bidmad/Bidmad-Unity/wiki/Preparing-for-iOS-14%5BENG%5D) to apply app tracking transparency approval request pop-up. SKAdNetwork lists are included in BidmadPostProcessBuild.cs file.<br>

*If you're looking for a guide to the privacy requirements of the Apple Store, [see here](https://github.com/bidmad/Bidmad-Unity/wiki/Apple-privacy-survey%5BENG%5D).

#### 1.3 iOS Migration Guide ( For Users migrating from 2.8.1 or under to the latest plugin )

1. Delete Assets → Plugins → iOS → Bidmad
2. Delete Assets → Resources → Bidmad
3. Delete Assets → Bidmad → Scripts
4. SKAdNetwork, Google App ID, User Tracking Usage Description Settings, which previously were set inside info.plist, are all moved to BidmadPostProcessBuild.cs (Please refer to the second step of 1.2 iOS Build Guide). Please set your App ID and User Tracking Usage Description in BidmadPostProcessBuild.cs file. It is not necessary for you to set SKAdNetwork as BidmadPostProcess is pre-set with needed SKAdNetworks. After setting and saving BidmadPostProcessBuild, there is no need for you to additionally set the info.plist. 
4. After following the steps above, please follow the steps in the section 1.2 iOS Build Guide.

### 2. Using Plugin

#### 2.1 Banner

- Create BidmadBanner to request banner advertisement. At this time, you must pass the height (y) value
```cpp
    static BidmadBanner banner;

    public void LoadBannerAd()
    {
#if UNITY_ANDROID
        banner = new BidmadBanner("Your Android ZoneId", 0);
#elif UNITY_IOS
        banner = new BidmadBanner("Your iOS ZoneId", 0);
#endif
        banner.load();
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
```


#### 2.2 Interstitial

- Create BidmadInterstitial to request interstitial ad.
- Before displaying an interstitial ad, check whether the ad is loaded through isLoaded.
```cpp
    static BidmadInterstitial interstitial;

    public void LoadInterstitialAd()
    {
#if UNITY_ANDROID
        if (interstitial == null)
            interstitial = new BidmadInterstitial("Your Android ZoneId");
#elif UNITY_IOS
        if (interstitial == null)
            interstitial = new BidmadInterstitial("Your iOS ZoneId");
#endif
        interstitial.load();
    }

    public void ShowInterstitialAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (interstitial.isLoaded())
        {
            interstitial.show();
        }
#endif
    }
```

#### 2.3 Reward

- BidmadReward is created to request a reward ad.
- Before displaying an reward ad, check whether the ad is loaded through isLoaded.
```cpp
    static BidmadReward reward;

    public void LoadRewardAd()
    {
#if UNITY_ANDROID
        if (reward == null)
            reward = new BidmadReward("Your Android ZoneId");
#elif UNITY_IOS
        if (reward == null)
            reward = new BidmadReward("Your iOS ZoneId");
#endif
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
```

#### 2.4 Reward Interstitial

Reward Interstitial is a new reward-type advertising format that can provide rewarded ads when switching between pages in an app.
Unlike Reward ads, users can view Reward Interstitial ads without user's consent. However, the developer needs to provide a screen announcing that there will be a reward for watching an ad and the user can cancel watching the ad if he/she wants. (Please check the BidmadSDK sample app)

- In order to request a Reward Interstitial Ad, please create a BidmadRewardInterstitial instance. 
```cpp
static BidmadRewardInterstitial rewardInterstitial;
public void LoadRewardInterstitialAd()
{
#if UNITY_ANDROID
    if (rewardInterstitial == null)
        rewardInterstitial = new BidmadRewardInterstitial("YOUR-ANDROID-ZONE-ID");
#elif UNITY_IOS
    if (rewardInterstitial == null)
        rewardInterstitial = new BidmadRewardInterstitial("YOUR-IOS-ZONE-ID");
#endif
    rewardInterstitial.load();

    rewardInterstitial.setRewardInterstitialLoadCallback(OnRewardInterstitialLoad);
    rewardInterstitial.setRewardInterstitialShowCallback(OnRewardInterstitialShow);
    rewardInterstitial.setRewardInterstitialFailCallback(OnRewardInterstitialFail);
    rewardInterstitial.setRewardInterstitialCompleteCallback(OnRewardInterstitialComplete);
    rewardInterstitial.setRewardInterstitialSkipCallback(OnRewardInterstitialSkip);
    rewardInterstitial.setRewardInterstitialCloseCallback(OnRewardInterstitialClose);
}
```

- Before displaying Reward Interstitial ad, please display a screen that allows users to cancel the ad. 
- Before displaying Reward Interstitial ad, please check if the ad is loaded by calling isLoaded.
```cpp
public void ShowRewardInterstitialAd()
{
    // Display a popup that gives users choice to opt out of Reward Interstitial Ads
    GameObject resource = Resources.Load<GameObject>("Prefabs/RewardInterstitialAdPopupSample"); 
    GameObject parent = GameObject.Find("Canvas"); 
    GameObject popup = Instantiate<GameObject>(resource, parent.transform, false); 
    popup.SetActive(true);

    RewardInterstitialAdPopupSample popupComponent = GameObject.Find("PopupSample").GetComponent<RewardInterstitialAdPopupSample>(); 

    // After certain time, if the user did not choose to opt out, automatically display the Reward Interstitial Ads  
    popupComponent.SetPositiveCallBack(() => { 
        Debug.Log("PositiveCallBack"); 

        #if UNITY_ANDROID || UNITY_IOS
            if (rewardInterstitial.isLoaded()){
                rewardInterstitial.show();
            }
        #endif

        Destroy(popup.gameObject); 
    });

    // If the user did choose to opt out, do not display the Reward Interstitial Ad.
    popupComponent.SetNagativeCallBack(() => { 
        Debug.Log("NagativeCallBack"); 
        Destroy(popup.gameObject); 
    });
}
```

### 3 Callback

- Plugin provides Callback such as Load / Show / Fail according to the advertisement type.<br>
- If separate processing is required for the corresponding action, register a function to handle it.
- To use Callback, BidmadManager must be registered as GameObject.
```cpp
    GameObject bidmadManager = new GameObject("BidmadManager");
    bidmadManager.AddComponent<BidmadManager>();
    DontDestroyOnLoad(bidmadManager);
```


#### 3.1 Banner Callback
```cpp
....
    banner.setBannerLoadCallback(OnBannerLoad);
    banner.setBannerFailCallback(OnBannerFail);
    banner.setBannerClickCallback(OnBannerClick);
...
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
```
#### 3.2 Interstitial Callback
```cpp
....
    interstitial.setInterstitialLoadCallback(OnInterstitialLoad);
    interstitial.setInterstitialShowCallback(OnInterstitialShow);
    interstitial.setInterstitialFailCallback(OnInterstitialFail);
    interstitial.setInterstitialCloseCallback(OnInterstitialClose);
...
    void OnInterstitialLoad()
    {
        Debug.Log("OnInterstitialLoad Deletgate Callback Complate!!!");
    }

    void OnInterstitialShow()
    {
        Debug.Log("OnInterstitialShow Deletgate Callback Complate!!!");
    }

    void OnInterstitialFail()
    {
        Debug.Log("OnInterstitialFail Deletgate Callback Complate!!!");
    }

    void OnInterstitialClose()
    {
        Debug.Log("OnInterstitialClose Deletgate Callback Complate!!!");
    }
```
#### 3.3 Reward Callback
```cpp
....
    reward.setRewardLoadCallback(OnRewardLoad);
    reward.setRewardShowCallback(OnRewardShow);
    reward.setRewardFailCallback(OnRewardFail);
    reward.setRewardCompleteCallback(OnRewardComplete);
    reward.setRewardSkipCallback(OnRewardSkip);
    reward.setRewardCloseCallback(OnRewardClose);
...
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
```
#### 3.4 Reward Interstitial Callback
```cpp
    ...
    rewardInterstitial.setRewardInterstitialLoadCallback(OnRewardInterstitialLoad);
    rewardInterstitial.setRewardInterstitialShowCallback(OnRewardInterstitialShow);
    rewardInterstitial.setRewardInterstitialFailCallback(OnRewardInterstitialFail);
    rewardInterstitial.setRewardInterstitialCompleteCallback(OnRewardInterstitialComplete);
    rewardInterstitial.setRewardInterstitialSkipCallback(OnRewardInterstitialSkip);
    rewardInterstitial.setRewardInterstitialCloseCallback(OnRewardInterstitialClose);
    ...
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
```
### 4. Plugin Function
#### 4.1 Banner

*Banner ads are handled through BidmadBanner and this is a list of functions for that.

Function|Description
---|---
public BidmadBanner(string zoneId, float _y)|This is the BidmadBanner constructor. set the ZoneId and banner height position value (y).
public BidmadBanner(string zoneId, float _x, float _y)|This is the BidmadBanner constructor, set the ZoneId and banner position x,y.
public void setRefreshInterval(int time)|Set the banner refresh cycle.(60s~120s)
public void removeBanner()|Remove the exposed banner.
public void load()|Request an ad with the ZoneId entered in the constructor.
public void pauseBanner()|Banner ads are stopped. It is mainly called when the OnPause event occurs. Only Android is supported.
public void resumeBanner()|Restart banner ads. It is mainly called when the OnResume event occurs. Only Android is supported.
public void hideBannerView()|Hide the banner View. 
public void showBannerView()|Show the banner View.
public void setBannerLoadCallback(Action callback)|If an Action is registered, the registered Action is executed when the banner is loaded.
public void setBannerFailCallback(Action callback)|If an Action is registered, the registered Action is executed when the banner load through ZoneId fails.
public void setBannerClickCallback(Action callback)|If an Action is registered, the registered Action is executed when a banner click event occurs.

#### 4.2 Interstitial

*Interstitial ads are handled through BidmadInterstitial, which is a list of functions for this.

Function|Description
---|---
public BidmadInterstitial(string zoneId)|This is the BidmadInterstitial constructor, Set the ZoneId
public void load()|Request an ad with the ZoneId entered in the constructor.
public void show()|Display the loaded advertisement.
public bool isLoaded()|Check if the ad is loaded.
public void setAutoReload(bool isAutoReload)|After the show, load the next advertisement. This option is applied as the default true, and if you receive a failCallback, you will not perform Reload action. 
public void setInterstitialLoadCallback(Action callback)|If an action is registered, the registered action is executed when the interstitial ad is loaded.
public void setInterstitialShowCallback(Action callback)|If an action is registered, the registered action is executed when the interstitial ad is shown.
public void setInterstitialFailCallback(Action callback)|If an Action is registered, the registered Action is executed when the load of interstitial ad through ZoneId fails.
public void setInterstitialCloseCallback(Action callback)|If an action is registered, the registered action is executed when the interstitial ad is closed.

#### 4.3 Reward

*Rewarded ads are processed through BidmadReward, and this is a list of functions for this.

Function|Description
---|---
public BidmadReward(string zoneId)|This is the BidmadReward constructor, Set the ZoneId
public void load()|Request an ad with the ZoneId entered in the constructor.
public void show()|Display the loaded advertisement.
public bool isLoaded()|Check if the ad is loaded.
public void setAutoReload(bool isAutoReload)|After the show, load the next advertisement. This option is applied as the default true, and if you receive a failCallback, you will not perform Reload action. 
public void setUserId(string id)|Called when server-side verification is required. It only works on some networks, and if you need to use it, please contact us. (Android Only)
public void setRewardLoadCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is loaded.
public void setRewardShowCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is shown.
public void setRewardFailCallback(Action callback)|If an Action is registered, the registered Action is executed when the load of reward ad through ZoneId fails.
public void setRewardCompleteCallback(Action callback)|If an Action is registered, the registered Action is executed when the reward payment criteria of the reward ad are met.
public void setRewardSkipCallback(Action callback)|If an Action is registered, the registered Action is executed when the reward payment standard of the reward ad is not met.
public void setRewardCloseCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is closed.

#### 4.4 Reward Interstitial

*Reward Interstitial ads are processed through BidmadRewardInterstitial, and this is a list of functions for this.

Function|Description
---|---
public BidmadRewardInterstitial(string zoneId)|This is the BidmadRewardInterstitial constructor, Set the ZoneId
public void setUserId(string userId)|Called when server-side verification is required. It only works on some networks, and if you need to use it, please contact us. (Android Only)
public void load()|Request an ad with the ZoneId entered in the constructor.
public void show()|Display the loaded advertisement.
public bool isLoaded()|Check if the ad is loaded.
public void setAutoReload(bool isAutoReload)|After the show, load the next advertisement. This option is applied as the default true, and if you receive a failCallback, you will not perform Reload action. 
public void setRewardInterstitialLoadCallback(Action callback)|If an action is registered, the registered action is executed when the Reward Interstitial ad is loaded.
public void setRewardInterstitialShowCallback(Action callback)|If an action is registered, the registered action is executed when the Reward Interstitial ad is shown.
public void setRewardInterstitialFailCallback(Action callback)|If an Action is registered, the registered Action is executed when Reward Interstitil ad loading fails.
public void setRewardInterstitialCompleteCallback(Action callback)|If an Action is registered, the registered Action is executed when the criteria for reward is met for the Reward Interstitial.
public void setRewardInterstitialSkipCallback(Action callback|If an Action is registered, the registered Action is executed when the cirteria for reward is not met.
public void setRewardInterstitialCloseCallback(Action callback)|If an action is registered, the registered action is executed when the Reward Interstitial ad is closed.

#### 4.5 iOS14 AppTrackingTransparencyAuthorization

*AppTrackingTransparencyAuthorization functions are provided through BidmadCommon.

Function|Description
---|---
public static void reqAdTrackingAuthorization(Action<BidmadTrackingAuthorizationStatus> callback)| App Tracking Transparency Displays the approval request popup and passes the result to the callback.
public static void setAdvertiserTrackingEnabled(bool enable)| Set the result for app tracking transparency approval request pop-up consent/rejection obtained with a function other than reqAdTrackingAuthorization.
public static bool getAdvertiserTrackingEnabled()| Set app tracking transparency approval request popup inquires the result of consent/rejection.

----
### Reference

- [GDPR Guide](https://github.com/bidmad/Bidmad-Unity/wiki/Unity-GDPR-Guide-%5BENG%5D)
