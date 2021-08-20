//
//  BIDMADUnityAdsReward.h
//  BidmadSDK
//
//  Created by Seungsub Oh on 2021/07/20.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UnityAds/UnityAds.h>
#import "BIDMADRewardVideo.h"

static NSMutableArray* UnityAdsRewardArray;

@interface BIDMADUnityAdsReward : NSObject <UnityAdsDelegate>

@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;
@property (strong,nonatomic) UIViewController *parentViewController;

- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *) userId;
- (void)loadUnityAdsRewardVideo:(UIViewController *)pvc;
- (void)showUnityAdsRewardVideo;

@end
