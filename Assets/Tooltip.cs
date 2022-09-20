using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        TooltipManager._tooltip.SetAndShowToolTip(message);
    }
    
    private void OnMouseExit()
    {
        TooltipManager._tooltip.HideToolTip();
    }
}
