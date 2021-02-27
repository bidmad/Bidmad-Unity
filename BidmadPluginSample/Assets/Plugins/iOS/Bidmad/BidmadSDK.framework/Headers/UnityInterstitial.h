//
//  UnityInterstitial.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/01/15.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#ifndef UnityInterstitial_h
#define UnityInterstitial_h

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"
#import "BIDMADInterstitial.h"

static NSMutableDictionary* BidmadInterstitialDic;

@interface UnityInterstitial : NSObject <BIDMADInterstitialDelegate>{
    NSString* mZoneId;
    id<BIDMADInterstitialDelegate> delegate;
    @public BIDMADInterstitial* bidmadInterstitial;
    UIViewController* mParentVC;
}

- (id)initWithZoneId:(NSString *)zoneId parentVC:(UIViewController *)parentVC;
+ (id)getInstance:(NSString *)zoneId;
- (void)setZoneID:(NSString *)zoneId;
- (void)setDelegate:(id) param;
- (void)load;
- (void)show;
- (bool)isLoaded;

@end

#endif /* UnityInterstitial_h */
