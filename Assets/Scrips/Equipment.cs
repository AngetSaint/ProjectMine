using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    //Animation states
    const string PICKAXE_MINING = "Mining";

    private Inventory inventory;
    public Player player;
    public GameObject equippedWeapon;
    public GameObject equippedPickaxe;
    public GameObject wep;
    public GameObject wap;
    private Animator animator;

    private string currentAnimaton;

    void Start()
    {
        inventory = gameObject.GetComponent<Inventory>();

        Invoke("EquipStart", 0.1f);
    }
    void EquipStart(){
        if(inventory.Weapons.Capacity > 0){
            EquipWeapon(1);
        }
        if(inventory.Pickaxes.Capacity > 0){
            EquipPickaxe(1);
        }
    }

    public void EquipWeapon(int step){
        int capacity = inventory.Weapons.Count;
        int index = inventory.Weapons.IndexOf(equippedWeapon);

        if(index + step >= capacity){
            index = 0;
        }
        else if(index + step < 0){
            index = capacity - 1;
        }
        else{
            index += step;
        }

        /*if(equippedWeapon != null){
            Destroy(wep);
        }*/

        equippedWeapon = inventory.Weapons[index];
    }

    public void EquipPickaxe(int step){
        int capacity = inventory.Pickaxes.Count;
        int index = inventory.Pickaxes.IndexOf(equippedPickaxe);

        if(index + step >= capacity){
            index = 0;
        }
        else if(index + step < 0){
            index = capacity - 1;
        }
        else{
            index += step;
        }

        /*if(equippedWeapon != null){
            Destroy(wep);
        }*/

        equippedPickaxe = inventory.Pickaxes[index];
        //UI_Image.sprite = wep.GetComponent<SpriteRenderer>().sprite;
    }

    public void AtackWithEquippedWeapon(){
        wep = Instantiate(equippedWeapon);
        wep.transform.SetParent(gameObject.transform.Find("Hand"));
        wep.GetComponent<SpriteRenderer>().sortingLayerName = player.hand.GetComponent<SpriteRenderer>().sortingLayerName;
        wep.transform.localPosition = Vector3.zero;
        wep.SetActive(true);
    }
    public void MineWithEquippedWeapon(){
        wep = Instantiate(equippedPickaxe);
        wep.transform.SetParent(gameObject.transform.Find("Hand"));
        wep.GetComponent<SpriteRenderer>().sortingLayerName = player.hand.GetComponent<SpriteRenderer>().sortingLayerName;
        wep.transform.localPosition = Vector3.zero;
        animator = wep.GetComponent<Animator>();
        if(player.lastMovement.x == 0)
            ChangeAnimationState(PICKAXE_MINING);
        wep.SetActive(true);
    }
    public void DestroyEquippedWeapon(){
        animator = null;
        currentAnimaton = null;
        Destroy(wep);
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}
