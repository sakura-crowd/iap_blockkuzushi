#import "AppCSDK.h"
#import "AppCSDKPushHandlerInternal.h"
#import <objc/runtime.h>

@implementation UIApplication(AppCSDKPushHandlerInternal)

+(void)load
{
    method_exchangeImplementations(class_getInstanceMethod(self,
                                                           @selector(setDelegate:)),
                                   class_getInstanceMethod(self,
                                                           @selector(appCSDKAppDelegate:)));
	UIApplication *app = [UIApplication sharedApplication];
	NSLog(@"Init app: %@, %@", app, app.delegate);
}

void appCSDKRunTimeDidRegisterForRemoteNotificationsWithDeviceToken(id self, SEL _cmd, id application, id devToken)
{
    [AppCSDK pushNotificationDidRegisterWithDeviceToken:devToken];
}

void appCSDKRunTimeDidFailToRegisterForRemoteNotificationsWithError(id self, SEL _cmd, id application, id error)
{
    [AppCSDK pushNotificationDidFailWithError:error];
}

void appCSDKRunTimeDidReceiveRemoteNotification(id self, SEL _cmd, id application, id userInfo)
{
    [AppCSDK pushNotificationDidReceive:userInfo appStat:((UIApplication*)application).applicationState];
}

static void exchangeMethodImplementations(Class class, SEL oldMethod, SEL newMethod, IMP impl, const char * signature)
{
	Method method = nil;
	method = class_getInstanceMethod(class, oldMethod);
	
	if (method)
    {
		class_addMethod(class, newMethod, impl, signature);
		method_exchangeImplementations(class_getInstanceMethod(class, oldMethod),
                                       class_getInstanceMethod(class, newMethod));
	}
    else
    {
		class_addMethod(class, oldMethod, impl, signature);
	}
}

- (void) appCSDKAppDelegate:(id<UIApplicationDelegate>)delegate
{
    
	static Class delegateClass = nil;
	
	if(delegateClass == [delegate class])
	{
		[self appCSDKAppDelegate:delegate];
		return;
	}
	
	delegateClass = [delegate class];
    
    exchangeMethodImplementations(delegateClass, @selector(application:didRegisterForRemoteNotificationsWithDeviceToken:),
		   @selector(application:appCSDKDidRegisterForRemoteNotificationsWithDeviceToken:),
                                  (IMP)appCSDKRunTimeDidRegisterForRemoteNotificationsWithDeviceToken, "v@:::");
    
	exchangeMethodImplementations(delegateClass, @selector(application:didFailToRegisterForRemoteNotificationsWithError:),
		   @selector(application:appCSDKDidFailToRegisterForRemoteNotificationsWithError:),
                                  (IMP)appCSDKRunTimeDidFailToRegisterForRemoteNotificationsWithError, "v@:::");
    
	exchangeMethodImplementations(delegateClass, @selector(application:didReceiveRemoteNotification:),
		   @selector(application:appCSDKDidReceiveRemoteNotification:),
                                  (IMP)appCSDKRunTimeDidReceiveRemoteNotification, "v@:::");
    
	[self appCSDKAppDelegate:delegate];
}

@end
