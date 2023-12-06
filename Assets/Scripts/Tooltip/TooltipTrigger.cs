using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public string header;

    [Multiline()]
    public string content;

    private static LTDescr delay;
    
    public void OnPointerEnter(PointerEventData eventData) {
        delay = LeanTween.delayedCall(0.5f, () => {
            TooltipManager.Show(content, header);
        });
    }

    public void OnPointerExit(PointerEventData eventData) {
        LeanTween.cancel(delay.uniqueId);
        TooltipManager.Hide();
    }

    public void ChangeTooltip(String headerText, String contentText) {
        header = headerText;
        content = contentText;
    }
}
