//
//  BIDMADOfferwall.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/02/05.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADUtil.h"
#import "BIDMADSetting.h"
#import "BIDMADTapjoy.h"

@protocol BIDMADOfferwallDelegate;
@protocol BIDMADOfferwallInnerDelegate;

@interface BIDMADOfferwall : NSObject <BIDMADOfferwallInnerDelegate>

@property (nonatomic, strong) id<BIDMADOfferwallDelegate> delegate;
@property (nonatomic, strong) id<BIDMADOfferwallInnerDelegate> innerDelegate;

@property (nonatomic, strong) UIViewController* parentViewController;
@property (strong, nonatomic) NSDictionary* ads_dic;
@property (strong, nonatomic) NSDictionary* ecmp_rev_info;
@property (strong, nonatomic) NSDictionary* area_info;
@property (strong, nonatomic) NSDictionary* change_info;
@property (strong, nonatomic) NSDictionary* date;

@property (nonatomic) NSString * zoneID;
@property (nonatomic) bool justLoading;
@property (nonatomic) int mediationNumber;
@property (nonatomic) BOOL isLabelService;
@property (nonatomic) BOOL isLabelServiceAdmin;
@property (nonatomic) NSString* realZoneId;
@property (nonatomic) BOOL isLoaded;

- (id)initWithZoneId:(NSString *)zoneId;
+ (BOOL)isSDKInit;
- (void)loadOfferwall;
- (void)showOfferwall;
- (void)getCurrencyBalance;
- (void)spendCurrency:(int)amount;
- (void)selectAds:(NSDictionary *)lv_dic;
- (void)sendLog :(NSDictionary *) info :(NSString *) advertisementType :(NSString *) logType;

@end

@protocol BIDMADOfferwallDelegate <NSObject>
@optional

- (void)BIDMADOfferwallInitSuccess:(BIDMADOfferwall *)core;
- (void)BIDMADOfferwallInitFail:(BIDMADOfferwall *)core error:(NSString *)error;

- (void)BIDMADOfferwallLoadAd:(BIDMADOfferwall *)core;
- (void)BIDMADOfferwallShowAd:(BIDMADOfferwall *)core;
- (void)BIDMADOfferwallFailedAd:(BIDMADOfferwall *)core;
- (void)BIDMADOfferwallCloseAd:(BIDMADOfferwall *)core;

- (void)BIDMADOfferwallGetCurrencyBalanceSuccess:(BIDMADOfferwall *)core currencyName:(NSString *)currencyName balance:(int)balance;
- (void)BIDMADOfferwallGetCurrencyBalanceFail:(BIDMADOfferwall *)core error:(NSString *)error;
- (void)BIDMADOfferwallSpendCurrencySuccess:(BIDMADOfferwall *)core currencyName:(NSString *)currencyName balance:(int)balance;
- (void)BIDMADOfferwallSpendCurrencyFail:(BIDMADOfferwall *)core error:(NSString *)error;

@end

@protocol BIDMADOfferwallInnerDelegate <NSObject>
@required

- (void)onInitSuccess:(BIDMADOfferwall *)core;
- (void)onInitFail:(BIDMADOfferwall *)core error:(NSString *)error;

- (void)onLoadAd:(BIDMADOfferwall *)core current:(NSDictionary *)currentDic;
- (void)onShowAd:(BIDMADOfferwall *)core current:(NSDictionary *)currentDic;
- (void)onFailedAd:(BIDMADOfferwall *)core code:(NSString *)error failType:(NSString *)failType current:(NSDictionary *)currentDic passbackStr:(NSString *)passBackStr passback:(NSDictionary *)passbackDic;
- (void)onCloseAd:(BIDMADOfferwall *)core current:(NSDictionary *)currentDic;

- (void)onGetCurrencyBalanceSuccess:(BIDMADOfferwall *)core currencyName:(NSString *)currencyName balance:(int)balance;
- (void)onGetCurrencyBalanceFail:(BIDMADOfferwall *)core error:(NSString *)error;
- (void)onSpendCurrencySuccess:(BIDMADOfferwall *)core currencyName:(NSString *)currencyName balance:(int)balance;
- (void)onSpendCurrencyFail:(BIDMADOfferwall *)core error:(NSString *)error;

@end

