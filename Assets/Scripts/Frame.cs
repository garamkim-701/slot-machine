using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour
{
    private Transform transform;
    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    public void resize(float scaleX, float scaleY)
    {
        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
    }
}
