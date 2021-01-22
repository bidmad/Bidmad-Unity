//
//  UnityBanner.h
//  BidmadSDK
//
//  Created by Kenneth on 2021/01/18.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#ifndef UnityBanner_h
#define UnityBanner_h

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"
#import "BIDMADBanner.h"

NSMutableDictionary* bannerDic;

@interface UnityBanner : NSObject <BIDMADBannerDelegate>{
    NSString* mZoneId;
    id<BIDMADBannerDelegate> delegate;
    @public BIDMADBanner* bidmadBanner;
    UIViewController* mParentVC;
}

- (id)initWithZoneId:(NSString *)zoneId parentVC:(UIViewController *)parentVC adsPosition:(CGPoint)point bannerSize:(bannerSizeType)bannerTypeParam;
+ (id)getIntance:(NSString *)zoneId;
- (void)setZoneID:(NSString *)zoneId;
- (void)setDelegate:(id) param;
- (void)load;
- (void)remove;
- (void)setRefreshInterval:(int)sec;

@end

#endif /* UnityBanner_h */
