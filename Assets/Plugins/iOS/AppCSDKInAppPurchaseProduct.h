//
//  AppCSDKInAppPurchaseProduct.h
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

/**
 * @brief appC SDK アプリ内課金アイテム クラス
 */
@interface AppCSDKInAppPurchaseProduct : NSObject

@property (nonatomic, retain) SKProduct* skProduct;
@property (nonatomic, retain) NSString *productIdentifier;
@property (nonatomic, retain) NSString *title;
//@property (nonatomic, retain) NSString *description;
@property (nonatomic, retain) NSString *desc;
@property (nonatomic, retain) NSString *price;
@property (nonatomic, retain) NSString *currencyCode;
@property (nonatomic, assign) double priceNum;

/**
 * @brief イニシャライザ（プロダクトID指定）
 *
 * @param productIdentifier プロダクトID
 * @return オブジェクト
 */
- (id)initWithProductIdentifier:(NSString *)productIdentifier;

/**
 * @brief イニシャライザ（プロダクト情報指定）
 *
 * @param product プロダクト情報
 * @return オブジェクト
 */
- (id)initWithProduct:(SKProduct *)product;

@end
