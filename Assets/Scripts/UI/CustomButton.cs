using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void ActionDelegate();

    public ActionDelegate onPointerDownEvent, onPointerUpEvent;

    [SerializeField] MMF_Player onPointerUpFeel, onPointerDownFeel;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onPointerDownFeel!=null)
        {
            onPointerDownFeel.PlayFeedbacks();
        }
        onPointerDownEvent?.Invoke();

    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (onPointerUpFeel!=null)
        {
            onPointerUpFeel.PlayFeedbacks();
        }
        onPointerUpEvent?.Invoke();
    }

    public void SetEnabled(bool enabled)
    {

        if (enabled)
        {
            gameObject.SetActive(true);
        }

        else
        {
            gameObject.SetActive(false);
        }
    }
}
