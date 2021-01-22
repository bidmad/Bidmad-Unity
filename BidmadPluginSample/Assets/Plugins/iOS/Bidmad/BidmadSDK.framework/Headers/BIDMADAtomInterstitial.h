//
//  BIDMADAtomInterstitial.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 4. 5..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BIDMADInterstitial.h"
#import <WebKit/WebKit.h>

@interface BIDMADAtomInterstitial : UIViewController<WKNavigationDelegate>
- (id)initWithAdDic:(NSDictionary *)dic;
- (void)loadInterstitialAds:(UIViewController *)pvc;
- (void)removeInterstitialAds;
- (void)showInterstitialAds;
@property (strong,nonatomic) BIDMADInterstitial* interstitial;
@property (nonatomic) UIInterfaceOrientation startingInterfaceOrientation;
@end
