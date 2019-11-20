using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratesUnits : Units
{
    public GameObject AttackFlag;
    List<PlayerUnits> playersInRange = new List<PlayerUnits>();
    bool isMoving;

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
                if (playersInRangePower < Health || weakestPlayer.Health <= AttackPower)
                {
                    destination = weakestPlayer.transform.position;
                    Vector3 dir = GetDirection(destination,transform.position);
                    Move(transform.position + 3 * dir);
                    canAttack = true;
                    attackUnit = weakestPlayer;
                }
                else
                {
                    RunAway();
                }
            }
            else
            {
                //else => get all players and search for the nearst and go in it's direction
                SearchForNearstEnemyOutOfAttackRange();
            }
        }
    }
    void RunAway()
    {
        float distance = 1000;
        Vector3 positionToRunFrom = Vector3.zero;
        //search for closest enemy
        for (int i = 0; i < UnitsManager.Instance.Players.Count; i++)
        {
            float tempDis = Vector3.Distance(UnitsManager.Instance.Players[i].transform.position, transform.position);
            if (tempDis < distance)
            {
                distance = tempDis;
                positionToRunFrom = UnitsManager.Instance.Players[i].transform.position;
            }
        }
        MoveInDirection(positionToRunFrom, -1);
    }
    public void ShowAttackFlag()
    { AttackFlag.SetActive(true); }
    public void HideAttackFlag()
    { AttackFlag.SetActive(false); }
    bool SearchForEnemiesInAttackRange()
    {
        playersInRange.Clear();
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
    Vector3 GetDirection(Vector3 target, Vector3 current)
    {
        Vector3 dir = (target - current).normalized;
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
        MoveInDirection(targetPos, 1);
        isMoving = true;
    }
    private void MoveInDirection(Vector3 targetPos,int dir)
    {
        Vector3 dirVal = GetDirection(targetPos, transform.position);
        destination = (dirVal * Steps * dir) + transform.position;
        agent.SetDestination(destination);
        UnitsManager.Instance.InvokeActionHappned(name + " Moves To New Location");
        moveToDistination = true;
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
