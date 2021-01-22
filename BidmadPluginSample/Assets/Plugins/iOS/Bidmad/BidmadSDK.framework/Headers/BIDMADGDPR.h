//
//  BIDMADGDPR.h
//  BidmadSDK
//
//  Created by 전혜연 on 23/06/2020.
//  Copyright © 2020 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADSetting.h"


@interface BIDMADGDPR : NSObject
/*
 1 : 사용자 정보 이용 동의
 0 : 사용자 정보 이용 미동의
-1 : 정보 동의 상태 알수없음.
-2 : 사용자 정보 이용 동의 필요없음.
*/

+ (void)setGDPRSetting:(bool) consent : (bool) useArea;

+ (NSInteger) getGDPRSetting:(bool) useArea;

+ (NSString*) getPRIVACYURL;

@end
