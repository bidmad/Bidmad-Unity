//
//  UnityGDPRforGoogle.h
//  BidmadSDK
//
//  Created by ADOP_Mac on 2021/04/08.
//  Copyright © 2021 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADGDPRforGoogle.h"

@interface UnityGDPRforGoogle : NSObject

@property (strong,nonatomic) id<BIDMADGDPRforGoogleProtocol> GDPRforGoogleDelegate;

+ (UnityGDPRforGoogle *)sharedInstance;
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
