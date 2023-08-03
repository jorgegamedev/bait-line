using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

// Used to override the method through which WebGL links are opened.
public class PressHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [Serializable]
    public class ButtonPressEvent : UnityEvent { }

    // Stores the type of press event.
    public ButtonPressEvent OnPress = new ButtonPressEvent();

    // Checks for pointer events (WebGL).
    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_WEBGL
        OnPress.Invoke();
#endif
    }

    // Checks for click events (Click).
    public void OnPointerClick(PointerEventData eventData)
    {
#if !UNITY_WEBGL
        OnPress.Invoke();
#endif
    }
}
