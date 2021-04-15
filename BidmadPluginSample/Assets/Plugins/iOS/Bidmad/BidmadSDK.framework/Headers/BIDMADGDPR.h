//
//  BIDMADGDPR.h
//  BidmadSDK
//
//  Created by 전혜연 on 23/06/2020.
//  Copyright © 2020 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"

typedef NS_ENUM(NSInteger, GDPRConsentStatus) {
    GDPRConsentStatusYES = 1,
    GDPRConsentStatusNO = 0,
    GDPRConsentStatusUNKNOWN = -1,
    GDPRConsentStatusUNUSE = -2
};

@interface BIDMADGDPR : NSObject

+ (void)setUseArea:(BOOL) useArea;
+ (void)setGDPRSetting:(BOOL) consent;
+ (NSInteger) getGDPRSetting;
+ (NSString*) getPRIVACYURL;

@end
