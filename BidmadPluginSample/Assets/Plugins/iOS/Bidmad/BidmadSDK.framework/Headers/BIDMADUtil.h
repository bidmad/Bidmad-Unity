//
//  BIDMADUtil.h
//  BidmadSDK
//
//  Created by 김선정 on 2015. 7. 20..
//  Copyright (c) 2015년 ADOP Co., Ltd. All rights reserved.

#import <UIKit/UIKit.h>
#import <Foundation/Foundation.h>
#import <CoreGraphics/CoreGraphics.h>
#import "BIDMADSetting.h"
#import "BIDMADBanner.h"
#import "BIDMADInterstitial.h"
#import "BIDMADRewardVideo.h"
#import <SystemConfiguration/CaptiveNetwork.h>
#import <CoreLocation/CoreLocation.h>
#define MB (1024*1024)
#define GB (MB*1024)

@interface BIDMADUtil : NSObject

+ (NSString *)identifierForAdvertising;

/// IMAGE RESIZING
+ (UIImage *)imageWithImage:(UIImage *)image scaledToSize:(CGSize)newSize;

/// GET DEVICE IP ADDRESS
+ (NSString *)getIPAddress;

/// KEYWORD RETURN ARRAY
+ (NSArray *) keywordReturnArr:(NSString *)keywords;

+ (UIImage *) getImageFromURL:(NSString *)fileURL;

+ (NSString*) deviceName;

+ (NSString *) getNetworkName:(NSString *)networkKey;

+ (id) nullCheck:(id)checkData;

+ (BOOL)isNull:(NSString*)str;

+ (void)setTimer;

+ (float)callTime;

+ (NSString*) getDeviceName;
+ (void)bidmadAlert:(NSString *)title message:(NSString *)message action:(NSString *)action;
+ (NSString *)getBundle:(NSString *)file type:(NSString *)type;

+ (NSString *)memoryFormatter:(long long)diskSpace;

+ (NSString *)totalDiskSpace;

+ (NSString *)freeDiskSpace;

+ (NSDictionary*) DataCounter;

+ (NSString *)GetCurrentWifiHotSpotName;

+ (UIColor *)colorFromHexString:(NSString *)hexString;

+ (BOOL) isEmpty:(NSString *)str;


+ (NSInteger) iOSMajorSystemVersion;

+ (UIWindow*) fetchKeyWindow;

+ (BOOL) isIphoneX;

+ (BOOL) isOverIOS11;

+ (NSString *) getCurrentRegionInfo;

+ (UIViewController*) topMostController;

+ (NSData*)bidmadResourceDataFetcherWithAssetName: (NSString*)assetName extension: (NSString*)extension;

+ (void)compassDataFetcherWithZoneId: (NSString *)zoneId completionBlock:(void (^)(NSDictionary *))completionBlock;

+ (void) sendLog :(NSDictionary *)info :(NSString *)advertisementType :(NSString *)logType :(NSString*)sessionId :(NSString*)realZoneId;

+ (void) sendLog :(NSDictionary *)info : (NSString *)advertisementType : (NSString *)logType : (NSString *)recvSessionId;

@end
