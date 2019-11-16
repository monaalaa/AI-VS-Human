using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnits : Units
{
    public LayerMask MovmentMask;

    bool canAttack = false;
    bool moveToDistination = false;

    PiratesUnits pirate;
    Vector3 destination;

    NavMeshAgent agent;
    new void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this == UnitsManager.Instance.Selectedplayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
           
            //Player JustMove
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == Mathf.Log(MovmentMask.value, 2))
                {
                    agent.stoppingDistance = AttackRange;
                   
                    destination = hit.point;

                    agent.SetDestination(destination);

                    UIManager.Instance.TextToNotify =name+ " Moves To New Location";
                }
                else 
                {
                    pirate = hit.collider.gameObject.GetComponent<PiratesUnits>();
                    if (pirate != null)
                    {
                        destination = pirate.transform.position;
                        agent.SetDestination(destination);
                        canAttack = true;
                        UIManager.Instance.TextToNotify = name + " Is Attacking Pirate " + pirate.name;
                    }
                }
                moveToDistination = true;

            }
        }

        if (moveToDistination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            if (distance <= AttackRange)
            {
                if (canAttack)
                {
                    Attack(pirate);
                }
                moveToDistination = false;
                UnSelectUnit();
                UnitsManager.Instance.TurnBase();
               
            }
        }
    }

    public override void Attack(Units unitToAttack)
    {
        base.Attack(unitToAttack);
        pirate = null;
        canAttack = false;
    }

    public override void RemoveFromList(Units destroiedUnit)
    {
        UnitsManager.Instance.Players.Remove(destroiedUnit as PlayerUnits);
    }
}
