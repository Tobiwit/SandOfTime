using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string header;
    public bool tooltipEnabled = true;

    [Multiline()]
    public string content;

    private static LTDescr delay;
    
    public void OnPointerEnter(PointerEventData eventData) {
        if(tooltipEnabled) {
            delay = LeanTween.delayedCall(0.5f, () => {
            TooltipManager.Show(content, header);
            });
    
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(tooltipEnabled) { 
            LeanTween.cancel(delay.uniqueId);
            TooltipManager.Hide();
        }
        
    }

    public void ChangeTooltip(String headerText, String contentText) {
        header = headerText;
        content = contentText;
    }

    public void hideTooltip() {
        LeanTween.cancel(delay.uniqueId);
        TooltipManager.Hide();
    }
}
