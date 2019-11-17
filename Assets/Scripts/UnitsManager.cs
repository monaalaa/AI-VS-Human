﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;
    public static Action ReadyToAction;

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

    // Start is called before the first frame update
    void Start()
    {
        TurnBase();
    }

   public void TurnBase()
    {
        if(Selectedplayer != null)
        UnSelectUnit();

        if (playerTurn)
        {
            GoToNextUnit(Players.Count, Players.Cast<Units>().ToList(), false);
        }
        else
        {
            GoToNextUnit(Pirates.Count, Pirates.Cast<Units>().ToList(), true);
        }

        HideFlags();
        UIManager.Instance.DisableActionPanel();
    }
    private void GoToNextUnit(int count,List<Units> units, bool isPlayerTurn)
    {
        if (CurrentUnitIndex < count)
        {
            Selectedplayer = units[CurrentUnitIndex];
            OnUnitSelected();
            CurrentUnitIndex++;
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

    public void PlayerReachedDistnation()
    {
        if (SearchForPirates())
        {
           InvokeReadyToAction();
        }
        else
          TurnBase();
    }
    public void InvokeReadyToAction()
    {
        if (ReadyToAction != null)
        {
            ReadyToAction.Invoke();
        }
    }
    public void HideFlags()
    {
        for (int i = 0; i < piratesInRange.Count; i++)
        {
            piratesInRange[i].HideAttackFlag();
        }
        piratesInRange.Clear();
    }



}
