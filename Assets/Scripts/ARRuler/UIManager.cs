/***
 * 
 *    Title: ARRuler
 *           主题: Ui管理类
 *    Description: 
 *           功能：管理UI面板的状态
 *                管理按钮的点击事件
 *                                  
 *    Date: 2020
 *    Version: 1.0版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;
using UnityEngine.UI;

/// <summary>管理场景中的UI</summary>
public class UIManager : MonoBehaviour
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

    private void Awake()
    {
        m_UIRoot = GameObject.Find("UIRoot");
        m_ButtonsPanel = m_UIRoot.transform.Find("ButtonsPanel").gameObject;
        m_FindPanelPanel = m_UIRoot.transform.Find("FindPanelPanel").gameObject;

        m_BtnAdd = m_ButtonsPanel.transform.Find("BtnAdd").GetComponent<Button>();
        m_BtnAdd.onClick.AddListener(OnClickCreateAnchorButton);
        AddButtonInteractable(false);

        m_BtnTakePictures = m_ButtonsPanel.transform.Find("BtnTakePictures").GetComponent<Button>();
        m_BtnTakePictures.onClick.AddListener(OnClickTakePhotosButton);
        TakePhotosButtonInteractable(false);


        m_BtnDelete = m_ButtonsPanel.transform.Find("BtnDelete").GetComponent<Button>();
        m_BtnDelete.onClick.AddListener(OnClickClearAnchorButton);
        DeleteButtonInteractable(false);

        m_BtnRevoke = m_ButtonsPanel.transform.Find("BtnRevoke").GetComponent<Button>();
        m_BtnRevoke.onClick.AddListener(OnClickRevokeAnchorButton);
        RevokeButtonInteractable(false);

        ButtonsPanelSetActive(false);
        FindPanelPanelSetActive(false);
    }

    /// <summary>设置查找平面面板显示或隐藏</summary>
    public void FindPanelPanelSetActive(bool value)
    {
        m_FindPanelPanel.SetActive(value);
    }

    /// <summary>设置按钮面板显示或者隐藏</summary>
    public void ButtonsPanelSetActive(bool value)
    {
        m_ButtonsPanel.SetActive(value);
    }

    /// <summary>设置添加锚点按钮是否可交互</summary>
    public void AddButtonInteractable(bool value)
    {
        m_BtnAdd.interactable = value;
    }

    /// <summary>设置添加拍照按钮是否可交互</summary>
    public void TakePhotosButtonInteractable(bool value)
    {
        m_BtnTakePictures.interactable = value;
    }

    /// <summary>设置删除锚点按钮是否可交互</summary>
    public void DeleteButtonInteractable(bool value)
    {
        m_BtnDelete.interactable = value;
    }

    /// <summary>设置撤销锚点按钮是否可交互</summary>
    public void RevokeButtonInteractable(bool value)
    {
        m_BtnRevoke.interactable = value;
    }

    /// <summary>点击创建锚点按钮</summary>
    private void OnClickCreateAnchorButton()
    {
        Debug.Log(GetType() + "/OnClickCreateAnchorButton()");
        ARRulerSceneManager.Instance.drawManager.CreateAnchor();
    }

    /// <summary>点击撤销锚点按钮</summary>
    private void OnClickRevokeAnchorButton()
    {
        Debug.Log(GetType() + "/OnClickRevokeAnchorButton()");
        ARRulerSceneManager.Instance.drawManager.RevokeAnchor();
    }

    /// <summary>点击删除锚点</summary>
    private void OnClickClearAnchorButton()
    {
        Debug.Log(GetType() + "/OnClickClearAnchorButton()");
        ARRulerSceneManager.Instance.drawManager.ClearAnchor();
    }

    /// <summary>点击拍照按钮</summary>
    private void OnClickTakePhotosButton()
    {
        Debug.Log(GetType() + "/OnClickTakePhotosButton()");
    }

}
