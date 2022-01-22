using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RectTransform‚ğŸè‚Éæ‚Á‚Ä‚­‚ê‚éŠg’£ƒƒ\ƒbƒh
/// </summary>
public static class HelpTrans
{
    public static RectTransform GetRectTransform(this GameObject obj)
    {
        return obj.GetComponent<RectTransform>();
    }
}
