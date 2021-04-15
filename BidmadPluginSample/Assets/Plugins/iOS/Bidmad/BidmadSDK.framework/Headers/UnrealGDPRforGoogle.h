//
//  UnrealGDPRforGoogle.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/04/13.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADGDPRforGoogle.h"

NS_ASSUME_NONNULL_BEGIN

@interface UnrealGDPRforGoogle : NSObject

@property (strong,nonatomic) id<BIDMADGDPRforGoogleProtocol> GDPRforGoogleDelegate;

+ (UnrealGDPRforGoogle *)sharedInstance;
- (id)initGDPRforGoogle;
- (void)setListener;
- (void)setDebug:(NSString*)testDeviceId isTestEurope:(BOOL)testEurope;
- (void)requestConsentInfoUpdate;
- (BOOL)isConsentFormAvailable;
- (void)loadForm;
- (NSNumber*)getConsentStatus;
- (void)reset;
- (void)showForm;
- (void)setDelegate:(id<BIDMADGDPRforGoogleProtocol>)delegate;

@end

NS_ASSUME_NONNULL_END
