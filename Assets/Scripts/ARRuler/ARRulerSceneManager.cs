/***
 * 
 *    Title: ARRuler
 *           主题: AR管理类
 *    Description: 
 *           功能：整个AR尺子功能的中间件
 *                负责管理AR尺子功能类的生命周期
 *                
 *                                  
 *    Date: 2020
 *    Version: 1.0版本
 *    Modify Recoder:      
 *
 */

using Mx.Util;

/// <summary>AR尺子场景管理类</summary>
public class ARRulerSceneManager : MonoSingleton<ARRulerSceneManager>
{
    /// <summary>ARFoundation功能管理类</summary>
    public ARRulerFoundtion aRRulerFoundtion { get; private set; }
    /// <summary>UI管理类</summary>
    public UIManager uIManager { get; private set; }
    /// <summary>标签管理类</summary>
    public LabelsManager labelsManager { get; private set; }
    /// <summary>绘制管理</summary>
    public DrawManager drawManager { get; private set; }

    private void Awake()
    {
        AddScripts();
    }

    /// <summary>添加脚本</summary>
    private void AddScripts()
    {
        aRRulerFoundtion = FindObjectOfType<ARRulerFoundtion>();
        if (aRRulerFoundtion == null) aRRulerFoundtion = gameObject.AddComponent<ARRulerFoundtion>();

        uIManager = FindObjectOfType<UIManager>();
        if (uIManager == null) uIManager = gameObject.AddComponent<UIManager>();

        labelsManager = FindObjectOfType<LabelsManager>();
        if (labelsManager == null) labelsManager = gameObject.AddComponent<LabelsManager>();

        drawManager = FindObjectOfType<DrawManager>();
        if (drawManager == null) drawManager = gameObject.AddComponent<DrawManager>();
    }
   
}
