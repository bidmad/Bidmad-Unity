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

@interface OpenBiddingHelperUnityBridge : NSObject<BIDMADOpenBiddingBannerDelegate,BIDMADOpenBiddingInterstitialDelegate,BIDMADOpenBiddingRewardVideoDelegate, OpenBiddingRewardInterstitialDelegate, BIDMADUnityCommonDelegate, BIDMADGDPRforGoogleProtocol>
+ (OpenBiddingHelperUnityBridge *)sharedInstance;
@end

@implementation OpenBiddingHelperUnityBridge

+ (OpenBiddingHelperUnityBridge *)sharedInstance{
    DEFINE_SHARED_INSTANCE_USING_BLOCK(^{
        return [[self alloc]init];
    });
}

/** Banner Callback Start **/
-(void)BIDMADOpenBiddingBannerLoad:(OpenBiddingBanner *)core{
    UnitySendMessage("BidmadManager", "OnBannerLoad", [core.zoneID UTF8String]);
}

- (void)BIDMADOpenBiddingBannerAllFail:(OpenBiddingBanner *)core{
    UnitySendMessage("BidmadManager", "OnBannerFail", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingBannerClick:(OpenBiddingBanner *)core{
    UnitySendMessage("BidmadManager", "OnBannerClick", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingBannerClosed:(OpenBiddingBanner *)core {
    
}

/** Banner Callback End **/
/** Interstitial Callback Start **/

-(void)BIDMADOpenBiddingInterstitialLoad:(OpenBiddingInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialLoad", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingInterstitialShow:(OpenBiddingInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialShow", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingInterstitialClose:(OpenBiddingInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialClose", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingInterstitialAllFail:(OpenBiddingInterstitial *)core{
    UnitySendMessage("BidmadManager", "OnInterstitialFail", [core.zoneID UTF8String]);
}

/** Interstitial Callback End **/
/** Reward Callback Start **/

-(void)BIDMADOpenBiddingRewardVideoLoad:(OpenBiddingRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardLoad", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingRewardVideoShow:(OpenBiddingRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardShow", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingRewardVideoAllFail:(OpenBiddingRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardFail", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingRewardVideoSucceed:(OpenBiddingRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardComplete", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingRewardSkipped:(OpenBiddingRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardSkip", [core.zoneID UTF8String]);
}

-(void)BIDMADOpenBiddingRewardVideoClose:(OpenBiddingRewardVideo *)core
{
    UnitySendMessage("BidmadManager", "OnRewardClose", [core.zoneID UTF8String]);
}

/** Reward Callback End **/

#pragma mark RewardInterstitial Callback

- (void)OpenBiddingRewardInterstitialAllFail:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialAllFail");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialAllFail", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialLoad:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialLoad");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialLoad", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialClose:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialClose");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialClose", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialShow:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialShow");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialShow", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialComplete:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialComplete");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialComplete", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialClick:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialClick");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialClick", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialSuccess:(OpenBiddingRewardInterstitial *)core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialSuccess");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialComplete", [core.zoneID UTF8String]);
}
- (void)OpenBiddingRewardInterstitialSkipped:(OpenBiddingRewardInterstitial *) core {
    NSLog(@"OpenBiddingHelper Unity Bridge Callback → OpenBiddingRewardInterstitialSkipped");
    UnitySendMessage("BidmadManager", "OnRewardInterstitialSkipped", [core.zoneID UTF8String]);
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
    OpenBiddingUnityBanner* banner = [OpenBiddingUnityBanner getInstance:_zoneID];
    [banner setRefreshInterval:time];
}

void _bidmadNewInstanceBannerAutoCenter(const char* zoneId, float _y) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UIViewController* pRootViewController = UnityGetGLViewController();
    OpenBiddingUnityBanner *banner = [[OpenBiddingUnityBanner alloc] initWithZoneId:_zoneID parentVC:pRootViewController adYPoint:(int)_y];
    [banner setDelegate:[OpenBiddingHelperUnityBridge sharedInstance]];
}

void _bidmadNewInstanceBanner(const char* zoneId, float _x, float _y) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UIViewController* pRootViewController = UnityGetGLViewController();
    OpenBiddingUnityBanner* banner = [[OpenBiddingUnityBanner alloc]initWithZoneId:_zoneID parentVC:pRootViewController adsPosition:CGPointMake(_x,_y)];
    [banner setDelegate:[OpenBiddingHelperUnityBridge sharedInstance]];
}

void _bidmadLoadBanner(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    OpenBiddingUnityBanner* banner = [OpenBiddingUnityBanner getInstance:_zoneID];
    [banner load];
}

void _bidmadRemoveBanner(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    OpenBiddingUnityBanner* banner = [OpenBiddingUnityBanner getInstance:_zoneID];
    [banner remove];
}

void _bidmadHideBannerView(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    OpenBiddingUnityBanner* banner = [OpenBiddingUnityBanner getInstance:_zoneID];
    [banner hideView];
}

void _bidmadShowBannerView(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    OpenBiddingUnityBanner* banner = [OpenBiddingUnityBanner getInstance:_zoneID];
    [banner showView];
}
/** Banner Interface End **/
/** Interstitial Interface Start **/
void _bidmadNewInstanceInterstitial(const char* zoneId) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UIViewController* pRootViewController = UnityGetGLViewController();
    OpenBiddingUnityInterstitial* interstitial = [[OpenBiddingUnityInterstitial alloc]initWithZoneId:_zoneID parentVC:pRootViewController];
    [interstitial setDelegate:[OpenBiddingHelperUnityBridge sharedInstance]];
}

void _bidmadLoadInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityInterstitial* interstitial = [OpenBiddingUnityInterstitial getInstance:_zoneID];
    
    if(![interstitial isLoaded]){
        [interstitial load];
    }
}

void _bidmadShowInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityInterstitial* interstitial = [OpenBiddingUnityInterstitial getInstance:_zoneID];
    
    if([interstitial isLoaded]){
        [interstitial show];
    }
    
}

void _bidmadSetAutoReloadInterstitial(const char* zoneId, bool isAutoReload) {
    NSString *_zoneId = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityInterstitial *openBiddingUnityInterstitial = [OpenBiddingUnityInterstitial getInstance:_zoneId];
    [openBiddingUnityInterstitial setAutoReload:isAutoReload];
}

bool _bidmadIsLoadedInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityInterstitial* interstitial = [OpenBiddingUnityInterstitial getInstance:_zoneID];
    
    return [interstitial isLoaded];
}
/** Interstitial Interface End **/
/** Reward Interface Start **/
void _bidmadNewInstanceReward(const char* zoneId) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];

    UIViewController* pRootViewController = UnityGetGLViewController();
    OpenBiddingUnityReward *reward = [[OpenBiddingUnityReward alloc]initWithZoneId:_zoneID parentVC:pRootViewController];
    [reward setDelegate:[OpenBiddingHelperUnityBridge sharedInstance]];
}

void _bidmadLoadRewardVideo(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityReward *reward = [OpenBiddingUnityReward getInstance:_zoneID];

    if(![reward isLoaded]){
        [reward load];
    }

}

void _bidmadShowRewardVideo(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityReward *reward = [OpenBiddingUnityReward getInstance:_zoneID];
    
    if([reward isLoaded]){
        [reward show];
    }
}

bool _bidmadIsLoadedReward(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityReward *reward = [OpenBiddingUnityReward getInstance:_zoneID];
    
    return [reward isLoaded];
}

void _bidmadSetAutoReloadRewardVideo(const char* zoneId, bool isAutoReload) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    OpenBiddingUnityReward *reward = [OpenBiddingUnityReward getInstance:_zoneID];
    [reward setAutoReload:isAutoReload];
}
/** Reward Interface End **/
#pragma mark RewardInterstitial Interface

void _bidmadNewInstanceRewardInterstitial(const char* zoneId) {
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    
    UIViewController* pRootViewController = UnityGetGLViewController();
    [[OpenBiddingUnityRewardInterstitial sharedInstance] openBiddingNewInstanceRewardInterstitial:_zoneID withDelegate:[OpenBiddingHelperUnityBridge sharedInstance] parentVC:pRootViewController];
}

void _bidmadLoadRewardInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    [[OpenBiddingUnityRewardInterstitial sharedInstance] openBiddingLoadRewardInterstitial: _zoneID];
}

void _bidmadShowRewardInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    [[OpenBiddingUnityRewardInterstitial sharedInstance] openBiddingShowRewardInterstitial: _zoneID];
}

bool _bidmadIsLoadedRewardInterstitial(const char* zoneId){
    NSString* _zoneID = [NSString stringWithUTF8String:zoneId];
    return [[OpenBiddingUnityRewardInterstitial sharedInstance] openBiddingIsLoadedRewardInterstitial: _zoneID];
}

void _bidmadSetAutoReloadRewardInterstitial(const char* zoneId, bool isAutoReload) {
    [[OpenBiddingUnityRewardInterstitial sharedInstance] setAutoReload:isAutoReload];
}

/** ETC Interface Start **/
void _bidmadSetDebug(bool isDebug) {
    [[UnityCommon sharedInstance] setDebugMode:isDebug];
}

void _bidmadSetGgTestDeviceid(const char* _deviceId){
    NSString* deviceId = [NSString stringWithUTF8String:_deviceId];
    [[UnityCommon sharedInstance] setGoogleTestId:deviceId];
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

const char* _bidmadGetPRIVACYURL() {
    NSString *privacyUrl = [BIDMADGDPR getPRIVACYURL];
    return strdup([privacyUrl UTF8String]);
}

typedef void (*CallbackT)(const char *foo);
// extern "C" void method(CallbackT callback);
void _bidmadReqAdTrackingAuthorization()
{
    [[UnityCommon sharedInstance] setDelegate:[OpenBiddingHelperUnityBridge sharedInstance]];
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
    [[UnityGDPRforGoogle sharedInstance] setDelegate: [OpenBiddingHelperUnityBridge sharedInstance]];
}

/** GDPRforGoogle End **/

