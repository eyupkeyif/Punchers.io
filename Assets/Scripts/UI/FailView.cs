using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailView : View
{
    [SerializeField] GameObject failParticle;
    public delegate void ActionDelegate();
    public ActionDelegate FailButtonEvent;
    [SerializeField] CustomButton failButton;

    public override void Initialize()
    {
        failButton.onPointerUpEvent += RestartLevel;
    }

    public void RestartLevel()
    {
        FailButtonEvent?.Invoke();

    }

    public void FailParticlePlay()
    {
        GameObject fail = Instantiate(failParticle, gameObject.transform);
    }
}
