//
//  BIDMADAdx.h
//  BidmadSDK
//
//  Created by 김선정 on 2017. 6. 19..
//  Copyright © 2017년 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "BIDMADRewardVideo.h"
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADGDPR.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#import <AppLovinSDK/AppLovinSDK.h>


@class BIDMADRewardVideo;

@interface BIDMADAppLoving : NSObject <ALAdLoadDelegate, ALAdRewardDelegate, ALAdDisplayDelegate, ALAdVideoPlaybackDelegate>

@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;
@property (nonatomic, strong) ALIncentivizedInterstitialAd *incentivizedInterstitial;

- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *)userId;
- (void)showAppLovingRewardVideo:(UIViewController *)pvc;
- (void)gogoRewardVideo;
- (void)setGDPRData;
@end
