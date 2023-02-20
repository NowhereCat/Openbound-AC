using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager _instance;

    [SerializeField] List<ItemSO> inventory;
    [SerializeField] int itemCap = 20;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemSO GetInventoryItem(int index)
    {
        if (inventory.Count == 0) 
            return null;

        return inventory[index];
    }

    public List<ItemSO> GetAllInventoryItems()
    {
        if(inventory.Count == 0)
            return null;
        else
            return inventory;
    }

    public int GetInvetorySize()
    {
        return inventory.Count;
    }

    public void AddItem(ItemSO item)
    {
        if (inventory.Count <= itemCap)
        {
            inventory.Add(item);

            UIManager._instance.UpdateInventoryUI();

        } else Debug.LogWarning("Iventory Full");
    }

}
