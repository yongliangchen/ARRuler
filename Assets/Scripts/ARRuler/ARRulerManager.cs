using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>AR尺子场景管理</summary>
public class ARRulerManager : ARObjectSceneBase
{
    /// <summary>屏幕中心位置</summary>
    private Vector2 m_ScreenCenter;

    public override void OnAwake()
    {
        base.OnAwake();
        m_ScreenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    /// <summary>AR初始化完成</summary>
    public override void InitARFinish()
    {
        base.InitARFinish();

        Debug.Log(GetType() + "/InitARFinish()/初始化AR完成！");
        planeEffectManager.SetAllPlanesActive(false);
    }

    /// <summary>创建锚点</summary>
    private void CreateAnchor()
    {
        if(!isSupportAR)
        {
            Debug.LogWarning(GetType() + "/CreateAnchor()/创建锚点失败！设备不支持AR功能或者AR功能还没有初始化完成！");
            return;
        }

        Raycast(m_ScreenCenter, (pose) => {

            //Todo
        });
    }

    private void ClearAnchor()
    {

    }


}
