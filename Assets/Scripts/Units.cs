using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum UnitType
{
    Pirate,
    Player
}

public class Units : MonoBehaviour
{
	public int AttackPower;
    public int AttackRange;
    public float Steps;

    [SerializeField]
    Image HealthBar;

    public GameObject SelectedFlag;
    public UnitType type;
    protected NavMeshAgent agent;

    protected Vector3 destination;
    protected bool moveToDistination = false;

    [SerializeField]
    private float health;
    [SerializeField]
    private Sprite DethSprite;
    float initialHealth;
    public float Health
    {
        get => health;

        set
        {
            health = value;
            UpdateUnitHealth();
            CheckIfItDestroyed();
        }
    }

    public void Start()
    {
        initialHealth = Health;
        HealthBar.fillAmount = 1;
        agent = GetComponent<NavMeshAgent>();
    }
    private void UpdateUnitHealth()
    {
        if (health > 0)
        {
            HealthBar.fillAmount = (health / initialHealth);
            if (HealthBar.fillAmount <= 0.5)
            {
                HealthBar.sprite = DethSprite;
            }
        }
    }
    public virtual void Move(Vector3 location)
    {
        destination = location;
        if (Vector3.Distance(destination, transform.position) < Steps)
        {
            agent.SetDestination(destination);
            UnitsManager.Instance.InvokeActionHappned(name + " Moves To New Location");
            moveToDistination = true;
        }
    }
    public virtual void Attack(Units unitToAttack) 
    {
        //Reduce enemy Helth
        unitToAttack.Health -= AttackPower;
        UnitsManager.Instance.InvokeActionHappned(name + " Is Attacking " + unitToAttack.name);
        UnitsManager.Instance.InvokeUnitAttack(this, unitToAttack);
    }
    void CheckIfItDestroyed()
    {
        if (health <= 0)
        {
            UnitsManager.Instance.InvokeUnitDestroied(this);
        }
    }
}
