//
//  BIDMADRewardedInterstitial.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/05/20.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol BIDMADRewardInterstitialDelegate;
@protocol BIDMADRewardInterstitialInnerDelegate;

#pragma mark BIDMADRewardInterstitial Interface

    @interface BIDMADRewardInterstitial : NSObject<BIDMADRewardInterstitialInnerDelegate>
        
    @property (nonatomic, strong) id<BIDMADRewardInterstitialDelegate> delegate;
    @property (nonatomic, strong) NSDictionary *ads_dic;
    @property (nonatomic, strong) UIViewController *parentViewController;
    @property (nonatomic) NSString *zoneID;
    @property (nonatomic) BOOL isLoaded;

    - (void) requestRewardInterstitial;
    - (void) showRewardInterstitialView;
    - (void)removeRewardInterstitialAds;

    @end

#pragma mark BIDMADRewardInterstitial Delegate

    @protocol BIDMADRewardInterstitialDelegate <NSObject>
    @optional

    - (void)BIDMADRewardInterstitialAllFail:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialLoad:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialClose:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialShow:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialComplete:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialClick:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialSuccess:(BIDMADRewardInterstitial *)core;
    - (void)BIDMADRewardInterstitialSkipped:(BIDMADRewardInterstitial *) core;

    @end

#pragma mark BIDMADRewardInterstitial Inner-Delegate

    @protocol BIDMADRewardInterstitialInnerDelegate <NSObject>
    @required

        - (void)onRewardInterstitialLoad:(BIDMADRewardInterstitial *)core current:(NSDictionary*) currentDic;
        - (void)onRewardInterstitialError:(NSString *)error failType:(NSString *)failType;
        - (void)onRewardInterstitialShow:(BIDMADRewardInterstitial *)core current:(NSDictionary*) currentDic;
        - (void)onRewardInterstitialClick:(BIDMADRewardInterstitial *)core current:(NSDictionary*) currentDic;
        - (void)onRewardInterstitialSuccess:(BIDMADRewardInterstitial *)core current:(NSDictionary*) currentDic;
        - (void)onRewardInterstitialSkipped:(BIDMADRewardInterstitial *) core current:(NSDictionary*) currentDic;
        - (void)onRewardInterstitialClose:(BIDMADRewardInterstitial *)core current:(NSDictionary*) currentDic;

    @end
