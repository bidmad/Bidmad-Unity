//
//  BIDMADInterstitial.h
//  BIDMADSDK
//
//  Created by 김선정 on 2015. 7. 15..
//  Copyright (c) 2015년 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADUtil.h"
#import "BIDMADSetting.h"
#import "BIDMADFacebook.h"
#import "BIDMADAdmanager.h"
#import "BIDMADAdmob.h"
#import "BIDMADAtomInterstitial.h"

@protocol BIDMADInterstitialDelegate <NSObject>

@optional

- (void)BIDMADInterstitialAllFail:(BIDMADInterstitial *)core;

/// PREPARED INTERSTITIAL VIEW
- (void)BIDMADInterstitialLoad:(BIDMADInterstitial *)core;

/// ERROR
- (void)BIDMADInterstitialError:(BIDMADInterstitial *)core code:(NSString *)error;

/// CLOSE INTERSTITIAL VIEW
- (void)BIDMADInterstitialClose:(BIDMADInterstitial *)core;

/// SHOW INTERSTITIAL VIEW
- (void)BIDMADInterstitialShow:(BIDMADInterstitial *)core;

@end

@protocol BIDMADInterstitialInnerDelegate <NSObject>
@required

- (void)onInterstitialLoad:(BIDMADInterstitial *)core       current:(NSDictionary*) currentDic;
- (void)onInterstitialError:(NSString *)error failType:(NSString *)failType;
- (void)onInterstitialShow:(BIDMADInterstitial *)core       current:(NSDictionary*) currentDic;
- (void)onInterstitialClick:(BIDMADInterstitial *)core      current:(NSDictionary*) currentDic;
- (void)onInterstitialClose:(BIDMADInterstitial *)core      current:(NSDictionary*) currentDic;

@end


@interface BIDMADInterstitial : NSObject <BIDMADInterstitialInnerDelegate>

@property (nonatomic, strong) id<BIDMADInterstitialDelegate> delegate;

@property (nonatomic, strong) id<BIDMADInterstitialInnerDelegate> innerDelegate;

@property (nonatomic, strong) UIViewController* parentViewController;

@property (strong, nonatomic) NSDictionary*             ads_dic;
@property (nonatomic) BOOL                     isDirectLoad;

@property (strong, nonatomic) NSDictionary*                      ecmp_rev_info;
@property (strong, nonatomic) NSDictionary*                      area_info;

@property (strong, nonatomic) NSDictionary*                    change_info;
@property (strong, nonatomic) NSDictionary*                    date;

@property (nonatomic) NSString * zoneID;

@property (nonatomic) bool justLoading;

@property (nonatomic) int mediationNumber;

@property (nonatomic) BOOL                       isLabelService;
@property (nonatomic) BOOL                       isLabelServiceAdmin;

@property (nonatomic) NSString *                 realZoneId;

@property (nonatomic) BOOL                       isLoaded;

///inititalize
- (id)init;

///InterstitialView Load
- (void)loadInterstitialView;

///InterstitialView Show
- (void)showInterstitialView;

///InterstitialView Direct Show
- (void)directShowInterstitialView;

/// 삭제할것
- (void)selectAds:(NSDictionary *)lv_dic;

- (void)removeInterstitialADS;

- (void)sendLog :(NSDictionary *) info :(NSString *) advertisementType :(NSString *) logType;

- (void) sendLog :(NSDictionary *) info :(NSString *) advertisementType :(NSString *) logType :(NSString *)recvSessionId;

@end




