using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.alictus.sdklite;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif
public class AlictusConsentScript : MonoBehaviour
{
    private void Start()
    {
        InitCallback();
    }

    private void InitCallback()
    {
        
#if UNITY_IOS
            ATTrackingStatusBinding.AuthorizationTrackingStatus status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
            {
                AlictusSDK.SetConsentStatus(true);
            }
            else
            {
                AlictusSDK.SetConsentStatus(false);
            }
#endif
    }
      
    
}