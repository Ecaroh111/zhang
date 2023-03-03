using NPOI.SS.UserModel;

public interface IRecorder
{
    void RecordToFile(IWorkbook workbook);
}