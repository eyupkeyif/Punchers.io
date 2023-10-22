using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSDKScript : MonoBehaviour
{
    private void Awake()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };

        MaxSdk.SetSdkKey("HH5sKCIsd_NGg4TVkCm5hdPJdoc23DOLVTCqfUfIT2T866eqJOv_0JId5wg984UdXgRC96Vd83mFAP3TozSoGm");
        MaxSdk.InitializeSdk();
    }
}
