using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetSliderSpawn : MonoBehaviour
{
    [SerializeField] Slider _slider;
    public Slider SetSliderBar()
    {
        Slider slider = Instantiate(_slider, transform);
        return slider;
    }
}
