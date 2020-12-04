using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransFormExtension
{
    /// <summary>
    /// 查找
    /// </summary>
    /// <param name="_root"></param>
    /// <param name="_targetName"></param>
    public static Transform FindChildTransform(this Transform _root, string _targetName)
    {
        if (_root.name == _targetName) return _root;
        foreach (Transform t in _root)
        {
            Transform target = FindChildTransform(t, _targetName);
            if (target != null) return target;
        }
        return null;
    }
    /// <summary>
    /// 查找目标的组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_root"></param>
    /// <param name="_targetName"></param>
    /// <returns></returns>
    public static T FindChildComponent<T>(this Transform _root, string _targetName)
    {
        if (_root.name == _targetName) return _root.GetComponent<T>();
        foreach (Transform t in _root)
        {
            Transform target = FindChildTransform(t, _targetName);
            if (target != null) return target.GetComponent<T>();
        }
        return default(T);
    }
    /// <summary>
    /// 获得所有子物体的名字
    /// </summary>
    /// <param name="_root">根节点</param>
    /// <returns></returns>
    public static List<string> GetAllChildName(this Transform _root)
    {
        List<string> nameLit = new List<string>();
        foreach (Transform t in _root)
        {
            nameLit.Add(t.name);
            if (t.childCount > 0)
                nameLit.AddRange(GetAllChildName(t));
        }
        return nameLit;
    }
}
