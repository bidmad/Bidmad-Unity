//
//  BIDMADLabelView.h
//  BidmadSDK
//
//  Created by 김선정 on 2018. 11. 21..
//  Copyright © 2018년 ADOP Co., Ltd. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BIDMADUtil.h"
#import "FMMoveTableView.h"
#import "FMMoveTableViewCell.h"

NS_ASSUME_NONNULL_BEGIN

@interface BIDMADLabelView : UIView<FMMoveTableViewDelegate,FMMoveTableViewDataSource,UIAlertViewDelegate>

@property (strong, nonatomic) IBOutlet FMMoveTableView *fmTableView;

@property (strong, nonatomic) IBOutlet UIImageView *adop_logo;


@property (strong, nonatomic) IBOutlet UIImageView *app_thumbnail;
@property (strong, nonatomic) IBOutlet UIView *loadingView;

@property (nonatomic, strong) NSString* currentMediation;

@property (nonatomic, strong) NSMutableArray* ad_order;
@property (nonatomic, strong) NSMutableArray* ad_ecpm;
@property (nonatomic, strong) NSMutableArray* ad_fill;
@property (nonatomic, strong) NSMutableArray* area_list;
@property (nonatomic, strong) NSMutableArray* adv_idx;
@property (nonatomic, strong) NSMutableArray* ad_state;
@property (nonatomic, strong) NSMutableArray* weight;
@property (nonatomic, strong) NSMutableArray* adtype;
@property (nonatomic, strong) NSMutableArray* mediationSequence;

@property (nonatomic, strong) UIButton*     appNameBtn;

@property (nonatomic, strong) UIButton*     zoneIdBtn;

@property (nonatomic, strong) UIButton*     areaNameBtn;

@property (nonatomic, strong) UIButton*     goToCompassBtn;

@property (nonatomic, strong) UIButton*     typeBtn;

@property (nonatomic, strong) UIButton*     op_typeBtn;

@property (strong, nonatomic) IBOutlet UILabel *appName;

@property (strong, nonatomic) IBOutlet UILabel *areaName;

@property (strong, nonatomic) IBOutlet UILabel *area_idx;

@property (strong, nonatomic) IBOutlet UILabel *sevenDays;

@property (strong, nonatomic) IBOutlet UILabel *type;

@property (strong, nonatomic) IBOutlet UILabel *op_type;

@property (strong, nonatomic) IBOutlet UILabel *ci_tier;

@property (strong, nonatomic) IBOutlet UILabel *imp;

@property (unsafe_unretained, nonatomic) IBOutlet UILabel *titleLabel;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *inventoryNameTitleLabel;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *weeklyProfitCompareLabel;
@property (unsafe_unretained, nonatomic) IBOutlet UIButton *saveBtn;
@property (unsafe_unretained, nonatomic) IBOutlet UIButton *cancelBtn;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *publisherTypeLabel;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *mediationTypeLabel;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *inventoryTypeLabel;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *inventoryZoneIdLabel;

@property (strong, nonatomic) IBOutlet UILabel *impValue;
@property (strong, nonatomic) IBOutlet UILabel *ecpm;
@property (strong, nonatomic) IBOutlet UILabel *ecpmValue;
@property (strong, nonatomic) IBOutlet UILabel *rev;
@property (strong, nonatomic) IBOutlet UILabel *revValue;
@property (strong, nonatomic) IBOutlet UIView *applying;
@property (strong, nonatomic) IBOutlet UIView *bigApplying;

@property (strong, nonatomic) IBOutlet UIView *mainView;
@property (strong, nonatomic) IBOutlet UIView *mediationEditView;
@property (strong, nonatomic) IBOutlet UIButton *mediationEditViewBtn;
@property (unsafe_unretained, nonatomic) IBOutlet UILabel *monthlyRevLabel;

@property (strong, nonatomic) IBOutlet UIView *adminView;

@property (strong, nonatomic) UIButton* collapseBtn;
@property (strong, nonatomic) IBOutlet UIImageView *impArrow;
@property (strong, nonatomic) IBOutlet UIImageView *ecpmArrow;
@property (strong, nonatomic) IBOutlet UIImageView *revArrow;
@property (strong, nonatomic) IBOutlet UIButton *closeBtn;
@property (strong, nonatomic) IBOutlet UIImageView *iconCopty;
@property (strong, nonatomic) IBOutlet UILabel *sevendaysEcpm;
@property (strong, nonatomic) IBOutlet UILabel *octRev;

@property (strong, nonatomic) IBOutlet UIView *inventoryNameView;
@property (strong, nonatomic) IBOutlet UIView *inventoryZoneIdView;
@property (strong, nonatomic) IBOutlet UIView *inventoryTypeView;
@property (strong, nonatomic) IBOutlet UIView *medTypeView;
@property (strong, nonatomic) IBOutlet UIView *PubTypeView;
@property (strong, nonatomic) IBOutlet UIView *mediTotalView;
@property (strong, nonatomic) IBOutlet UILabel *standardDate;
@property (strong, nonatomic) IBOutlet UIView *bottomInfoView;
@property (strong, nonatomic) IBOutlet UIImageView *icon_info;

@property (nonatomic) float ypoint;
@property (nonatomic) BOOL isAdmin;
@property (strong, nonatomic) IBOutlet NSLayoutConstraint *inventoryNameCons;
@property (strong, nonatomic) IBOutlet NSLayoutConstraint *mediViewCons;
@property (strong, nonatomic) IBOutlet NSLayoutConstraint *InvTopCons;
@property (strong, nonatomic) IBOutlet NSLayoutConstraint *mainScrCons;

@property (strong, nonatomic) IBOutlet UIView *topView;


@property (strong, nonatomic) IBOutlet UIScrollView *mainScrollView;


- (IBAction)mediationEditViewBtn:(id)sender;
- (IBAction)mediationSaveBtn:(id)sender;
- (IBAction)mediationEditCancel:(id)sender;
- (IBAction)copyAreaIdx:(id)sender;
- (IBAction)closeBtn:(id)sender;
-(void)removeCollapseBtn;

- (void)showCollapseBtn:(float)ypoint;

- (id)initWithEcpm_rev_info:(NSDictionary *)ecpm_rev_info area_info:(NSDictionary *)area_info change_info:(NSDictionary *)change_info mediationNumber:(int)mNumber network:(NSString *)network ads_dic:(NSDictionary *)ads_dic date_dic:(NSDictionary *)date_dic isAdmin:(BOOL)isAdmin;

- (void)collapseBtn:(id)sender;
- (IBAction)infoDate:(id)sender;

@end

NS_ASSUME_NONNULL_END
