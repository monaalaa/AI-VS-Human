using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnits : Units
{
    public LayerMask MovmentMask;
    public GameObject Range;
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
           
            if (Physics.Raycast(ray, out hit))
            {
                //Click Ground To Move
                if (hit.collider.gameObject.layer == Mathf.Log(MovmentMask.value, 2)) 
                {
                    Move(hit);
                }

                else //ForAttack
                {
                    pirate = hit.collider.gameObject.GetComponent<PiratesUnits>();
                    if (pirate != null)
                    {
                        Attack(pirate);
                    }
                }
            }
        }

        if (moveToDistination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            if (distance <= 1.1f)
            {
                moveToDistination = false;

                UnitsManager.Instance.PlayerReachedDistnation();
            }
        }
    }

    private void Move(RaycastHit hit)
    {
        destination = hit.point;
        if (Vector3.Distance(destination, transform.position) < Steps)
        {
            agent.SetDestination(destination);

            UIManager.Instance.TextToNotify = name + " Moves To New Location";
            moveToDistination = true;
        }
    }

    public override void Attack(Units unitToAttack)
    {
        destination = unitToAttack.transform.position;
        if (Vector3.Distance(destination, transform.position) < AttackRange)
        {
            base.Attack(unitToAttack);
            UIManager.Instance.TextToNotify = name + " Is Attacking Pirate " + pirate.name;
            UnitsManager.Instance.TurnBase();
            pirate = null;
        }
    }

    public override void RemoveFromList(Units destroiedUnit)
    {
        UnitsManager.Instance.Players.Remove(destroiedUnit as PlayerUnits);
    }
}
