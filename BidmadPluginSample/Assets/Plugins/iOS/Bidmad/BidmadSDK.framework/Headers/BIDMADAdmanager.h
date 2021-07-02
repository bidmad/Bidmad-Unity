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
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADLabelView.h"
#if __has_include("GoogleMobileAds/GoogleMobileAds.h")
#import "GoogleMobileAds/GoogleMobileAds.h"
#define GOOGLESDK_EXIST
#endif

@class BIDMADBanner;
@class BIDMADInterstitial;
@class BIDMADRewardVideo;

#ifdef GOOGLESDK_EXIST
@interface BIDMADAdmanager : NSObject<GADBannerViewDelegate, GADFullScreenContentDelegate>

#else
@interface BIDMADAdmanager : NSObject
#endif
@property (weak,nonatomic) BIDMADBanner* banner;
@property (weak,nonatomic) BIDMADInterstitial* interstitial;
@property (weak,nonatomic) BIDMADRewardVideo* rewardVideo;

- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (id)initWithInterstitial:(NSDictionary *)dic bidmadInterstitial:(BIDMADInterstitial *)interstitial;
- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo;
- (void)showAdmanagerBanner:(UIViewController *)pvc bannerContainer:(UIView *)bannerContainer;
- (void)showAdmanagerInterstitial:(UIViewController *)pvc;
- (void)showAdmanagerRewardVideo:(UIViewController *)pvc;
- (void)removeBannerAds;
- (void)removeInterstitialAds;
- (void)gogoInterstitial;
- (void)gogoInterstitial:(UIViewController *) parentViewController;
- (void)gogoRewardVideo;
- (void)setTestDevice:(NSString *)deviceId;
//- (void)setGDPRData;

@end
