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
#import "BIDMADAppOpenAd.h"

#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#if __has_include("GoogleMobileAds/GoogleMobileAds.h")
#import "GoogleMobileAds/GoogleMobileAds.h"
#define GOOGLESDK_EXIST
#endif

@class BIDMADBanner;
@class BIDMADInterstitial;
@class BIDMADRewardVideo;
@class BIDMADAppOpenAd;

#ifdef GOOGLESDK_EXIST
@interface BIDMADAdmob : NSObject<GADBannerViewDelegate, GADFullScreenContentDelegate>

#else
@interface BIDMADAdmob : NSObject
#endif

@property (strong,nonatomic) BIDMADBanner* banner;
@property (strong,nonatomic) BIDMADInterstitial* interstitial;
@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;
@property (strong,nonatomic) BIDMADAppOpenAd * appOpenAd;

- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (id)initWithInterstitial:(NSDictionary *)dic bidmadInterstitial:(BIDMADInterstitial *)interstitial;
- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *) userId;
- (id)initWithAppOpenAd:(NSDictionary *)dic bidmadAppOpenAd:(BIDMADAppOpenAd *)appOpenAd;

- (void)showAdmobBanner:(UIViewController *)pvc bannerContainer:(UIView*)bannerContainer;
- (void)showAdmobInterstitial:(UIViewController *)pvc;
- (void)showAdmobRewardVideo: (UIViewController *)pvc;
- (void)showAdmobAppOpenAd: (UIViewController *)pvc;

- (void)removeBannerAds;
- (void)removeInterstitialAds;
- (void)gogoInterstitial;
- (void)gogoRewardVideo;
- (void)gogoAppOpenAd;

- (void)setTestDevice:(NSString *) deviceId;
//- (void)setGDPRData;

@end
