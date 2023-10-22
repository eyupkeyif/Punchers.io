using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    [SerializeField] private RectTransform canvasParent;
    public abstract void Initialize();

    public virtual void Hide() => gameObject.SetActive(false);

    public virtual void Show() => gameObject.SetActive(true);


}
