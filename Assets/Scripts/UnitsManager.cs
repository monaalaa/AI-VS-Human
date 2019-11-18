using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;
    public static Action ReadyToMakeAction;
    public static Action UnitSelected;
    public static Action<Units, Units> UnitAttack;
    public static Action<Units> UnitDetroyed;


    [HideInInspector]
    public Units Selectedplayer;

    public List<PlayerUnits> Players = new List<PlayerUnits>();
    public List<PiratesUnits> Pirates = new List<PiratesUnits>();

    int CurrentUnitIndex = 0;
    bool playerTurn = true;

    List<PiratesUnits> piratesInRange = new List<PiratesUnits>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        TurnBase();
    }
    public void TurnBase()
    {
        if (Selectedplayer != null)
            UnSelectUnit();

        if (playerTurn)
        {
            GoToNextUnit(Players.Count, Players.Cast<Units>().ToList(), false);
            if(Selectedplayer.GetComponent<PlayerUnits>()!=null)
            Selectedplayer.GetComponent<PlayerUnits>().canMove = true;
        }
        else
        {
            GoToNextUnit(Pirates.Count, Pirates.Cast<Units>().ToList(), true);
        }

        HideFlags();
        UIManager.Instance.DisableActionPanel();
    }
    private void GoToNextUnit(int count, List<Units> units, bool isPlayerTurn)
    {
        if (CurrentUnitIndex < count)
        {
            Selectedplayer = units[CurrentUnitIndex];
            OnUnitSelected();
            CurrentUnitIndex++;
            if (UnitSelected != null)
                UnitSelected.Invoke();
        }
        else
        {
            CurrentUnitIndex = 0;
            playerTurn = isPlayerTurn;
            TurnBase();
        }


    }
    public void OnUnitSelected()
    {
        Selectedplayer.SelectedFlag.SetActive(true);
        PlayerUnits temp = Selectedplayer.GetComponent<PlayerUnits>();
        if (temp != null)
            temp.Range.SetActive(true);
    }
    public void UnSelectUnit()
    {
        Selectedplayer.SelectedFlag.SetActive(false);
        PlayerUnits temp = Selectedplayer.GetComponent<PlayerUnits>();
        if (temp != null)
            temp.Range.SetActive(false);
    }
    public bool SearchForPirates()
    {
        Collider[] colliders;
        colliders = Physics.OverlapSphere(Selectedplayer.transform.position, Selectedplayer.AttackRange, 1);
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
    //Note: De mesh elmfrod tkon hena 3lshan hya 7aga khasa b elplayer bs lkn el pirate mesh by3ml keda
    public void WhenPlayerReachedDistnation()
    {
        if (SearchForPirates())
        {
            InvokeReadyToAction();
        }
        else
            TurnBase();
    }

    public void HideFlags()
    {
        for (int i = 0; i < piratesInRange.Count; i++)
        {
            piratesInRange[i].HideAttackFlag();
        }
        piratesInRange.Clear();
    }

    public void InvokeReadyToAction()
    {
        if (ReadyToMakeAction != null)
        {
            ReadyToMakeAction.Invoke();
        }
    }
    public void InvokeUnitDestroied(Units unit)
    {
        if (UnitDetroyed != null)
            UnitDetroyed.Invoke(unit);
        Destroy(unit.gameObject);
    }
    public void InvokeUnitAttack(Units arg1,Units arg2)
    {
        if (UnitAttack != null)
            UnitAttack.Invoke(arg1,arg2);
    }
}
