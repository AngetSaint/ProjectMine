﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public void Interact(){
        PickUp();
    }

    void PickUp(){
        Debug.Log("Picking up " + item.name);
        bool wasPickedUp = Inventory.instance.AddItem(item);
        if(wasPickedUp)
            Destroy(gameObject);
    }

}
