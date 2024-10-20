using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideLinesPopup : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(hideLinesPopup);
    }

    public void hideLinesPopup()
    {
        GetComponent<Transform>().parent.gameObject.GetComponent<LinesPopup>().hide();
    }
}
