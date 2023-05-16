using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    public GameObject panelInfo;
    private void Update()
    {
        Time.timeScale = panelInfo.activeSelf ? 0 : 1;
    }

}
