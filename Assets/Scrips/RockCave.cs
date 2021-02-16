using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCave : MonoBehaviour
{
    public LootTable thisLoot;

    public void MakeLoot(){
        if(thisLoot != null){
            ItemProperty current = thisLoot.LootItem();
            if(current != null){
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
}
