using System;
using System.Collections.Generic;
using UnityEngine;

public class NumberManager : MonoBehaviour
{
    public static NumberManager instance;

    private readonly Dictionary<int, Number> mNumberMap = new();
    private int mNextNumber = 1;
    private List<int> mIds = new();

    public event Action<int> OnNext;
    public event Action OnReset;
    public event Action OnWin;

    private void Awake()
    {
        instance = this;

        Layout();
    }

    public void Layout()
    {
        var numbers = GetComponentsInChildren<Number>();
        if (mIds.Count <= 0)
        {
            for (var i = 1; i <= numbers.Length; ++i)
            {
                mIds.Add(i);
            }
        }
        mIds = RandomSort(mIds);

        mNumberMap.Clear();
        for (var i = 0; i < numbers.Length; ++i)
        {
            var number = numbers[i];
            number.Id = mIds[i];
            number.RegisterEvent(OnNumberClick, OnNumberDown);

            mNumberMap[number.Id] = number;
        }

        mNextNumber = 1;
    }

    private List<int> RandomSort(List<int> list)
    {
        var newList = new List<int>(list.Count);
        var random = new System.Random();
        foreach (var number in list)
        {
            newList.Insert(random.Next(newList.Count + 1), number);
        }

        return newList;
    }

    private void OnNumberClick(int id)
    {
        if (id != mNextNumber)
        {
            ResetNumber();
        }
        else
        {
            OnNext?.Invoke(mNextNumber);
            if (mNextNumber == mNumberMap.Count)
            {
                OnWin?.Invoke();
            }
            ++mNextNumber;
        }
    }

    private void OnNumberDown(int id)
    {
        var number = mNumberMap[id];
        number.DownDisplay(id == mNextNumber);
    }

    private void ResetNumber()
    {
        mNextNumber = 1;
        OnReset?.Invoke();
    }
}
