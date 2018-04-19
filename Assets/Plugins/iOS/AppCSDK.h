//
//  AppCSDK.h
//

#import "AppCSDKDefine.h"
#import "AppCSDKDelegate.h"
#import "ItemStoreDefine.h"

/**
 * @brief appC SDKのエントリクラス
 *
 */
@interface AppCSDK : NSObject

/**
 * @brief メディアキーを指定してappCを初期設定する
 *
 * @param mk_ メディアキー
 * @param option_ オプション
 *
 */
+(void)setupAppCWithMediaKey:(NSString*)mk_
                      option:(NSUInteger)option_;

/**
 * @brief メディアキーを指定してappCを初期設定する
 *
 * @param mk_ メディアキー
 * @param option_ オプション
 * @param launchOptions_ ランチオプション
 *
 */
+(void)setupAppCWithMediaKey:(NSString*)mk_
                      option:(NSUInteger)option_
               launchOptions:(NSDictionary *)launchOptions_;


/**
 * @brief 新リカバリ画面表示
 *
 */
+(void)recoverGenerate;

/**
 * @brief 新リカバリコード入力画面表示
 *
 */
+(void)recoverRestore;

/**
 * @brief リカバリ後リセット
 *
 */
+(void)resetItemStoreInfo;


/**
 * @brief リカバリを開始する
 *
 */
+(void)startRecovery;

/**
 * @brief delegate設定
 *
 * @param delegate AppCSDKDelegate
 */
+ (void)setDelegate:(id<AppCSDKDelegate>)delegate;


////////////////////////////////////////
// for itemstore

/**
 * @brief アプリ内課金アイテムリスト画面表示
 *
 * @return 実行可否
 */
+ (BOOL)itemStoreShowList;

/**
 * @brief アプリ内課金復元処理開始
 *
 * @return 実行可否
 */
+ (BOOL)itemStoreRestore;

/**
 * @brief itemstore有効アイテム追加
 * setupAppCWithMediaKey より前でコールしてください。
 * コールしない場合は管理画面で登録された全てのアイテムが有効となります。
 *
 * @param category_key カテゴリーキー
 * @param product_ids プロダクトIDの配列
 */
+ (BOOL)itemStoreAddActiveKey:(NSString *)category_key ids:(NSArray *)product_ids;

/**
 * @brief itemstoreアイテム数増減
 *
 * @param category_key カテゴリーキー
 * @param reduce 増減数
 * @return アイテム数増減後の総数（エラー時は-1）
 */
+ (NSInteger)itemStoreAddCountWithKey:(NSString *)category_key
                               reduce:(NSInteger)reduce;

/**
 * @brief itemstoreアイテム数設定
 *
 * @param category_key カテゴリーキー
 * @param count アイテム数
 * @return アイテム数設定後の総数（エラー時は-1）
 */
+ (NSInteger)itemStoreSetCountWithKey:(NSString *)category_key
                                count:(NSInteger)count;

/**
 * @brief itemstoreアイテム数取得
 *
 * @param category_key カテゴリーキー
 * @return アイテム数（エラー時は-1）
 */
+ (NSInteger)itemStoreGetCountWithKey:(NSString *)category_key;

/**
 * @brief itemstoreアイテム取得（全データ）
 *
 * @return itemstoreアイテム全データ
 */
+ (NSArray *)itemStoreGetAllData;

////////////////////////////////////////
// for Push 通知

/**
 * @brief Notificationの登録完了を通知する
 *
 */
+(void)pushNotificationDidRegisterWithDeviceToken:(NSData*)token;

/**
 * @brief Notificationの登録失敗を通知する
 *
 */
+(void)pushNotificationDidFailWithError:(NSError *)error;


/**
 * @brief Notificationの受信を通知する
 *
 */
+(void)pushNotificationDidReceive:(NSDictionary *)userInfo
                          appStat:(UIApplicationState)appStat;


+(NSString *)pushNotificationGetCustomParameter;

/**
 * @brief お問い合わせキー返却
 *
 * @return お問い合わせキー
 */
+ (NSString *)inquiryKey;


/**
 * @brief データストア保存
 *
 * @return 保存成否
 */
+ (BOOL)putDataStore:(NSString *)key_
              value:(NSString *)value_;

/**
 * @brief データストア取得
 *
 * @return データ
 */
+ (NSString *)getDataStore:(NSString *)key;

/**
 * @brief 引き継ぎ済みチェック
 *
 * @return 引き継ぎ済み=YES
 */
+(BOOL)confirmRestored;

@end
