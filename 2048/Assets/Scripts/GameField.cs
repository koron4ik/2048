using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameField : MonoBehaviour 
{

    public Tile emptyTile;
    public Tile[] emptyTiles;

    private void Awake()
    {
        emptyTile = Resources.Load<Tile>("EmptyTile");
        emptyTiles = new Tile[16];

        FillFieldWithEmptyTiles();
    }

    private void FillFieldWithEmptyTiles()
    {
        for (int i = 0; i < 16; i++)
        {
            emptyTiles[i] = Instantiate(emptyTile, transform);
        }
    }

}
