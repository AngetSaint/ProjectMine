using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public Collider2D p_LeftCollider;
    public Collider2D p_RightCollider;
    public Collider2D p_MiddleCollider;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Rock")){
            other.gameObject.GetComponent<RockCave>().MakeLoot();
            Destroy(other.gameObject);
        }
    }
}
