﻿using System;
using UnityEditor;
using UnityEngine;

namespace HT.Framework
{
    [CustomEditor(typeof(CustomModuleManager))]
    [GiteeURL("https://gitee.com/SaiTingHu/HTFramework")]
    [GithubURL("https://github.com/SaiTingHu/HTFramework")]
    [CSDNBlogURL("https://wanderer.blog.csdn.net/article/details/103390089")]
    internal sealed class CustomModuleManagerInspector : InternalModuleInspector<CustomModuleManager>
    {
        private ICustomModuleHelper _customModuleHelper;

        protected override string Intro
        {
            get
            {
                return "CustomModule Manager, manager of all custom modules, such as starting or stopping a custom module!";
            }
        }

        protected override Type HelperInterface
        {
            get
            {
                return typeof(ICustomModuleHelper);
            }
        }

        protected override void OnRuntimeEnable()
        {
            base.OnRuntimeEnable();

            _customModuleHelper = _helper as ICustomModuleHelper;
        }
        
        protected override void OnInspectorRuntimeGUI()
        {
            base.OnInspectorRuntimeGUI();

            GUILayout.BeginHorizontal();
            GUILayout.Label("CustomModules: " + _customModuleHelper.CustomModules.Count);
            GUILayout.EndHorizontal();

            foreach (var item in _customModuleHelper.CustomModules)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(item.Key + "[" + item.Value.GetType().FullName + "]");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(40);
                if (item.Value.IsRunning)
                {
                    GUILayout.Label("[Running]");
                    if (GUILayout.Button("Stop", EditorStyles.miniButton, GUILayout.Width(40)))
                    {
                        item.Value.IsRunning = false;
                    }
                }
                else
                {
                    GUILayout.Label("[Stopped]");
                    if (GUILayout.Button("Run", EditorStyles.miniButton, GUILayout.Width(40)))
                    {
                        item.Value.IsRunning = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}