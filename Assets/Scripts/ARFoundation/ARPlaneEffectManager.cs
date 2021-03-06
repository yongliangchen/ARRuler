﻿using UnityEngine;
using UnityEngine.XR.ARFoundation;

/// <summary>AR平面显示管理器</summary>
public class ARPlaneEffectManager : MonoBehaviour
{
    private ARPlaneManager m_ARPlaneManager = null;
    private Color m_DefaultPlaneColor = Color.white;
    private Color m_TransparentPlaneColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0);
   
    private void Awake()
    {
        m_ARPlaneManager = FindObjectOfType<ARPlaneManager>();
    }

    /// <summary>
    /// 显示或者隐藏平面网格特效
    /// </summary>
    /// <param name="value">显示或者隐藏</param>
    public void SetAllPlanesActive(bool value)
    {
        if (m_ARPlaneManager == null || m_ARPlaneManager.planePrefab.gameObject.GetComponent<MeshRenderer>().sharedMaterial == null) return;

        if (value) m_ARPlaneManager.planePrefab.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_TexTintColor", m_DefaultPlaneColor);
        else m_ARPlaneManager.planePrefab.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_TexTintColor", m_TransparentPlaneColor);

        foreach (var plane in m_ARPlaneManager.trackables)
        {
            if (value) plane.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_TexTintColor", m_DefaultPlaneColor);
            else plane.gameObject.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_TexTintColor", m_TransparentPlaneColor);
        }
    }
}
