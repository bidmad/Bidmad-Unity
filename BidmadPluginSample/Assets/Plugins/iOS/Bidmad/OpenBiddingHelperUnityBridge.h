//
//  OpenBiddingHelperUnityBridge.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 9. 13..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#ifndef OpenBiddingHelperUnityBridge_h
#define OpenBiddingHelperUnityBridge_h

#import "BidmadSDK/BIDMADGDPR.h"
#import "BidmadSDK/BIDMADSetting.h"
#import "BidmadSDK/UnityGDPRforGoogle.h"
#import <OpenBiddingHelper/BidmadBannerAdForGame.h>
#import <OpenBiddingHelper/BidmadInterstitialAdForGame.h>
#import <OpenBiddingHelper/BidmadRewardAdForGame.h>
#import <OpenBiddingHelper/BidmadAdFreeInformation.h>

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    void UnitySendMessage(const char* obj, const char* method, const char* msg);

    /*Banner*/
    void _bidmadSetRefreshInterval(const char* zoneId, int time);
    void _bidmadNewInstanceBannerAutoCenter(const char* zoneId, float _y);
    void _bidmadNewInstanceBanner(const char* zoneId, float _x, float _y);
    void _bidmadNewInstanceBannerAdPosition(const char* zoneId, int position);
    void _bidmadLoadBanner(const char* zoneId);
    void _bidmadRemoveBanner(const char* zoneId);
    void _bidmadHideBannerView(const char* zoneId);
    void _bidmadShowBannerView(const char* zoneId);
    void _bidmadUpdateBannerViewPositionAnchor(const char* zoneId, int position);
    void _bidmadUpdateBannerViewPositionXYCoordinate(const char* zoneId, float _x, float _y);
    void _bidmadUpdateBannerViewPositionYCoordinateAutoCenter(const char* zoneId, float _y);

    /*Interstitial*/
    void _bidmadNewInstanceInterstitial(const char* zoneId);
    void _bidmadLoadInterstitial(const char* zoneId);
    void _bidmadShowInterstitial(const char* zoneId);
    void _bidmadSetAutoReloadInterstitial(bool isAutoReload);
    bool _bidmadIsLoadedInterstitial(const char* zoneId);

    /*Reward*/
    void _bidmadNewInstanceReward(const char* zoneId);
    void _bidmadLoadRewardVideo(const char* zoneId);
    void _bidmadShowRewardVideo(const char* zoneId);
    bool _bidmadIsLoadedReward(const char* zoneId);
    void _bidmadSetAutoReloadRewardVideo(bool isAutoReload);

    /*ETC*/
    void _bidmadInitializeSdk(const char* appDomain);
    void _bidmadInitializeSdkWithCallback(const char* appDomain);
    void _bidmadSetDebug(bool isDebug);
    void _bidmadSetGgTestDeviceid(const char* _deviceId);
    void _bidmadSetUseArea(bool useArea);
    void _bidmadSetGDPRSetting(bool consent);
    int _bidmadGetGdprConsent();
    void _bidmadSetCuid(const char* cuid);
    void _bidmadSetUseServerSideCallback(bool isServerSideCallback);
    const char* _bidmadGetPRIVACYURL();
    void _bidmadSetAdFreeEventListener();
    bool _bidmadIsAdFree();

     /* ATT */
    void _bidmadReqAdTrackingAuthorization();
    void _bidmadSetAdvertiserTrackingEnabled(bool enable);
    bool _bidmadGetAdvertiserTrackingEnabled();

    /* GDPRforGoogle */
    void _bidmadGDPRforGoogleNewInstance();
    void _bidmadGDPRforGoogleSetListener();
    void _bidmadGDPRforGoogleSetDebug(const char* testDeviceId, bool isTestEurope);
    void _bidmadGDPRforGoogleRequestConsentInfoUpdate();
    bool _bidmadGDPRforGoogleIsConsentFormAvailable();
    void _bidmadGDPRforGoogleLoadForm();
    void _bidmadGDPRforGoogleShowForm();
    int _bidmadGDPRforGoogleGetConsentStatus();
    void _bidmadGDPRforGoogleReset();
    void _bidmadGDPRforGoogleSetDelegate();
    
#ifdef __cplusplus
}
#endif

#endif /* OpenBiddingHelperUnityBridge_h */
