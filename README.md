# BidmadPlugin

BidmadPlugin은 모바일 앱 광고 SDK인 Bidmad를 Unity에서 사용하기 위한 Plugin입니다.<br>
Plugin을 사용하여 Unity 모바일 앱에서 배너 / 전면 / 보상형 광고를 게재 할 수 있습니다.<br>

## 시작하기
- [최신 샘플 프로젝트 다운로드](https://github.com/bidmad/Bidmad-Unity/archive/master.zip)
- [최신 Plugin 다운로드](https://github.com/bidmad/Bidmad-Unity/releases)

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

1. 다운로드 받은 최신 버전 SDK를 프로젝트에 Import합니다. <br>
2. Assets → Bidmad → Editor → BidmadPostProcessBuild.cs 파일을 수정합니다.<br>
    User Tracking Usage Description 과 Google App ID 를 변경해주십시오. 
    ( GADApplicationIdentifier는 구글 애드몹에서 확인하실 수 있습니다 )<br>
    ![Bidmad-Guide-3](https://i.imgur.com/xPuJaSC.png)<br>
3. Assets → External Dependency Manager → iOS Resolver → Settings 경로를 통해 세팅을 열어주십시오.<br>
    ![Bidmad-Guide-4](https://i.imgur.com/8cvpZR0.png)<br>
    Setting 패널에서 <strong>Link Frameworks Statically</strong> 를 체크해주십시오.<br>
4. iOS Xcode 프로젝트를 빌드한 이후, iOS 프로젝트 폴더에서 <strong>.xcworkspace</strong> 확장자의 파일을 열어주십시오.<br>
5. [App Tracking Transparency Guide](https://github.com/bidmad/Bidmad-Unity/wiki/Preparing-for-iOS-14%5BKOR%5D)에 따라 앱 추적 투명성 승인 요청 팝업을 적용시켜주십시오. SKAdNetwork 리스트는 BidmadPostProcessBuild.cs 파일에 포함되어있습니다.<br>

*Apple Store에서 요구하는 개인정보 보호에 관한 가이드가 필요한 경우 [이곳](https://github.com/bidmad/Bidmad-Unity/wiki/Apple-privacy-survey%5BKOR%5D)을 참고하세요.

#### 1.3 iOS Migration Guide ( 2.8.1 혹은 이전 버전에서 2.9.0 이상 버전으로 업데이트 할 경우 )

1. Assets → Plugins → iOS → Bidmad 폴더 및 내부 파일 전체를 삭제하십시오
2. Assets → Resources → Bidmad 폴더 및 내부 파일 전체를 삭제하십시오.
3. Assets → Bidmad → Scripts 폴더 및 내부 파일 전체를 삭제하십시오.
3. info.plist 내부 SKAdNetwork, Google App ID, User Tracking Usage Description 세팅 모두 BidmadPostProcessBuild.cs 파일로 옮겨졌습니다 (1.2 iOS 빌드가이드 2번 참고).
    이전에 info.plist에 설정하셨던 Google App ID / User Tracking Usage Description을 BidmadPostProcessBuild.cs 파일로 이전시켜주십시오. SKAdNetwork는 옮기실 필요가 없으며, BidmadPostProcessBuild에 App ID / User Tracking Usage Description 세팅 후 저장한 뒤에는, 추가로 info.plist 세팅을 할 부분이 없습니다.
4. 이후 위 1.2 iOS 빌드 가이드를 따라 해주시면 되겠습니다. 

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

- 보상형광고를 요청하기 위해 BidmadReward 생성합니다.
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

#### 2.4 전면보상형

- 전면보상형광고는 앱 내부 자연스러운 페이지 전환 시 자동으로 게재되는 광고를 통해 리워드를 제공할 수 있는 새로운 보상형 광고 형식입니다. 
보상형 광고와 달리 사용자는 수신 동의하지 않고도 전면보상형광고를 볼 수 있습니다. 
광고 시청에 대한 리워드를 공지하고 사용자가 원할 경우 광고 수신 해제할 수 있는 시작 화면이 필요합니다. (BidmadSDK 샘플 앱을 확인해주십시오)

- 전면보상형광고를 요청하기 위해 BidmadRewardInterstitial을 생성합니다.
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

- 전면보상형광고를 노출하기 전, 유저가 광고 수신에 대해 거부할 수 있는 시작 화면을 디스플레이 합니다.
- Reward Interstitial 광고 노출 전, isLoaded 를 통해 광고 로드 여부를 체크합니다.
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
#### 3.4 전면보상형 Callback
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
#### 4.1 배너

*배너 광고는 BidmadBanner를 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadBanner(string zoneId, float _y)|BidmadBanner 생성자, ZoneId와 배너 높이 위치 값(y)을 설정합니다.
public BidmadBanner(string zoneId, float _x, float _y)|BidmadBanner 생성자, ZoneId와 배너의 위치정보 X,Y를 설정합니다.
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

*보상형 광고는 BidmadReward를 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadReward(string zoneId)|BidmadReward 생성자, ZoneId를 설정합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void show()|Load한 광고를 노출 시킵니다.
public bool isLoaded()|광고가 Load된 상태인지 체크합니다.
public void setUserId(string id)|서버측 인증이 필요한 경우 호출합니다. 일부 네트워크에서만 동작하며, 사용이 필요한 경우 문의 바랍니다. (Android Only)
public void setRewardLoadCallback(Action callback)|Action을 등록했다면 보상형광고를 Load 했을 때 등록한 Action을 실행합니다.
public void setRewardShowCallback(Action callback)|Action을 등록했다면 보상형광고를 Show 했을 때 등록한 Action을 실행합니다.
public void setRewardFailCallback(Action callback)|Action을 등록했다면 ZoneId를 통한 보상형광고 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setRewardCompleteCallback(Action callback)|Action을 등록했다면 보상형광고의 리워드 지급기준을 충족 했을 때 등록한 Action을 실행합니다.
public void setRewardSkipCallback(Action callback)|Action을 등록했다면 보상형광고의 리워드 지급기준에 미달 했을 때 등록한 Action을 실행합니다.
public void setRewardCloseCallback(Action callback)|Action을 등록했다면 보상형광고를 Close 했을 때 등록한 Action을 실행합니다.

#### 4.4 전면보상형

*전면보상형광고는 BidmadRewardInterstitial를 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadRewardInterstitial(string zoneId)|BidmadRewardInterstitial 생성자, ZoneId를 설정합니다.
public void setUserId(string userId)|서버측 인증이 필요한 경우 호출합니다. 일부 네트워크에서만 동작하며, 사용이 필요한 경우 문의 바랍니다. (Android Only)
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void show()|Load한 광고를 노출 시킵니다.
public bool isLoaded()|광고가 Load된 상태인지 체크합니다.
public void setRewardInterstitialLoadCallback(Action callback)|Action을 등록했다면 전면보상형광고를 Load 했을 때 등록한 Action을 실행합니다.
public void setRewardInterstitialShowCallback(Action callback)|Action을 등록했다면 전면보상형광고를 Show 했을 때 등록한 Action을 실행합니다.
public void setRewardInterstitialFailCallback(Action callback)|Action을 등록했다면 ZoneId를 통한 전면보상형광고 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setRewardInterstitialCompleteCallback(Action callback)|Action을 등록했다면 전면보상형광고의 리워드 지급기준을 충족 했을 때 등록한 Action을 실행합니다.
public void setRewardInterstitialSkipCallback(Action callback)|Action을 등록했다면 전면보상형광고의 리워드 지급기준에 미달 했을 때 등록한 Action을 실행합니다.
public void setRewardInterstitialCloseCallback(Action callback)|Action을 등록했다면 전면보상형광고를 Close 했을 때 등록한 Action을 실행합니다.

#### 4.5 iOS14 앱 추적 투명성 승인 요청

*앱 추적 투명성 승인 요청에 관한 함수는 BidmadCommon을 통해 제공됩니다.

Function|Description
---|---
public static void reqAdTrackingAuthorization(Action<BidmadTrackingAuthorizationStatus> callback)| 앱 추적 투명성 승인 요청 팝업을 발생시키고 결과를 callback으로 전달 합니다.
public static void setAdvertiserTrackingEnabled(bool enable)|reqAdTrackingAuthorization 이외의 함수로 앱 추적 투명성 승인 요청 팝업 동의/거절을 얻는 경우 이에 대한 결과를 설정합니다.
public static bool getAdvertiserTrackingEnabled()|설정된 앱 추적 투명성 승인 요청 팝업 동의/거절에 대한 결과를 조회합니다.

----
### 참고사항

- [GDPR 가이드](https://github.com/bidmad/Bidmad-Unity/wiki/Unity-GDPR-Guide-%5BKOR%5D)

