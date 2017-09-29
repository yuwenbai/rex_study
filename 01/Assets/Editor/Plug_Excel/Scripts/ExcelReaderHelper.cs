/****************************************************
*
*  读取Exl表格到SqLiter数据库工具类
*  作者: lyb
*  日期：2017年7月25日
*
*****************************************************/

using UnityEngine;
using Mono.Data.Sqlite;

namespace PlugProject
{
    public class ExcelReaderHelper
    {
        #region 判断该数据表是否存在 -------------------------------

        /// <summary>
        /// 判断该数据表是否存在
        /// </summary>
        public static bool IsTableExist(string tableName, SQLiteHelper sql)
        {
            bool isTabel = false;

            //SQLiteHelper sql = new SQLiteHelper("data source=" + Application.dataPath + "/Editor/SQLiter/Databases/game_conf.db");

            string sqlStr = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", tableName);

            SqliteDataReader reader = sql.ExecuteQuery(sqlStr);

            if (reader.Read())
            {
                PlugMenuItemMain.PlugLog_Show(" #[ExcelReaderHelper]# - { " + tableName + " } 表存在执行删除操作");

                string deleteStr = string.Format("delete from '{0}'", tableName);
                sql.ExecuteQuery(deleteStr);

                isTabel = true;
            }
            else
            {
                PlugMenuItemMain.PlugLog_Show(" #[ExcelReaderHelper]# - { " + tableName + " } 数据表不存在 , 添加");
            }

            reader.Close();

            return isTabel;
        }

        #endregion --------------------------------------------------
    }
}