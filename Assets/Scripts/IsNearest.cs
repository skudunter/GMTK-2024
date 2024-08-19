using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNearest : MonoBehaviour
{
    private bool isNearest = false;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (isNearest)
        {
            // transform.get
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
    public void SetIsNearest(bool isNearest)
    {
        this.isNearest = isNearest;
    }
    public bool GetIsNearest()
    {
        return isNearest;
    }
}
