using System.IO;
using UnityEngine;
using ExcelDataReader;
using System.Data;
public class SheetOpt : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ReadOneExcel("111.xlsx");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void GetExcels(DirectoryInfo directoryInfo)
    {

    }
    void ReadOneExcel(string fileName)
    {
        //xml.Load(Application.dataPath + "/赛事后台需求_20170412_mjc.xlsx");

        FileStream stream = File.Open(Application.dataPath + "/" + fileName, FileMode.Open, FileAccess.Read);
        //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        int columns = result.Tables[0].Columns.Count;//获取列数  
        int rows = result.Tables[0].Rows.Count;//获取行数  


        //从第二行开始读  
        for (int i = 1; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                string nvalue = result.Tables[0].Rows[i][j].ToString();
                Debug.Log(nvalue);
            }

            //创建xml文档
            //FileInfo newFile = new FileInfo(Application.dataPath + "/赛事后台需求_20170412_mjc.xlsx");

            //using (ExcelPackage pck = new ExcelPackage(newFile))
            //{

            //}



            //ExcelWorksheet sheet = new ExcelWorksheet();
            //string filepath = Application.dataPath + "/StreamingAssets" + "/" + fileName;
            //FileStream fs;
            //using (fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    XSSFWorkbook  book = new XSSFWorkbook(fs);
            //    Debug.Log("rextest sheetName is " + book.GetSheetName(0));
            //    fs.Close();
            //}
        }
    }
}
