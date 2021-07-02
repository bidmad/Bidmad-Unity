//
//  Cocos2dxBanner.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/06/29.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADBanner.h"

@interface Cocos2dxBanner : NSObject<BIDMADBannerDelegate>

@property (nonatomic, strong) id<BIDMADBannerDelegate> delegate;
@property (nonatomic, strong) NSNumber *refreshInterval;

- (void)load:(int)y;
- (void)load:(int)x :(int)y;
- (void)setInterval:(int)interval;
- (void)removeBanner;
- (void)hideBannerView;
- (void)showBannerView;

- (void)BIDMADBannerAllFail:(BIDMADBanner *)core;
- (void)BIDMADBannerError:(BIDMADBanner *)core code:(NSString *)error;
- (void)BIDMADBannerClosed:(BIDMADBanner *)core;
- (void)BIDMADBannerLoad:(BIDMADBanner *)core;
- (void)BIDMADBannerClick:(BIDMADBanner*) core;

@end
