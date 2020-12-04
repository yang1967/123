using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
public partial class TemPlateScript
{
    private static readonly string[] Key =
    {
        "Tra",
        "Img",
        "Btn",
        "Txt",
        "Drop",
        "Solo"
    };
    private static readonly string[] Value =
    {
        "Transform",
        "Image",
        "Button",
        "Text",
        "Dropdown",
        "Transform"
    };
    private static Dictionary<string, string> ObjNameDics = null;
}
public partial class TemPlateScript : Editor
{
    /// <summary>
    /// 构造函数
    /// </summary>
    static TemPlateScript()
    {
        ObjNameDics = new Dictionary<string, string>();
        for (int i = 0; i < Key.Length; i++)
        {
            ObjNameDics.Add(Key[i], Value[i]);
        }
    }
    /// <summary>
    /// 创建模板脚本
    /// </summary>
    [MenuItem("Assets/Tools/CreatTPScript", priority = 0)]
    public static void CreatPageScript()
    {
        GameObject[] selectObjects = Selection.gameObjects;
        foreach (GameObject selectObj in selectObjects)
        {
            CreatScript(selectObj);
        }
    }
    private static void CreatScript(GameObject _obj)
    {
        string path_UIPanel = Application.dataPath + "/Scripts/UI/Panel/" + $"{_obj.transform.name}.cs";

        List<string> allName = _obj.transform.GetAllChildName();
        List<string> targetName = new List<string>();
        foreach (string name in allName)
        {
            if (ObjNameDics.ContainsKey(name.Split('_')[0]))
            {
                targetName.Add(name);
            }
        }
        //主体脚本
        StringBuilder mainScript = new StringBuilder(BaseTemplate.NewMainClass);
        //替换的内容
        StringBuilder replacePanelName = new StringBuilder();
        StringBuilder replaceDeclareUI = new StringBuilder();
        StringBuilder replaceInitializeUI = new StringBuilder();
        StringBuilder replaceRelease = new StringBuilder();
        foreach (string name in targetName)
        {
            replacePanelName.Append($"public const string {name} = \"{name}\";\n\t\t");
            replaceDeclareUI.Append($"private {ObjNameDics[name.Split('_')[0]]} {name} = null;\n\t\t");
            replaceInitializeUI.Append($"{name} = transform.FindChildComponent<{ObjNameDics[name.Split('_')[0]]}>({_obj.transform.name}UI.{name});\n\t\t");
            replaceRelease.Append($"{name} = null;\n\t\t");
        }
        List<string> allPath = new List<string>(AssetDatabase.GetAllAssetPaths());
        List<string> l = allPath.FindAll(it => it.Contains("Assets/Scripts"));

        string s = path_UIPanel.Substring(path_UIPanel.IndexOf("Assets"), path_UIPanel.Length - path_UIPanel.IndexOf("Assets"));

        mainScript = mainScript.Replace("#UIPanel#", _obj.transform.name);
        mainScript = mainScript.Replace("#declareUI#", replaceDeclareUI.ToString());
        mainScript = mainScript.Replace("#Initialize#", replaceInitializeUI.ToString());
        mainScript = mainScript.Replace("#OnRelease#", replaceRelease.ToString());

        mainScript = mainScript.Replace("#UIName#", _obj.transform.name + "UI");
        mainScript = mainScript.Replace("#UINames#", replacePanelName.ToString());

        if (l.Contains(s))
        {
            string allContent = AssetTool.GetFileContent(path_UIPanel);
            string before = allContent.Substring(0, allContent.IndexOf("#region InitializeUI"));
            string T = mainScript.ToString();
            string updateStr = before + T.Substring(T.IndexOf("#region InitializeUI"), T.Length - T.IndexOf("#region InitializeUI"));
            AssetTool.WriteContentToFile(path_UIPanel, updateStr);
            Debug.Log(path_UIPanel.Split('/')[path_UIPanel.Split('/').Length - 1] + "脚本更新成功!!!");
        }
        else
        {
            AssetTool.WriteContentToFile(path_UIPanel, mainScript.ToString());
        }
    }
    /// <summary>
    /// 创建Solo模板脚本
    /// </summary>
    [MenuItem("Assets/Tools/CreatSoloTPScript", priority = 0)]
    public static void CreatSoloScript()
    {
        GameObject[] selectObjects = Selection.gameObjects;
        foreach (GameObject selectObj in selectObjects)
        {
            CreatSoloScript(selectObj);
        }
    }
    private static void CreatSoloScript(GameObject _obj)
    {
        string path = string.Format("{0}{1}{2}{3}",Application.dataPath,"/SoloScript/", _obj.name,".cs");

        List<string> allName = _obj.transform.GetAllChildName();
        List<string> targetName = new List<string>();
        foreach (string name in allName)
        {
            if (ObjNameDics.ContainsKey(name.Split('_')[0]))
            {
                targetName.Add(name);
            }
        }
        //主体脚本
        StringBuilder mainScript = new StringBuilder(BaseTemplate.SoloClass);
        //替换的内容
        StringBuilder replaceDeclareUI = new StringBuilder();
        StringBuilder replaceInitializeUI = new StringBuilder();
        StringBuilder replaceRelease = new StringBuilder();
        StringBuilder replaceSoloReturn = new StringBuilder();
        foreach (string name in targetName)
        {
            replaceDeclareUI.Append($"private {ObjNameDics[name.Split('_')[0]]} {name} = null;\n\t\t");
            replaceInitializeUI.Append($"{name} = transform.FindChildComponent<{ObjNameDics[name.Split('_')[0]]}>(\"{name}\");\n\t\t");
            replaceRelease.Append($"{name} = null;\n\t\t");        
        }
        string name1 = _obj.name.Replace(_obj.name.Substring(0, 1), _obj.name.Substring(0, 1).ToLower());
        replaceSoloReturn.Append($"{_obj.name} {name1} = cloneObj.AddComponent<{_obj.name}>();\n{name1}.Initialize();\nreturn {name1};\n");

        List<string> allPath = new List<string>(AssetDatabase.GetAllAssetPaths());
        List<string> l = allPath.FindAll(it => it.Contains("Assets/Scripts"));

        string s = path.Substring(path.IndexOf("Assets"), path.Length - path.IndexOf("Assets"));

        mainScript = mainScript.Replace("#UIPanel#", _obj.transform.name);
        mainScript = mainScript.Replace("#declareUI#", replaceDeclareUI.ToString());
        mainScript = mainScript.Replace("#Initialize#", replaceInitializeUI.ToString());
        mainScript = mainScript.Replace("#OnRelease#", replaceRelease.ToString());
        mainScript = mainScript.Replace("#SoloReturn#", replaceSoloReturn.ToString());
        if (l.Contains(s))
        {
            string allContent = AssetTool.GetFileContent(path);
            string before = allContent.Substring(0, allContent.IndexOf("#region InitializeUI"));
            string T = mainScript.ToString();
            string updateStr = before + T.Substring(T.IndexOf("#region InitializeUI"), T.Length - T.IndexOf("#region InitializeUI"));
            AssetTool.WriteContentToFile(path, updateStr);
            Debug.Log(path.Split('/')[path.Split('/').Length - 1] + "脚本更新成功!!!");
        }
        else
        {
            AssetTool.WriteContentToFile(path, mainScript.ToString());
        }


    }

}
