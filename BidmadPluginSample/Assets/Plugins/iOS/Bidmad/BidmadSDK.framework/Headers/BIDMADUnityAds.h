//
//  BIDMADAdx.h
//  BidmadSDK
//
//  Created by 김선정 on 2017. 6. 19..
//  Copyright © 2017년 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "BIDMADBanner.h"
#import "BIDMADRewardVideo.h"
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADGDPR.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#import <UnityAds/UnityAds.h>


@class BIDMADRewardVideo;
@class BIDMADBanner;

@interface BIDMADUnityAds : NSObject <UnityAdsDelegate, UADSBannerViewDelegate>

@property (strong,nonatomic) BIDMADBanner* banner;
@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;

- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *)userId;

- (void)showUnityBanner:(UIViewController *)pvc bannerContainer:(UIView*)bannerView;
- (void)showUnityAdsRewardVideo:(UIViewController *)pvc;

- (void)removeBannerAds;
- (void)gogoRewardVideo;
- (void)setGDPRData;


@end
