//
//  BIDMADAdmobAppOpenAd.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/07/06.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GoogleMobileAds/GoogleMobileAds.h>
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADAppOpenAd.h"

#define APP_OPEN_TEST_APPID @"ca-app-pub-3940256099942544/5662855259"

@class BIDMADAppOpenAd;

@interface BIDMADAdmobAppOpenAd : NSObject<GADFullScreenContentDelegate>

@property (nonatomic, strong) NSString* sessionId;

- (id)initWithAppOpenAd:(NSDictionary *)dic bidmadAppOpenAd:(BIDMADAppOpenAd *)appOpenAd;
- (void)loadAdmobAppOpenAd;
- (void)showAppOpenAdOnVC: (UIViewController *)viewController;
- (void)setTestDevice;
- (void)removeAppOpenAds;

@end
