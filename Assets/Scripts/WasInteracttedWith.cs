using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WasInteracttedWith : MonoBehaviour
{
    private bool wasInteracttedWith = false;

    // private SpriteRenderer spriteRenderer;
    // // private void Start()
    // // {
    // //     spriteRenderer = GetComponent<SpriteRenderer>();
    // // }
    // // void Update(){
    // //     if (wasInteracttedWith)
    // //     {
    // //         spriteRenderer.color = Color.red;
    // //     }
    // // }

    public void SetWasInteracttedWith(bool wasInteracttedWith)
    {
        this.wasInteracttedWith = wasInteracttedWith;
    }

    public bool GetWasInteracttedWith()
    {
        return wasInteracttedWith;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SoundManager.PlayCollisionSound(transform.position);
            wasInteracttedWith = true;
        }
        WasInteracttedWith otherWasInteracttedWith =
            collision.gameObject.GetComponent<WasInteracttedWith>();
        if (otherWasInteracttedWith != null)
        {
            //asteroid
            SoundManager.PlayCollisionSound(transform.position);
            if (otherWasInteracttedWith.wasInteracttedWith)
            {
                wasInteracttedWith = true;
            }
        }
    }
}
