/***
 * 
 *    Title: ARRuler
 *           主题: AR尺子AR功能管理
 *    Description: 
 *           功能：管理AR尺子中的ARFoundtion功能
 *                
 *                                  
 *    Date: 2020
 *    Version: 1.0版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;
using UnityEngine.XR.ARFoundation;


/// <summary>AR尺子AR功能管理</summary>
public class ARRulerFoundtion: ARObjectSceneBase
{
    /// <summary>判断是否识别到平面</summary>
    private bool m_IsPlaneChanged = false;

    /// <summary>瞄准器模型</summary>
    private GameObject m_TakeAim;
    /// <summary>屏幕中心位置</summary>
    private Vector2 m_ScreenCenter;


    /// <summary>Awake</summary>
    public override void OnAwake()
    {
        base.OnAwake();

        m_ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        m_TakeAim = Instantiate(Resources.Load("Prefabs/TakeAim") as GameObject, transform);
        m_TakeAim.name = "TakeAim";
        m_TakeAim.SetActive(false);
    }

    /// <summary>Update</summary>
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

        ARRulerSceneManager.Instance.uIManager.FindPanelPanelSetActive(true);

        planeEffectManager.SetAllPlanesActive(false);
    }

    /// <summary>平面识别发送改变</summary>
    public override void OnPlanesChanged(ARPlanesChangedEventArgs obj)
    {
        base.OnPlanesChanged(obj);
        if (m_IsPlaneChanged) return;
        m_IsPlaneChanged = true;

        ARRulerSceneManager.Instance.uIManager.ButtonsPanelSetActive(true);
        ARRulerSceneManager.Instance.uIManager.FindPanelPanelSetActive(false);
    }

    /// <summary>更新瞄准器的位置信息</summary>
    private void UpdateTakeAimPosition(bool hit, ARRaycastHit aRRaycastHit)
    {
        m_TakeAim.SetActive(hit);

        ARRulerSceneManager.Instance.uIManager.AddButtonInteractable(hit);
        ARRulerSceneManager.Instance.uIManager.TakePhotosButtonInteractable(hit);

        if (hit)
        {
            m_TakeAim.transform.position = aRRaycastHit.pose.position;
            m_TakeAim.transform.rotation = aRRaycastHit.pose.rotation;

            Vector3 size = Vector3.one;
            float value = 1;
            if (aRRaycastHit.distance < 1) value = 3.3f;
            else if (aRRaycastHit.distance < 2) value = 4;
            else value = aRRaycastHit.distance * 2;
            m_TakeAim.transform.localScale = size * value;
        }
    }

}
