using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public int imageNumber;
    private SlotController slotController;
    private SpriteRenderer sr;
    private Transform transform;
    private Animator animator;
    public int i, j;
    private Vector3 dest = Vector3.zero;

    void Awake()
    {
        slotController = GameObject.Find("Slot").GetComponent<SlotController>();
        imageNumber = Random.Range(1, 9);
        
        animator = GetComponent<Animator>();
        //animator.GetComponent<SpriteRenderer>().sprite = slotController.getIconSprite(imageNumber);

        animator.runtimeAnimatorController = slotController.getIconAnimatorController(imageNumber);
        animator.runtimeAnimatorController.animationClips[0].wrapMode = WrapMode.ClampForever;

        transform = GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (animator.GetBool("isSpinning") || dest != Vector3.zero){
            if(dest != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, dest, 600 * Time.deltaTime / (transform.position.y - dest.y));
            } else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - 600 * Time.deltaTime, transform.position.z);
            }
        }

        if(transform.localPosition.y < -0.69f)
        {
            Destroy(gameObject);
        }
    }

    public int getImageNumber()
    {
        return imageNumber;
    }

    public void resize(float scaleX, float scaleY)
    {

        transform.localScale = new Vector3(scaleX, scaleY, transform.localScale.z);
    }

    public void setIsSpinning(bool isSpinning)
    {
        animator.SetBool("isSpinning", isSpinning);
    }

    public void setDestination(Vector3 dest_)
    {
        dest = dest_;
    }

    public void setIndex(int x, int y)
    {
        i = x;
        j = y;
    }

    void OnDestroy()
    {
        if (slotController != null)
        {
            slotController.createNewIcon(i, j);
        }
    }
}
