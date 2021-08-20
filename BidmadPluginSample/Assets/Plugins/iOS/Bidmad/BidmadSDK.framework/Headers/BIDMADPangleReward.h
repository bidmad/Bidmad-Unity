//
//  BIDMADPangleReward.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/06/25.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADRewardVideo.h"
#import <BUFoundation/BUFoundation.h>
#import <BUAdSDK/BUAdSDK.h>
#import <BUVAAuxiliary/BUVAAuxiliary.h>

#define PANGLE_REWARD_TEST_ID_VERTICAL @"946273162"
#define PANGLE_REWARD_TEST_ID_HORIZONTAL @"946273166"
#define PANGLE_REWARD_TEST_APPID @"5185174"

@interface BIDMADPangleReward : NSObject<BURewardedVideoAdDelegate>

@property (weak,nonatomic) BIDMADRewardVideo* rewardVideo;

- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *) userId;
- (void)loadPangleRewardVideo: (UIViewController *)pvc;
- (void)showPangleRewardVideo;
- (void)removeRewardAds;

@end
