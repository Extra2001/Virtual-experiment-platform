using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WisdomTree.Common.Function
{
    public class DataCommunicationSetting : EditorWindow
    {
        //[MenuItem("WisdomTree/DataCommunicationSetting", priority = 2)]
        //public static void Open()
        //{
        //    DataCommunicationSetting window = GetWindow<DataCommunicationSetting>("DataCommunicationSetting");
        //    window.position = new Rect(0, 0, 600, 600);
        //    window.Show();

        //    if (BuilderSettings.GetInt("courseId") == 0)
        //    {
        //        BuilderSettings.SetInt("courseId", 2000062136);
        //    }
        //    if (BuilderSettings.GetInt("experimentId") == 0)
        //    {
        //        BuilderSettings.SetInt("experimentId", 175);
        //    }
        //}

        //private void OnGUI()
        //{
        //    EditorGUILayout.Space(30);
        //    EditorGUILayout.BeginVertical();
        //    EditorGUILayout.LabelField("请输入UUID：（Editor测试使用，打包后自动获取）");
        //    BuilderSettings.SetString("uuid", EditorGUILayout.TextField(BuilderSettings.GetString("uuid")));
        //    EditorGUILayout.Space(20);
        //    EditorGUILayout.LabelField("请输入课程ID：（Editor测试，并且打包时写入index，产品提供，测试时建议使用2000062136）");
        //    BuilderSettings.SetInt("courseId", int.Parse(EditorGUILayout.TextField(BuilderSettings.GetInt("courseId").ToString())));
        //    EditorGUILayout.Space(20);
        //    EditorGUILayout.LabelField("请输入实验ID：（Editor测试，并且打包时写入index，产品提供，测试时建议使用175）");
        //    BuilderSettings.SetInt("experimentId", int.Parse(EditorGUILayout.TextField(BuilderSettings.GetInt("experimentId").ToString())));
        //    EditorGUILayout.Space(20);
        //    EditorGUILayout.LabelField("请输入appID：（Editor测试，并且打包时写入index，产品提供）");
        //    BuilderSettings.SetString("appId", EditorGUILayout.TextField(BuilderSettings.GetString("appId")));
        //    EditorGUILayout.Space(20);
        //    EditorGUILayout.LabelField("请输入secret：（Editor测试，并且打包时写入index，产品提供）");
        //    BuilderSettings.SetString("secret", EditorGUILayout.TextField(BuilderSettings.GetString("secret")));
        //    EditorGUILayout.EndVertical();
        //}
    }
}