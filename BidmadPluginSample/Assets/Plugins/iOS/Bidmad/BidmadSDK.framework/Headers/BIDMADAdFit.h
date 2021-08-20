//
//  BIDMADAdFit.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/05/27.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADBanner.h"

#if __has_include(<AdFitSDK/AdFitSDK.h>) || __has_include("AdFitSDK.h")
@import AdFitSDK;
#endif

@interface BIDMADAdFit : NSObject

@property (weak,nonatomic) BIDMADBanner* banner;

- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (void)showAdfitBanner:(UIViewController *)pvc bannerContainer:(UIView *)bannerContainer;
- (void)removeBannerAds;

@end
