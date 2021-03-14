using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuUI : MonoBehaviour
{
    public GameObject upgradeMenuUI;

    public void OpenClose(){
        upgradeMenuUI.SetActive(!upgradeMenuUI.activeSelf);
    }
}
