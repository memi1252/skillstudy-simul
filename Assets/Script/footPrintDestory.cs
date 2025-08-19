using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class footPrintDestory : MonoBehaviour
{

    public SpriteRenderer SpriteRenderer;
    private Color color;
    private float index = 1;

    private void Start()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        color = SpriteRenderer.color;
        Destroy(gameObject, 15);
    }

    private void Update()
    {   if(index  > 0)
        {
            index -= Time.deltaTime / 10;
            SpriteRenderer.color = new Color(color.r, color.g, color.b, index);
        }
        
    }
}
