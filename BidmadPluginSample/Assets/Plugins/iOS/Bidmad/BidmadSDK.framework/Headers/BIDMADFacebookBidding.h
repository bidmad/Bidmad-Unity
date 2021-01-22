//
//  BIDMADFacebookBidding.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 8. 21..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>


#if __has_include("FBAudienceNetwork/FBAudienceNetwork.h")
#import "FBAudienceNetwork/FBAudienceNetwork.h"
#define FBSDK_EXIST
#endif


#if __has_include("FBAudienceNetworkBiddingKit/FBAudienceNetworkBiddingKit.h")
#import "FBAudienceNetworkBiddingKit/FBAudienceNetworkBiddingKit.h"
#define FBBIDDINGSDK_EXIST
#endif


#import "BIDMADBanner.h"
#import "BIDMADInterstitial.h"
#import "BIDMADRewardVideo.h"

@class BIDMADBanner;
@class BIDMADInterstitial;
@class BIDMADRewardVideo;

#ifdef FBSDK_EXIST
@interface BIDMADFacebookBidding : NSObject<FBAdViewDelegate,FBInterstitialAdDelegate,FBRewardedVideoAdDelegate>
#else
@interface BIDMADFacebookBidding : NSObject
#endif

- (id)initWithFBAds:(NSDictionary *)dic bidmadBanner:(BIDMADBanner *)banner;
- (id)initWithFBInterstital:(NSDictionary *)dic bidmadInterstitial:(BIDMADInterstitial *)interstitial;
- (id)initWithFBRewardVideo:(NSDictionary *)dic bidmadRewardVideo:(BIDMADRewardVideo *)rewardVideo;
- (void)setBidding:(UIViewController *)pvc;
- (void)setInterstitialBidding:(UIViewController *)pvc;
- (void)setRewardVideoBidding :(UIViewController *)pvc;
- (void)removeAdsbidding;
- (void)gogoInterstitial;
- (void)gogoRewardVideo;

@property(strong, nonatomic) NSString*          bidPayload;
@property(nonatomic) CGFloat                    bidPrice;
#ifdef FBSDK_EXIST
@property(strong, nonatomic)FBAdView*           fbAdView;
@property(strong, nonatomic)FBInterstitialAd*   interstitialAds;
@property(strong, nonatomic)FBRewardedVideoAd*  rewardAds;
@property(strong, nonatomic)FBAdBidResponse*    bidResponse;
#endif
@property(strong, nonatomic)BIDMADBanner*         banner;
@property(strong, nonatomic)BIDMADInterstitial*   interstitial;
@property(strong, nonatomic)BIDMADRewardVideo*    rewardVideo;

@end
