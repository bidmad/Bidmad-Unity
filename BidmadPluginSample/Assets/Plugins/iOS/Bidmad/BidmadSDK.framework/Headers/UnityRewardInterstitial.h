//
//  UnityRewardInterstitial.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/07/09.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"
#import "BIDMADRewardInterstitial.h"

@interface UnityRewardInterstitial : NSObject<BIDMADRewardInterstitialDelegate>

+ (id)sharedInstance;

- (void)bidmadNewInstanceRewardInterstitial:(NSString *)zoneId withDelegate:(id<BIDMADRewardInterstitialDelegate>)delegate;
- (void)bidmadLoadRewardInterstitial:(NSString *)zoneId;
- (void)bidmadShowRewardInterstitial:(NSString *)zoneId;
- (bool)bidmadIsLoadedRewardInterstitial:(NSString *)zoneId;

@end
