using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif



public class PrivacyPolicy : MonoBehaviour
{
    private int PP;
    [SerializeField] private Button accept_button;

    private void Awake()
    {

        if (PlayerPrefs.HasKey("PP"))
        {
            PP = PlayerPrefs.GetInt("PP");

        }
        else
        {
            PP = 0;
        }

        if (PP == 1)
        {
            LoadGame();
        }

        accept_button.onClick.AddListener(AcceptPP);
    }


    void LoadGame()
    {
        SceneManager.LoadScene(0);
    }

    void AcceptPP()
    {
        PlayerPrefs.SetInt("PP", 1);
        LoadGame();

    }
}
