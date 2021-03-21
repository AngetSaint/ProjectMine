using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelPoint : MonoBehaviour
{
    public Transform playerPos;

    public void TravelToPoint(){
        Player.Instance.gameObject.transform.position = playerPos.transform.position;
    }
}
