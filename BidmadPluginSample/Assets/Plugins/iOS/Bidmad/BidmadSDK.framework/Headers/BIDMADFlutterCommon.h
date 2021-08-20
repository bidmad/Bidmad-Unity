//
//  BIDMADFlutterCommon.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/03/25.
//

#import <Foundation/Foundation.h>

#if __has_include(<Flutter/Flutter.h>) || __has_include("Flutter.h")
@import Flutter;
#endif

NS_ASSUME_NONNULL_BEGIN

@interface BIDMADFlutterCommon : NSObject

//- (void)initChannel: (NSObject<FlutterPluginRegistrar>*)registrar;
- (void)initChannel: (id)registrar;

//@property (strong, nonatomic) FlutterMethodChannel* flutterChannel;
@property (strong, nonatomic) id flutterChannel;

@property (strong, nonatomic) NSString * channelName;

@end

NS_ASSUME_NONNULL_END
