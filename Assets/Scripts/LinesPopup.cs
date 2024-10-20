using UnityEngine;
using UnityEngine.UI;

public class LinesPopup : CanvasResize
{
    private GameObject page1;
    private GameObject page2;
    void Awake()
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("UI Camera").GetComponent<Camera>();
        page1 = GetComponent<Transform>().Find("Page1").gameObject;
        page2 = GetComponent<Transform>().Find("Page2").gameObject;

        page1.transform.GetComponentInChildren<Button>().onClick.AddListener(openPage2);
        page2.transform.GetComponentInChildren<Button>().onClick.AddListener(openPage1);
        page2.SetActive(false);
    }
    // Start is called before the first frame update
    public void hide()
    {
        Destroy(gameObject);
    }

    public void openPage1()
    {
        page2.SetActive(false);
        page1.SetActive(true);
    }

    public void openPage2()
    {
        page1.SetActive(false);
        page2.SetActive(true);
    }
}
