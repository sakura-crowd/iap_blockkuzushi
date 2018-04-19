#import <Foundation/Foundation.h>
#import "AppCSDK.h"
#import "UnityAppController.h"
//#import "iPhone_target_Prefix.pch"

//extern void UnitySendMessage(const char*, const char*, int);
extern void UnitySendMessage(const char*, const char*, const char*);

static BOOL setup = NO;             // starting setup
static BOOL setupResult = NO;       // setup result
static NSMutableArray* arrServiceId = [[NSMutableArray alloc] init];

static NSString* _gameObjName = @"";

char* MakeStringCopyAppc (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

@interface AppCSDKController : UnityAppController
+(void)load;
@end
@implementation AppCSDKController
+(void)load
{
    extern const char* AppControllerClassName;
    AppControllerClassName = "AppCSDKController";
}
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    NSUserDefaults* launchOpts = [NSUserDefaults standardUserDefaults];
    [launchOpts setObject:launchOptions forKey:@"launchOptions"];
    
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    return YES;
}
@end

//////////////////////////////////////////////////
// Class for AppCSDK Delegate (interface)
@interface AppCSDKDelegate : NSObject
<
AppCSDKDelegate
>
@end

//////////////////////////////////////////////////
// Class for AppCSDK Delegate (implementation)
@implementation AppCSDKDelegate

- (void)setupAppC:(NSString*)mk option:(NSUInteger)option
{
    NSUserDefaults* launchOpts = [NSUserDefaults standardUserDefaults];
    NSDictionary* launchOptions = [launchOpts dictionaryForKey:@"launchOptions"];
    
    // make thread
    dispatch_queue_t queue;
    queue = dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_LOW, 0);
    dispatch_async(queue, ^{
        dispatch_async(dispatch_get_main_queue(), ^ {
            [AppCSDK setupAppCWithMediaKey:mk
                                    option:option
                             launchOptions:launchOptions];
        });
    });
}

- (void)finishedSetupAppC:(BOOL)succeed
{
    NSLog(@"finishedSetupAppC");
    setupResult = succeed;
    setup = YES;
    
    if ([_gameObjName compare: @""] == NSOrderedSame) {
        _gameObjName = @"AppC";
    }
    
    NSString* result = [NSString stringWithFormat:@"%s", succeed ? "true" : "false"];
    NSString* customParam = [AppCSDK pushNotificationGetCustomParameter];
    
    NSString* callbackText = [NSString stringWithFormat:@"%@%%INIT%%%@", result, customParam];
    const char* retCallbackText = [callbackText UTF8String];
    
    UnitySendMessage([_gameObjName UTF8String], "OnCallBack", MakeStringCopyAppc(retCallbackText));
    //CFRelease((CFTypeRef)_gameObjName);
}

- (void)closedItemStore
{
    NSLog(@"closedItemstoreView");
    
    if ([_gameObjName compare: @""] == NSOrderedSame) {
        _gameObjName = @"AppC";
    }
    
    UnitySendMessage([_gameObjName UTF8String], "OnCallBackClosedItemstoreView", "");
}

- (void)recoverReset
{
    NSLog(@"recoverReset");
    
    if ([_gameObjName compare: @""] == NSOrderedSame) {
        _gameObjName = @"AppC";
    }
    
    UnitySendMessage([_gameObjName UTF8String], "OnCallBackRecoverReset", "");
}

- (void)recoverRestored
{
    NSLog(@"recoverRestored");
    
    if ([_gameObjName compare: @""] == NSOrderedSame) {
        _gameObjName = @"AppC";
    }
    
    UnitySendMessage([_gameObjName UTF8String], "OnCallBackRecoverRestored", "");
}


- (void)inAppPurchased:(AppCSDKInAppPurchaseProduct*)productIdentifier
{
    NSLog(@"inAppPurchased");
}

- (void)inAppPurchaseRestored:(AppCSDKInAppPurchaseProduct*)productIdentifier
{
    NSLog(@"inAppPurchaseRestored");
}

- (void)inAppPurchaseRestoreCompleted
{
    NSLog(@"inAppPurchaseRestoreCompleted");
}

- (void)inAppPurchaseRestoreFailed:(NSError *)error
{
    NSLog(@"inAppPurchaseRestoreFailed");
}

- (void)getService:(NSString *)service_id
{
    NSLog(@"getService");
    [arrServiceId addObject:service_id];
}

@end


//////////////////////////////////////////////////
// Root view controller of Unity screen.
extern UIViewController* UnityGetGLViewController();

static AppCSDKDelegate* appc_sdk = nil;


#pragma mark Plug-in Functions

//////////////////////////////////////////////////
// Setup Media Key
extern "C" void setupAppCWithMediaKey(const char* gameObjName,
                                      const char* mk,
                                      int opt_itemStore,
                                      int opt_push,
                                      int opt_datastore) {
    _gameObjName = [NSString stringWithUTF8String:gameObjName];
    CFRetain((CFTypeRef)_gameObjName);
    
    NSUInteger opt = 0;
    if (opt_itemStore) opt |= APPC_SDK_ITEMSTORE;
    if (opt_push) opt |= APPC_SDK_PUSH;
    if (opt_datastore) opt |= APPC_SDK_DATASTORE;
    
    appc_sdk = [[AppCSDKDelegate alloc] init];
    [AppCSDK setDelegate:appc_sdk];
    setup = NO;
    [appc_sdk setupAppC:[NSString stringWithUTF8String:mk]
                 option:opt];
}

//////////////////////////////////////////////////
// for AppCSDK
extern "C" const char* getInquiryKey() {
    NSString* inquiryKey = [AppCSDK inquiryKey];
    const char* retInquiryKey = [inquiryKey UTF8String];
    return MakeStringCopyAppc(retInquiryKey);
}

extern "C" void openRecoverGenerate() {
    [AppCSDK recoverGenerate];
}

extern "C" void openRecoverRestore() {
    [AppCSDK recoverRestore];
}

extern "C" void confirmRestored() {
    [AppCSDK confirmRestored];
}

//////////////////////////////////////////////////
// for AppCSDK InAppPurchase
extern "C" void openItemStore() {
    [AppCSDK itemStoreShowList];
}

extern "C" void itemStoreRestore() {
    [AppCSDK itemStoreRestore];
}

extern "C" void itemStoreAddActiveItem(const char* key, const char* product_ids[], int count) {
    NSString* key0 = [NSString stringWithUTF8String:key];
    NSMutableArray* activeItems = [NSMutableArray array];
    for (int i=0; i<count; i++) {
        NSString* s = [NSString stringWithUTF8String:product_ids[i]];
        [activeItems addObject:s];
    }
    // set Active Items
    [AppCSDK itemStoreAddActiveKey:key0
                               ids:activeItems];
}

extern "C" const char* getGroupName(const char* groupID) {
    NSArray* items = [AppCSDK itemStoreGetAllData];
    NSString* groupName = @"";
    
    for (NSDictionary* item in items) {
        NSString* categoryKey = [item objectForKey:@"categoryKey"];
        if ([[NSString stringWithCString:groupID encoding:NSUTF8StringEncoding] isEqualToString:categoryKey]) {
            NSString* categoryName = [item objectForKey:@"categoryName"];
            groupName = categoryName;
            break;
        }
    }
    
    const char* retGroupName = [groupName UTF8String];
    return MakeStringCopyAppc(retGroupName);
}

extern "C" int getItemCount(const char* key) {
    return [AppCSDK itemStoreGetCountWithKey:[NSString stringWithUTF8String:key]];
}

extern "C" void addItemCount(const char* key, int addCount) {
    [AppCSDK itemStoreAddCountWithKey:[NSString stringWithUTF8String:key]
                               reduce:addCount];
}

extern "C" void setItemCount(const char* key, int setCount) {
    [AppCSDK itemStoreSetCountWithKey:[NSString stringWithUTF8String:key]
                                count:setCount];
}

//////////////////////////////////////////////////
// for AppCSDK Datastore
extern "C" bool putDataStore(const char* key, const char* value) {
    return [AppCSDK putDataStore:[NSString stringWithUTF8String:key] value:[NSString stringWithUTF8String:value]];
}

extern "C" const char* getDataStore(const char* key) {
    const char* retDataStore = [[AppCSDK getDataStore:[NSString stringWithUTF8String:key]] UTF8String];
    return MakeStringCopyAppc(retDataStore);
}

