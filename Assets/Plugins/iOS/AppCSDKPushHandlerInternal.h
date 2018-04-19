#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface UIApplication(SupressWarnings)
- (void)application:(UIApplication *)application appCSDKDidRegisterForRemoteNotificationsWithDeviceToken:(NSData *)devToken;
- (void)application:(UIApplication *)application appCSDKDidFailToRegisterForRemoteNotificationsWithError:(NSError *)err;
- (void)application:(UIApplication *)application appCSDKDidReceiveRemoteNotification:(NSDictionary *)userInfo;
@end
