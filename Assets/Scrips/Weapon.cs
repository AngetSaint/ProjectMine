using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int power = 0;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            other.gameObject.GetComponent<Enemy>().TakeDamage(power);
        }
    }
}
