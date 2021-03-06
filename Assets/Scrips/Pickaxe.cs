﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public int power;

    public Collider2D p_LeftCollider;
    public Collider2D p_RightCollider;
    public Collider2D p_MiddleCollider;

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Trigger");
        if(other.gameObject.CompareTag("Rock")){
            other.gameObject.GetComponent<RockCave>().DestroyRock(power);
        }
    }
}
