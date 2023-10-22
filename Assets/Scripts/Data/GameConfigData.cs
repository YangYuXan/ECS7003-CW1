using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigData
{
    private List<Dictionary<string, string>> dataDic;

    public GameConfigData(string str)
    {
        dataDic = new List<Dictionary<string, string>>();

        // �����и�
        string[] lines = str.Split('\n');
        // ��һ���Ǵ洢���ݵ�����
        string[] title = lines[0].Trim().Split('\t');
        // �ӵ������±�2��ʼ���������ݣ��ڶ��������ǽ���˵��
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
