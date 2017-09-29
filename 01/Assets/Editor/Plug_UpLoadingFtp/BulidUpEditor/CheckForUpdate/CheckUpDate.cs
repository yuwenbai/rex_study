/**
 * @Author GarFey
 * 检查更新上传数据类
 */
using System.Collections.Generic;
using System.Linq;

namespace BuildUpdateEditor
{
    public class CheckUpDate
    {
        public static CheckUpDate Instan;
        public static CheckUpDate GetInstance
        {
            get { return Instan == null ? Instan = new CheckUpDate() : Instan; }
        }
        private Dictionary<string, CheckListInfo> mFileInfoDic = new Dictionary<string, CheckListInfo>();
        /// <summary>
        /// 比较列表刷新Dic
        /// </summary>
        public Dictionary<string, CheckListInfo> getFileInfoDic
        {
            get { return mFileInfoDic; }
        }

        /// <summary>
        /// 添加ListInfo for FileInfoDic
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="info"></param>
        public void AddOrReviseFileInfoDic(string fileName,CheckListInfo info)
        {
            if(mFileInfoDic!=null&&mFileInfoDic.ContainsKey(fileName))
            {
                mFileInfoDic.Remove(fileName);
            }
            mFileInfoDic.Add(fileName, info);
        }
        /// <summary>
        /// 删除ListInfo for FileInfoDic
        /// </summary>
        /// <param name="fileName"></param>
        public void DelFileInfoDic(string fileName)
        {
            if (mFileInfoDic != null && mFileInfoDic.ContainsKey(fileName))
            {
                mFileInfoDic.Remove(fileName);
            }
        }
        /// <summary>
        /// 清空FileInfoDic
        /// </summary>
        public void ClearFileInfoDic()
        {
            if (mFileInfoDic != null)
            {
                mFileInfoDic.Clear();
            }
        }
        /// <summary>
        /// 查找ListInfo for FileInfoDic By fileName
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public CheckListInfo FindFileInfoDicByName(string fileName)
        {
            CheckListInfo mclInfo = null;
            if (mFileInfoDic != null && mFileInfoDic.ContainsKey(fileName))
            {
                mclInfo = mFileInfoDic[fileName];
            }
            return mclInfo;
        }
        /// <summary>
        /// 排序
        /// </summary>
        public void OrderByFileInfoDic()
        {
            Dictionary<string, CheckListInfo> tmpDic = CheckUpDate.GetInstance.getFileInfoDic.
                OrderBy(a => a.Value.mfileState).
                ThenBy(b=>b.Value._FileVersion).
                ThenBy(bc=>bc.Value._FileName).
                ToDictionary(p => p.Key, v => v.Value);
            mFileInfoDic = tmpDic;
        }

        public void OnDestroy()
        {
            ClearFileInfoDic();
        }
    }
}
