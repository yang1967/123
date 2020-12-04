using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AssetPath 
{
    /// <summary>
    /// 是否是文件
    /// </summary>
    public static bool IsFile(string path)
    {
        return Regex.IsMatch(path,"\\.");
    }
}
