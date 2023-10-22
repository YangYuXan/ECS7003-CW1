using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigData
{
    private List<Dictionary<string, string>> dataDic;

    public GameConfigData(string str)
    {
        dataDic = new List<Dictionary<string, string>>();

        // 换行切割
        string[] lines = str.Split('\n');
        // 第一行是存储数据的类型
        string[] title = lines[0].Trim().Split('\t');
        // 从第三行下标2开始）遍历数据，第二行数据是解释说明
        for(int i = 2;  i < lines.Length; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] tempArr = lines[i].Trim().Split("\t");

            for(int j = 0; j < tempArr.Length; j++)
            {
                dic.Add(title[j], tempArr[j]);
            }
        }
    }
}
