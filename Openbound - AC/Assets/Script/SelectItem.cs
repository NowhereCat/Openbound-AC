using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : MonoBehaviour
{

    [SerializeField] int index = 0;

    public void ShowItem()
    {
        UIManager._instance.DisplayItem(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

}
