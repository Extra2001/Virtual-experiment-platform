﻿using System.Collections.Generic;
using UnityEngine;

namespace HT.Framework
{
    /// <summary>
    /// 网格轮廓高亮工具箱
    /// </summary>
    public static class MeshOutlineToolkit
    {
        private static List<MeshOutlineObject> MOCache = new List<MeshOutlineObject>();
        private static HashSet<MeshOutlineObject> MOs = new HashSet<MeshOutlineObject>();

        /// <summary>
        /// 开启网格轮廓高亮，使用默认颜色
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="intensity">强度</param>
        public static void OpenMeshOutline(this GameObject target, float intensity = 1)
        {
            target.OpenMeshOutline(Color.yellow, intensity);
        }
        /// <summary>
        /// 开启网格轮廓高亮，使用指定颜色
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="color">高亮颜色</param>
        /// <param name="intensity">强度</param>
        public static void OpenMeshOutline(this GameObject target, Color color, float intensity = 1)
        {
            if (!Main.m_Controller.EnableHighlightingEffect)
                return;

            target.ClearMeshOutlineInChildren();
            target.ClearMeshOutlineInParent();

            MeshOutlineObject mo = target.GetComponent<MeshOutlineObject>();
            if (mo == null) mo = target.AddComponent<MeshOutlineObject>();

            if (!MOs.Contains(mo))
            {
                MOs.Add(mo);
            }
            mo.Open(color, intensity);
        }
        /// <summary>
        /// 重置高亮网格轮廓
        /// </summary>
        /// <param name="target">目标物体</param>
        public static void ResetOutline(this GameObject target)
        {
            MeshOutlineObject mo = target.GetComponent<MeshOutlineObject>();
            if (mo != null)
            {
                mo.ResetOutline();
            }
        }
        /// <summary>
        /// 关闭网格轮廓发光
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁实例</param>
        public static void CloseMeshOutline(this GameObject target, bool die = false)
        {
            MeshOutlineObject mo = target.GetComponent<MeshOutlineObject>();
            if (mo == null) return;

            if (MOs.Contains(mo))
            {
                MOs.Remove(mo);
            }
            mo.Close();
            if (die) mo.Die();
        }
        /// <summary>
        /// 关闭所有的网格轮廓发光
        /// </summary>
        /// <param name="die">是否销毁实例</param>
        public static void CloseAllMeshOutline(bool die = false)
        {
            foreach (var mo in MOs)
            {
                if (mo)
                {
                    mo.Close();
                    if (die) mo.Die();
                }
            }
            MOs.Clear();
        }

        /// <summary>
        /// 清空所有子物体上的轮廓高光效果，不包括自身
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void ClearMeshOutlineInChildren(this GameObject target, bool die = false)
        {
            MOCache.Clear();
            target.transform.GetComponentsInChildren(true, MOCache);
            for (int i = 0; i < MOCache.Count; i++)
            {
                if (MOCache[i].gameObject != target)
                {
                    MOCache[i].Close();
                    if (die) MOCache[i].Die();
                }
            }
        }
        /// <summary>
        /// 清空所有父物体上的轮廓高光效果，不包括自身
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="die">是否销毁高光实例</param>
        public static void ClearMeshOutlineInParent(this GameObject target, bool die = false)
        {
            MOCache.Clear();
            target.transform.GetComponentsInParent(true, MOCache);
            for (int i = 0; i < MOCache.Count; i++)
            {
                if (MOCache[i].gameObject != target)
                {
                    MOCache[i].Close();
                    if (die) MOCache[i].Die();
                }
            }
        }
    }
}