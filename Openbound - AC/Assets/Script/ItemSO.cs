using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    [TextArea(5,10)]
    public string itemDescription;
}
