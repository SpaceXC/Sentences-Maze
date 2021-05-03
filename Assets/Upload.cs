using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Net;

public class Upload : Singleton<Upload>
{

	NetworkCredential credential=new NetworkCredential("Xiaochang","MangoLiu2190");
	public StudentNameButton studentNameButton;

	public string ServerIP;
	public StudentManager studentManager;
	
	public void TxtWrite( string strToWrite , string filename)
    {
        FileStream fs = new FileStream( filename , FileMode . Create , FileAccess . Write);
        StreamWriter sw = new StreamWriter( fs);
        sw . BaseStream . Seek( 0 , SeekOrigin . Begin);
        sw . WriteLine( strToWrite);
        sw . Close();
    }

	public void UploadFile(string str, string filename)
	{
		//如果文件存在，删除文件
		if (Directory.Exists(Application.persistentDataPath+"/"+filename+".json"))
		{
			DelectDir(Application.persistentDataPath+filename+".json");
			Debug.Log("Deleted file:"+filename+".json!");
		}
		TxtWrite(str,Application.persistentDataPath+"/"+filename+".json");
		Debug.Log("Saved File!");
		FtpUploadFile(Application.persistentDataPath+"/"+filename+".json", "ftp://"+ServerIP+"/data2/"+filename+".json", "Xiaochang", "MangoLiu2190");
		Debug.Log("Uploaded File!");
	}

	public void DonwloadFile()
	{
		DeleteDirectory(Application.persistentDataPath+"/"+"data2");
		CreateDirectory(Application.persistentDataPath+"/"+"data2");
		DownloadFtpDirectory("ftp://"+ServerIP+"/data2/",credential,Application.persistentDataPath+"/"+"Data2");
		Debug.Log("Downloaded files!");
		Files2Json();
	}
	
	// Use FTP to upload a file.
	public void FtpUploadFile(string filename, string to_uri, string user_name, string password)
	{
		// Get the object used to communicate with the server.
		FtpWebRequest request =
			(FtpWebRequest)WebRequest.Create(to_uri);
		request.Method = WebRequestMethods.Ftp.UploadFile;

		// Get network credentials.
		request.Credentials =
			new NetworkCredential(user_name, password);

		// Read the file's contents into a byte array.
		byte[] bytes = System.IO.File.ReadAllBytes(filename);

		// Write the bytes into the request stream.
		request.ContentLength = bytes.Length;
		using (Stream request_stream = request.GetRequestStream())
		{
			request_stream.Write(bytes, 0, bytes.Length);
			request_stream.Close();
		}
	}
	
	public static void DelectDir(string srcPath)
	{
		try
		{
			DirectoryInfo dir = new DirectoryInfo(srcPath);
			FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
			foreach (FileSystemInfo i in fileinfo)
			{                  
				File.Delete(i.FullName);      //删除指定文件
			}
		}
		catch 
		{
			throw;
		}
	}
	
	public void DownloadFtpDirectory(string url, NetworkCredential credentials, string localPath) 
	{
		FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
		listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
		listRequest.Credentials = new NetworkCredential("Xiaochang","MangoLiu2190");

		List<string> lines = new List<string>();

		using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
		using (Stream listStream = listResponse.GetResponseStream())
		using (StreamReader listReader = new StreamReader(listStream))
		{
			while (!listReader.EndOfStream)
			{
				lines.Add(listReader.ReadLine());
			} 
		}

		foreach (string line in lines)
		{
			string[] tokens =
				line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
			string name = tokens[8];
			string permissions = tokens[0];

			string localFilePath = Path.Combine(localPath, name);
			string fileUrl = url + name;

			if (permissions[0] == 'd')
			{
				if (!Directory.Exists(localFilePath))
				{
					Directory.CreateDirectory(localFilePath);
				}

				DownloadFtpDirectory(fileUrl + "/", credentials, localFilePath);
			}
			else
			{
				FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
				downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
				downloadRequest.Credentials = credentials;

				using (FtpWebResponse downloadResponse =
						(FtpWebResponse)downloadRequest.GetResponse())
				using (Stream sourceStream = downloadResponse.GetResponseStream())
				using (Stream targetStream = File.Create(localFilePath))
				{
					byte[] buffer = new byte[10240];
					int read;
					while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						targetStream.Write(buffer, 0, read);
					}
				}
			}
		}
	}
	
	public static void CreateDirectory(string newDirectoryName)
	{
		if (!Directory.Exists(newDirectoryName))
		{
			Directory.CreateDirectory(newDirectoryName);
		}
	}
	
	void DeleteDirectory(string path)
	{
		DirectoryInfo dir = new DirectoryInfo(path); 
		if (dir.Exists)
		{
			DirectoryInfo[] childs = dir.GetDirectories();
			foreach (DirectoryInfo child in childs)
			{
				child.Delete(true);
			}
			dir.Delete(true);
		}
	}

	public void Files2Json()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath+"/"+"Data2");

		FileInfo[] files = directoryInfo.GetFiles();

		for (int i = 0; i < files.Length; i++)

		{

			if(files[i].Extension.Equals(".json"))   //判断是否为json文件

			{

				string[] strs = File.ReadAllLines(Application.persistentDataPath+"/"+"Data2" + "/" + files[i].Name);	//文本文件完整路径

				for (int j = 0; j < strs.Length; j++)

				{
					string file = strs[j].ToString();
					Debug.Log(file);
					
					var newData = ScriptableObject.CreateInstance<StudentData>();
					JsonUtility.FromJsonOverwrite(file,newData);
					/*studentManager.StudentsList.Add(new StudentData
					{
						score = newData.score, studentName = newData.studentName, completion = newData.completion,
						WrongOptions = newData.WrongOptions
					});*/
					studentManager.StudentsList.Add(new StudentManager.StudentBasicInfo()
						{data = newData, score = newData.score});
				}

			}

		}
		
		
	}

	public void SetupStudentList()
	{
		for (int i = 0; i < studentManager.StudentsList.Count+studentManager.ArrangedStudentList.Count; i++)
		{
			var maxValue = studentManager.StudentsList.Max(t => t.score);
			Debug.Log(maxValue);
			Debug.Log(studentManager.StudentsList.FindIndex(max_num => max_num.score == maxValue));
			studentManager.ArrangedStudentList.Add(studentManager.StudentsList[studentManager.StudentsList.FindIndex(max_num => max_num.score == maxValue)].data);
			studentManager.StudentsList.RemoveAt(studentManager.StudentsList.FindIndex(max_num => max_num.score == maxValue));
		}
		
		foreach (var student in studentManager.ArrangedStudentList)
		{
			var newStudent = Instantiate(studentNameButton, studentManager.content);
			Debug.Log(student.studentName);
			newStudent.SetupStudentNameButton(student);
		}
	}
}
