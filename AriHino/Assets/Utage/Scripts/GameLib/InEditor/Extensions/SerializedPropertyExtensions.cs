#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UtageExtensions
{
	//SerializedPropertyの拡張メソッド
    public static class SerializedPropertyExtensions
    {
	    //Value（オブジェクト）とFiledInfoを取得
	    //値型の場合はboxingされたものが返る
        public static (object, FieldInfo) GetValueAndFieldInfo(this SerializedProperty property)
        {
	        //ルートのオブジェクト
	        FieldInfo field = null;
	        object obj = property.serializedObject.targetObject;

	        //ルートのオブジェクトから、
	        //.で区切られたパスをたどってオブジェクトを取得していく
	        var propertyPathSplit = property.propertyPath.Split('.');
	        for(int i= 0; i < propertyPathSplit.Length; ++i)
	        {
		        var filedName = propertyPathSplit[i];
		        object parentObj = obj;
		        if (filedName == "Array")
		        {
			        //配列の要素の場合は、
			        //Array.data[インデックス]
			        //がパスとなっている
			        
			        int arrayIndex = int.Parse(propertyPathSplit[i + 1].Replace("data[", "").Replace("]", ""));
			        //配列の各要素にはField情報はない
			        field = null;
			        //配列の要素として内容を取得する
			        obj = ((System.Collections.IList)parentObj)[arrayIndex];
			        i += 1;
		        }
		        else
		        {
			        field = parentObj.GetType().GetField(filedName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			        if (field == null)
			        {
				        Debug.LogError($"Not found field {filedName} in {property.propertyPath}",
					        property.serializedObject.targetObject);
			        }
			        else
			        {
				        obj = field.GetValue(parentObj);
			        }
		        }
	        }
	        return (obj,field);
        }

        //Valueを取得
        //値型の場合はboxingされたものが返る
        public static object GetValue(this SerializedProperty property)
        {
	        return property.GetValueAndFieldInfo().Item1;
        }

        //FiledInfoを取得（Attributeを調べるためなどに利用）
        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
	        return property.GetValueAndFieldInfo().Item2;
        }
    }
}
#endif
