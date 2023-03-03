using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public Image image;

    private Tween mTween;

    private int mId = 0;
    public int Id
    {
        get => mId;
        set
        {
            mId = value;
            numberText.text = mId.ToString();
        }
    }

    public event Action<int> OnClick;
    public event Action<int> OnDown;

    private void Awake()
    {
        var button = GetComponentInChildren<NumberButton>();
        button.OnClick += () =>
        {
            OnClick?.Invoke(Id);
        };
        button.OnDown += () =>
        {
            OnDown?.Invoke(Id);
        };
    }

    public void RegisterEvent(Action<int> clickAction, Action<int> downAction)
    {
        OnClick = clickAction;
        OnDown = downAction;
    }

    public void DownDisplay(bool isRight)
    {
        mTween?.Kill();
        mTween = image.DOColor(isRight ? Color.grey : Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
}
