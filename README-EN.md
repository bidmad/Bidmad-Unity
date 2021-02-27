# BidmadPlugin

BidmadPlugin is a plugin for using Bidmad, a mobile app advertisement SDK, in Unity.<br>
You can use the plugin to serve banner/interstitial/reward ads in your Unity mobile app.<br>

## Getting started
- [Download the latest sample project](https://github.com/bidmad/Bidmad-Unity/archive/master.zip)
- [Download the latest plugin](https://github.com/bidmad/Bidmad-Unity/releases/download/2.6.0/BidmadUnityPlugin_2.6.0.unitypackage)

### 1. Add Plugin
#### 1.1 Android

1. Import the latest downloaded SDK to the project.<br>
2. Find the [apply plugin:'com.android.application'] code in the mainTemplate.gradle file in the project and declare the path to the Bidmad Gradle file under it.<br>

```cpp
apply plugin: 'com.android.application'

apply from: "${getRootDir()}/../../Assets/Plugins/Android/bidmad/bidmad.gradle" //Path of Bidmad Gradle.
```
*Bidmad uses the AndroidX library. If it is not an AndroidX project, please migrate to AndroidX.

#### 1.2 iOS

1. Import the latest downloaded SDK to the project.<br>
2. Change the settings in the project's Build Settings.<br>
- Set Enable BitCode = No<br>
- Add Other Linker Flags = -ObjC<br>
3. Add GADApplicationIdentifier to info.plist.<br>
*GADApplicationIdentifier can be found in Google Admob.
```
    <key>GADApplicationIdentifier</key>
    <string>ca-app-pub-XXXXXX~XXXXXX</string>
```
4. For 2019.03 and later versions, manually add bidmad_assets.bundle to [Build Phases > Capy Bundle Resources] of Unity-iPhone target.
5. Follow the [guide](https://github.com/bidmad/Bidmad-Unity/wiki/Preparing-for-iOS-14%5BENG%5D) to apply app tracking transparency approval request pop-up and SKAdNetwork.

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

- BidmadInterstitial is created to request a reward ad.
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
### 4. Plugin Function
#### 4.1 Banner

*Banner ads are handled through BidmadBanner and this is a list of functions for that.

Function|Description
---|---
public BidmadBanner(string zoneId, float _y)|This is the BidmadBanner constructor. set the ZoneId and banner height position value (y).
public BidmadBanner(string zoneId, float _x, float _y)|This is the BidmadBanner constructor, set the ZoneId and banner position x,y.(Only Android support)
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
public void setInterstitialLoadCallback(Action callback)|If an action is registered, the registered action is executed when the interstitial ad is loaded.
public void setInterstitialShowCallback(Action callback)|If an action is registered, the registered action is executed when the interstitial ad is shown.
public void setInterstitialFailCallback(Action callback)|If an Action is registered, the registered Action is executed when the load of interstitial ad through ZoneId fails.
public void setInterstitialCloseCallback(Action callback)|If an action is registered, the registered action is executed when the interstitial ad is closed.

#### 4.3 Reward

*Rewarded ads are processed through BidmadBanner, and this is a list of functions for this.

Function|Description
---|---
public BidmadReward(string zoneId)|This is the BidmadReward constructor, Set the ZoneId
public void load()|Request an ad with the ZoneId entered in the constructor.
public void show()|Display the loaded advertisement.
public bool isLoaded()|Check if the ad is loaded.
public void setRewardLoadCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is loaded.
public void setRewardShowCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is shown.
public void setRewardFailCallback(Action callback)|If an Action is registered, the registered Action is executed when the load of reward ad through ZoneId fails.
public void setRewardCompleteCallback(Action callback)|If an Action is registered, the registered Action is executed when the reward payment criteria of the reward ad are met.
public void setRewardSkipCallback(Action callback)|If an Action is registered, the registered Action is executed when the reward payment standard of the reward ad is not met.
public void setRewardCloseCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is closed.

#### 4.4 iOS14 AppTrackingTransparencyAuthorization

*AppTrackingTransparencyAuthorization functions are provided through BidmadCommon.

Function|Description
---|---
public static void reqAdTrackingAuthorization(Action<BidmadTrackingAuthorizationStatus> callback)| App Tracking Transparency Displays the approval request popup and passes the result to the callback.
public static void setAdvertiserTrackingEnabled(bool enable)| Set the result for app tracking transparency approval request pop-up consent/rejection obtained with a function other than reqAdTrackingAuthorization.
public static bool getAdvertiserTrackingEnabled()| Set app tracking transparency approval request popup inquires the result of consent/rejection.