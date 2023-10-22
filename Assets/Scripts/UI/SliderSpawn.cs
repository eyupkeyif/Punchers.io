using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSpawn : MonoBehaviour
{
    [SerializeField] GameObject sliderCanvas;
    [SerializeField] Transform punchSliderPos;
    CharacterController controller;
    SliderFollow sliderFollow;
    SetSliderSpawn setSlider;
    public Slider slider;
    public void SetupSlider()
    {
        controller = GetComponent<CharacterController>();
        
        GameObject punchSlider = Instantiate(sliderCanvas, punchSliderPos.position, Quaternion.identity);
        punchSlider.SetActive(true);
        sliderFollow = punchSlider.GetComponent<SliderFollow>();
        setSlider = punchSlider.GetComponent<SetSliderSpawn>();
        slider = setSlider.SetSliderBar();
        sliderFollow.SetupSlider(gameObject, controller.height, punchSliderPos);

       
    }

    public void DestroySlider(int currentHealth) {

        sliderFollow.DestroySlider(currentHealth);

    }


}
