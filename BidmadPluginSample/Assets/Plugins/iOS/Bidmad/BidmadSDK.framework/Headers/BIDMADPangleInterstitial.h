//
//  BIDMADPangleInterstitial.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/06/25.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADInterstitial.h"
#import <BUFoundation/BUFoundation.h>
#import <BUAdSDK/BUAdSDK.h>
#import <BUVAAuxiliary/BUVAAuxiliary.h>

#define PANGLE_INTERSTITIAL_TEST_ID_VERTICAL @"946273141"
#define PANGLE_INTERSTITIAL_TEST_ID_HORIZONTAL @"946273143"
#define PANGLE_INTERSTITIAL_TEST_APPID @"5185174"

@interface BIDMADPangleInterstitial : NSObject<BUFullscreenVideoAdDelegate>

@property (weak,nonatomic) BIDMADInterstitial* interstitial;

- (id)initWithInterstitial:(NSDictionary *)dic bidmadInterstitial:(BIDMADInterstitial *)interstitial;
- (void)loadPangleInterstitial:(UIViewController *)pvc;
- (void)showPangleInterstitial;
- (void)removeInterstitialAds;

@end
