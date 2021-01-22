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
#import "BidmadSDK/BIDMADGDPR.h"

#ifdef __cplusplus
extern "C" {
#endif
    UIViewController* UnityGetGLViewController();
    void UnitySendMessage(const char* obj, const char* method, const char* msg);

    /*Banner*/
    void _setRefreshInterval(const char* zoneId, int time);
    void _newInstanceBanner(const char* zoneId, float _x, float _y);
    void _loadBanner(const char* zoneId);
    void _removeBanner(const char* zoneId);

    /*Interstitial*/
    void _newInstanceInterstitial(const char* zoneId);
    void _loadInterstitial(const char* zoneId);
    void _showInterstitial(const char* zoneId);
    bool _isLoadedInterstitial(const char* zoneId);

    /*Reward*/
    void _newInstanceReward(const char* zoneId);
    void _loadRewardVideo(const char* zoneId);
    void _showRewardVideo(const char* zoneId);
    bool _isLoadedReward(const char* zoneId);

    /*ETC*/
    void _setIsDebug(bool isDebug); 
    void _setTestMode(bool isDebug);
    void _setGgTestDeviceid(const char* deviceId);
    void _setGdprConsent(bool consent, bool useArea);
    int  _getGdprConsent(bool useArea);
    
#ifdef __cplusplus
}
#endif

#endif /* BidmadUnityBridge_h */
