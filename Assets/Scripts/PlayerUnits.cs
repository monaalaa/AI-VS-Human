using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnits : Units
{
    public LayerMask MovmentMask;
    public GameObject Range;

    PiratesUnits pirate;
    new void Start()
    {
        base.Start(); 
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
                    Move(hit.point);
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

                UnitsManager.Instance.WhenPlayerReachedDistnation();
            }
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
