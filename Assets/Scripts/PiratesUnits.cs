using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratesUnits : Units
{
    public GameObject AttackFlag;
    public LayerMask AttackingLayer;
    List<PlayerUnits> playersInRange = new List<PlayerUnits>();
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    void TakeDecision()
    {
        //Search For anemy neighbour
        if (SearchForPlayers())
        {
            //if found => Check if am save (get the weakest and attack ) else ( runaway )
            int playersInRangePower = 0;
            PlayerUnits weakestPlayer = new PlayerUnits(); ;
            for (int i = 0; i < playersInRange.Count; i++)
            {
                if (Vector3.Distance(transform.position, destination) < playersInRange[i].AttackRange)//He Can attack me
                {
                    playersInRangePower += playersInRange[i].AttackPower;
                }
                weakestPlayer = GetWeakestPlayer(weakestPlayer, playersInRange[i]);
            }

            if (playersInRangePower > AttackPower)
            { //Should Run Away
            }
            else
            {
                Vector3 dir = GetDirection(weakestPlayer.transform.position);
            }
        }
        //else => get all players and search for the nearst and go in it's direction
    }

    private PlayerUnits GetWeakestPlayer(PlayerUnits playerA, PlayerUnits PlayerB)
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

    public void ShowAttackFlag()
    { AttackFlag.SetActive(true); }
    public void HideAttackFlag()
    { AttackFlag.SetActive(false); }

    bool SearchForPlayers()
    {
        Units temp = UnitsManager.Instance.Selectedplayer;
        Collider[] colliders;
        colliders = Physics.OverlapSphere(temp.transform.position, temp.AttackRange, (int)Mathf.Log(AttackingLayer.value, 2));
     
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
        Vector3 dir = (transform.position - target).normalized;
        return dir;
    }

    public override void RemoveFromList(Units destroiedUnit)
    {
        UnitsManager.Instance.Pirates.Remove(destroiedUnit as PiratesUnits);
    }
}
