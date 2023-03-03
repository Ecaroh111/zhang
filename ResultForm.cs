using NPOI.XSSF.UserModel;
using System.IO;
using TMPro;
using UnityEngine;
using System;

public class ResultForm : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI pathText;

    private string mSavePath;

    private void Awake()
    {
        mSavePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "record " + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx");
        pathText.text = mSavePath;
    }

    public void Open(string useTime, params IRecorder[] recorders)
    {
        gameObject.SetActive(true);
        timeText.text = useTime;

        var workbook = new XSSFWorkbook();
        foreach (var recorder in recorders)
        {
            recorder.RecordToFile(workbook);
        }

        using var fs = new FileStream(mSavePath, FileMode.Create, FileAccess.Write);
        workbook.Write(fs);
    }
}
