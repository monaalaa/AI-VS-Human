using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnits : Units
{
    public LayerMask MovmentMask;
    public GameObject Range;

    PiratesUnits pirate;
    internal bool canMove = false;
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
                if (hit.collider.gameObject.layer == Mathf.Log(MovmentMask.value, 2) && canMove)
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
                canMove = false;
                UnitsManager.Instance.WhenPlayerReachedDistnation();
            }
        }
    }

    public override void Attack(Units unitToAttack)
    {
        destination = unitToAttack.transform.position;
        int x = (int)Vector3.Distance(destination, transform.position);
        if (x < AttackRange)
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
