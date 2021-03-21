using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeItemSlot : MonoBehaviour
{
    public int itemID;
    public int itemCost;
    public int upgradeID;

    public bool ableToUpgrade;

    public Pickaxe starterPickaxe;
    public Weapon starterSword;

    public void TryingToUpgrade(){
        ableToUpgrade = CollectionsDatabase.instance.CheckForItemsInCollection(itemID, itemCost);
        if(ableToUpgrade){
            CollectionsDatabase.instance.RemoveItemsInCollection(itemID,itemCost);
            UpgradeItem(upgradeID);
            Destroy(this.gameObject);
        }

    }

    void UpgradeItem(int upgradeId){
        switch(upgradeId){
            case 0:
                starterPickaxe.power ++;
                break;
            case 1:
                starterSword.power = 100;
                break;
        }
    }
}
