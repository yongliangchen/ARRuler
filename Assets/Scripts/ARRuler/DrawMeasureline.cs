using System.Collections.Generic;
using UnityEngine;

/// <summary>绘制测量线条</summary>
public class DrawMeasureline : MonoBehaviour
{
    #region 私有函数

    private Camera m_ARCamera;
    private LineRenderer m_LineRender;
	private List<Vector3> m_LinePoints = new List<Vector3>();
	private List<GameObject> m_TargetList = new List<GameObject>();

    private Material m_DrawMaterial;
    private Texture2D m_Dottedline;
    private Material m_NormalMaterial;
    private bool m_DrawComplete = false;

    public static DrawMeasureline Create()
	{
        DrawMeasureline drawline = new GameObject("Measureline").AddComponent<DrawMeasureline>();
		return drawline;
	}

    #endregion

    #region 公开函数

    private void Awake()
    {
        m_ARCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

        m_Dottedline = Resources.Load<Texture2D>("Textures/dottedline");
        m_DrawMaterial = new Material(Shader.Find("GUI/Text Shader"));
        if (m_Dottedline != null) m_DrawMaterial.mainTexture = m_Dottedline;
        m_DrawMaterial.color = Color.green;
        m_NormalMaterial = new Material(Shader.Find("Unlit/Color"));

        m_LineRender = gameObject.AddComponent<LineRenderer>();
        m_LineRender.enabled = false;
        if (m_DrawMaterial != null) m_LineRender.material = m_DrawMaterial;
    }

    private void Update()
    {
        UpdatPosition();
    }

    /// <summary>
    /// 添加点
    /// </summary>
    /// <param name="target"></param>
    public void Add(GameObject target)
	{
        m_TargetList.Add(target);
	}

    /// <summary>
    /// 移除指定目标点
    /// </summary>
    /// <param name="target"></param>
	public void Remove(GameObject target)
	{
		m_TargetList.Remove(target);
	}

    /// <summary>删除所有线条</summary>
	public void Delete()
	{
		Destroy(gameObject);
	}

    /// <summary>绘制线条结束</summary>
    public void DrawComplete()
    {
        if (m_NormalMaterial != null) m_LineRender.material = m_NormalMaterial;
        m_DrawComplete = true;
    }

    #endregion

    #region 私有函数

    /// <summary>更新位置</summary>
    private void UpdatPosition()
    {
        if (m_TargetList == null || m_TargetList.Count < 2) return;

        m_LinePoints.Clear();
        for (int i = 0; i < m_TargetList.Count; i++)
        {
            if (m_TargetList[i] != null) m_LinePoints.Add(m_TargetList[i].transform.position);
        }

        if (m_LinePoints.Count >= 2)
        {
            float dis = (m_LinePoints[0] - m_LinePoints[1]).magnitude;
            SetDrawMaterial(dis);

            Vector3 center=(m_LinePoints[0] + m_LinePoints[1]) / 2;
            float centerDis=(m_ARCamera.transform.position- center).magnitude;
            SetLineSize(centerDis);
        }

        m_LineRender.SetPositions(m_LinePoints.ToArray());
        m_LineRender.enabled = true;
    }

    /// <summary>根据距离设置材质大小</summary>
    private void SetDrawMaterial(float distance)
    {
        if (m_Dottedline == null) return;
        float value = distance * 100f;
        m_DrawMaterial.SetTextureScale("_MainTex", new Vector2(value, 1));
    }

    /// <summary>设置线条大小</summary>
    private void SetLineSize(float distance)
    {
        float size = distance * 0.0035f;
        size = Mathf.Clamp(size, 0.0018f, 10f);

        if (!m_DrawComplete && m_Dottedline != null) size *= 3;

        m_LineRender.startWidth = size;
        m_LineRender.endWidth = size;
    }

    #endregion
}