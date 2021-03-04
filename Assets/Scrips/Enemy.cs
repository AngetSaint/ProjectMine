using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;
    public LootTable thisLoot;

    public int power = 0;
    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        if(currentHealth <= 0){
            MakeLoot();
            Destroy(this.gameObject);
        }

        healthBar.SetHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<Player>().TakeDamage(power);
        }
    }

    public void MakeLoot(){
        if(thisLoot != null){
            ItemProperty current = thisLoot.LootItem();
            if(current != null){
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
}
