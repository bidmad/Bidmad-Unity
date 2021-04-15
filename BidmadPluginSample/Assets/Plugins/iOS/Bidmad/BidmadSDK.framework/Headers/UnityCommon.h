//
//  UnityCommon.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/02/23.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#ifndef UnityCommon_h
#define UnityCommon_h

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"

@protocol BIDMADUnityCommonDelegate <NSObject>

@optional

- (void)BIDMADAdTrackingAuthorizationResponse:(NSString*)response;

@end

@interface UnityCommon : NSObject <BIDMADUnityCommonDelegate>{
    id<BIDMADUnityCommonDelegate> delegate;
}

+ (UnityCommon*)sharedInstance;
- (void)setDelegate:(id)param;
- (void)setDebugMode:(BOOL)isDebug;
- (void)setAdvertiserTrackingEnabled:(BOOL)enable;
- (BOOL)getAdvertiserTrackingEnabled;
- (void)reqAdTrackingAuthorization;
- (void)setGoogleTestId:(NSString*)testDeviceId;

@end

#endif /* UnityCommon_h */
