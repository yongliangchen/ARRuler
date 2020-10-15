using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

/// <summary>AR物体场景的基类</summary>
public class ARObjectSceneBase : MonoBehaviour
{
    private ARSession m_ARSession;
    private GameObject m_ARSessionOrigin;
    /// <summary>AR平面管理</summary>
    public ARPlaneManager planeManager { get; private set; }
    /// <summary>AR平面特效管理</summary>
    public ARPlaneEffectManager planeEffectManager { get; private set; }
    /// <summary>射线检测管理</summary>
    public ARRaycastManager aRRaycastManager { get; private set; }

    /// <summary>错误事件</summary>
    public Action<string> onErrorEvent = null;
    /// <summary>设备是否支持AR功能</summary>
    public bool isSupportAR { get; private set; } = false;

    private static List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        m_ARSessionOrigin = GameObject.Find("AR Session Origin").gameObject;
        AddScripts();

        m_ARSession = FindObjectOfType<ARSession>();
        if (m_ARSession != null)
        {
            StartCoroutine(CheckSupport());
        }
        else { ErrorInfo("/Awake()/检测设备是否支持AR功能失败，ARSession is Null!"); }

        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    private void OnDestroy()
    {
        planeManager.planesChanged -= OnPlanesChanged;
        OnDispose();
    }

    /// <summary>添加脚本</summary>
    private void AddScripts()
    {
        if (m_ARSessionOrigin == null)
        {
            ErrorInfo("/AddScripts()/ARSessionOrigin is Null!");
            return;
        }

        //添加AR平面管理脚本
        planeManager = FindObjectOfType<ARPlaneManager>();
        if (planeManager == null)
        {
            planeManager = m_ARSessionOrigin.AddComponent<ARPlaneManager>();
            GameObject aRDefaultPlane = Resources.Load<GameObject>("Prefabs/AR Feathered Plane Fade");
            planeManager.planePrefab = aRDefaultPlane;
        }
 
        //添加射线检测
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        if (aRRaycastManager == null)
        {
            aRRaycastManager=m_ARSessionOrigin.AddComponent<ARRaycastManager>();
        }

        //添加AR平面特效管理
        planeEffectManager = FindObjectOfType<ARPlaneEffectManager>();
        if(planeEffectManager==null)
        {
            planeEffectManager= m_ARSessionOrigin.AddComponent<ARPlaneEffectManager>();
        }
    }

    /// <summary>检查设备是否支持AR支持</summary>
    private IEnumerator CheckSupport()
    {
        yield return ARSession.CheckAvailability();

        //当前设备不支持AR功能
        if (ARSession.state == ARSessionState.Unsupported) { OnUnsupported(); }
        else
        {
            //设备支持 AR，但需要安装相应软件(这里指手机端的 ARCore 或者 ARKit)
            if (ARSession.state == ARSessionState.NeedsInstall) { yield return ARSession.Install(); }

            isSupportAR = true;
            OnInitARFinish();
        }
    }

    public virtual void OnAwake() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnDispose() { }

    /// <summary>初始化AR完成</summary>
    public virtual void OnInitARFinish()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    /// <summary>当前设备不支持AR功能</summary>
    public virtual void OnUnsupported() { }

    /// <summary>平面识别发生改变</summary>
    public virtual void OnPlanesChanged(ARPlanesChangedEventArgs obj){ }

    /// <summary>
    /// 从屏幕触摸位置发射一条射线
    /// </summary>
    /// <param name="result">返回结果</param>
    public void TouchRaycast(Action<bool, Pose> result)
    {
        if (Input.touchCount == 0) return;
        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        Raycast(touch.position, result);
    }

    /// <summary>
    /// 从指定位置发射一条射线
    /// </summary>
    /// <param name="ps">发射射线的位置</param>
    /// <param name="result">返回结果</param>
    public void Raycast(Vector2 ps, Action<bool,Pose> result)
    {
        if (aRRaycastManager.Raycast(ps, Hits, TrackableType.PlaneWithinPolygon | TrackableType.PlaneWithinBounds))
        {
            if (result != null) result(true,Hits[0].pose);
        }
        else
        {
            if (result != null) result(false, new Pose());
        }
    }

    /// <summary>
    /// 错误事件
    /// </summary>
    /// <param name="msg">错误消息</param>
    private void ErrorInfo(string msg)
    {
        if (onErrorEvent != null) onErrorEvent(msg);
        Debug.LogWarning(GetType() + msg);
    }

}
