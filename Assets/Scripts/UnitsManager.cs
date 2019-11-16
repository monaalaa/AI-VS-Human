using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public static UnitsManager Instance;

    [HideInInspector]
    public Units Selectedplayer;

    public List<PlayerUnits> Players = new List<PlayerUnits>();
    public List<PiratesUnits> Pirates = new List<PiratesUnits>();

    int CurrentUnitIndex = 0;
    bool playerTurn = true;

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
        if (playerTurn)
        {
            GoToNextUnit(Players.Count, Players.Cast<Units>().ToList(), false);
        }
        else
        {
            GoToNextUnit(Pirates.Count, Pirates.Cast<Units>().ToList(), true);

        }
    }
    private void GoToNextUnit(int count,List<Units> units, bool isPlayerTurn)
    {
        if (CurrentUnitIndex < count)
        {
            Selectedplayer = units[CurrentUnitIndex];
            units[CurrentUnitIndex].OnUnitSelected();
            CurrentUnitIndex++;
        }
        else
        {
            CurrentUnitIndex = 0;
            playerTurn = isPlayerTurn;
            TurnBase();
        }


    }
}
