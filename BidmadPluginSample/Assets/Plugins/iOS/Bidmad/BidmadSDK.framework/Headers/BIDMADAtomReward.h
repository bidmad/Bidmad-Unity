//
//  BIDMADAtomReward.h
//  BidmadSDK
//
//  Created by 김선정 on 13/03/2019.
//  Copyright © 2019 ADOP Co., Ltd. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "BIDMADRewardVideo.h"
#import <AVFoundation/AVFoundation.h>
#import <AVKit/AVKit.h>
#import <QuartzCore/QuartzCore.h>
#import <WebKit/WebKit.h>

@class BIDMADRewardVideo;

@interface BIDMADAtomReward : UIViewController<WKNavigationDelegate>
@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;
@property (nonatomic, strong) UIProgressView *progressView;
@property (nonatomic, strong) AVPlayer *videoPlayer;
@property (nonatomic, strong) AVPlayerLayer *playerLayer;
@property (nonatomic, strong) UILabel*  remainLbl;
@property (nonatomic, strong) UIButton*  closeBtn;

@property (nonatomic, strong) UIButton*  rewardSkipBtn;
@property (nonatomic, strong) UIButton*  rewardSkipCancelBtn;

@property (nonatomic, strong) id timeObserver;

@property (nonatomic, strong) WKWebView* endcardTemView;
@property (nonatomic, strong) UIView* endcardImgView;
@property (nonatomic, strong) UIImageView *endCarImageViewEx;

@property (nonatomic) UIInterfaceOrientation startingInterfaceOrientation;//
- (id)initWithAtomRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo parentViewController:(UIViewController *) parentViewCtrl;
- (void)gogoRewardVideo;
- (void)showAtomRewardVideo:(UIViewController *)pvc;
@end
