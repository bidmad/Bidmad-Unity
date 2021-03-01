//
//  UnrealCommon.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/02/24.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#ifndef UnrealCommon_h
#define UnrealCommon_h

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"

@protocol BIDMADUnrealCommonDelegate <NSObject>

@optional

- (void)BIDMADAdTrackingAuthorizationResponse:(int)response;

@end

@interface UnrealCommon : NSObject <BIDMADUnrealCommonDelegate>{
    id<BIDMADUnrealCommonDelegate> delegate;
}

+ (UnrealCommon*)sharedInstance;
- (void)setDelegate:(id)param;
- (void)setDebugMode:(BOOL)isDebug;
- (void)setAdvertiserTrackingEnabled:(BOOL)enable;
- (BOOL)getAdvertiserTrackingEnabled;
- (void)reqAdTrackingAuthorization;

@end

#endif /* UnrealCommon_h */
