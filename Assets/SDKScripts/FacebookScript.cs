using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
public class FacebookScript : MonoBehaviour
{
    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
    }

    private void InitCallback()
    {
        Debug.Log("FB Init Callback");
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            Debug.Log("FB Initialized");
#if UNITY_IOS
            ATTrackingStatusBinding.AuthorizationTrackingStatus status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
            {
                FB.Mobile.SetAdvertiserTrackingEnabled(true);
            }
            else
            {
                FB.Mobile.SetAdvertiserTrackingEnabled(false);
            }
#endif
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }
}
