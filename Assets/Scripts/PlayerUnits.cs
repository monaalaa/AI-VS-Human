using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnits : Units
{
    public LayerMask MovmentMask;
    public GameObject Range;

    internal bool canMove = false;

    PiratesUnits pirate;
    List<PiratesUnits> piratesInRange = new List<PiratesUnits>();

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
                    if (pirate != null && canAttack)
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
                canAttack = true;
                OnDistnationReached();
            }
        }
    }

    public override void Attack(Units unitToAttack)
    {
        destination = unitToAttack.transform.position;
        int dis = (int)Vector3.Distance(destination, transform.position);
        if (dis <= AttackRange)
        {
            base.Attack(unitToAttack);
            UnitsManager.Instance.TurnBase();
            canAttack = false;
            pirate = null;
        }
    }

    public bool SearchForPirates()
    {
        piratesInRange.Clear();
        Collider[] colliders;
        colliders = Physics.OverlapSphere(transform.position, AttackRange, 1);
        for (int i = 0; i < colliders.Length; i++)
        {
            piratesInRange.Add(colliders[i].gameObject.GetComponent<PiratesUnits>());
            piratesInRange[i].ShowAttackFlag();
        }

        if (piratesInRange.Count > 0)
        {
            return true;
        }

        return false;
    }

    public void OnDistnationReached()
    {
        if (SearchForPirates())
        {
           UnitsManager.Instance.InvokeReadyToAction();
        }
        else
            UnitsManager.Instance.TurnBase();
    }

}

