//
//  AppCSDKDefine.h
//

#import <Foundation/Foundation.h>
#import "AppCSDKInAppPurchaseProduct.h"
#import "ItemStoreDefine.h"

@protocol AppCSDKDelegate <NSObject>

@optional

/**
 * @brief setupAppCWithMediaKey の完了通知
 *
 * @param succeed 成功・失敗フラグ
 */
- (void)finishedSetupAppC:(BOOL)succeed;



/**
 * @brief getRecoveryCode の完了通知
 *
 * @param recovery_code リカバリコード
 */
- (void)finishedGetRecoveryCode:(NSString *)recovery_code;

/**
 * @brief execRecover の完了通知
 *
 * @param succeed 成功・失敗
 */
- (void)finishedExecRecover:(BOOL)succeed;

/**
 * @brief アプリ内課金：購入完了デリゲートメソッド
 *
 * @param productIdentifier プロダクトID
 */
- (void)inAppPurchased:(AppCSDKInAppPurchaseProduct *)productIdentifier;

/**
 * @brief アプリ内課金：購入失敗デリゲートメソッド
 *
 * @param productIdentifier プロダクトID
 * @param error エラー情報
 */
- (void)inAppPurchaseFailed:(AppCSDKInAppPurchaseProduct *)productIdentifier
                      error:(NSError *)error;

/**
 * @brief アプリ内課金：リストア通知
 *
 * @param productIdentifier プロダクトID
 */
- (void)inAppPurchaseRestored:(AppCSDKInAppPurchaseProduct *)productIdentifier;

/**
 * @brief アプリ内課金：リストア処理完了
 *
 */
- (void)inAppPurchaseRestoreCompleted;

/**
 * @brief アプリ内課金：リストア処理失敗
 *
 * @param error エラー情報
 */
- (void)inAppPurchaseRestoreFailed:(NSError *)error;

/**
 * @brief サービス獲得
 *
 * @param service_id サービスID
 */
- (void)getService:(NSString *)service_id;

- (void)closedItemStore;

/**
 * @brief 移動済みユーザ検知
 *
 */


//リセット完了
- (void)recoverReset;

//リストア完了
- (void)recoverRestored;


@end
