using System;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private float mTime = 0f;
    private bool mIsStart = false;

    private void Awake()
    {
        ShowTime();
    }

    public void ResetTime()
    {
        mTime = 0f;
        mIsStart = true;
    }

    public void Stop()
    {
        mIsStart = false;
    }

    private void Update()
    {
        if (!mIsStart) return;

        mTime += Time.deltaTime;
        ShowTime();
    }

    private void ShowTime()
    {
        timeText.text = new TimeSpan(0, 0, Mathf.FloorToInt(mTime)).ToString("c");
    }
}