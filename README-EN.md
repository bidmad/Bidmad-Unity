# BidmadPlugin

BidmadPlugin is a plugin for using Bidmad, a mobile app advertisement SDK, in Unity.<br>
You can use the plugin to serve banner/interstitial/reward ads in your Unity mobile app.<br>

## Getting started
- [Download the latest sample project](https://github.com/bidmad/Bidmad-Unity/archive/master.zip)
- [Download the latest plugin](https://github.com/bidmad/Bidmad-Unity/releases)

### 1. Add Plugin
#### 1.1 Android

1. Import the latest downloaded SDK to the project.<br>
2. Check Use Gradle Daemon in Android Resolver Settings<br>
*Assets → External Dependency Manager → Android Resolver → Settings<br>
3. Run force resolve of External Dependency Manager.<br>
*If you are updating from a version earlier than 2.18.0, please refer to the [guide](https://github.com/bidmad/Bidmad-Unity/wiki/AOS-EDM4U-Migration-Guide) to remove the settings of the old version.<br>
4. Apps that target children and are vetted by the PlayStore require additional setup to use certified ad networks.<br> 
If your app is targeting children, check out our [guide](https://github.com/bidmad/Bidmad-Unity/wiki/PlayStore-%EC%95%B1-%ED%83%80%EA%B2%9F%ED%8C%85-%EC%97%B0%EB%A0%B9%EC%97%90-%EB%94%B0%EB%A5%B8-%EC%B6%94%EA%B0%80-%EC%84%A4%EC%A0%95.) for further setup.<br>
5. If you are using Proguard, add the rule below.
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

6. Activate Project Settings → Publish Settings → Custom Main Manifest and  Declare the code below inside the application tag of AndroidManifest.xml ([Guide](https://github.com/bidmad/SDK/wiki/Find-your-app-key%5BEN%5D#app-id-from-admob-dashboard))<br>
   *Please check the value of com.google.android.gms.ads.APPLICATION_ID on the Admob dashboard.

```xml
<application>
   ...
   <meta-data android:name="com.google.android.gms.ads.APPLICATION_ID" android:value="APPLICATION_ID"/>
   ...
</application>
```

7.  If targeting Android 12 version, please check [AD_ID Permission Guide](https://github.com/bidmad/Bidmad-Unity/wiki/AD_ID-Permission-Guide%5BENG%5D).

*Bidmad uses the AndroidX library. If it is not an AndroidX project, please migrate to AndroidX.

#### 1.2 iOS

*Bidmad supports Xcode 13.4 or higher. If your Xcode version is lower than 13.4, please update to 13.4 or higher.

1. Please import the latest plugin.<br>
2. Please make adjustments to BidmadPostProcessBuild.cs file in Assets → Bidmad → Editor.<br>
    Make sure to change User Tracking Usage Description and Google App ID. 
    You can get Google App ID by referring to the "App ID from ADMOB Dashboard" section inside the [Find your App Key](https://github.com/bidmad/SDK/wiki/Find-your-app-key%5BEN%5D) guide.<br><br>
    ![Bidmad-Guide-3](https://i.imgur.com/xPuJaSC.png)<br>
3. Please open the settings panel from Assets - External Dependency Manager - iOS Resolver - Settings.<br>
    ![Bidmad-Guide-4](https://i.imgur.com/8cvpZR0.png)<br>
    Please check and click the OK button on <strong>Link Frameworks Statically</strong> inside the settings panel.<br>
4. After building iOS Xcode Project, iOS Xcode Project folder will contain a project file with <strong>.xcworkspace</strong> extension. Please open it. <br>
5. Unity-iPhone Project Settings → Build Settings → UnityFramework Target → Set Enable Bitcode to "No".<br>
    ![Bidmad-Guide-4](https://i.imgur.com/cgCHNQA.png)<br>
6. Inside the Unity-iPhone Target Project Settings → General → Frameworks, Libraries, and Embedded Content, Click the + button inside and add OMSDK_Pubmatic.xcframework, ADOPUtility.xcframework, BidmadAdapterDynamic.xcframework, FBLPromises.framework, OMSDK_Teadstv.xcframework, TeadsSDK.xcframework.
    ![Bidmad-Guide-5](https://i.imgur.com/997NKID.png)<br>
7. Drag and drop Pods → Pods → AdFitSDK → Frameworks → AdFitSDK.framework into the Unity-iPhone target project settings → General → Frameworks, Libraries, and Embedded Content. Please refer to the GIF below.
    ![Bidmad-Guide-6](https://i.imgur.com/2ztRu9H.gif)<br>
8. Inside the Unity-iPhone Target Project Settings -> Build Phases, click the + button inside and click Add New Run Phase button.
    ![Bidmad-Guide-7](https://i.imgur.com/jlmk9sF.png)<br>
9. Copy and paste the following code into the Shell Script section of Run Script tab.
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
10. Follow the [guide](https://github.com/bidmad/Bidmad-Unity/wiki/Preparing-for-iOS-14%5BENG%5D) to apply app tracking transparency approval request pop-up. SKAdNetwork lists are included in BidmadPostProcessBuild.cs file.<br>

*If you're looking for a guide to the privacy requirements of the Apple Store, [see here](https://github.com/bidmad/Bidmad-Unity/wiki/Apple-privacy-survey%5BENG%5D).

#### 1.3 iOS Migration Guide (For Users migrating from 2.8.1 or under to the latest plugin)

1. Delete Assets → Plugins → iOS → Bidmad
2. Delete Assets → Resources → Bidmad
3. Delete Assets → Bidmad → Scripts
4. SKAdNetwork, Google App ID, User Tracking Usage Description Settings, which previously were set inside info.plist, are all moved to BidmadPostProcessBuild.cs (Please refer to the second step of 1.2 iOS Build Guide). Please set your App ID and User Tracking Usage Description in BidmadPostProcessBuild.cs file. It is not necessary for you to set SKAdNetwork as BidmadPostProcess is pre-set with needed SKAdNetworks. After setting and saving BidmadPostProcessBuild, there is no need for you to additionally set the info.plist. 
4. After following the steps above, please follow the steps in the section 1.2 iOS Build Guide.

### 2. Using Plugin

#### 2.1 Migration (updating from Bidmad Unity Plugin 2.21.0 or lower version to 3.0.0 version)
Prior to initial configuration of the app, when updating from version 2.21.0 or lower to version 3.0.0, please refer to [API Migration Guide](https://github.com/bidmad/Bidmad-Unity/wiki/v3.0.0-Migration-Guide) to update the app. After that, go through the process of adding the initializeSdk method below.<br>

#### 2.2 BidmadSDK Initialization
Performs tasks required to run BidmadSDK. The SDK won't allow ads to load unless you call the initializeSdk method.<br>
The initializeSdk method receives the App Key as a parameter, and the App Key can be copied from ADOP Insight. You can get the App Key by referring to the [Find your App Key](https://github.com/bidmad/SDK/wiki/Find-your-app-key%5BEN%5D) guide.<br>
Before loading ads, call the initializeSdk method as shown in the following example at the beginning of app.

```
#if UNITY_IOS
    BidmadCommon.initializeSdk("IOS APP KEY");
#elif UNITY_ANDROID
    BidmadCommon.initializeSdk("ANDROID APP KEY"); 
#endif
```

Or, if you are using Bidmad Plugin version 3.4.0 or later, you can check the initialization status by putting an Action function that receives a bool type as a parameter value as a parameter value of the initializeSdk method.

```
#if UNITY_IOS
    BidmadCommon.initializeSdk("IOS APP KEY", onInitialized);
#elif UNITY_ANDROID
    BidmadCommon.initializeSdk("ANDROID APP KEY", onInitialized);
#endif

void onInitialized(bool isComplete){
}
```

#### 2.3 Banner

- Create BidmadBanner to request banner advertisement. At this time, you must pass the height (y) value. 
- 
```cpp
    static BidmadBanner banner;

    public void LoadBannerAd()
    {
#if UNITY_ANDROID
        banner = new BidmadBanner("Your Android ZoneId", (float)0); // Explicitly specify the data type as float.
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

- If you are using Bidmad Plugin with 3.2.0 or higher version, setting the ad position instead of Y coordinate is also supported. The ad position values include Center, Top, Bottom, Left, Right, TopLeft, TopRight, BottomLeft, BottomRight. Refer to the following example for setting the ad position for your banner ads.

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

- If you are using Bidmad Plugin with 3.4.0 or higher version, After loading, it is possible to change the banner position.

```
    banner.load();
    // Reposition
    banner.updateViewPosition(0, 130);
```

#### 2.4 Interstitial

- Create BidmadInterstitial to request interstitial ad.
- Call the load method to request loading of an ad, and call the show method to display the loaded ad.
- If there is no ad loaded due to an ad loading failure, calling the show method will automatically trigger loading the advertisement again.
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

#### 2.5 Reward

- BidmadReward is created to request a reward ad.
- Call the load method to request loading of an ad, and call the show method to display the loaded ad.
- If there is no ad loaded due to an ad loading failure, calling the show method will automatically trigger loading the advertisement again.
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
#### 3.2 Interstitial Callback
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
#### 3.3 Reward Callback
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
#### 4.1 Banner

*Banner ads are handled through BidmadBanner and this is a list of functions for that.

Function|Description
---|---
public BidmadBanner(string zoneId, float _y)|This is the BidmadBanner constructor. set the ZoneId and banner height position value (y).
public BidmadBanner(string zoneId, float _x, float _y)|This is the BidmadBanner constructor, set the ZoneId and banner position x,y.
public BidmadBanner(string zoneId, AdPosition position);|This BidmadBanner constructor sets the ZoneId and Banner Position.
public void setRefreshInterval(int time)|Set the banner refresh cycle.(60s~120s)
public void removeBanner()|Remove the exposed banner.
public void load()|Request an ad with the ZoneId entered in the constructor.
public void updateViewPosition(float _y)|After loading, the banner view will be repositioned based on the bottom of the view, so that it is centered horizontally along the x value.
public void updateViewPosition(float _x, float _y)|After loading, the banner view will be repositioned based on the left of the view for the x value, and the bottom of the view for the y value.
public void updateViewPosition(AdPosition position)|After loading, the banner view will be repositioned based on AdPosition values
public void pauseBanner()|Banner ads are stopped. It is mainly called when the OnPause event occurs. Only Android is supported.
public void resumeBanner()|Restart banner ads. It is mainly called when the OnResume event occurs. Only Android is supported.
public void hideBannerView()|Hide the banner View. 
public void showBannerView()|Show the banner View.
public void setBannerLoadCallback(Action callback)|If an Action is registered, the registered Action is executed when the banner is loaded.
public void setBannerFailCallback(Action<string> callback)|If an Action is registered, the registered Action is executed when the banner load through ZoneId fails.
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
public void setInterstitialFailCallback(Action<string> callback)|If an Action is registered, the registered Action is executed when the load of interstitial ad through ZoneId fails.
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
public void setRewardLoadCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is loaded.
public void setRewardShowCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is shown.
public void setRewardFailCallback(Action<string> callback)|If an Action is registered, the registered Action is executed when the load of reward ad through ZoneId fails.
public void setRewardCompleteCallback(Action callback)|If an Action is registered, the registered Action is executed when the reward payment criteria of the reward ad are met.
public void setRewardSkipCallback(Action callback)|If an Action is registered, the registered Action is executed when the reward payment standard of the reward ad is not met.
public void setRewardCloseCallback(Action callback)|If an action is registered, the registered action is executed when the reward ad is closed.

#### 4.5 Other Interfaces

*Other Interfaces are included in BidmadCommon.

Function|Description
---|---
public static void initializeSdk(string appkey)|initialize the BidmadSDK configurations and preload the interstitial and reward ads.
public static void initializeSdkWithCallback(string appkey, Action<bool> callback)|initialize the BidmadSDK configurations and preload the interstitial and reward ads. Action<bool> receive Initialize Status
public static void setIsDebug(bool isDebug)|Debug logs are exposed.
public static void setGgTestDeviceid(string deviceId)|Test Device registration function for AdMob and AdManager.
public static void setCuid(string cuid)|a function to set the cuid(Customer User Identifier)
public static void setUseServerSideCallback(bool isServerSideCallback)|a function to enable Server Side Callbacks   
public static void setGdprConsent(bool consent, bool useArea)|Set Wether the user consented on GDPR. consent: User Consent / useArea: whether user is in EU region. 
public static int getGdprConsent(bool useArea)|Get GDPR Consent info
public static string getPRIVACYURL()|Get Bidmad Privacy URL 

#### 4.6 iOS14 AppTrackingTransparencyAuthorization

*AppTrackingTransparencyAuthorization functions are provided through BidmadCommon.

Function|Description
---|---
public static void reqAdTrackingAuthorization(Action<BidmadTrackingAuthorizationStatus> callback)| App Tracking Transparency Displays the approval request popup and passes the result to the callback.
public static void setAdvertiserTrackingEnabled(bool enable)| Set the result for app tracking transparency approval request pop-up consent/rejection obtained with a function other than reqAdTrackingAuthorization.
public static bool getAdvertiserTrackingEnabled()| Set app tracking transparency approval request popup inquires the result of consent/rejection.

----
### Reference

- [GDPR Guide](https://github.com/bidmad/Bidmad-Unity/wiki/Unity-GDPR-Guide-%5BENG%5D)
- [v3.0.0 Migration Guide](https://github.com/bidmad/Bidmad-Unity/wiki/v3.0.0-Migration-Guide)
