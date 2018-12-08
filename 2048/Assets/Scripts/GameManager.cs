using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}

public enum State
{
    Playing, 
    Pause
}

public class LastMove
{
    public Tile[,] tiles;
    public int score;

    public LastMove(Tile[,] tiles, int score)
    {
        this.tiles = tiles;
        this.score = score;
    }
}

public class GameManager : MonoBehaviour
{
    GameField gameField;

    Tile[,] tiles;
    List<Tile> emptyTiles;
    LastMove lastMove;
    State state;

    bool won;
    bool returned = true;

    public float delay;

    public GameObject gameOverPanel;
    public GameObject winPanel;

    readonly int rowCount = 4;
    readonly int columnCount = 4;

    void Awake()
    {
        gameField = GetComponentInChildren<GameField>();
        tiles = new Tile[4,4];
        emptyTiles = new List<Tile>();
        state = State.Playing;

        FillTiles();
    }

    void FillTiles()
    {
        for (int i = 0; i < rowCount; i++)
            for (int j = 0; j < columnCount; j++)
                AddNewTile(0, i, j);
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

    void UpdateEmptyTiles()
    {
        emptyTiles.Clear();
        for (int i = 0; i < rowCount; i++)
            for (int j = 0; j < columnCount; j++)
                if (tiles[i, j].isEmpty) 
                    emptyTiles.Add(tiles[i, j]);
    }

    void Move(Direction dir)
    {
        SaveMove();

        for (int i = 0; i < 3; i++)
            StartCoroutine(MoveCoroutine(dir));

        GenerateDefaultTile();
        ResetMergeFlags();

        if (Win() && !won)
        {
            winPanel.SetActive(true);
            won = true;
        }
        else if (NoMoves())
        {
            state = State.Pause;
            gameOverPanel.SetActive(true);
        }
        returned = false;
    }

    bool Win()
    {
        foreach (var tile in tiles)
            if (tile.number == 2048)
                return true;
        return false;
    }

    bool NoMoves()
    {
        return !(CanMove(Direction.UP) || CanMove(Direction.DOWN) || CanMove(Direction.LEFT) || CanMove(Direction.RIGHT));
    }

    void GenerateDefaultTile()
    {
        UpdateEmptyTiles();
        int newTileIndex = Random.Range(0, emptyTiles.Count);
        AddNewTile(2, emptyTiles[newTileIndex].row, emptyTiles[newTileIndex].column);
    }

    void ResetMergeFlags()
    {
        foreach (Tile tile in tiles)
            if (tile != null)
                tile.isMerged = false;
    }

    void SaveMove()
    {
        lastMove = new LastMove((Tile[,])tiles.Clone(), Score.Instance.score);
    }

    IEnumerator MoveCoroutine(Direction dir)
    {
        StartCoroutine(MoveTiles(dir));
        yield return null;
    }

    void ReadInput()
    {
        if (state == State.Playing)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Direction.LEFT))
                Move(Direction.LEFT);
            else if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Direction.RIGHT))
                Move(Direction.RIGHT);
            else if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Direction.UP))
                Move(Direction.UP);
            else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Direction.DOWN))
                Move(Direction.DOWN);
        }
    }

    IEnumerator MoveTiles(Direction dir)
    {
        switch (dir)
        {
            case Direction.LEFT:
                for (int i = 0; i < 4; i++)
                    for (int j = 1; j <= 3; j++)
                        Moving(tiles[i, j], tiles[i, j - 1]);
                break;
            case Direction.RIGHT:
                for (int i = 0; i < 4; i++)
                    for (int j = 2; j >= 0; j--)
                        Moving(tiles[i, j], tiles[i, j + 1]);
                break;
            case Direction.UP:
                for (int j = 0; j < 4; j++)
                    for (int i = 1; i <= 3; i++)
                        Moving(tiles[i, j], tiles[i - 1, j]);
                break;
            case Direction.DOWN:
                for (int j = 0; j < 4; j++)
                    for (int i = 2; i >= 0; i--)
                        Moving(tiles[i, j], tiles[i + 1, j]);
                break;
        }
        yield return new WaitForSeconds(delay);
    }

    bool CanMove(Direction dir)
    {
        switch (dir)
        {
            case Direction.LEFT:
                for (int i = 0; i < 4; i++)
                    for (int j = 1; j <= 3; j++)
                        if (!tiles[i, j].isEmpty && (tiles[i, j - 1].isEmpty || CanMerge(tiles[i, j], tiles[i, j - 1])))
                            return true;
                break;
            case Direction.RIGHT:
                for (int i = 0; i < 4; i++)
                    for (int j = 2; j >= 0; j--)
                        if (!tiles[i, j].isEmpty && (tiles[i, j + 1].isEmpty || CanMerge(tiles[i, j], tiles[i, j + 1])))
                            return true;
                break;
            case Direction.UP:
                for (int j = 0; j < 4; j++)
                    for (int i = 1; i <= 3; i++)
                        if (!tiles[i, j].isEmpty && (tiles[i - 1, j].isEmpty || CanMerge(tiles[i, j], tiles[i - 1, j])))
                            return true;
                break;
            case Direction.DOWN:
                for (int j = 0; j < 4; j++)
                    for (int i = 2; i >= 0; i--)
                        if (!tiles[i, j].isEmpty && (tiles[i + 1, j].isEmpty || CanMerge(tiles[i, j], tiles[i + 1, j])))
                            return true;
                break;
        }
        return false;
    }

    void Moving(Tile curTile, Tile newTile)
    {
        if (!curTile.isEmpty)
        {
            if (CanMoveTo(newTile))
                MoveTile(curTile, newTile);
            else if (CanMerge(curTile, newTile))
                MergeTwoTiles(curTile, newTile);
        }
    }

    void AddNewTile(int number, int row, int column)
    {
        Tile tile = Resources.Load<Tile>(number.ToString());

        tile.number = number;
        tile.row = row;
        tile.column = column;
        tile.Transform();
        tiles[row, column] = Instantiate(tile, 
                                         gameField.emptyTiles[row * rowCount + column].transform.position, 
                                         Quaternion.identity, 
                                         gameField.emptyTiles[row * rowCount + column].transform);
    }

    void MoveTile(Tile curTile, Tile newTile)
    {
        Destroy(curTile.gameObject);

        AddNewTile(0, curTile.row, curTile.column);
        AddNewTile(curTile.number, newTile.row, newTile.column);
    }

    void MergeTwoTiles(Tile curTile, Tile newTile)
    {
        Destroy(newTile.gameObject);
        Destroy(curTile.gameObject);

        AddNewTile(0, curTile.row, curTile.column);
        AddNewTile(curTile.number * 2, newTile.row, newTile.column);
        tiles[newTile.row, newTile.column].isMerged = true;

        Score.Instance.AddScore(tiles[newTile.row, newTile.column].number);
    }

    bool CanMoveTo(Tile tile)
    {
        return tile.isEmpty;
    }

    bool CanMerge(Tile curTile, Tile newTile)
    {
        return curTile.number == newTile.number && !newTile.isMerged && !curTile.isMerged;
    }

    public void NewGameButtonPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void KeepGoingButtonPressed()
    {
        winPanel.SetActive(false);
    }

    public void UndoButtonPressed()
    {
        if (!returned)
        {
            UpdateUI();
            returned = true;
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                Destroy(tiles[i, j].gameObject);

        FillTiles();
        tiles = lastMove.tiles;
        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 4; j++)
                AddNewTile(tiles[i, j].number, tiles[i, j].row, tiles[i, j].column);

        Score.Instance.score = lastMove.score;
        Score.Instance.scoreText.text = lastMove.score.ToString();
    }
}
