using System.Collections.Generic;
using UnityEngine;

/// <summary>对象上面画线</summary>
public class ObjectDrawline : MonoBehaviour
{

    private float lineSize = 0.1f;
    public Material material;

    private LineRenderer lineRender;
    private List<Vector3> linePoints = new List<Vector3>();
    public List<GameObject> targetList = new List<GameObject>();

    private void Awake()
    {
        //m_Parent = transform.parent.parent.parent.gameObject;
        lineRender = gameObject.GetComponent<LineRenderer>();
        if (lineRender == null)
        {
            lineRender = gameObject.AddComponent<LineRenderer>();
        }
       
        lineRender.enabled = false;
        lineRender.startWidth = lineSize;
        lineRender.endWidth = lineSize;
        lineRender.material = material;
    }

    private void Update()
    {
        UpdatPosition();
    }

    private void UpdatPosition()
    {
        if (targetList == null || targetList.Count < 2) return;
        lineRender.startWidth = lineSize;
        lineRender.endWidth = lineSize;

        linePoints.Clear();
        for (int i = 0; i < targetList.Count; i++)
        {
            if (targetList[i] != null) linePoints.Add(targetList[i].transform.position);
        }

        if (linePoints.Count >= 2)
        {
            float dis = (linePoints[0] - linePoints[1]).magnitude;
            Vector3 center = (linePoints[0] + linePoints[1]) / 2;
        }

        lineRender.SetPositions(linePoints.ToArray());
        lineRender.enabled = true;
    }

    public Vector3 GetObjectSize(GameObject model)
    {
        Vector3 length = model.GetComponent<MeshFilter>().mesh.bounds.size;
        float xlength = length.x * transform.lossyScale.x;
        float ylength = length.y * transform.lossyScale.y;
        float zlength = length.z * transform.lossyScale.z;
        return new Vector3(xlength, ylength, zlength);
    }

    private float GetLineSize(Vector3 vector3)
    {
        float mini = vector3.x;
        if (vector3.y < mini) mini = vector3.y;
        if (vector3.z < mini) mini = vector3.z;

        float average = (vector3.x + vector3.y + vector3.z) / 3;
        return Mathf.Clamp((mini + (average * 0.3f)) * 0.1f, 0.0005f, 1f);
    }
}
