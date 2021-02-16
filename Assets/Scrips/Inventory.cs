using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;

    void Awake() {
        if(instance != null){
            Debug.Log("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 12;

    public List<Item> items = new List<Item>();

    public List<GameObject> Weapons;
    private GameObject allWeapons;

    public List<GameObject> Pickaxes;
    private GameObject allPickaxes;

    void Start() {
        allWeapons = GameObject.Find("AllWeapons");
        foreach (Transform weapon in allWeapons.transform)
        {
            AddWeapon(weapon.gameObject);
        }

        allPickaxes = GameObject.Find("AllPickaxes");
        foreach (Transform pickaxe in allPickaxes.transform)
        {
            AddPickaxe(pickaxe.gameObject);
        }
    }

    public bool AddItem(Item item){
        if(!item.isDefaultItem)
        {
            if(items.Count >= space){
                Debug.Log("Not enough room");
                return false;
            }
            items.Add(item);

            if(onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
        }
        return true;
    }
    public void RemoveItem(Item item){
        items.Remove(item);

        if(onItemChangedCallback != null)
                onItemChangedCallback.Invoke();
    }

    void AddWeapon(GameObject weapon){
        Weapons.Add(weapon);
    }

    void AddPickaxe(GameObject pickaxe){
        Pickaxes.Add(pickaxe);
    }
}
