public class BaseTemplate
{
public static string NewNormalClass =
@"public class #PanelName# 
{  
    #PanelUINameInitialize#
}";
public static string NewMainClass =
@"using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.EventSystems;
 using System;
public class #UIPanel#:UIBase
{
    #region Initialize
    public override void Initialize()
    {
        #Initialize#
    }          
    #declareUI#
    #endregion 
    #region Release
    public override void OnRelease()
    {
        #OnRelease#
    }    
    #endregion 
}
public class #UIName#
{
    #UINames#
}
";

public static string SoloClass =
@"using UnityEngine;
 using UnityEngine.UI;
 using System;
public class #UIPanel#:MonoBehaviour
{
    public void InitData() 
    {
    } 
    public void Initialize()
    {
        #Initialize#
    }          
    public void OnRelease()
    {
        #OnRelease#
    }
    public static #UIPanel# CloneSelf(Transform selfTra,Transform parent)
    {
        GameObject cloneObj = null;
        if (parent != null)
            cloneObj = Instantiate(selfTra.gameObject,parent);
        else
            cloneObj = Instantiate(selfTra.gameObject);
        #SoloReturn#
    }
    #declareUI#
}
";
}
