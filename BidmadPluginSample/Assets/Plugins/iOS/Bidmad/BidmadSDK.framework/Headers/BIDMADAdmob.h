//
//  BIDMADAdx.h
//  BidmadSDK
//
//  Created by 김선정 on 2017. 6. 19..
//  Copyright © 2017년 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "BIDMADBanner.h"
#import "BIDMADInterstitial.h"
#import "BIDMADRewardVideo.h"
#import "BIDMADRewardInterstitial.h"

#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#if __has_include("GoogleMobileAds/GoogleMobileAds.h")
#import "GoogleMobileAds/GoogleMobileAds.h"
#define GOOGLESDK_EXIST
#endif

#define REWARD_INTERSTITIAL_TEST_AD @"ca-app-pub-3940256099942544/6978759866"

@class BIDMADBanner;
@class BIDMADInterstitial;
@class BIDMADRewardVideo;
@class BIDMADAppOpenAd;
@class BIDMADRewardInterstitial;

#ifdef GOOGLESDK_EXIST
@interface BIDMADAdmob : NSObject<GADBannerViewDelegate, GADFullScreenContentDelegate>

#else
@interface BIDMADAdmob : NSObject
#endif

@property (weak,nonatomic) BIDMADBanner* banner;
@property (weak,nonatomic) BIDMADInterstitial* interstitial;
@property (weak,nonatomic) BIDMADRewardVideo* rewardVideo;

- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (id)initWithInterstitial:(NSDictionary *)dic bidmadInterstitial:(BIDMADInterstitial *)interstitial;
- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *) userId;
- (id)initWithRewardInterstitial:(NSDictionary *)dic bidmadInterstitial:(BIDMADRewardInterstitial *)rewardInterstitialArg;

- (void)showAdmobBanner:(UIViewController *)pvc bannerContainer:(UIView*)bannerContainer;
- (void)showAdmobInterstitial:(UIViewController *)pvc;
- (void)showAdmobRewardVideo: (UIViewController *)pvc;
- (void)loadAdmobRewardInterstitial:(UIViewController *)pvc;
- (void)showAdmobRewardInterstitial;

- (void)removeBannerAds;
- (void)removeInterstitialAds;
- (void)gogoInterstitial;
- (void)gogoRewardVideo;

- (void)setTestDevice:(NSString *) deviceId;
//- (void)setGDPRData;

@end
