//
//  BIDMADAdx.h
//  BidmadSDK
//
//  Created by 김선정 on 2017. 6. 19..
//  Copyright © 2017년 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "BIDMADRewardVideo.h"
#import "BIDMADBanner.h"
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#import <AdColony/AdColony.h>


@class BIDMADRewardVideo;
@class BIDMADBanner;

@interface BIDMADAdColony : NSObject<AdColonyInterstitialDelegate, AdColonyAdViewDelegate>

@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;
@property (strong,nonatomic) BIDMADBanner* banner;

@property (nonatomic, weak) AdColonyAdView * bannerView;
@property (nonatomic, weak) AdColonyInterstitial *ad;
@property (nonatomic) bool  isRewardComplte;

- (id)  initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (id)  initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *)userId;

- (void)showAdColonyBanner:(UIViewController *)pvc bannerContainer:(UIView*)bannerView;
- (void)showAdColonyRewardVideo:(UIViewController *)pvc;


- (void)removeBannerAds;
- (void)gogoRewardVideo;
- (void)setCompleteBoolValue:(BOOL) isComplete;
- (void)setGDPRData;

@end
