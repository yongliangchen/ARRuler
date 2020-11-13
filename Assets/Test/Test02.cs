using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test02 : MonoBehaviour
{
    public GameObject label;

    private void Update()
    {
        if (Vector3.Dot(transform.forward, label.transform.position) > 0)
            Debug.Log(true);
        else
            Debug.Log(false);
    }
}
