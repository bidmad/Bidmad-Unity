//
//  BIDMADFlutter-GDPR-UMP.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/04/06.
//

#import <Foundation/Foundation.h>
#import <UserMessagingPlatform/UserMessagingPlatform.h>
#import "BIDMADGDPRforGoogle.h"

#if __has_include(<Flutter/Flutter.h>) || __has_include("Flutter.h")
@import Flutter;
#endif

@interface BIDMADFlutterGDPRforGoogle : NSObject<BIDMADGDPRforGoogleProtocol>

@property (strong, nonatomic) id flutterChannel;
//- (id)initGDPRforGoogle:(NSObject<FlutterPluginRegistrar>*)registrar channelName:(NSString*)chanNm;
- (id)initGDPRforGoogle:(id)registrar channelName:(NSString*)chanNm;

@end
