//------------------------------------------------------------------------------
// Author : Strix
// Github : https://github.com/KorStrix/Google_SpreadSheetParser
//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class global_Container : ScriptableObject
{
    
    public List<global> listData;
    
    public Dictionary<EGlobalKey, string> mapData_Type_Is_string;
    
    public Dictionary<EGlobalKey, float> mapData_Type_Is_float;
    
    public Dictionary<EGlobalKey, int> mapData_Type_Is_int;
    
    public void DoInit()
    {
#if UNITY_EDITOR
        listData.Clear();
        Object[] arrObject = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(UnityEditor.AssetDatabase.GetAssetPath(this));
        for (int i = 0; i < arrObject.Length; i++)
           listData.Add((global)arrObject[i]);
       UnityEditor.EditorUtility.SetDirty(this);
#endif
        this.Init_mapData_Type_Is_string();
        this.Init_mapData_Type_Is_float();
        this.Init_mapData_Type_Is_int();
    }
    
    private void Init_mapData_Type_Is_string()
    {
        var arrLocal = listData.Where(x => x.strType == "string");
        this.mapData_Type_Is_string = arrLocal.ToDictionary(p => p.eGlobalKey, p => p.strValue);
    }
    
    private void Init_mapData_Type_Is_float()
    {
        var arrLocal = listData.Where(x => x.strType == "float");
        this.mapData_Type_Is_float = arrLocal.ToDictionary(p => p.eGlobalKey, p => float.Parse(p.strValue));
    }
    
    private void Init_mapData_Type_Is_int()
    {
        var arrLocal = listData.Where(x => x.strType == "int");
        this.mapData_Type_Is_int = arrLocal.ToDictionary(p => p.eGlobalKey, p => int.Parse(p.strValue));
    }
}

public enum EGlobalKey
{
    
    /// <summary>
    /// 플롯설명
    /// </summary>
    floatValue,
    
    /// <summary>
    /// 스트링설명
    /// </summary>
    stringValue,
    
    /// <summary>
    /// 인트설명
    /// </summary>
    intValue,
    
    /// <summary>
    /// 인트설명22
    /// </summary>
    intValue22,
    
    strValue22,
}
