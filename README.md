# BidmadPlugin

BidmadPlugin은 모바일 앱 광고 SDK인 Bidmad를 Unity에서 사용하기 위한 Plugin입니다.<br>
Plugin을 사용하여 Unity 모바일 앱에서 배너 / 전면 / 보상형 광고를 게재 할 수 있습니다.<br>

## 시작하기
- [최신 샘플 프로젝트 다운로드](https://github.com/bidmad/Bidmad-Unity/archive/master.zip)
- [최신 Plugin 다운로드](https://github.com/bidmad/Bidmad-Unity/releases/download/2.5.2/BidmadUnityPlugin_2.5.2.unitypackage)

### 1. Plugin 추가하기
#### 1.1 Android

1. 다운로드 받은 최신 버전 SDK를 프로젝트에 Import합니다.<br>
2. 프로젝트 내 mainTemplate.gradle 파일에서 [apply plugin: 'com.android.application'] 코드를 찾아 그 아래에 Bidmad Gradle 파일 경로 선언합니다.

```cpp
apply plugin: 'com.android.application'

apply from: "${getRootDir()}/../../Assets/Plugins/Android/bidmad/bidmad.gradle" //Bidmad Gradle 경로.
```
*Bidmad는 AndroidX 라이브러리를 사용합니다. AndroidX 프로젝트가 아니라면 AndroidX로 마이그레이션 바랍니다.

#### 1.2 iOS

1. 다운로드 받은 최신 버전 SDK를 프로젝트에 Import합니다.<br>
2. 프로젝트의 Build Settings에서 설정을 수정합니다.<br>
- Enable BitCode = No 설정<br>
- Other Linker Flags = -ObjC 추가<br>
3. info.plist에 GADApplicationIdentifier를 추가합니다.<br>
*GADApplicationIdentifier는 Google Admob에서 확인할 수 있습니다. 
```
    <key>GADApplicationIdentifier</key>
    <string>ca-app-pub-XXXXXX~XXXXXX</string>
```
4. 2019.03 이상 버전에서는 수동으로 Unity-iPhone 타겟의 Build Phases > Capy Bundle Resources에 bidmad_assets.bundle를 추가합니다.

### 2. Plugin 사용하기

#### 2.1 배너

- 배너광고를 요청하기 위해 BidmadBanner를 생성합니다. 이때 배너 View를 노출 시킬 높이(y) 값을 같이 전달 합니다.
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


#### 2.2 전면

- 전면광고를 요청하기 위해 BidmadInterstitial를 생성합니다.
- 전면광고를 노출하기전에 isLoaded를 통해 광고 로드 여부를 체크합니다.
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

#### 2.3 보상형

- 보상형광고를 요청하기 위해 BidmadInterstitial를 생성합니다.
- 보상형광고를 노출하기전에 isLoaded를 통해 광고 로드 여부를 체크합니다.
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

### 3. Callback 사용하기

- Plugin에서는 광고 타입에 따라 Load / Show / Fail등 Callback을 제공합니다.<br>
- 해당 동작에 대해 별도 처리가 필요한 경우 함수를 등록하여 처리합니다.
- Callback을 사용하기 위해서는 BidmadManager를 GameObject로 등록해야 합니다.
```cpp
    GameObject bidmadManager = new GameObject("BidmadManager");
    bidmadManager.AddComponent<BidmadManager>();
    DontDestroyOnLoad(bidmadManager);
```


#### 3.1 배너 Callback
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
#### 3.2 전면 Callback
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
#### 3.3 보상형 Callback
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
#### 4.1 배너

*배너 광고는 BidmadBanner를 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadBanner(string zoneId, float _y)|BidmadBanner 생성자, ZoneId와 배너 높이 위치 값(y)을 설정합니다.
public BidmadBanner(string zoneId, float _x, float _y)|BidmadBanner 생성자, ZoneId와 배너의 위치정보 X,Y를 설정합니다.(Only Android support)
public void setRefreshInterval(int time)|Banner Refresh 주기를 설정합니다.(60s~120s)
public void removeBanner()|노출된 배너를 제거합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void pauseBanner()|배너 광고를 정지 시킵니다. 주로 OnPause 이벤트 발생 시 호출하며, Android만 지원합니다. 
public void resumeBanner()|배너 광고를 다시 시작합니다. 주로 OnResume 이벤트 발생 시 호출하며, Android만 지원합니다. 
public void hideBannerView()|배너 광고 View를 숨깁니다. 
public void showBannerView()|배너 광고 View를 노출시킵니다.
public void setBannerLoadCallback(Action callback)|Action을 등록했다면 배너를 Load 했을 때 등록한 Action을 실행합니다.
public void setBannerFailCallback(Action callback)|Action을 등록했다면 ZoneId를 통한 배너 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setBannerClickCallback(Action callback)|Action을 등록했다면 배너 클릭 이벤트 발생 시 등록한 Action을 실행합니다.

#### 4.2 전면

*전면 광고는 BidmadInterstitial 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadInterstitial(string zoneId)|BidmadInterstitial 생성자, ZoneId를 설정합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void show()|Load한 광고를 노출 시킵니다.
public bool isLoaded()|광고가 Load된 상태인지 체크합니다.
public void setInterstitialLoadCallback(Action callback)|Action을 등록했다면 전면광고를 Load 했을 때 등록한 Action을 실행합니다.
public void setInterstitialShowCallback(Action callback)|Action을 등록했다면 전면광고를 Show 했을 때 등록한 Action을 실행합니다.
public void setInterstitialFailCallback(Action callback)|Action을 등록했다면 ZoneId를 통한 전면광고 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setInterstitialCloseCallback(Action callback)|Action을 등록했다면 전면광고를 Close 했을 때 등록한 Action을 실행합니다.

#### 4.3 보상형

*보상형 광고는 BidmadBanner를 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadReward(string zoneId)|BidmadReward 생성자, ZoneId를 설정합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void show()|Load한 광고를 노출 시킵니다.
public bool isLoaded()|광고가 Load된 상태인지 체크합니다.
public void setRewardLoadCallback(Action callback)|Action을 등록했다면 보상형광고를 Load 했을 때 등록한 Action을 실행합니다.
public void setRewardShowCallback(Action callback)|Action을 등록했다면 보상형광고를 Show 했을 때 등록한 Action을 실행합니다.
public void setRewardFailCallback(Action callback)|Action을 등록했다면 ZoneId를 통한 보상형광고 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setRewardCompleteCallback(Action callback)|Action을 등록했다면 보상형광고의 리워드 지급기준을 충족 했을 때 등록한 Action을 실행합니다.
public void setRewardSkipCallback(Action callback)|Action을 등록했다면 보상형광고의 리워드 지급기준에 미달 했을 때 등록한 Action을 실행합니다.
public void setRewardCloseCallback(Action callback)|Action을 등록했다면 보상형광고를 Close 했을 때 등록한 Action을 실행합니다.