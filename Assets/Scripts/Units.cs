using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Units : MonoBehaviour
{
	public int AttackPower;
    public int AttackRange;
    public float Steps;
    [SerializeField]
    Image HealthBar;

    public GameObject SelectedFlag;

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

    public void Start()
    {
        initialHealth = Health;
        HealthBar.fillAmount = 1;
        agent = GetComponent<NavMeshAgent>();
    }

    public virtual void Move(Vector3 location)
    {
        destination = location;
        if (Vector3.Distance(destination, transform.position) < Steps)
        {
            agent.SetDestination(destination);

            UIManager.Instance.TextToNotify = name + " Moves To New Location";
            moveToDistination = true;
        }
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
