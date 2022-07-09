using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null)
        {
            // For Shield flicker effect
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
        
    }
}
