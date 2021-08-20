//
//  BIDMADUnityAdsBanner.h
//  BidmadSDK
//
//  Created by Seungsub Oh on 2021/07/20.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADBanner.h"

@interface BIDMADUnityAdsBanner : NSObject

@property (strong,nonatomic) BIDMADBanner* banner;
- (id)initWithAdBanner:(NSDictionary *)dic bidmadbanner:(BIDMADBanner *)banner;
- (void)showUnityBanner:(UIViewController *)pvc bannerContainer:(UIView*)bannerView;
- (void)removeBannerAds;
- (void)setGDPRData;

@end
