//
//  ISN_NativeCore.h
//  Unity-iPhone
//
//  Created by lacost on 9/6/15.
//
//

@interface ISN_DataConvertor : NSObject

+ (NSString*) charToNSString: (char*)value;
+ (const char *) NSIntToChar: (NSInteger) value;
+ (const char *) NSStringToChar: (NSString *) value;
+ (NSArray*) charToNSArray: (char*)value;

+ (const char *) serializeErrorWithData:(NSString *)description code: (int) code;
+ (const char *) serializeError:(NSError *)error;

+ (NSString *) serializeErrorWithDataToNSString:(NSString *)description code: (int) code;
+ (NSString *) serializeErrorToNSString:(NSError *)error;


+ (const char *) NSStringsArrayToChar:(NSArray *) array;
+ (NSString *) serializeNSStringsArray:(NSArray *) array;

@end


@interface ISN_NativeUtility : NSObject

@property (strong)  UIActivityIndicatorView *spinner;

+ (id) sharedInstance;
+ (int) majorIOSVersion;
+ (BOOL) IsIPad;

- (void) redirectToRatingPage: (NSString *) appId;
- (void) setApplicationBagesNumber:(int) count;

- (void) ShowSpinner;
- (void) HideSpinner;
- (void) GetIFA;
- (void) ISN_NativeLog: (NSString *) appId, ...;
- (void) ISN_SetLogState: (BOOL) appId;

@end

@interface CloudManager : NSObject


+ (id) sharedInstance;

- (void) initialize;
- (void) setString:(NSString*) val key:(NSString*) key;
- (void) setDouble:(double) val key:(NSString*) key;
- (void) setData:(NSData*) val key:(NSString*) key;

-(void) requestDataForKey:(NSString*) key;

@end



@interface AppEventListener : NSObject

+ (id) sharedInstance;
-(void) subscribe;

@end




@interface ISNSharedApplication : NSObject

+ (id)  sharedInstance;

- (void) checkUrl:(NSString*)url;
- (void) openUrl:(NSString*)url;


@end






@interface ISN_NativePopUpsManager : NSObject
+ (ISN_NativePopUpsManager *) sharedInstance;
@end



@interface IOSNativeNotificationCenter : NSObject


+ (IOSNativeNotificationCenter *)sharedInstance;
- (void) scheduleNotification: (int) time message: (NSString*) message sound: (bool *)sound alarmID:(NSString *)alarmID badges: (int)badges notificationData: (NSString*) notificationData notificationSoundName: (NSString*) notificationSoundName;
- (void) cleanUpLocalNotificationWithAlarmID: (NSString *)alarmID;
- (void) cancelNotifications;
- (void) applicationIconBadgeNumber: (int)badges;
- (void) RegisterForNotifications;
- (void) requestNotificationSettings;

@end










