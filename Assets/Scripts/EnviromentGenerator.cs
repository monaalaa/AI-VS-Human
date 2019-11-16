using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentGenerator : MonoBehaviour
{
    public GameObject Player;
    public GameObject Pairets;
    public Transform PlayersParent;
    public Transform PairetsParent;

    public int X_Range;
    public int Z_Range;
    int numberOFPlayers;
    int numberOFPairets;

    List<Positions> positionsList = new List<Positions>();
    // Start is called before the first frame update
    void Start()
    {
        PrepareGeneration();
        GeneratePositions();
        GenerateUnitesAtRandomPositions();
    }

    void PrepareGeneration()
    {
        numberOFPlayers = Random.Range(2, 4);
        numberOFPairets = Random.Range(4, 8);
    }

    void GeneratePositions()
    {
        for (int x = -X_Range; x < X_Range; x++)
        {
            for (int z = -Z_Range; z < Z_Range; z++)
            {
                positionsList.Add(new Positions(x, z));
            }
        }
    }

    void GenerateUnitesAtRandomPositions()
    {
        for (int i = 0; i < numberOFPlayers; i++)
        {
            GameObject tempObj = InstantiateUnit(i,PlayersParent,Player);
            UnitsManager.Instance.Players.Add(tempObj.GetComponent<PlayerUnits>());
        }

        for (int i = 0; i < numberOFPairets; i++)
        {
            GameObject tempObj = InstantiateUnit(i, PairetsParent, Pairets);
            UnitsManager.Instance.Pirates.Add(tempObj.GetComponent<PiratesUnits>());
        }
    }

    private GameObject InstantiateUnit(int i,Transform parent,GameObject type)
    {
        int index = Random.Range(0, positionsList.Count - 1);
        Vector3 temPos = new Vector3(positionsList[index].XValue, 0, positionsList[index].ZValue);
        GameObject tempObj = GameObject.Instantiate(type, temPos, new Quaternion());
        tempObj.name = "Player " + i.ToString();
        tempObj.transform.parent = parent;
        return tempObj;
    }
}

public struct Positions
{
    public Positions(int xValue, int zValue)
    {
        XValue = xValue;
        ZValue = zValue;
    }

    public int XValue { get; private set; }
    public int ZValue { get; private set; }
}
