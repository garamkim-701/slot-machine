using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasResize : MonoBehaviour
{
    private RectTransform rt;
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        
        //Camera.main <- get first enabled Camera which is tagged "MainCamera", Read Only
        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        rt.sizeDelta = new Vector2(screenX, screenY);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
