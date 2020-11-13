using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject label;
    public GameObject A;
    public GameObject B;
    private Vector3 targetRot;


    private void Update()
    {
        Vector3 StartPs = A.transform.position;
        Vector3 EndPs = B.transform.position;
        label.transform.position=(StartPs + EndPs) /2;
        targetRot = Quaternion.LookRotation(EndPs - StartPs).eulerAngles;
        label.transform.localEulerAngles = targetRot;

        //Debug.Log();

        // Debug.Log(Cross(Camera.main.transform.forward, label.transform.position));

        if(Vector3.Dot(Camera.main.transform.forward, label.transform.position)>0)
            Debug.Log(true);
        else
            Debug.Log(false);


        //SetLabelRotate();
    }

    /// <summary>设置标签的旋转角度</summary>
    private void SetLabelRotate()
    {
        Vector3 cameraPs = Camera.main.transform.position;
        Vector3 cameraToLabelDir = label.transform.position - cameraPs;
        Vector3 quaternion = Quaternion.LookRotation(cameraToLabelDir, Vector3.up).eulerAngles;

        label.transform.localEulerAngles = new Vector3(targetRot.x, targetRot.y, quaternion.x);

        //if (!Cross(A.transform.position, B.transform.position))
        //{
        //    label.transform.localRotation = new Quaternion(targetRot.x, targetRot.y, quaternion.z, targetRot.w);
        //}
        //else
        //{
        //    label.transform.localRotation = new Quaternion(quaternion.x, targetRot.y, targetRot.z, targetRot.w);
        //}
    }

    private bool Cross(Vector3 tmpA, Vector3 tmpB)
    {
        Vector3 result = Vector3.Cross(tmpA, tmpB);
        return (result.y > 0);
    }
}
