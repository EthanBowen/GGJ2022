using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    LayerMask HoverHighlightLayer;
    LayerMask StartingLayer;

    // Start is called before the first frame update
    void Start()
    {
        HoverHighlightLayer = LayerMask.NameToLayer("HoverHighlight");

        StartingLayer = gameObject.layer;
    }

    public void TurnOn()
    {
        if (HoverHighlightLayer != -1)
        {
            gameObject.layer = HoverHighlightLayer;
        }
    }
    
    public void TurnOff()
    {
        gameObject.layer = StartingLayer;
    }
}
