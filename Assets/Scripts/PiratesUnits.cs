using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratesUnits : Units
{
    public GameObject AttackFlag;
    List<PlayerUnits> playersInRange = new List<PlayerUnits>();
    bool isMoving;
    bool canAttack;

    PlayerUnits attackUnit;
    new void Start()
    {
        base.Start();
        UnitsManager.UnitSelected += TakeDecision;
    }
    private void Update()
    { 
        if (moveToDistination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            if (distance <= 1.1f+agent.stoppingDistance)
            {
                moveToDistination = false;
                //Check if he gonna attack
                if (canAttack)
                {
                    Attack(attackUnit);
                    canAttack = false;
                }
                UnitsManager.Instance.TurnBase();
                isMoving = false;
            }
        }
    }
    void TakeDecision()
    {
        if (UnitsManager.Instance.Selectedplayer == this)
        {
            if (SearchForEnemiesInAttackRange())
            {
                int playersInRangePower = 0;

                //get Weakest enemy in attack range
                PlayerUnits weakestPlayer = SearchForWeakestEnemy(ref playersInRangePower);

                //Check if am safe (get the weakest and attack ) else ( runaway )
                if (playersInRangePower < Health)
                {
                    destination = weakestPlayer.transform.position;
                    Vector3 dir = GetDirection(destination);
                    agent.SetDestination(transform.position + 3 * dir);
                    moveToDistination = true;
                    canAttack = true;
                    attackUnit = weakestPlayer;
                }
                else
                {
                    //runaway
                }
            }
            else
            {
                //else => get all players and search for the nearst and go in it's direction
                SearchForNearstEnemyOutOfAttackRange();
            }
        }
    }
    public void ShowAttackFlag()
    { AttackFlag.SetActive(true); }
    public void HideAttackFlag()
    { AttackFlag.SetActive(false); }
    bool SearchForEnemiesInAttackRange()
    {
        Units temp = UnitsManager.Instance.Selectedplayer;
        Collider[] colliders;
        colliders = Physics.OverlapSphere(temp.transform.position, temp.AttackRange, 1 << 8);
     
        for (int i = 0; i < colliders.Length; i++)
        {
            playersInRange.Add(colliders[i].gameObject.GetComponent<PlayerUnits>());
        }

        if (playersInRange.Count > 0)
        {
            return true;
        }

        return false;
    }
    Vector3 GetDirection(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        return dir;
    }
    void SearchForNearstEnemyOutOfAttackRange()
    {
        PlayerUnits player = new PlayerUnits();
        float distance = 1000;
        Vector3 tempPos = Vector3.zero;
        Vector3 targetPos = Vector3.zero;

        for (int i = 0; i < UnitsManager.Instance.Players.Count; i++)
        {
             tempPos = UnitsManager.Instance.Players[i].transform.position;

            //Search fo find nearst one and set it as target
            if (Vector3.Distance(transform.position, tempPos) < distance)
            {
                distance = Vector3.Distance(tempPos, transform.position);
                targetPos = tempPos;
            } 
        }
        destination = targetPos;
        Vector3 dir = GetDirection(targetPos) * Steps;
        destination = dir + transform.position;
        agent.SetDestination(destination);

        isMoving = true;
        moveToDistination = true;
    }
    public override void RemoveFromList(Units destroiedUnit)
    {
        UnitsManager.Instance.Pirates.Remove(destroiedUnit as PiratesUnits);
    }
    private PlayerUnits SearchForWeakestEnemy(ref int playersInRangePower)
    {
        PlayerUnits weakestPlayer = new PlayerUnits();
        for (int i = 0; i < playersInRange.Count; i++)
        {
            weakestPlayer = ComparePlayersHealth(weakestPlayer, playersInRange[i]);
            playersInRangePower += playersInRange[i].AttackPower;
        }
        return weakestPlayer;
    }
    private PlayerUnits ComparePlayersHealth(PlayerUnits playerA, PlayerUnits PlayerB)
    {
        if (playerA != null)
        {
            if (playerA.Health < PlayerB.Health)
                playerA = PlayerB;
        }
        else
            playerA = PlayerB;
        return playerA;
    }
    private void OnDestroy()
    {
        UnitsManager.UnitSelected -= TakeDecision;
    }
}
