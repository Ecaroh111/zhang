using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

public class ClickRecorder : IRecorder
{
    private const string clickRecord = "clickRecord";
    
    private readonly List<ClickRecordInfo> mClickInfos = new();

    public void Reset()
    {
        mClickInfos.Clear();
    }

    public void Add(int id)
    {
        mClickInfos.Add(new ClickRecordInfo
        {
            id = id,
            time = DateTime.UtcNow
        });
    }

    public void RecordToFile(IWorkbook workbook)
    {
        var excelSheet = workbook.CreateSheet(clickRecord);
        var row = excelSheet.CreateRow(0);
        row.CreateCell(0).SetCellValue("number");
        row.CreateCell(1).SetCellValue("time");

        int rowIndex = 1;
        foreach (var info in mClickInfos)
        {
            row = excelSheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(info.id.ToString());
            row.CreateCell(1).SetCellValue(info.time.ToString());
        }
    }
}

public class ClickRecordInfo
{
    public int id;
    public DateTime time;
}