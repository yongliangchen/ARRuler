using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>AR尺子场景管理</summary>
public class ARRulerManager : ARObjectSceneBase
{
    /// <summary>瞄准器模型</summary>
    private GameObject m_TakeAim;
    /// <summary>屏幕中心位置</summary>
    private Vector2 m_ScreenCenter;

    public override void OnAwake()
    {
        base.OnAwake();
        m_ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        m_TakeAim = Instantiate(Resources.Load("Prefabs/TakeAim") as GameObject,transform);
        m_TakeAim.name = "TakeAim";
        m_TakeAim.SetActive(false);
    }

    /// <summary>AR初始化完成</summary>
    public override void OnInitARFinish()
    {
        base.OnInitARFinish();

        Debug.Log(GetType() + "/InitARFinish()/初始化AR完成！");
        planeEffectManager.SetAllPlanesActive(false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (isSupportAR) Raycast(m_ScreenCenter, UpdateTakeAimPosition);
    }

    /// <summary>点击添加按钮</summary>
    private void OnClickAddButton()
    {
        if (!isSupportAR)
        {
            Debug.LogWarning(GetType() + "/CreateAnchor()/创建锚点失败！设备不支持AR功能或者AR功能还没有初始化完成！");
            return;
        }
    }

    /// <summary>更新瞄准器的位置信息</summary>
    private void UpdateTakeAimPosition(bool hit, Pose pose)
    {
        m_TakeAim.SetActive(hit);
        if (hit) m_TakeAim.transform.position = pose.position;
    }

    private void CreateAnchor(Vector3 ps)
    {

    }

    private void ClearAnchor()
    {

    }


}
