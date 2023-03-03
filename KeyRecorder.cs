using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyRecorder : MonoBehaviour, IRecorder
{
    private const string keyRecord = "keyRecord";

    public float keyInterval = 5f;
    public float longPressTime = 1f;

    private readonly List<KeyRecordInfo> mKeyInfos = new();
    private bool mIsStart = false;
    private readonly KeyCode[] keys = (KeyCode[])Enum.GetValues(typeof(KeyCode));

    public void ResetKey()
    {
        mKeyInfos.Clear();
        mIsStart = true;
    }

    public void Stop()
    {
        mIsStart = false;
    }

    public void Update()
    {
        if (!mIsStart) return;

        foreach (KeyCode keyCode in keys)
        {
            if (keyCode == KeyCode.Mouse0) continue;
            if (keyCode == KeyCode.Mouse1) continue;
            if (keyCode == KeyCode.Mouse2) continue;
            if (keyCode == KeyCode.Mouse3) continue;
            if (keyCode == KeyCode.Mouse4) continue;
            if (keyCode == KeyCode.Mouse5) continue;
            if (keyCode == KeyCode.Mouse6) continue;

            if (Input.GetKeyDown(keyCode))
            {
                var recordInfo = mKeyInfos.Find(x => x.key == keyCode);
                if (recordInfo == null)
                {
                    recordInfo = new KeyRecordInfo { key = keyCode };
                    mKeyInfos.Add(recordInfo);
                }

                recordInfo.pressTime = DateTime.UtcNow;
            }

            if (Input.GetKeyUp(keyCode))
            {
                var recordInfo = mKeyInfos.Find(x => x.key == keyCode);
                var now = DateTime.UtcNow;
                if ((now - recordInfo.pressTime).TotalSeconds >= longPressTime)
                {
                    recordInfo.longTimes.Add(recordInfo.pressTime);
                    recordInfo.longTimes.Add(now);
                }
                else
                {
                    recordInfo.times.Add(recordInfo.pressTime);
                }
            }
        }
    }

    public void RecordToFile(IWorkbook workbook)
    {
        var excelSheet = workbook.CreateSheet(keyRecord);
        var row = excelSheet.CreateRow(0);
        row.CreateCell(0).SetCellValue("key");
        row.CreateCell(1).SetCellValue("time");

        int rowIndex = 1;
        foreach (var info in mKeyInfos)
        {
            row = excelSheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(info.key.ToString());

            //var splitTimes = SplitTimes(info.times);
            //for (var i = 0; i < splitTimes.Count; i++)
            //{
            //    string time;
            //    if (splitTimes[i].Count == 1)
            //        time = splitTimes[i][0].ToString();
            //    else
            //    {
            //        var subTimes = splitTimes[i];
            //        time = subTimes[0].ToString() + " - " + subTimes[^1].ToString();
            //    }
            //    row.CreateCell(i + 1).SetCellValue(time + " " + splitTimes[i].Count);
            //}

            row = excelSheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(info.key.ToString());
            for (var i = 0; i < info.times.Count; i++)
            {
                row.CreateCell(i + 1).SetCellValue(info.times[i].ToString());
            }

            if (info.longTimes.Count <= 0) continue;

            row = excelSheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(info.key.ToString() + " Long");
            var cellIndex = 0;
            for (var i = 0; i < info.longTimes.Count; i += 2)
            {
                var startTime = info.longTimes[i];
                var endTime = info.longTimes[i + 1];
                row.CreateCell(++cellIndex).SetCellValue(startTime.ToString() + " - " + endTime.ToString() + " " + (endTime - startTime).TotalSeconds);
            }
        }
    }

    private List<List<DateTime>> SplitTimes(List<DateTime> times)
    {
        var splitTimes = new List<List<DateTime>>();
        for (var i = 0; i < times.Count;)
        {
            var index = i;
            var subTimes = new List<DateTime>();
            for (; index < times.Count; index++)
            {
                if (index == i || (times[index] - times[index - 1]).TotalSeconds <= keyInterval)
                    subTimes.Add(times[index]);
                else
                    break;
            }
            splitTimes.Add(subTimes);

            i = index;
        }

        return splitTimes;
    }
}

public class KeyRecordInfo
{
    public KeyCode key;
    public List<DateTime> times = new();
    public List<DateTime> longTimes = new();
    public DateTime pressTime;
}