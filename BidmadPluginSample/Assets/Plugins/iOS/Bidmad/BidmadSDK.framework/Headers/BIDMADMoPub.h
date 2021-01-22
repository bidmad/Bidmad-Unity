#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "BIDMADRewardVideo.h"
#import "BIDMADSetting.h"
#import "BIDMADGDPR.h"
#import "BIDMADUtil.h"
#import "BIDMADLabelView.h"
#import "UIButton+circle.h"
#import <MoPub/MoPub.h>


@class BIDMADRewardVideo;


@interface BIDMADMoPub : NSObject <MPRewardedVideoDelegate>

@property (strong,nonatomic) BIDMADRewardVideo* rewardVideo;

- (id)      initWithRewardVideo:(NSDictionary *)dic bidmadReward:(BIDMADRewardVideo *)rewardVideo userId:(NSString *)userId;
- (void)    showMopubAdsRewardVideo:(UIViewController *)pvc;
- (void)    gogoRewardVideo;
- (void)    setGDPRData;

@end
