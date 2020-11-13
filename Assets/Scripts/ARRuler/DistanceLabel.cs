using UnityEngine;
using UnityEngine.UI;

/// <summary>距离标签</summary>
public class DistanceLabel : MonoBehaviour
{
	private Camera m_ARCamera;

    private Transform m_WorldUIRoot;
    private const string DISTANCE_LABEL_PATH = "Prefabs/DistanceLabel";

    public GameObject m_StartObject { get; set; }
    public GameObject m_EndObject { get; set; }

    private GameObject m_Label;
    private Text m_Text1;
    private Text m_Text2;
    private Vector3 targetRot;


    private void Awake()
    {
        m_ARCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
        m_WorldUIRoot = GameObject.Find("WorldUIRoot").transform;
        GameObject prefab = Resources.Load<GameObject>(DISTANCE_LABEL_PATH);
        m_Label = Instantiate(prefab, m_WorldUIRoot);
        m_Text1 = m_Label.transform.Find("Canvas/Text1").GetComponent<Text>();
        m_Text2 = m_Label.transform.Find("Canvas/Text2").GetComponent<Text>();
    }

    private void Update()
    {
        if (m_StartObject == null || m_EndObject == null || m_Label == null)
        {
            if (m_Label != null && m_Label.activeSelf) m_Label.SetActive(false);
        }
        else
        {
            if(!m_Label.activeSelf) m_Label.SetActive(true);

            SetLabelPosition();
            SetLabelRotate();
            SetLabelSize();
            SetLabelText();
        }
    }

    private void OnDestroy()
    {
        Delete();
    }

    public void DrawComplete()
    {
      
    }

    /// <summary>设置标签的位置</summary>
    private void SetLabelPosition()
    {
        Vector3 StartPs = m_StartObject.transform.position;
        Vector3 EndPs = m_EndObject.transform.position;
        m_Label.transform.position = (StartPs + EndPs) / 2;
        targetRot = Quaternion.LookRotation(EndPs - StartPs).eulerAngles;
        m_Label.transform.localEulerAngles = targetRot;
    }

    /// <summary>设置标签的旋转角度</summary>
    private void SetLabelRotate()
    {
        Vector3 cameraPs = m_ARCamera.transform.position;
        Vector3 cameraToLabelDir = m_Label.transform.position - cameraPs;
        Vector3 quaternion = Quaternion.LookRotation(cameraToLabelDir, Vector3.up).eulerAngles;
        m_Label.transform.localEulerAngles = quaternion;
    }

    /// <summary>设置标签的大小</summary>
    private void SetLabelSize()
    {
        if (m_ARCamera == null) return;

        float direction = (m_ARCamera.transform.position - m_Label.transform.position).magnitude;
        float size = direction * 0.04f;
        size = Mathf.Clamp(size, 0.02f,50f);
        m_Label.transform.localScale = new Vector3(size, size, size);
    }

    /// <summary>设置标签的文本内容</summary>
    private void SetLabelText()
    {
        string distance = Distance(m_StartObject.transform.position, m_EndObject.transform.position);
        m_Text1.text = distance;
        m_Text2.text = distance;
    }

    /// <summary>计算两个点的距离</summary>
    private string Distance(Vector3 StartPs, Vector3 endPs)
    {
        float dis = (StartPs - endPs).magnitude;
        return (dis >= 1) ? dis.ToString("f2") + "m" : (dis * 100).ToString("f2") + "cm";
    }

    /// <summary>删除标签</summary>
    private void Delete()
    {
        if (m_Label != null) Destroy(m_Label);
    }

}
