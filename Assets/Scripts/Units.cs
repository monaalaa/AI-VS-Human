using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Units : MonoBehaviour
{
	public int AttackPower;
    public int AttackRange;
    public float Steps;

    public GameObject SelectedFlag;
    public Text PlayerHelth;

    [SerializeField]
    private int helth;

    public int Helth
    {
        get => helth;

        set
        {
            helth = value;
            if (helth > 0)
                PlayerHelth.text = "Helth " + helth.ToString();
            CheckIfItDisdroied();
        }
    }

    public void Start()
    {
        PlayerHelth.text = "Helth " + helth.ToString();
    }

    public virtual void Attack(Units unitToAttack) 
    {
        //Reduce enemy Helth
        unitToAttack.Helth -= AttackPower;
    }

    void CheckIfItDisdroied()
    {
        if (helth <= 0)
        {
            //Show Destroy Particle
            RemoveFromList(this);
            Destroy(gameObject);
        }
    }

    public virtual void RemoveFromList(Units destoiedUnit) { }

    public void OnUnitSelected()
    {
        SelectedFlag.SetActive(true);   
    }
    public void UnSelectUnit()
    {
        SelectedFlag.SetActive(false);
    }
}
