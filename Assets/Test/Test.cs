using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject label;
    public GameObject A;
    public GameObject B;

    private void Update()
    {
        Vector3 StartPs = A.transform.position;
        Vector3 EndPs = B.transform.position;
        label.transform.position=(StartPs + EndPs) /2;
        Quaternion targetRot = Quaternion.LookRotation(EndPs - StartPs);
        label.transform.localRotation = targetRot;
    }

    //Vector3 target = StartPs + ((EndPs - StartPs) / count) * j;
    //Quaternion targetRot = Quaternion.LookRotation(EndPs - StartPs);
}
