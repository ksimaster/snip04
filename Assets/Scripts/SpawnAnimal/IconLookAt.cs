using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public GameObject miniCamera;

    private void Update()
    {
        transform.LookAt(miniCamera.transform);
    }

}
