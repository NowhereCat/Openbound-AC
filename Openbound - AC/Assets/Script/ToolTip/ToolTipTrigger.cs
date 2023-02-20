using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    static LTDescr delay;

    public string ttName;
    [TextArea(5, 10)]
    public string ttDescription;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("INSIDE | " + eventData);
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.Show(ttDescription, ttName);
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.Hide();
    }
}
