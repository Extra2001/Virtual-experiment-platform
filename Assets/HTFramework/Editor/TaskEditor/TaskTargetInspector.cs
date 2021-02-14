﻿using System;
using UnityEditor;
using UnityEngine;

namespace HT.Framework
{
    [CustomEditor(typeof(TaskTarget))]
    internal sealed class TaskTargetInspector : HTFEditor<TaskTarget>
    {
        protected override bool IsEnableRuntimeData
        {
            get
            {
                return false;
            }
        }

        protected override void OnInspectorDefaultGUI()
        {
            base.OnInspectorDefaultGUI();

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.cyan;
            Button(GenerateGUID, "Generate GUID");
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            TextField(Target.GUID, out Target.GUID, "GUID");
            GUILayout.EndHorizontal();
        }

        private void GenerateGUID()
        {
            if (Target.GUID == "<None>")
            {
                Target.GUID = Guid.NewGuid().ToString();
            }
            else
            {
                if (EditorUtility.DisplayDialog("Prompt", "Are you sure you want to regenerate the GUID？", "Yes", "No"))
                {
                    Target.GUID = Guid.NewGuid().ToString();
                }
            }
        }
    }
}