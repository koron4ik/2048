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
    public Tile[] tiles;
    public float delay;

    void Awake()
    {
        gameField = GetComponentInChildren<GameField>();
        tiles = new Tile[16];
    }

    void Start()
    {
        GenerateDefaultTile();
        GenerateDefaultTile();
    }

    void Update()
    {
        ReadInput();
    }

    int[] GetEmptyTilesIndexes()
    {
        List<int> numbers = new List<int>();

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] == null)
            {
                numbers.Add(i);
            }
        }

        return numbers.ToArray();
    }

    void AddNewTile(int number, int index)
    {
        Tile tile = Resources.Load<Tile>(number.ToString());

        RectTransform rectTransform = tile.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        tile.transform.localPosition = Vector3.zero;

        tile.number = number;

        tiles[index] = Instantiate(tile, gameField.emptyTiles[index].transform.position, Quaternion.identity, gameField.emptyTiles[index].transform);
    }

    void MoveTile(int curIndex, int newIndex)
    {
        tiles[newIndex] = Instantiate(tiles[curIndex], gameField.emptyTiles[newIndex].transform.position, Quaternion.identity, gameField.emptyTiles[newIndex].transform);
        Destroy(tiles[curIndex].gameObject);
        tiles[curIndex] = null;
    }

    void Move(Direction dir)
    {
        ResetMergeFlags();
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(MoveCoroutine(dir));
        }

        GenerateDefaultTile();
    }

    void GenerateDefaultTile()
    {
        int[] emptyTilesIndexes = GetEmptyTilesIndexes();
        int newTileIndex = Random.Range(0, emptyTilesIndexes.Length);
        AddNewTile(2, emptyTilesIndexes[newTileIndex]);
    }

    void ResetMergeFlags()
    {
        foreach (Tile tile in tiles)
        {
            if (tile != null)
            {
                tile.isMerged = false;
            }
        }
    }

    IEnumerator MoveCoroutine(Direction dir)
    {
        switch (dir)
        {
            case Direction.LEFT:
                StartCoroutine(MoveTilesLeft());
                break;
            case Direction.RIGHT:
                StartCoroutine(MoveTilesRight());
                break;
            case Direction.UP:
                StartCoroutine(MoveTilesUp());
                break;
            case Direction.DOWN:
                StartCoroutine(MoveTilesDown());
                break;
        }
        yield return null;
    }

    void ReadInput()
    {
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

    IEnumerator MoveTilesDown()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 2; i >= 0; i--)
            {
                int curIndex = i * 4 + j;
                int newIndex = (i + 1) * 4 + j;

                if (tiles[curIndex] != null)
                {
                    if (CanMove(newIndex))
                    {
                        MoveTile(curIndex, newIndex);
                    }
                    else if (CanMerge(curIndex, newIndex))
                    {
                        MergeTwoTiles(curIndex, newIndex);
                    }
                }
            }
        }
        yield return new WaitForSeconds(delay);
    }

    IEnumerator MoveTilesUp()
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 1; i <= 3; i++)
            {
                int curIndex = i * 4 + j;
                int newIndex = (i - 1) * 4 + j;

                if (tiles[curIndex] != null)
                {
                    if (CanMove(newIndex))
                    {
                        MoveTile(curIndex, newIndex);
                    }
                    else if (CanMerge(curIndex, newIndex))
                    {
                        MergeTwoTiles(curIndex, newIndex);
                    }
                }
            }
        }
        yield return new WaitForSeconds(delay);
    }
    
    IEnumerator MoveTilesRight()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 2; j >= 0; j--)
            {
                int curIndex = i * 4 + j;
                int newIndex = i * 4 + j + 1;

                if (tiles[curIndex] != null)
                {
                    if (CanMove(newIndex))
                    {
                        MoveTile(curIndex, newIndex);
                    }
                    else if (CanMerge(curIndex, newIndex))
                    {
                        MergeTwoTiles(curIndex, newIndex);
                    }
                }
            }
        }
        yield return new WaitForSeconds(delay);
    }

    IEnumerator MoveTilesLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j <= 3; j++)
            {
                int curIndex = i * 4 + j;
                int newIndex = i * 4 + j - 1;

                if (tiles[curIndex] != null)
                {
                    if (CanMove(newIndex))
                    {
                        MoveTile(curIndex, newIndex);
                    }
                    else if (CanMerge(curIndex, newIndex))
                    {
                        MergeTwoTiles(curIndex, newIndex);
                    }
                }
            }
        }
        yield return new WaitForSeconds(delay);
    }

    void MergeTwoTiles(int curTileIndex, int newTileIndex)
    {
        Destroy(tiles[newTileIndex].gameObject);
        tiles[newTileIndex] = null;

        AddNewTile(tiles[curTileIndex].number * 2, newTileIndex);

        Destroy(tiles[curTileIndex].gameObject);
        tiles[curTileIndex] = null;
    }

    bool CanMove(int index)
    {
        return tiles[index] == null;
    }

    bool CanMerge(int curTileIndex, int newTileIndex)
    {
        return tiles[newTileIndex].number == tiles[curTileIndex].number && !tiles[curTileIndex].isMerged;
    }
}
