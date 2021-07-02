//
//  Cocos2dxReward.h
//  BidmadSdk
//
//  Created by Kenneth on 2021/04/27.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"
#import "BIDMADRewardVideo.h"

@interface Cocos2dxReward : NSObject <BIDMADRewardVideoDelegate>{
    NSString* mZoneId;
    id<BIDMADRewardVideoDelegate> delegate;
    @public BIDMADRewardVideo* bidmadReward;
}

- (id)initWithZoneId:(NSString *)zoneId;
- (void)setZoneID:(NSString *)zoneId;
- (void)setDelegate:(id) param;
- (void)load;
- (void)show;
- (bool)isLoaded;

@end

