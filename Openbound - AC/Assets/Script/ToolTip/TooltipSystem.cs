using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    static TooltipSystem current;

    public Tooltip tooltip;

    private void Awake()
    {
        current = this;

        tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
    }

    public static void Show()
    {
        current.tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
