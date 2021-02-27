//
//  UnrealReward.h
//  BidmadSDK
//
//  Created by 전혜연 on 2020/12/08.
//  Copyright © 2020 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"
#import "BIDMADRewardVideo.h"

static NSMutableDictionary* BidmadUnrealRewardDic;

@interface UnrealReward : NSObject <BIDMADRewardVideoDelegate>{
    NSString* mZoneId;
    id<BIDMADRewardVideoDelegate> delegate;
    @public BIDMADRewardVideo* bidmadReward;
}
- (id)initWithZoneId:(NSString *)zoneId;
+ (id)getInstance:(NSString *)zoneId;
- (void)setZoneID:(NSString *)zoneId;
- (void)setDelegate:(id) param;
- (void)load;
- (void)show;
- (bool)isLoaded;

@end
