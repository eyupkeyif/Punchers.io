using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class LoadingScene : MonoBehaviour
{
    int PP, TT;
    [SerializeField] float timer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FakeLoadingScreen(timer));
        RequestTrackingPermission();



    }

    public void RequestTrackingPermission()
    {

        

        #if UNITY_IOS
                        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                        {
                            ATTrackingStatusBinding.RequestAuthorizationTracking();
                        }
        #endif

    }

    IEnumerator FakeLoadingScreen(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (PlayerPrefs.HasKey("PP"))
        {
            PP = PlayerPrefs.GetInt("PP");

        }


        if(PP == 1)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene("Privacy Policy");
        }



        yield break;
    }
}
