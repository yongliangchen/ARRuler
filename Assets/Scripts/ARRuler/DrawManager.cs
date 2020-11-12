/***
 * 
 *    Title: ARRuler
 *           主题: 绘制管理
 *    Description: 
 *           功能：整个AR尺子功能的绘制工作
 *                                            
 *    Date: 2020
 *    Version: 1.0版本
 *    Modify Recoder:      
 *
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>绘制管理</summary>
public class DrawManager : MonoBehaviour
{
    /// <summary>瞄准器模型</summary>
    private GameObject m_TakeAim;

    private List<GameObject> m_ListPoint = new List<GameObject>();
    private List<DrawMeasureline> m_ListDrawline = new List<DrawMeasureline>();
    private Transform m_AnchorParent;

    private void Start()
    {
        m_AnchorParent = new GameObject("AnchorParent").transform;
        m_AnchorParent.parent = transform;
        m_TakeAim = transform.Find("TakeAim").gameObject;
        if (m_TakeAim == null)
        {
            Debug.LogError(GetType() + "/Start()/m_TakeAim is null!");
        }
    }

    /// <summary>创建锚点</summary>
    public void CreateAnchor()
    {
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        item.transform.parent = m_AnchorParent;
        item.transform.localScale = Vector3.one * 0.006f;
        item.transform.position = m_TakeAim.transform.position;
        Material material = new Material(Shader.Find("Unlit/Color"));
        item.GetComponent<Renderer>().material = material;
        m_ListPoint.Add(item);

        //创建模型为单数的时候开始绘制线
        if (m_ListPoint.Count % 2 != 0)
        {
            DrawMeasureline drawline = DrawMeasureline.Create();
            drawline.Add(item);
            drawline.Add(m_TakeAim);
            m_ListDrawline.Add(drawline);

            item.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            m_ListDrawline[m_ListDrawline.Count - 1].Remove(m_TakeAim);
            m_ListDrawline[m_ListDrawline.Count - 1].Add(item);
            m_ListDrawline[m_ListDrawline.Count - 1].DrawComplete();

            m_ListPoint[m_ListPoint.Count - 1].GetComponent<Renderer>().material.color = Color.white;
            m_ListPoint[m_ListPoint.Count - 2].GetComponent<Renderer>().material.color = Color.white;
        }

        SetDeleteButtonInteractable();
        SetRevokeButtonInteractable();
    }

    /// <summary>撤销锚点</summary>
    public void RevokeAnchor()
    {
        if (m_ListDrawline != null && m_ListDrawline.Count > 0)
        {
            m_ListDrawline[m_ListDrawline.Count - 1].Delete();
            m_ListDrawline.RemoveAt(m_ListDrawline.Count - 1);
        }

        if (m_ListPoint != null && m_ListPoint.Count > 1)
        {
            Destroy(m_ListPoint[m_ListPoint.Count - 1]);
            Destroy(m_ListPoint[m_ListPoint.Count - 2]);
            m_ListPoint.RemoveAt(m_ListPoint.Count - 1);
            m_ListPoint.RemoveAt(m_ListPoint.Count - 1);
        }

        SetDeleteButtonInteractable();
        SetRevokeButtonInteractable();
    }

    /// <summary>删除锚点</summary>
    public void ClearAnchor()
    {
        if (m_ListDrawline != null && m_ListDrawline.Count > 0)
        {
            for (int i = m_ListDrawline.Count - 1; i >= 0; i--) { m_ListDrawline[i].Delete(); }
            m_ListDrawline.Clear();
        }

        if (m_ListPoint != null && m_ListPoint.Count > 0)
        {
            for (int i = m_ListPoint.Count - 1; i >= 0; i--) { Destroy(m_ListPoint[i]); }
            m_ListPoint.Clear();
        }

        SetDeleteButtonInteractable();
        SetRevokeButtonInteractable();
    }

    /// <summary>设置删除按钮是否可交互</summary>
    private void SetDeleteButtonInteractable()
    {
        ARRulerSceneManager.Instance.uIManager.DeleteButtonInteractable(m_ListPoint != null && m_ListPoint.Count > 0);
    }

    /// <summary>设置撤销锚点按钮是否可交互</summary>
    private void SetRevokeButtonInteractable()
    {
        ARRulerSceneManager.Instance.uIManager.RevokeButtonInteractable(m_ListPoint != null && m_ListPoint.Count > 0);
    }
}
