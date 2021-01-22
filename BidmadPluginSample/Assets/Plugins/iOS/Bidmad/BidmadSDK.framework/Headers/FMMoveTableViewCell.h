//
//  FMMoveTableViewCell.h
//  FMFramework
//
//  Created by Florian Mielke.
//  Copyright 2012 Florian Mielke. All rights reserved.
//  
#import <UIKit/UIKit.h>

@interface FMMoveTableViewCell : UITableViewCell
@property (strong, nonatomic) IBOutlet UILabel *labelTxt;
@property (strong, nonatomic) IBOutlet UILabel *listIdx;
@property (strong, nonatomic) IBOutlet UIView *bgView;
@property (strong, nonatomic) IBOutlet UIImageView *burgerBtn;
@property (strong, nonatomic) IBOutlet UILabel *ecpmLabel;
@property (strong, nonatomic) IBOutlet UILabel *fillrateLabel;

- (void)prepareForMoveSnapshot;
- (void)prepareForMove;
@end
