using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebsiteButton : MonoBehaviour
{
    [SerializeField] private string URL;
    [SerializeField] private Button button;
    // Start is called before the first frame update
    private void Awake()
    {
        button.onClick.AddListener(OpenURL);
    }

    void OpenURL()
    {
        Application.OpenURL(URL);
    }
}
