#import "iOSNativeShare.h"
#import "UnityAppController.h"

extern UIViewController* UnityGetGLViewController();

#ifdef __cplusplus
extern "C"
{
#endif
    void _showAlertMessage(const char *title, const char *msg) {
        [[iOSNativeShare shareManager] showAlertMessage:[NSString stringWithUTF8String:title] message:[NSString stringWithUTF8String:msg]];
    }
    
    void _showShareFacebook(const char *subject, const char *msg, const char *imagePath, const char *url) {
        [[iOSNativeShare shareManager] showShareFacebook:[NSString stringWithUTF8String:subject] message:[NSString stringWithUTF8String:msg] imagePath:[NSString stringWithUTF8String:imagePath] url:[NSString stringWithUTF8String:url]];
    }
    
    void _showShareTwitter(const char *subject, const char *msg, const char *imagePath, const char *url) {
        [[iOSNativeShare shareManager] showShareTwitter:[NSString stringWithUTF8String:subject] message:[NSString stringWithUTF8String:msg] imagePath:[NSString stringWithUTF8String:imagePath] url:[NSString stringWithUTF8String:url]];
    }
#ifdef __cplusplus
}
#endif

@implementation iOSNativeShare

static iOSNativeShare * shareNativeManager;

+ (iOSNativeShare *) shareManager {
    @synchronized (self) {
        if (shareNativeManager == nil) {
            shareNativeManager = [[self alloc]init];
        }
    }
    return shareNativeManager;
}

- (id) init {
    return [super init];
}

- (void) showAlertMessage:(NSString *) title message:(NSString *)message {
    UIAlertView *alertView = [[UIAlertView alloc]
                              initWithTitle:title
                              message:message
                              delegate:self
                              cancelButtonTitle:@"OK"
                              otherButtonTitles:nil];
    [alertView show];
}

- (void) showShareFacebook:(NSString *)subject message:(NSString *)msg imagePath:(NSString *)imagePath url:(NSString *)url {
    if ([SLComposeViewController isAvailableForServiceType:SLServiceTypeFacebook])
    {
        UIViewController *rootViewController = UnityGetGLViewController();
        
        UIImage *image = nil;
        NSURL *formattedURL = nil;
        if (url.length > 10)
            formattedURL= [NSURL URLWithString:url];
        
        NSFileManager *fileMgr = [NSFileManager defaultManager];
        if ([fileMgr fileExistsAtPath:imagePath])
        {
            NSData *dataImage = [NSData dataWithContentsOfFile:imagePath];
            image = [UIImage imageWithData:dataImage];
        }
        
        SLComposeViewController *fbPostSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeFacebook];
        [fbPostSheet setInitialText:msg];
        [fbPostSheet setTitle:subject];
        if (image != nil)
        {
            [fbPostSheet addImage:image];
        }
        if (formattedURL != nil)
        {
            [fbPostSheet addURL:formattedURL];
        }
        [rootViewController presentViewController:fbPostSheet animated:YES completion:nil];
    } else
    {
        [self showAlertMessage:@"Sorry" message:@"You do not have this service"];
    }
}

- (void) showShareTwitter:(NSString *)subject message:(NSString *)msg imagePath:(NSString *)imagePath url:(NSString *)url {
    if ([SLComposeViewController isAvailableForServiceType:SLServiceTypeTwitter])
    {
        UIViewController *rootViewController = UnityGetGLViewController();
        
        UIImage *image = nil;
        NSURL *formattedURL = nil;
        if (url.length > 10)
            formattedURL= [NSURL URLWithString:url];
        
        NSFileManager *fileMgr = [NSFileManager defaultManager];
        if ([fileMgr fileExistsAtPath:imagePath])
        {
            NSData *dataImage = [NSData dataWithContentsOfFile:imagePath];
            image = [UIImage imageWithData:dataImage];
        }
        
        SLComposeViewController *twPostSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeTwitter];
        [twPostSheet setInitialText:msg];
        [twPostSheet setTitle:subject];
        if (image != nil)
        {
            [twPostSheet addImage:image];
        }
        if (formattedURL != nil)
        {
            [twPostSheet addURL:formattedURL];
        }
        [rootViewController presentViewController:twPostSheet animated:YES completion:nil];
    } else
    {
        [self showAlertMessage:@"Sorry" message:@"You do not have this service"];
    }
}

@end