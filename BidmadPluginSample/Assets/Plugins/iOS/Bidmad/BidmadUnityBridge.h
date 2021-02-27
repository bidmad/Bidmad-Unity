//
//  BidmadUnityBridge.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 9. 13..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#ifndef BidmadUnityBridge_h
#define BidmadUnityBridge_h

#import "BidmadSDK/UnityBanner.h"
#import "BidmadSDK/UnityInterstitial.h"
#import "BidmadSDK/UnityReward.h"
#import "BidmadSDK/UnityCommon.h"
#import "BidmadSDK/BIDMADGDPR.h"
#import "BidmadSDK/BIDMADSetting.h"

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    void UnitySendMessage(const char* obj, const char* method, const char* msg);

    /*Banner*/
    void _bidmadSetRefreshInterval(const char* zoneId, int time);
    void _bidmadNewInstanceBanner(const char* zoneId, float _x, float _y);
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

    /*ETC*/
    void _bidmadSetDebug(bool isDebug); 
    void _bidmadSetGgTestDeviceid(const char* deviceId);
    void _bidmadSetGdprConsent(bool consent, bool useArea);
    int _bidmadGetGdprConsent(bool useArea);

    void _bidmadReqAdTrackingAuthorization();
    void _bidmadSetAdvertiserTrackingEnabled(bool enable);
    bool _bidmadGetAdvertiserTrackingEnabled();
    
#ifdef __cplusplus
}
#endif

#endif /* BidmadUnityBridge_h */
