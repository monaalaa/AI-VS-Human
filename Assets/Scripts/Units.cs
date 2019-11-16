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
    public Text PlayerHealth;

    [SerializeField]
    private int health;

    public int Health
    {
        get => health;

        set
        {
            health = value;
            if (health > 0)
                PlayerHealth.text = "Helth " + health.ToString();
            CheckIfItDestroyed();
        }
    }

    public void Start()
    {
        PlayerHealth.text = "Helth " + health.ToString();
    }

    public virtual void Attack(Units unitToAttack) 
    {
        //Reduce enemy Helth
        unitToAttack.Health -= AttackPower;
    }

    void CheckIfItDestroyed()
    {
        if (health <= 0)
        {
            //Show Destroy Particle
            RemoveFromList(this);
            Destroy(gameObject);
        }
    }

    public virtual void RemoveFromList(Units destroyedUnit) { }

}
