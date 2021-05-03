using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StudentManager : Singleton<StudentManager>
{
    [System.Serializable]
    public class StudentBasicInfo
    {
        public StudentData data;
        public int score;
    }
    
    
    public RectTransform content;
    public Button clear;
    public Button back;
    
    public List<StudentBasicInfo> StudentsList = new List<StudentBasicInfo>();
    public List<StudentData> ArrangedStudentList=new List<StudentData>();

    void Awake()
    {
        clear.onClick.AddListener(Clear);
        back.onClick.AddListener(BackToMain);
    }
    
    private void OnEnable()
    {
        foreach (Transform i in content.transform)
        {
            Destroy(i.gameObject);
        }
        Upload.Instance.DonwloadFile();
        Upload.Instance.SetupStudentList();
        StudentUI.Instance.Clear();
    }

    public void RefreshUI()
    {
        foreach (Transform i in content.transform)
        {
            Destroy(i.gameObject);
        }

        StudentsList.Clear();
        Upload.Instance.DonwloadFile();
        StudentUI.Instance.Clear();
    }
    
    //TODO:计算每个单词错误数量
    
    public void Clear()
    {
        /*foreach (var file in StudentsList)
        {
            if(DeleteFile(file.studentName + ".json"))
                print("True");
            else
            {
                print("false");
            }
            DeleteFile(file.studentName + ".json");
            Debug.Log("删除:"+file.studentName + ".json");
        }*/
        StartCoroutine(delete());
        
    }
    
    public IEnumerator DeleteFile(string fileName)
    {
        try
        {
            string uri = "ftp://"+Upload.Instance.ServerIP + "/data2/" + fileName;
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

            reqFTP.Credentials = new NetworkCredential("Xiaochang", "MangoLiu2190");
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

            string result = String.Empty;
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            long size = response.ContentLength;
            Stream datastream = response.GetResponseStream();
            StreamReader sr = new StreamReader(datastream);
            result = sr.ReadToEnd();
            sr.Close();
            datastream.Close();
            response.Close();
            //Buffer.Log(string.Format("Ftp文件{1}删除成功！", DateTime.Now.ToString(), fileName));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        yield break;
    }

    IEnumerator delete()
    {
        foreach (var file in ArrangedStudentList)
        {
            StartCoroutine(DeleteFile(file.studentName + ".json"));
            Debug.Log("删除:"+file.studentName + ".json");
        }
        RefreshUI();
        yield break;
    }

    private void BackToMain()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
