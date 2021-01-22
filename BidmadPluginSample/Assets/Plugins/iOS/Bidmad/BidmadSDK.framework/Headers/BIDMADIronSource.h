//
//  BIDMADIronSource.h
//  BidmadSDK
//
//  Created by 전혜연 on 2020/08/12.
//  Copyright © 2020 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "BIDMADRewardVideo.h"
#import "BIDMADBanner.h"
#import "BIDMADInterstitial.h"
#import "BIDMADRewardVideo.h"
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADGDPR.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#import "IronSource.h"

@class BIDMADBanner;
@class BIDMADRewardVideo;
@class BIDMADInterstitial;

@interface BIDMADIronSource : NSObject <ISRewardedVideoDelegate, ISInterstitialDelegate, ISBannerDelegate>

@property (strong,nonatomic) BIDMADBanner* banner;
@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;
@property (strong,nonatomic) BIDMADInterstitial* interstitial;

- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (void)showIronsourceBanner:(UIViewController *)pvc bannerContainer:(UIView*)bannerView;
- (void)removeBannerAds;
- (id)initWithInterstitial:(NSDictionary *)dic bidmadInterstitial:(BIDMADInterstitial *)interstitial;
- (id)initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *)userId;
- (void)showIronSourceAdsRewardVideo:(UIViewController *)pvc;
- (void)showIronSourceInterstitialAds:(UIViewController *)pvc;
- (void)gogoRewardVideo;
- (void)setGDPRData;
- (void)showAdmobInterstitial:(UIViewController *)pvc;
- (void)gogoInterstitial;
- (void)gogoInterstitial:(UIViewController *) parentViewController;
- (void)removeLabel;


@end
