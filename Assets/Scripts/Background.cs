using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer sr;
    //private Camera Camera;
    void Awake()
    {
        //Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        sr = GetComponent<SpriteRenderer>();

        float spriteX = sr.sprite.bounds.size.x;
        float spriteY = sr.sprite.bounds.size.y;

        //Camera.main <- get first enabled Camera which is tagged "MainCamera", Read Only
        float screenY = Camera.main.orthographicSize * 2;
        float screenX = screenY / Screen.height * Screen.width;

        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        transform.localScale = new Vector2(Mathf.Ceil(screenX / spriteX), Mathf.Ceil(screenY / spriteY));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
