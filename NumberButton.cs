using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumberButton : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public event Action OnClick;
    public event Action OnDown;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke();
    }
}
