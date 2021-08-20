//
//  BIDMADTapjoy.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/02/05.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "BIDMADOfferwall.h"
#import "BIDMADSetting.h"
#import "BIDMADUtil.h"
#import "BIDMADGDPR.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#import <Tapjoy/Tapjoy.h>


@class BIDMADOfferwall;

@interface BIDMADTapjoy : NSObject <TJPlacementDelegate>

@property (strong,nonatomic) BIDMADOfferwall* offerwall;
@property (strong, nonatomic) TJPlacement *offerwallPlacement;

- (id)initWithOfferwall:(NSDictionary *)dic bidmadOfferwall:(BIDMADOfferwall *)offerwall;
- (void)initSDK:(NSDictionary *)dic bidmadOfferwall:(BIDMADOfferwall *)offerwall;
- (void)loadOfferwall:(UIViewController *)pvc;
- (void)showOfferwall;
- (void)getCurrency;
- (void)spendCurrency:(int)amount;
- (void)setGDPRData;
@end
