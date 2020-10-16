using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

/// <summary>AR尺子场景管理</summary>
public class ARRulerManager : ARObjectSceneBase
{
    /// <summary>UI根目录</summary>
    private GameObject m_UIRoot;
    /// <summary>按钮面板</summary>
    private GameObject m_ButtonsPanel;
    /// <summary>添加锚点按钮</summary>
    private Button m_BtnAdd;
    /// <summary>拍照按钮</summary>
    private Button m_BtnTakePictures;
    /// <summary>删除锚点按钮</summary>
    private Button m_BtnDelete;
    /// <summary>撤销按钮</summary>
    private Button m_BtnRevoke;
    /// <summary>查找平面面板</summary>
    private GameObject m_FindPanelPanel;
    private bool isPlaneChanged = false;

    /// <summary>瞄准器模型</summary>
    private GameObject m_TakeAim;
    /// <summary>屏幕中心位置</summary>
    private Vector2 m_ScreenCenter;

    private List<GameObject> m_ListPoint = new List<GameObject>();
    private List<DrawMeasureline> m_ListDrawline = new List<DrawMeasureline>();
    private GameObject m_AnchorParent;

    public override void OnAwake()
    {
        base.OnAwake();

        m_ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        m_TakeAim = Instantiate(Resources.Load("Prefabs/TakeAim") as GameObject,transform);
        m_TakeAim.name = "TakeAim";
        m_TakeAim.SetActive(false);

        m_AnchorParent = new GameObject("AnchorParent");
        m_AnchorParent.transform.parent = transform;

        m_UIRoot = GameObject.Find("UIRoot");
        m_ButtonsPanel = m_UIRoot.transform.Find("ButtonsPanel").gameObject;
        m_FindPanelPanel= m_UIRoot.transform.Find("FindPanelPanel").gameObject;

        m_BtnAdd = m_ButtonsPanel.transform.Find("BtnAdd").GetComponent<Button>();
        m_BtnAdd.onClick.AddListener(CreateAnchor);
        m_BtnAdd.interactable = false;

        m_BtnTakePictures = m_ButtonsPanel.transform.Find("BtnTakePictures").GetComponent<Button>();
        m_BtnTakePictures.interactable = false;

        m_BtnDelete = m_ButtonsPanel.transform.Find("BtnDelete").GetComponent<Button>();
        m_BtnDelete.onClick.AddListener(ClearAnchor);
        m_BtnDelete.interactable = false;

        m_BtnRevoke = m_ButtonsPanel.transform.Find("BtnRevoke").GetComponent<Button>();
        m_BtnRevoke.onClick.AddListener(RevokeAnchor);
        m_BtnRevoke.interactable = false;

        m_ButtonsPanel.SetActive(false);
        m_FindPanelPanel.SetActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (isSupportAR) Raycast(m_ScreenCenter, UpdateTakeAimPosition);
    }

    /// <summary>AR初始化完成</summary>
    public override void OnInitARFinish()
    {
        base.OnInitARFinish();

        Debug.Log(GetType() + "/InitARFinish()/初始化AR完成！");
        m_FindPanelPanel.SetActive(true);
        planeEffectManager.SetAllPlanesActive(false);
    }

    /// <summary>平面识别发送改变</summary>
    public override void OnPlanesChanged(ARPlanesChangedEventArgs obj)
    {
        base.OnPlanesChanged(obj);
        if (isPlaneChanged) return;
        m_ButtonsPanel.SetActive(true);
        m_FindPanelPanel.SetActive(false);
        isPlaneChanged = true;
    }

    /// <summary>更新瞄准器的位置信息</summary>
    private void UpdateTakeAimPosition(bool hit, ARRaycastHit aRRaycastHit)
    {
        m_TakeAim.SetActive(hit);
        m_BtnAdd.interactable = hit;
        m_BtnTakePictures.interactable = hit;
        if (hit)
        {
            m_TakeAim.transform.position = aRRaycastHit.pose.position;
            m_TakeAim.transform.rotation = aRRaycastHit.pose.rotation;

           Vector3 size = Vector3.one;
            float value = 1;
            if (aRRaycastHit.distance < 1) value = 3.3f;
            else if (aRRaycastHit.distance < 2) value = 4;
            else value = aRRaycastHit.distance * 2;
            m_TakeAim.transform.localScale = size* value;
        }
    }

    /// <summary>创建锚点</summary>
    private void CreateAnchor()
    {
        m_BtnDelete.interactable = true;
        m_BtnRevoke.interactable = true;
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        item.transform.parent = m_AnchorParent.transform;
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
    }

    /// <summary>撤销锚点</summary>
    private void RevokeAnchor()
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

        if (m_ListDrawline.Count < 1 || m_ListPoint.Count < 1)
        {
            m_BtnDelete.interactable = false;
            m_BtnRevoke.interactable = false;
        }
    }

    /// <summary>删除锚点</summary>
    private void ClearAnchor()
    {
        m_BtnDelete.interactable = false;
        m_BtnRevoke.interactable = false;

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
    }


}
