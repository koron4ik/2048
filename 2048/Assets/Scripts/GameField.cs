using UnityEngine;

public class GameField : MonoBehaviour 
{
    public Tile[] emptyTiles;

    void Awake()
    {
        Tile emptyTile = Resources.Load<Tile>("0");
        emptyTiles = new Tile[16];

        for (int i = 0; i < 16; i++)
        {
            emptyTiles[i] = Instantiate(emptyTile, transform);
        }
    }
}
