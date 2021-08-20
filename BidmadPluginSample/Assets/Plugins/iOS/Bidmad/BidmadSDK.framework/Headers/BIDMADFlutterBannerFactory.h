#import "BIDMADFlutterCommon.h"

#if __has_include(<Flutter/Flutter.h>) || __has_include("Flutter.h")
@import Flutter;

@interface BIDMADFlutterBannerFactory : NSObject <FlutterPlatformViewFactory>
#else
@interface BIDMADFlutterBannerFactory : NSObject
#endif

//- (instancetype)initWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar;
- (instancetype)initWithRegistrar:(id)registrar;
@end
