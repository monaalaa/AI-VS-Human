using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratesUnits : Units
{
    public GameObject AttackFlag;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public void ShowAttackFlag()
    { AttackFlag.SetActive(true); }
    public void HideAttackFlag()
    { AttackFlag.SetActive(false); }
}
