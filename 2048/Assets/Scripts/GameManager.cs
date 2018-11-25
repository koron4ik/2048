using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Direction 
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public class GameManager : MonoBehaviour 
{
    public GameField gameField;
    public List<Tile> tiles;

    private void Awake()
    {
        gameField = GetComponentInChildren<GameField>();
        tiles = new List<Tile>();
    }

    void Start () 
    {
        AddNewTile(2);
    }
    
    void Update () 
    {
        ReadInput();
    }

    private void AddNewTile(int number)
    {
        Tile tile = Resources.Load<Tile>(number.ToString());
        tiles.Add(Instantiate(tile, gameField.emptyTiles[2].transform.position, Quaternion.identity, gameField.emptyTiles[2].transform));

        RectTransform rectTransform = tiles[0].GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    private void Move(Direction dir) 
    {
        switch (dir) 
        {
            case Direction.LEFT:
                break;
            case Direction.RIGHT:
                break;
            case Direction.UP:
                break;
            case Direction.DOWN:
                break;
        }
    }

    private void ReadInput() {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Direction.RIGHT);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Direction.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Direction.DOWN);
        }
    }

}
