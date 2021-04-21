using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class StudentManager : Singleton<StudentManager>
{
    public RectTransform content;
    public Button clear;
    
    [System.Serializable]
    public class StudentDataList
    {
        public StudentData StudentData;
    }
    
    public List<StudentData> StudentsList = new List<StudentData>();

    void Awake()
    {
        clear.onClick.AddListener(Clear);
    }
    
    private void OnEnable()
    {
        foreach (Transform i in content.transform)
        {
            Destroy(i.gameObject);
        }
        Upload.Instance.DonwloadFile();
    }

    public void RefreshUI()
    {
        foreach (Transform i in content.transform)
        {
            Destroy(i.gameObject);
        }
        //Upload.Instance.DonwloadFile();
    }
    
    //TODO:计算每个单词错误数量
    
    public void Clear()
    {
        foreach (var file in StudentsList)
        {
            if(DeleteFile(file.studentName + ".json"))
                print("True");
            else
            {
                print("false");
            }
            DeleteFile(file.studentName + ".json");
            Debug.Log("删除:"+file.studentName + ".json");
        }
        RefreshUI();
    }
    
    public static bool DeleteFile(string fileName)
    {
        try
        {
            string url = "ftp://"+Upload.Instance.ServerIP+"/data2/" + fileName;
            FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
            reqFtp.UseBinary = true;
            reqFtp.KeepAlive = false;
            reqFtp.Method = WebRequestMethods.Ftp.DeleteFile;
            reqFtp.Credentials = new NetworkCredential("Xiaochang", "MangoLiu2190");
            FtpWebResponse response = (FtpWebResponse)reqFtp.GetResponse();
            response.Close();
            return true;
        }
        catch (Exception ex)
        {
            //errorinfo = string.Format("因{0},无法下载", ex.Message);
            return false;
        }
    }
}
