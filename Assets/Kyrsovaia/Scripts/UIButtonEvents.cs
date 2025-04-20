using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonEvents : EventTrigger
{
    public UnityEvent OnButtonDown = new UnityEvent();
    public UnityEvent OnButtonUp = new UnityEvent();

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        OnButtonDown.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        OnButtonUp.Invoke();
    }
}
