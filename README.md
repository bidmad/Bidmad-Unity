

# BidmadPlugin

BidmadPlugin은 모바일 앱 광고 SDK인 Bidmad를 Unity에서 사용하기 위한 Plugin입니다.<br>
Plugin을 사용하여 Unity 모바일 앱에서 배너 / 전면 / 보상형 광고를 게재 할 수 있습니다.<br>

## 시작하기
- [최신 샘플 프로젝트 다운로드](https://github.com/bidmad/Bidmad-Unity/archive/master.zip)
- [최신 Plugin 다운로드](https://github.com/bidmad/Bidmad-Unity/releases)

### 1. Plugin 추가하기
#### 1.1 Android

1. 다운로드 받은 최신 버전 SDK를 프로젝트에 Import합니다.<br>
2. Assets → External Dependency Manager → Android Resolver → Settings에서 Use Gradle Daemon 체크<br>
3. Assets → External Dependency Manager → Android Resolver → Force Resolve 실행<br>
*2.18.0 이전 버전에서 업데이트 하는 경우 [가이드](https://github.com/bidmad/Bidmad-Unity/wiki/AOS-EDM4U-%EC%A0%84%ED%99%98-%EA%B0%80%EC%9D%B4%EB%93%9C)를 참고하여 이전 버전 요소를 제거 바랍니다.<br>
4. 아동을 타겟으로 하고 PlayStore에 심사를 받는 앱은 인증된 광고 네트워크를 사용을 위해 추가 설정이 필요합니다.<br> 
앱이 아동을 타겟하고 있다면 추가 설정을 위해 [가이드](https://github.com/bidmad/Bidmad-Unity/wiki/PlayStore-%EC%95%B1-%ED%83%80%EA%B2%9F%ED%8C%85-%EC%97%B0%EB%A0%B9%EC%97%90-%EB%94%B0%EB%A5%B8-%EC%B6%94%EA%B0%80-%EC%84%A4%EC%A0%95.)를 확인하세요.<br>
5. 프로젝트에서 Proguard를 적용하고 있다면 아래의 룰을 추가하세요.
```cpp
-keep class com.adop.sdk.** { *; }
-keep class ad.helper.openbidding.** { *; }
-keep class com.adop.adapter.fc.** { *; }
-keep class com.adop.adapter.fnc.** { *; }
-keepnames class * implements java.io.Serializable
-keepclassmembers class * implements java.io.Serializable {
    static final long serialVersionUID;
    private static final java.io.ObjectStreamField[] serialPersistentFields;
    !static !transient <fields>;
    private void writeObject(java.io.ObjectOutputStream);
    private void readObject(java.io.ObjectInputStream);
    java.lang.Object writeReplace();
    java.lang.Object readResolve();
}
-keepclassmembers class * {
    @android.webkit.JavascriptInterface <methods>;
}

# Pangle
-keep class com.bytedance.sdk.** { *; }
-keep class com.bykv.vk.openvk.component.video.api.** { *; }

# Tapjoy
-keep class com.tapjoy.** { *; }
-keep class com.moat.** { *; }
-keepattributes JavascriptInterface
-keepattributes *Annotation*
-keep class * extends java.util.ListResourceBundle {
protected Object[][] getContents();
}
-keep public class com.google.android.gms.common.internal.safeparcel.SafeParcelable {
public static final *** NULL;
}
-keepnames @com.google.android.gms.common.annotation.KeepName class *
-keepclassmembernames class * {
@com.google.android.gms.common.annotation.KeepName *;
}
-keepnames class * implements android.os.Parcelable {
public static final ** CREATOR;
}
-keep class com.google.android.gms.ads.identifier.** { *; }
-dontwarn com.tapjoy.**
```

6. Project Settings  → Publish Settings  → Custom Main Manifest를 활성화하여 AndroidManifest.xml의 application 태그 안에 아래 코드를 선언합니다([가이드](https://github.com/bidmad/SDK/wiki/Find-your-app-key%5BEN%5D#app-id-from-admob-dashboard))<br>
   *com.google.android.gms.ads.APPLICATION_ID의 value는 Admob 대시보드에서 확인 바랍니다.

```xml
<application>
   ...
   <meta-data android:name="com.google.android.gms.ads.APPLICATION_ID" android:value="APPLICATION_ID"/>
   ...
</application>
```

7. Android 12버전을 Target하는 경우 [AD_ID 권한 추가 선언 가이드](https://github.com/bidmad/Bidmad-Unity/wiki/AD_ID-Permission-Guide%5BKOR%5D)를 확인바랍니다.

*Bidmad는 AndroidX 라이브러리를 사용합니다. AndroidX 프로젝트가 아니라면 AndroidX로 마이그레이션 바랍니다.

#### 1.2 iOS

*Bidmad 는 Xcode 13.4 이상을 지원합니다. Xcode 버전이 13.4 미만이라면 13.4 이상 버전으로 업데이트 바랍니다. 

1. 다운로드 받은 최신 버전 SDK를 프로젝트에 Import합니다. <br>
2. Assets → Bidmad → Editor → BidmadPostProcessBuild.cs 파일을 수정합니다.<br>
    User Tracking Usage Description 과 Google App ID 를 변경해주십시오. 
    Google App ID 는 [App Key 찾기](https://github.com/bidmad/SDK/wiki/Find-your-app-key%5BKR%5D) 가이드 내부 "App ID from ADMOB Dashboard" 섹션을 참고해 가져올 수 있습니다.<br>
    ![Bidmad-Guide-3](https://i.imgur.com/xPuJaSC.png)<br>
3. Assets → External Dependency Manager → iOS Resolver → Settings 경로를 통해 세팅을 열어주십시오.<br>
    ![Bidmad-Guide-4](https://i.imgur.com/8cvpZR0.png)<br>
    Setting 패널에서 <strong>Link Frameworks Statically</strong> 를 체크한 뒤, OK 버튼을 눌러주십시오.<br>
4. iOS Xcode 프로젝트를 빌드한 이후, iOS 프로젝트 폴더에서 <strong>.xcworkspace</strong> 확장자의 파일을 열어주십시오.<br>
5. Unity-iPhone 프로젝트 세팅 → Build Settings → UnityFramework 타겟 → Enable Bitcode 를 "No" 로 설정하십시오.<br>
    ![Bidmad-Guide-4](https://i.imgur.com/cgCHNQA.png)<br>
6. Unity-iPhone 타겟 대상 프로젝트 세팅 → General → Frameworks, Libraries, and Embedded Content 내부 + 버튼 클릭 후 OMSDK_Pubmatic.xcframework, ADOPUtility.xcframework, BidmadAdapterDynamic.xcframework, FBLPromises.framework, OMSDK_Teadstv.xcframework, TeadsSDK.xcframework 추가하십시오.
    ![Bidmad-Guide-5](https://i.imgur.com/997NKID.png)<br>
7. Unity-iPhone 타겟 대상 프로젝트 세팅 → General → Frameworks, Libraries, and Embedded Content 내부에 Pods → Pods → AdFitSDK → Frameworks → AdFitSDK.framework 를 드래그해 넣습니다. 아래 GIF 를 참고해주세요.
    ![Bidmad-Guide-6](https://i.imgur.com/2ztRu9H.gif)<br>
8. Unity-iPhone 타겟 대상 프로젝트 세팅 → Build Phases 내부 + 버튼 클릭 후 New Run Script Phase 를 클릭하세요.
    ![Bidmad-Guide-7](https://i.imgur.com/jlmk9sF.png)<br>
9. 아래 코드를 복사해 Run Script 탭 아래 Shell Script 내부에 붙여넣으세요.
```
APP_PATH="${TARGET_BUILD_DIR}/${WRAPPER_NAME}"

# This script loops through the frameworks embedded in the application and
# removes unused architectures.
find "$APP_PATH" -name 'AdFitSDK.framework' -type d | while read -r FRAMEWORK
do
    FRAMEWORK_EXECUTABLE_NAME=$(defaults read "$FRAMEWORK/Info.plist" CFBundleExecutable)
    FRAMEWORK_EXECUTABLE_PATH="$FRAMEWORK/$FRAMEWORK_EXECUTABLE_NAME"
    echo "Executable is $FRAMEWORK_EXECUTABLE_PATH"

    EXTRACTED_ARCHS=()

    for ARCH in $ARCHS
    do
        echo "Extracting $ARCH from $FRAMEWORK_EXECUTABLE_NAME"
        lipo -extract "$ARCH" "$FRAMEWORK_EXECUTABLE_PATH" -o "$FRAMEWORK_EXECUTABLE_PATH-$ARCH"
        EXTRACTED_ARCHS+=("$FRAMEWORK_EXECUTABLE_PATH-$ARCH")
    done

    echo "Merging extracted architectures: ${ARCHS}"
    lipo -o "$FRAMEWORK_EXECUTABLE_PATH-merged" -create "${EXTRACTED_ARCHS[@]}"
    rm "${EXTRACTED_ARCHS[@]}"

    echo "Replacing original executable with thinned version"
    rm "$FRAMEWORK_EXECUTABLE_PATH"
    mv "$FRAMEWORK_EXECUTABLE_PATH-merged" "$FRAMEWORK_EXECUTABLE_PATH"

done
```
![Bidmad-Guide-8](https://i.imgur.com/SKRjDhg.png)<br>
10. [App Tracking Transparency Guide](https://github.com/bidmad/Bidmad-Unity/wiki/Preparing-for-iOS-14%5BKOR%5D)에 따라 앱 추적 투명성 승인 요청 팝업을 적용시켜주십시오. SKAdNetwork 리스트는 BidmadPostProcessBuild.cs 파일에 포함되어있습니다.<br>

*Apple Store에서 요구하는 개인정보 보호에 관한 가이드가 필요한 경우 [이곳](https://github.com/bidmad/Bidmad-Unity/wiki/Apple-privacy-survey%5BKOR%5D)을 참고하세요.

#### 1.3 iOS Migration Guide (2.8.1 이하 버전에서 2.9.0 이상 버전으로 업데이트 할 경우)

1. Assets → Plugins → iOS → Bidmad 폴더 및 내부 파일 전체를 삭제하십시오
2. Assets → Resources → Bidmad 폴더 및 내부 파일 전체를 삭제하십시오.
3. Assets → Bidmad → Scripts 폴더 및 내부 파일 전체를 삭제하십시오.
3. info.plist 내부 SKAdNetwork, Google App ID, User Tracking Usage Description 세팅 모두 BidmadPostProcessBuild.cs 파일로 옮겨졌습니다 (1.2 iOS 빌드가이드 2번 참고).
    이전에 info.plist에 설정하셨던 Google App ID / User Tracking Usage Description을 BidmadPostProcessBuild.cs 파일로 이전시켜주십시오. SKAdNetwork는 옮기실 필요가 없으며, BidmadPostProcessBuild에 App ID / User Tracking Usage Description 세팅 후 저장한 뒤에는, 추가로 info.plist 세팅을 할 부분이 없습니다.
4. 이후 위 1.2 iOS 빌드 가이드를 따라 해주시면 되겠습니다. 

### 2. Plugin 사용하기

#### 2.1 Migration (Bidmad Unity Plugin 2.21.0 이하 버전에서 3.0.0 버전으로 업데이트 할 경우)
앱 초기 구성에 앞서, 2.21.0 이하 버전에서 3.0.0 버전으로 업데이트하는 경우 [API Migration Guide](https://github.com/bidmad/Bidmad-Unity/wiki/v3.0.0-Migration-Guide) 를 참고해 앱 업데이트를 진행하십시오. 이후, 아래 initializeSdk 메서드 추가 과정도 거치십시오.<br>

#### 2.2 BidmadSDK 초기화
BidmadSDK 실행에 필요한 작업을 수행합니다. SDK는 initializeSdk 메서드를 호출하지 않은 경우 광고 로드를 허용하지 않습니다.<br>
initializeSdk 메서드는 ADOP Insight 에서 확인가능한 App Key 를 인자값으로 받고 있습니다. App Key 는 [App Key 찾기](https://github.com/bidmad/SDK/wiki/Find-your-app-key%5BKR%5D) 가이드를 참고해 가져올 수 있습니다.<br>
광고를 로드하기 전, 앱 실행 초기에 다음 예시와 같이 initializeSdk 메서드를 호출해주십시오.

```
#if UNITY_IOS
    BidmadCommon.initializeSdk("IOS APP KEY");
#elif UNITY_ANDROID
    BidmadCommon.initializeSdk("ANDROID APP KEY");
#endif
```

혹은, 3.4.0 이상 버전의 Bidmad Plugin을 사용하는 경우, bool 타입을 인자값으로 받는 Action 함수를 initializeSdk 메서드의 인자값으로 넣어 초기화 여부를 확인할 수 있습니다.

```
#if UNITY_IOS
    BidmadCommon.initializeSdk("IOS APP KEY", onInitialized);
#elif UNITY_ANDROID
    BidmadCommon.initializeSdk("ANDROID APP KEY", onInitialized);
#endif

void onInitialized(bool isComplete){
}
```

#### 2.3 배너

- 배너광고를 요청하기 위해 BidmadBanner를 생성합니다. 이때 배너 View를 노출 시킬 높이(y) 값을 같이 전달 합니다.
```cpp
    static BidmadBanner banner;

    public void LoadBannerAd()
    {
#if UNITY_ANDROID
        banner = new BidmadBanner("Your Android ZoneId", (float)0); // y 좌표 사용 시, float 타입 명시.
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

- 3.2.0 이상 버전의 Bidmad Plugin을 사용하는 경우, Y 좌표 대신 광고 위치를 설정하는 것도 지원됩니다. 광고 위치 값에는 Center, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight가 포함됩니다. 배너 광고의 광고 위치 설정은 다음 예시를 참고하세요.

```
    static BidmadBanner banner;

    public void LoadBannerAd()
    {
#if UNITY_ANDROID
        banner = new BidmadBanner("Your Android ZoneId", AdPosition.Bottom);
#elif UNITY_IOS
        banner = new BidmadBanner("Your iOS ZoneId", AdPosition.Bottom);
#endif
        banner.load();
    }
```

- 3.4.0 이상 버전의 Bidmad Plugin을 사용하는 경우, 로드 요청 이후 banner 위치를 재 조정 할 수 있습니다.

```
    banner.load();
    // RePosition
    banner.updateViewPosition(0, 130);
```

#### 2.4 전면

- 전면광고를 요청하기 위해 BidmadInterstitial를 생성합니다.
- load 메서드를 호출해 광고 로드를 요청하고, show 메서드를 호출해 로드된 광고를 노출시킵니다.
- 만약 광고 로드 실패로 인해 로드된 광고가 없을 경우, show 메서드 호출 시 자동으로 다시 광고 로드를 요청합니다. 
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
        interstitial.show();
#endif
    }
```

#### 2.5 보상형

- 보상형광고를 요청하기 위해 BidmadReward 생성합니다.
- load 메서드를 호출해 광고 로드를 요청하고, show 메서드를 호출해 로드된 광고를 노출시킵니다.
- 만약 광고 로드 실패로 인해 로드된 광고가 없을 경우, show 메서드 호출 시 자동으로 다시 광고 로드를 요청합니다.
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
    }

    public void ShowRewardAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        reward.show();
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
    banner.setBannerFailCallback(OnBannerLoadFail);
    banner.setBannerClickCallback(OnBannerClick);
...
    void OnBannerLoad()
    {
        Debug.Log("OnBannerLoad Deletgate Callback Complate!!!");
    }

    void OnBannerLoadFail(string errorInfo)
    {
        Debug.Log("OnBannerLoadFail Deletgate Callback Complate!!!");
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
    interstitial.setInterstitialFailCallback(OnInterstitialLoadFail);
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

    void OnInterstitialLoadFail(string errorInfo) 
    {
        Debug.Log("OnInterstitialLoadFail Deletgate Callback Complate!!! : "+errorInfo);
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
    reward.setRewardFailCallback(OnRewardLoadFail);
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

    void OnRewardLoadFail(string errorInfo)
    {
        Debug.Log("OnRewardLoadFail Deletgate Callback Complate!!! : " + errorInfo);
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
public BidmadBanner(string zoneId, float _x, float _y)|BidmadBanner 생성자, ZoneId와 배너의 위치정보 X,Y를 설정합니다.
public BidmadBanner(string zoneId, AdPosition position);|BidmadBanner 생성자, ZoneId와 배너의 위치를 설정합니다.
public void setRefreshInterval(int time)|Banner Refresh 주기를 설정합니다.(60s~120s)
public void removeBanner()|노출된 배너를 제거합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void updateViewPosition(float _y)|로드 이후 배너 뷰를 x기준 중앙 정렬된 상태로 y를 재배치 합니다. y는 뷰의 하단을 기준으로 합니다.
public void updateViewPosition(float _x, float _y)|로드 이후 배너 뷰 x, y 위치를 재배치 합니다. x는 뷰의 좌측, y는 뷰의 하단을 기준으로 합니다.
public void updateViewPosition(AdPosition position)|로드 이후 배너 뷰의 AdPosition 값으로 재배치 합니다.
public void pauseBanner()|배너 광고를 정지 시킵니다. 주로 OnPause 이벤트 발생 시 호출하며, Android만 지원합니다. 
public void resumeBanner()|배너 광고를 다시 시작합니다. 주로 OnResume 이벤트 발생 시 호출하며, Android만 지원합니다. 
public void hideBannerView()|배너 광고 View를 숨깁니다. 
public void showBannerView()|배너 광고 View를 노출시킵니다.
public void setBannerLoadCallback(Action callback)|Action을 등록했다면 배너를 Load 했을 때 등록한 Action을 실행합니다.
public void setBannerFailCallback(Action<string> callback)|Action을 등록했다면 ZoneId를 통한 배너 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setBannerClickCallback(Action callback)|Action을 등록했다면 배너 클릭 이벤트 발생 시 등록한 Action을 실행합니다.

#### 4.2 전면

*전면 광고는 BidmadInterstitial 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadInterstitial(string zoneId)|BidmadInterstitial 생성자, ZoneId를 설정합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void show()|Load한 광고를 노출 시킵니다.
public bool isLoaded()|광고가 Load된 상태인지 체크합니다.
public void setAutoReload(bool isAutoReload)|Show 이후 다음 광고를 Load 합니다. 해당 옵션은 기본 true로 적용되어있으며, failCallback을 수신한 경우에는 Reload 동작을 하지 않습니다. 
public void setInterstitialLoadCallback(Action callback)|Action을 등록했다면 전면광고를 Load 했을 때 등록한 Action을 실행합니다.
public void setInterstitialShowCallback(Action callback)|Action을 등록했다면 전면광고를 Show 했을 때 등록한 Action을 실행합니다.
public void setInterstitialFailCallback(Action<string> callback)|Action을 등록했다면 ZoneId를 통한 전면광고 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setInterstitialCloseCallback(Action callback)|Action을 등록했다면 전면광고를 Close 했을 때 등록한 Action을 실행합니다.

#### 4.3 보상형

*보상형 광고는 BidmadReward를 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public BidmadReward(string zoneId)|BidmadReward 생성자, ZoneId를 설정합니다.
public void load()|생성자에서 입력한 ZoneId로 광고를 요청합니다.
public void show()|Load한 광고를 노출 시킵니다.
public bool isLoaded()|광고가 Load된 상태인지 체크합니다.
public void setAutoReload(bool isAutoReload)|Show 이후 다음 광고를 Load 합니다. 해당 옵션은 기본 true로 적용되어있으며, failCallback을 수신한 경우에는 Reload 동작을 하지 않습니다.
public void setRewardLoadCallback(Action callback)|Action을 등록했다면 보상형광고를 Load 했을 때 등록한 Action을 실행합니다.
public void setRewardShowCallback(Action callback)|Action을 등록했다면 보상형광고를 Show 했을 때 등록한 Action을 실행합니다.
public void setRewardFailCallback(Action<string> callback)|Action을 등록했다면 ZoneId를 통한 보상형광고 Load가 실패 했을 때 등록한 Action을 실행합니다.
public void setRewardCompleteCallback(Action callback)|Action을 등록했다면 보상형광고의 리워드 지급기준을 충족 했을 때 등록한 Action을 실행합니다.
public void setRewardSkipCallback(Action callback)|Action을 등록했다면 보상형광고의 리워드 지급기준에 미달 했을 때 등록한 Action을 실행합니다.
public void setRewardCloseCallback(Action callback)|Action을 등록했다면 보상형광고를 Close 했을 때 등록한 Action을 실행합니다.

#### 4.5 기타 인터페이스

*기타 인터페이스는 BidmadCommon을 통해 처리되며 이를 위한 함수 목록입니다.

Function|Description
---|---
public static void initializeSdk(string appkey)|BidmadSDK 환경 설정을 초기화하고, 전면 및 리워드 광고를 프리로드합니다.
public static void initializeSdkWithCallback(string appkey, Action<bool> callback)|BidmadSDK 환경 설정을 초기화하고, 전면 및 리워드 광고를 프리로드합니다. Action<bool> 함수로 초기화 여부를 받습니다.
public static void setIsDebug(bool isDebug)|디버그 로그를 노출시킵니다.
public static void setGgTestDeviceid(string deviceId)|구글 애드몹 / 애드매니저를 위한 테스트 디바이스 등록 함수입니다.
public static void setCuid(string cuid)|cuid(Customer User Identifier)를 설정을 위한 함수 입니다.
public static void setUseServerSideCallback(bool isServerSideCallback)|Server Side Callback 사용시 세팅을 위한 함수입니다. 
public static void setGdprConsent(bool consent, bool useArea)|GDPR 동의여부를 등록합니다. consent: 동의여부 / useArea: 유럽지역 여부 
public static int getGdprConsent(bool useArea)|GDPR 동의여부를 가져옵니다.
public static string getPRIVACYURL()|Bidmad 개인정보 방침 웹 URL을 가져옵니다.

#### 4.6 iOS14 앱 추적 투명성 승인 요청

*앱 추적 투명성 승인 요청에 관한 함수는 BidmadCommon을 통해 제공됩니다.

Function|Description
---|---
public static void reqAdTrackingAuthorization(Action<BidmadTrackingAuthorizationStatus> callback)| 앱 추적 투명성 승인 요청 팝업을 발생시키고 결과를 callback으로 전달 합니다.
public static void setAdvertiserTrackingEnabled(bool enable)|reqAdTrackingAuthorization 이외의 함수로 앱 추적 투명성 승인 요청 팝업 동의/거절을 얻는 경우 이에 대한 결과를 설정합니다.
public static bool getAdvertiserTrackingEnabled()|설정된 앱 추적 투명성 승인 요청 팝업 동의/거절에 대한 결과를 조회합니다.

----
### 참고사항

- [GDPR 가이드](https://github.com/bidmad/Bidmad-Unity/wiki/Unity-GDPR-Guide-%5BKOR%5D)
- [v3.0.0 Migration Guide](https://github.com/bidmad/Bidmad-Unity/wiki/v3.0.0-Migration-Guide)
