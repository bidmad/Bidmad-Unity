//
//  Cocos2dxInterstitial.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/04/27.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"
#import "BIDMADInterstitial.h"

@interface Cocos2dxInterstitial : NSObject <BIDMADInterstitialDelegate>{
    NSString* mZoneId;
    id<BIDMADInterstitialDelegate> delegate;
    @public BIDMADInterstitial* bidmadInterstitial;
}

- (id)initWithZoneId:(NSString *)zoneId;
- (void)setZoneID:(NSString *)zoneId;
- (void)setDelegate:(id) param;
- (void)load;
- (void)show;
- (bool)isLoaded;

@end
