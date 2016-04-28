using UnityEngine;
using System.Collections;

public class TestShare : MonoBehaviour
{
    public void ShareFacebook()
    {
        ShareApp.Share(ShareApp.ShareAppProvider.FACEBOOK, "This is title", "This is messae", "", "");
    }

    public void ShareTwitter()
    {
        ShareApp.Share(ShareApp.ShareAppProvider.TWITTER, "This is title", "This is messae", "", "");
    }
}
