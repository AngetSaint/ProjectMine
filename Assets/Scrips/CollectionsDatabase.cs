using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionsDatabase : MonoBehaviour
{
    #region Singleton

    public static CollectionsDatabase instance;

    void Awake() {
        BuildCollectionsDatabase();
        if(instance != null){
            Debug.Log("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public List<Collections> collection = new List<Collections>();
    
    void BuildCollectionsDatabase(){
        collection = new List<Collections>(){
            new Collections(0, "Copper Ore", 0),
            new Collections(1, "Iron Ore", 0),
            new Collections(2, "Gold Ore", 0),
            new Collections(3, "Rare Crystal", 0),
            new Collections(4, "Wazowski Ball", 0),
        };
    }

    public void AddItemToCollection(int itemId){
        collection[itemId].amount ++;
    }
    public void RemoveItemsInCollection(int itemId, int amount){
        collection[itemId].amount -= amount;
    }

    public bool CheckForItemsInCollection(int itemId, int amount){
        if(collection[itemId].amount >= amount)
            return true;
        else return false;
    }
}
