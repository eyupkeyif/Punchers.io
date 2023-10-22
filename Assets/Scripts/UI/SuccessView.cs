using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SuccessView : View
{
    [SerializeField] GameObject successParticle;
    public delegate void ActionDelegate();
    public ActionDelegate SuccessButtonEvent;

    [SerializeField] CustomButton SuccessButton;

    public override void Initialize()
    {
        SuccessButton.onPointerUpEvent += NextLevel;
    }

    public void NextLevel()
    {
        SuccessButtonEvent?.Invoke();
    }

    public void SuccessParticlePlay()
    {

        GameObject success = Instantiate(successParticle, gameObject.transform);
        
    }

}
