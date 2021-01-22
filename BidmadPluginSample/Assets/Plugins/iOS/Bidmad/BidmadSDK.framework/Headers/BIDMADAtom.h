//
//  BIDMADAtom.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 4. 2..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>
#import "BIDMADBanner.h"

@interface BIDMADAtom : WKWebView<WKNavigationDelegate>

- (id)initWithAdDic:(NSDictionary *)dic;
- (void)showBannerAds:(BIDMADBanner *)pv parentViewController:(UIViewController *)pvc bannerContainer:(UIView *)parentUIView;

@property (strong,nonatomic) BIDMADBanner* banner;

@end
