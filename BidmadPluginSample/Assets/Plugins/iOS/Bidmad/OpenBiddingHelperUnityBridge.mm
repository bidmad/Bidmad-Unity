//
//  OpenBiddingHelperUnityBridge.m
//  BidmadSDK
//
//  Created by 김선정 on 2018. 8. 10..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//
#define DEFINE_SHARED_INSTANCE_USING_BLOCK(block) \
static dispatch_once_t pred = 0; \
__strong static id _sharedObject = nil; \
dispatch_once(&pred, ^{ \
_sharedObject = block(); \
}); \
return _sharedObject; \

#import "OpenBiddingHelperUnityBridge.h"

@interface OpenBiddingHelperUnityBridge : NSObject<BIDMADOpenBiddingBannerDelegate,BIDMADOpenBiddingInterstitialDelegate,BIDMADOpenBiddingRewardVideoDelegate, BIDMADGDPRforGoogleProtocol>
+ (OpenBiddingHelperUnityBridge *)sharedInstance;
@end

@implementation OpenBiddingHelperUnityBridge

+ (OpenBiddingHelperUnityBridge *)sharedInstance{
    DEFINE_SHARED_INSTANCE_USING_BLOCK(^{
        return [[self alloc]init];
    });
}

- (void)onLoadAd:(id)bidmadAd {
    if ([bidmadAd isKindOfClass:OpenBiddingBanner.class]) {
        UnitySendMessage("BidmadManager", "OnBannerLoad", [(OpenBiddingBanner *)bidmadAd zoneID].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingInterstitial.class]) {
        UnitySendMessage("BidmadManager", "OnInterstitialLoad", [(OpenBiddingInterstitial *)bidmadAd zoneID].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        UnitySendMessage("BidmadManager", "OnRewardLoad", [(OpenBiddingRewardVideo *)bidmadAd zoneID].UTF8String);
    }
}

- (void)onLoadFailAd:(id)bidmadAd error:(NSError *)error {
    if ([bidmadAd isKindOfClass:OpenBiddingBanner.class]) {
        UnitySendMessage("BidmadManager", "OnBannerLoadFail", [NSString stringWithFormat:@"%@+[code: %ld][message: %@]", [(OpenBiddingBanner *)bidmadAd zoneID], error.code, error.localizedDescription].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingInterstitial.class]) {
        UnitySendMessage("BidmadManager", "OnInterstitialLoadFail", [NSString stringWithFormat:@"%@+[code: %ld][message: %@]", [(OpenBiddingInterstitial *)bidmadAd zoneID], error.code, error.localizedDescription].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        UnitySendMessage("BidmadManager", "OnRewardLoadFail", [NSString stringWithFormat:@"%@+[code: %ld][message: %@]", [(OpenBiddingRewardVideo *)bidmadAd zoneID], error.code, error.localizedDescription].UTF8String);
    }
}

- (void)onClickAd:(id)bidmadAd {
    if ([bidmadAd isKindOfClass:OpenBiddingBanner.class]) {
        UnitySendMessage("BidmadManager", "OnBannerClick", [(OpenBiddingBanner *)bidmadAd zoneID].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingInterstitial.class]) {
        /* Click Callback Unsupported by Unity Engine */
    } else if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        /* Click Callback Unsupported by Unity Engine */
    }
}

- (void)onCloseAd:(id)bidmadAd {
    if ([bidmadAd isKindOfClass:OpenBiddingBanner.class]) {
        /* Close Callback Unsupported by Unity Engine */
    } else if ([bidmadAd isKindOfClass:OpenBiddingInterstitial.class]) {
        UnitySendMessage("BidmadManager", "OnInterstitialClose", [(OpenBiddingInterstitial *)bidmadAd zoneID].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        UnitySendMessage("BidmadManager", "OnRewardClose", [(OpenBiddingRewardVideo *)bidmadAd zoneID].UTF8String);
    }
}

- (void)onShowAd:(id)bidmadAd {
    if ([bidmadAd isKindOfClass:OpenBiddingInterstitial.class]) {
        UnitySendMessage("BidmadManager", "OnInterstitialShow", [(OpenBiddingInterstitial *)bidmadAd zoneID].UTF8String);
    } else if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        UnitySendMessage("BidmadManager", "OnRewardShow", [(OpenBiddingRewardVideo *)bidmadAd zoneID].UTF8String);
    }
}

- (void)onCompleteAd:(id)bidmadAd {
    if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        UnitySendMessage("BidmadManager", "OnRewardComplete", [(OpenBiddingRewardVideo *)bidmadAd zoneID].UTF8String);
    }
}

- (void)onSkipAd:(id)bidmadAd {
    if ([bidmadAd isKindOfClass:OpenBiddingRewardVideo.class]) {
        UnitySendMessage("BidmadManager", "OnRewardSkip", [(OpenBiddingRewardVideo *)bidmadAd zoneID].UTF8String);
    }
}

/** Common Callback Start **/

- (void)BIDMADAdTrackingAuthorizationResponse:(NSString*)response
{
    NSLog(@"BIDMADAdTrackingAuthorizationResponse");
    UnitySendMessage("BidmadManager", "OnAdTrackingAuthorizationResponse", [response UTF8String]);
}

/** Common Callback End **/
/** GDPRforGoogle Callback Start **/
 
// "Error [Message : " + formError.getMessage() + "][Code : " + formError.getErrorCode() + "]"

- (NSString *)errorArgumentGenerator:(NSError *)formError {
    NSString *errorArg;
    
    if (formError) {
        NSNumber *errorCode = [NSNumber numberWithLong:formError.code];
        NSString *errorDescription = formError.localizedDescription;
        errorArg = [NSString stringWithFormat:@"Error [Message : %@][Code : %@]", errorDescription, errorCode.stringValue];
    } else {
        errorArg = [NSString stringWithFormat:@"Error [Message : %@][Code : %@]", @"No Error was Found.", [[NSNumber numberWithInt:0] stringValue]];
    }
    return errorArg;
}

- (void)onConsentFormDismissed:(NSError *)formError {
    NSString *errorArg = [self errorArgumentGenerator:formError];
    
    UnitySendMessage("BidmadManager", "onConsentFormDismissed", [errorArg UTF8String]);
    
}

- (void)onConsentFormLoadFailure:(NSError *)formError {
    NSString *errorArg = [self errorArgumentGenerator:formError];
    
    UnitySendMessage("BidmadManager", "onConsentFormLoadFailure", [errorArg UTF8String]);
    
}

- (void)onConsentFormLoadSuccess {
    UnitySendMessage("BidmadManager", "onConsentFormLoadSuccess", "");
}

- (void)onConsentInfoUpdateFailure:(NSError *)formError {
    NSString *errorArg = [self errorArgumentGenerator:formError];
    
    UnitySendMessage("BidmadManager", "onConsentInfoUpdateFailure", [errorArg UTF8String]);
    
}

- (void)onConsentInfoUpdateSuccess {
    UnitySendMessage("BidmadManager", "onConsentInfoUpdateSuccess", "");
}

/** GDPRforGoogle Callback End **/

@end

/** Banner Interface Start **/
void _bidmadSetRefreshInterval(const char* zoneId, int time){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    [BidmadBannerAdForGame setRefreshInterval:time withZoneID:_zoneID];
}

void _bidmadNewInstanceBannerAutoCenter(const char* zoneId, float _y) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UIViewController* pRootViewController = UnityGetGLViewController();
    [BidmadBannerAdForGame initialSetupForZoneID:_zoneID viewController:pRootViewController yCoordinate:_y];
    [BidmadBannerAdForGame setDelegate:OpenBiddingHelperUnityBridge.sharedInstance];
}

void _bidmadNewInstanceBanner(const char* zoneId, float _x, float _y) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UIViewController* pRootViewController = UnityGetGLViewController();
    [BidmadBannerAdForGame initialSetupForZoneID:_zoneID viewController:pRootViewController xCoordinate:_x yCoordinate:_y];
    [BidmadBannerAdForGame setDelegate:OpenBiddingHelperUnityBridge.sharedInstance];
}

void _bidmadNewInstanceBannerAdPosition(const char* zoneId, int position) {
    NSString *_zoneID = [NSString stringWithUTF8String:zoneId];
    UIViewController *pRootViewController = UnityGetGLViewController();
    [BidmadBannerAdForGame initialSetupForZoneID:_zoneID viewController:pRootViewController adPosition:(BIDMADAdPosition)position];
    [BidmadBannerAdForGame setDelegate:OpenBiddingHelperUnityBridge.sharedInstance];
}

void _bidmadLoadBanner(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    [BidmadBannerAdForGame loadWithZoneID:_zoneID];
}

void _bidmadRemoveBanner(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    [BidmadBannerAdForGame removeWithZoneID:_zoneID];
}

void _bidmadHideBannerView(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    dispatch_async(dispatch_get_main_queue(), ^{
        [BidmadBannerAdForGame hideWithZoneID:_zoneID];
    });
}

void _bidmadShowBannerView(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    dispatch_async(dispatch_get_main_queue(), ^{
        [BidmadBannerAdForGame showWithZoneID:_zoneID];
    });
}

void _bidmadUpdateBannerViewPositionAnchor(const char* zoneId, int position) {
    [BidmadBannerAdForGame initialSetupForZoneID:[NSString stringWithUTF8String:zoneId] viewController:UnityGetGLViewController() adPosition:(BIDMADAdPosition)position];
    [BidmadBannerAdForGame updateViewPositionWithZoneID:[NSString stringWithUTF8String:zoneId]];
}

void _bidmadUpdateBannerViewPositionXYCoordinate(const char* zoneId, float _x, float _y) {
    [BidmadBannerAdForGame initialSetupForZoneID:[NSString stringWithUTF8String:zoneId] viewController:UnityGetGLViewController() xCoordinate:_x yCoordinate:_y];
    [BidmadBannerAdForGame updateViewPositionWithZoneID:[NSString stringWithUTF8String:zoneId]];
}

void _bidmadUpdateBannerViewPositionYCoordinateAutoCenter(const char* zoneId, float _y) {
    [BidmadBannerAdForGame initialSetupForZoneID:[NSString stringWithUTF8String:zoneId] viewController:UnityGetGLViewController() yCoordinate:_y];
    [BidmadBannerAdForGame updateViewPositionWithZoneID:[NSString stringWithUTF8String:zoneId]];
}

/** Banner Interface End **/
/** Interstitial Interface Start **/
void _bidmadNewInstanceInterstitial(const char* zoneId) {
    [BidmadInterstitialAdForGame initialSetupForZoneID:[NSString stringWithUTF8String:zoneId]];
    [BidmadInterstitialAdForGame setDelegate:OpenBiddingHelperUnityBridge.sharedInstance];
}

void _bidmadLoadInterstitial(const char* zoneId){
    [BidmadInterstitialAdForGame loadWithZoneID:[NSString stringWithUTF8String:zoneId]];
}

void _bidmadShowInterstitial(const char* zoneId){
    [BidmadInterstitialAdForGame showWithZoneID:[NSString stringWithUTF8String:zoneId] viewController:UnityGetGLViewController()];
}

void _bidmadSetAutoReloadInterstitial(bool isAutoReload) {
    [BidmadInterstitialAdForGame setAutoReload:isAutoReload];
}

bool _bidmadIsLoadedInterstitial(const char* zoneId){
    return [BidmadInterstitialAdForGame isLoadedWithZoneID:[NSString stringWithUTF8String:zoneId]];
}
/** Interstitial Interface End **/
/** Reward Interface Start **/
void _bidmadNewInstanceReward(const char* zoneId) {
    [BidmadRewardAdForGame initialSetupForZoneID:[NSString stringWithUTF8String:zoneId]];
    [BidmadRewardAdForGame setDelegate:[OpenBiddingHelperUnityBridge sharedInstance]];
}

void _bidmadLoadRewardVideo(const char* zoneId){
    [BidmadRewardAdForGame loadWithZoneID:[NSString stringWithUTF8String:zoneId]];
}

void _bidmadShowRewardVideo(const char* zoneId){
    if ([BidmadRewardAdForGame isLoadedWithZoneID:[NSString stringWithUTF8String:zoneId]]) {
        UnitySetAudioSessionActive(true);
    }
    
    [BidmadRewardAdForGame showWithZoneID:[NSString stringWithUTF8String:zoneId] viewController:UnityGetGLViewController()];
}

bool _bidmadIsLoadedReward(const char* zoneId){
    return [BidmadRewardAdForGame isLoadedWithZoneID:[NSString stringWithUTF8String:zoneId]];
}

void _bidmadSetAutoReloadRewardVideo(bool isAutoReload) {
    [BidmadRewardAdForGame setAutoReload:isAutoReload];
}
/** Reward Interface End **/

/** ETC Interface Start **/
void _bidmadInitializeSdk(const char* appKey) {
    NSString * _appKey = [NSString stringWithUTF8String:appKey];
    [BIDMADSetting.sharedInstance initializeSdkWithKey:_appKey];
}

void _bidmadInitializeSdkWithCallback(const char* appKey) {
    NSString * _appKey = [NSString stringWithUTF8String:appKey];
    [BIDMADSetting.sharedInstance initializeSdkWithKey:_appKey completionHandler:^(BOOL initStatus) {
        if (initStatus) {
            UnitySendMessage("BidmadManager", "OnInitialized", "true");
        } else {
            UnitySendMessage("BidmadManager", "OnInitialized", "false");
        }
    }];
}

void _bidmadSetDebug(bool isDebug) {
    BIDMADSetting.sharedInstance.isDebug = isDebug;
}

void _bidmadSetGgTestDeviceid(const char* _deviceId){
    NSString* deviceId = [NSString stringWithUTF8String:_deviceId];
    BIDMADSetting.sharedInstance.testDeviceId = deviceId;
}

void _bidmadSetUseArea(bool useArea){
    [BIDMADGDPR setUseArea:useArea];
}

void _bidmadSetGDPRSetting(bool consent) {
    [BIDMADGDPR setGDPRSetting: consent];
}

int _bidmadGetGdprConsent(){
    return ((int)[BIDMADGDPR getGDPRSetting]);
}

void _bidmadSetCuid(const char* cuid) {
    NSString* _cuid = [NSString stringWithUTF8String:cuid];
    BIDMADSetting.sharedInstance.cuid = _cuid;
}

void _bidmadSetUseServerSideCallback(bool isServerSideCallback) {
    BIDMADSetting.sharedInstance.useServerSideCallback = [NSNumber numberWithBool:isServerSideCallback];
}

const char* _bidmadGetPRIVACYURL() {
    NSString *privacyUrl = [BIDMADGDPR getPRIVACYURL];
    return strdup([privacyUrl UTF8String]);
}

void _bidmadReqAdTrackingAuthorization()
{
    [[BIDMADSetting sharedInstance] reqAdTrackingAuthorizationWithCompletionHandler:^(BidmadTrackingAuthorizationStatus status) {
        [OpenBiddingHelperUnityBridge.sharedInstance BIDMADAdTrackingAuthorizationResponse:[NSString stringWithFormat:@"%d",status]];
    }];
}
void _bidmadSetAdvertiserTrackingEnabled(bool enable)
{
    [BIDMADSetting.sharedInstance setAdvertiserTrackingEnabled:enable];
}

bool _bidmadGetAdvertiserTrackingEnabled()
{
    return [BIDMADSetting.sharedInstance getAdvertiserTrackingEnabled];
}
/** ETC Interface End **/
/** GDPRforGoogle Start **/

void _bidmadGDPRforGoogleNewInstance(){
    [UnityGDPRforGoogle sharedInstance];
}

void _bidmadGDPRforGoogleSetListener(){
    [[UnityGDPRforGoogle sharedInstance] setListener];
}

void _bidmadGDPRforGoogleSetDebug(const char* testDeviceId, bool isTestEurope){
    [[UnityGDPRforGoogle sharedInstance] setDebug: [NSString stringWithUTF8String:testDeviceId] isTestEurope: isTestEurope];
}

void _bidmadGDPRforGoogleRequestConsentInfoUpdate(){
    [[UnityGDPRforGoogle sharedInstance] requestConsentInfoUpdate];
}

bool _bidmadGDPRforGoogleIsConsentFormAvailable(){
    return [[UnityGDPRforGoogle sharedInstance] isConsentFormAvailable];
}

void _bidmadGDPRforGoogleLoadForm(){
    [[UnityGDPRforGoogle sharedInstance] loadForm];
}

void _bidmadGDPRforGoogleShowForm(){
    [[UnityGDPRforGoogle sharedInstance] showForm];
}

int _bidmadGDPRforGoogleGetConsentStatus(){
    return [[[UnityGDPRforGoogle sharedInstance] getConsentStatus] intValue];
}

void _bidmadGDPRforGoogleReset(){
    [[UnityGDPRforGoogle sharedInstance] reset];
}

void _bidmadGDPRforGoogleSetDelegate(){
    [[UnityGDPRforGoogle sharedInstance] setDelegate: [OpenBiddingHelperUnityBridge sharedInstance]];
}

/** GDPRforGoogle End **/

