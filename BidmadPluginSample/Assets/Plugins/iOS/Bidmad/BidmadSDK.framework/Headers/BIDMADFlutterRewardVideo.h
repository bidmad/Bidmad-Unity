//
//  BIDMADFlutterRewardVideo.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/03/26.
//

#import <Foundation/Foundation.h>
#import "BIDMADRewardVideo.h"
#import "BIDMADSetting.h"

#if __has_include(<Flutter/Flutter.h>) || __has_include("Flutter.h")
@import Flutter;
#endif

@interface BIDMADFlutterRewardVideo : NSObject<BIDMADRewardVideoDelegate>

@property (strong, nonatomic) id flutterChannel;
- (id)initRewardVideo:(id)registrar channelName:(NSString*)chanNm;

@end
