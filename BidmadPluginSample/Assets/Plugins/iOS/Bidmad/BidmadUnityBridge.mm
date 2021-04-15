//
//  BIDMADUnityBridge.m
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

#import "BidmadUnityBridge.h"

@interface BidmadUnityBridge : NSObject<BIDMADBannerDelegate,BIDMADInterstitialDelegate,BIDMADRewardVideoDelegate, BIDMADUnityCommonDelegate, BIDMADGDPRforGoogleProtocol>
+ (BidmadUnityBridge *)sharedInstance;
@end

@implementation BidmadUnityBridge

+ (BidmadUnityBridge *)sharedInstance{
    DEFINE_SHARED_INSTANCE_USING_BLOCK(^{
        return [[self alloc]init];
    });
}

/** Banner Callback Start **/
-(void)BIDMADBannerLoad:(BIDMADBanner *)core{
    UnitySendMessage("BidmadManager", "OnBannerLoad", [core.zoneID UTF8String]);
}

- (void)BIDMADBannerAllFail:(BIDMADBanner *)core{
    UnitySendMessage("BidmadManager", "OnBannerFail", [core.zoneID UTF8String]);
}

-(void)BIDMADBannerClick:(BIDMADBanner *)core{
    UnitySendMessage("BidmadManager", "OnBannerClick", [core.zoneID UTF8String]);
}


/** Banner Callback End **/
/** Interstitial Callback Start **/

-(void)BIDMADInterstitialLoad:(BIDMADInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialLoad", [core.zoneID UTF8String]);
}

-(void)BIDMADInterstitialShow:(BIDMADInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialShow", [core.zoneID UTF8String]);
}

- (void)BIDMADInterstitialAllFail:(BIDMADInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialFail", [core.zoneID UTF8String]);
}

-(void)BIDMADInterstitialClose:(BIDMADInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialClose", [core.zoneID UTF8String]);
}

/** Interstitial Callback End **/
/** Reward Callback Start **/

-(void)BIDMADRewardVideoLoad:(BIDMADRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardLoad", [core.zoneID UTF8String]);
}

- (void)BIDMADRewardVideoShow:(BIDMADRewardVideo *)core{
    UnitySendMessage("BidmadManager", "OnRewardShow", [core.zoneID UTF8String]);
}

- (void)BIDMADRewardVideoAllFail:(BIDMADRewardVideo *)core{
    UnitySendMessage("BidmadManager", "OnRewardFail", [core.zoneID UTF8String]);
}

-(void)BIDMADRewardVideoSucceed:(BIDMADRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardComplete", [core.zoneID UTF8String]);
}

-(void)BIDMADRewardSkipped:(BIDMADRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardSkip", [core.zoneID UTF8String]);
}

-(void)BIDMADRewardVideoClose:(BIDMADRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardClose", [core.zoneID UTF8String]);
}

/** Reward Callback End **/
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

static NSString* __testDeviceId = nil;

/** Banner Interface Start **/
void _bidmadSetRefreshInterval(const char* zoneId, int time){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityBanner* banner = [UnityBanner getIntance:_zoneID]; 
    [banner setRefreshInterval:time];
}

void _bidmadNewInstanceBanner(const char* zoneId, float _x, float _y) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UIViewController* pRootViewController = UnityGetGLViewController();
    UnityBanner* banner = [[UnityBanner alloc]initWithZoneId:_zoneID parentVC:pRootViewController adsPosition:CGPointMake(_x,_y) bannerSize: banner_320_50];
    [banner setDelegate:[BidmadUnityBridge sharedInstance]];
}

void _bidmadLoadBanner(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    UnityBanner* banner = [UnityBanner getIntance:_zoneID];
    [banner load];
}

void _bidmadRemoveBanner(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    UnityBanner* banner = [UnityBanner getIntance:_zoneID];
    [banner remove];
}

void _bidmadHideBannerView(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    UnityBanner* banner = [UnityBanner getIntance:_zoneID];
    [banner hideView];
}

void _bidmadShowBannerView(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    UnityBanner* banner = [UnityBanner getIntance:_zoneID];
    [banner showView];
}

/** Banner Interface End **/
/** Interstitial Interface Start **/
void _bidmadNewInstanceInterstitial(const char* zoneId) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UIViewController* pRootViewController = UnityGetGLViewController();
    UnityInterstitial* interstitial = [[UnityInterstitial alloc]initWithZoneId:_zoneID parentVC:pRootViewController];
    [interstitial setDelegate:[BidmadUnityBridge sharedInstance]];
}

void _bidmadLoadInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityInterstitial* interstitial = [UnityInterstitial getInstance:_zoneID];
    
    if(![interstitial isLoaded]){
        [interstitial load];
    }
}

void _bidmadShowInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityInterstitial* interstitial = [UnityInterstitial getInstance:_zoneID];
    
    if([interstitial isLoaded]){
        [interstitial show];
    }
    
}

bool _bidmadIsLoadedInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityInterstitial* interstitial = [UnityInterstitial getInstance:_zoneID];
    
    return [interstitial isLoaded];
}
/** Interstitial Interface End **/
/** Reward Interface Start **/
void _bidmadNewInstanceReward(const char* zoneId) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UnityReward *reward = [[UnityReward alloc]initWithZoneId:_zoneID];
    [reward setDelegate:[BidmadUnityBridge sharedInstance]];
}

void _bidmadLoadRewardVideo(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityReward *reward = [UnityReward getInstance:_zoneID];

    if(![reward isLoaded]){
        [reward load];
    }

}

void _bidmadShowRewardVideo(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityReward *reward = [UnityReward getInstance:_zoneID];
    
    if([reward isLoaded]){
        [reward show];
    }
}

bool _bidmadIsLoadedReward(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    UnityReward *reward = [UnityReward getInstance:_zoneID];
    
    return [reward isLoaded];
}
/** Reward Interface End **/
/** ETC Interface Start **/
void _bidmadSetDebug(bool isDebug) {
    [[UnityCommon sharedInstance] setDebugMode:isDebug];
}

void _bidmadSetGgTestDeviceid(const char* _deviceId){
    NSString* deviceId = [NSString stringWithUTF8String:_deviceId];
    __testDeviceId = deviceId;
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

typedef void (*CallbackT)(const char *foo);
// extern "C" void method(CallbackT callback);
void _bidmadReqAdTrackingAuthorization()
{
    [[UnityCommon sharedInstance] setDelegate:[BidmadUnityBridge sharedInstance]];
    [[UnityCommon sharedInstance] reqAdTrackingAuthorization];
}
void _bidmadSetAdvertiserTrackingEnabled(bool enable)
{
    [[UnityCommon sharedInstance] setAdvertiserTrackingEnabled:enable];
}

bool _bidmadGetAdvertiserTrackingEnabled()
{
    return [[UnityCommon sharedInstance] getAdvertiserTrackingEnabled];
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
    [[UnityGDPRforGoogle sharedInstance] setDelegate: [BidmadUnityBridge sharedInstance]];
}

/** GDPRforGoogle End **/