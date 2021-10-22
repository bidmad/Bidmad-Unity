//
//  OpenBiddingHelperUnityBridge.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 9. 13..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#ifndef OpenBiddingHelperUnityBridge_h
#define OpenBiddingHelperUnityBridge_h

#import "BidmadSDK/UnityCommon.h"
#import "BidmadSDK/BIDMADGDPR.h"
#import "BidmadSDK/BIDMADSetting.h"
#import "BidmadSDK/UnityGDPRforGoogle.h"
#import <OpenBiddingHelper/OpenBiddingUnityReward.h>
#import <OpenBiddingHelper/OpenBiddingUnityInterstitial.h>
#import <OpenBiddingHelper/OpenBiddingUnityBanner.h>
#import <OpenBiddingHelper/OpenBiddingUnityRewardInterstitial.h>

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    void UnitySendMessage(const char* obj, const char* method, const char* msg);

    /*Banner*/
    void _bidmadSetRefreshInterval(const char* zoneId, int time);
    void _bidmadNewInstanceBanner(const char* zoneId, float _x, float _y);
    void _bidmadNewInstanceBannerAutoCenter(const char* zoneId, float _y);
    void _bidmadLoadBanner(const char* zoneId);
    void _bidmadRemoveBanner(const char* zoneId);
    void _bidmadHideBannerView(const char* zoneId);
    void _bidmadShowBannerView(const char* zoneId);

    /*Interstitial*/
    void _bidmadNewInstanceInterstitial(const char* zoneId);
    void _bidmadLoadInterstitial(const char* zoneId);
    void _bidmadShowInterstitial(const char* zoneId);
    bool _bidmadIsLoadedInterstitial(const char* zoneId);

    /*Reward*/
    void _bidmadNewInstanceReward(const char* zoneId);
    void _bidmadLoadRewardVideo(const char* zoneId);
    void _bidmadShowRewardVideo(const char* zoneId);
    bool _bidmadIsLoadedReward(const char* zoneId);

    /*RewardInterstitial*/
    void _bidmadNewInstanceRewardInterstitial(const char* zoneId);
    void _bidmadLoadRewardInterstitial(const char* zoneId);
    void _bidmadShowRewardInterstitial(const char* zoneId);
    bool _bidmadIsLoadedRewardInterstitial(const char* zoneId);

    /*ETC*/
    void _bidmadSetDebug(bool isDebug); 
    void _bidmadSetGgTestDeviceid(const char* deviceId);
    void _bidmadSetUseArea(bool useArea);
    void _bidmadSetGDPRSetting(bool consent);
    int _bidmadGetGdprConsent();

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
