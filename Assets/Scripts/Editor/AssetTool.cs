using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class AssetTool
{

    /// <summary>
    /// 获得路径
    /// </summary>
    public static string GetPath(string _str)
    {
        return _str.Replace("\\","/");
    }
    /// <summary>
    /// 检查文件
    /// </summary>
    private static bool CheckAsset(string _path)
    {
        string path = GetPath(_path);
        string dirPath = path.Substring(0,path.LastIndexOf("/"));
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (!File.Exists(path))
        {
            File.Create(path).Close();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 写入内容
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="content">写入的内容</param>
    public static void WiteContentToFile(string path,string content)
    {
        if (true == CheckAsset(path))
        {
            FileStream fStream = new FileStream(path,FileMode.OpenOrCreate);
          
            //写入内容
            StreamWriter sWriter = new StreamWriter(fStream,System.Text.Encoding.UTF8); 
            sWriter.Write(content);
            sWriter.Flush();
            sWriter.Close();
            fStream.Close();
        }
    }
    /// <summary>
    /// 得到文件内容
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns>文件内容</returns>
    public static string GetFileContent(string path)
    {
         return  File.ReadAllText(path);
    }
    /// <summary>
    /// 读取文件字节
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>字节数组</returns>
    public static byte[] ReadByte(string path)
    {
        return File.ReadAllBytes(path);
    }

    /// <summary>
    /// 选择的文件夹和文件
    /// </summary>
    /// <returns></returns>
    public static (List<DirectoryInfo> directoryInfos, List<FileInfo> fileInfos) GetSelectAssets()
    {
        List<DirectoryInfo> _directoryInfos = new List<DirectoryInfo>();
        List<FileInfo> _fileInfos = new List<FileInfo>();
        string[] guids = Selection.assetGUIDs;
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (AssetPath.IsFile(path))
            {
                _fileInfos.Add(new FileInfo(path));
            }
            else
            {
                _directoryInfos.Add(new DirectoryInfo(path));
            }
        }
        return (_directoryInfos, _fileInfos);
    }

    public static void WriteContentToFile(string pathPanelName, string content)
    {
        WiteContentToFile(pathPanelName, content);
    }
}
