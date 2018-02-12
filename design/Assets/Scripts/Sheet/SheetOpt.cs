using System.IO;
using UnityEngine;
using ExcelDataReader;
using System.Data;
using System.Reflection;
using EPPlusSamples;
using System;

public class SheetOpt : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ReadOneExcel("111.xlsx");
        
        DirectoryInfo outputDir = new DirectoryInfo(@"E:\SampleApp");
        if (!outputDir.Exists) throw new Exception("outputDir does not exist!");
        SheetWrite.RunSample12("", outputDir, @"\sample12.xlsx");
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

        FileStream stream = File.Open(Application.streamingAssetsPath + "/" + fileName, FileMode.Open, FileAccess.Read);
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
        }
        
    }
}
