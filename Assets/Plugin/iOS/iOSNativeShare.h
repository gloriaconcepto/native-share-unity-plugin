#import <Social/Social.h>
#import <Foundation/Foundation.h>

@interface iOSNativeShare : NSObject {
    
}

+ (iOSNativeShare*) shareManager;
- (void) showAlertMessage:(NSString *)title message:(NSString *)message;
- (void) showShareFacebook:(NSString *)subject message:(NSString *)msg imagePath:(NSString *)imagePath url:(NSString *)url;
- (void) showShareTwitter:(NSString *)subject message:(NSString *)msg imagePath:(NSString *)imagePath url:(NSString *)url;

@end