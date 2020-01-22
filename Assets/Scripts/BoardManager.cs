using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : Singleton<BoardManager>
{

    #region Variables

    [Header("References")]
    public GameObject tilePrefab;

    [Header("Settings")]
    public Color TileColor;
    public Color TileMouseOverColor;


    [Range(0f,0.2f),SerializeField]
    private float spacing = 0.1f;

    public void SetTileSpacing(float newSpacing)
    {
        dirtyFlag = true;
        spacing = newSpacing;
    }

    [SerializeField]
    int matchAcceptLimit = 3;


    Tile[,] board;
    private int boardSize = 5;

    private int matchCounter = 0;

    public int GetBoardSize()
    {
        return boardSize;
    }

    public void SetBoardSize(int newSize)
    {
        dirtyFlag = true;
        boardSize = newSize;
    }


    float tileSize = 1f;


    float CameraWidth => Camera.main.aspect * CameraHeight;

    float CameraHeight => Camera.main.orthographicSize;

    Vector2 BoardLeftBottom => new Vector2(transform.position.x - (CameraWidth), transform.position.y - (CameraWidth));

    #endregion

    void Start()
    {
        Rebuild();
    }

    void CalculateTileSize() => tileSize = (CameraWidth * 2) / boardSize - spacing;

    bool dirtyFlag = true;
    void Update()
    {

        if (dirtyFlag) {
            CalculateTileSize();

            float posy = BoardLeftBottom.y + tileSize * 0.5f;

            for (int y = 0; y < boardSize; y++) {
                float posx = BoardLeftBottom.x + tileSize * 0.5f;

                for (int x = 0; x < boardSize; x++) {

                    Vector3 tilePosition = new Vector3(posx, posy, 0f);
                    board[x, y].go.transform.position = tilePosition;
                    board[x, y].go.transform.localScale = Vector3.one * tileSize;

                    posx += tileSize + spacing;
                }
                posy += tileSize + spacing;
            }
            dirtyFlag = false;
        }
    }

    public void Rebuild()
    {
        if (board != null) {
            foreach (Tile tile in board) {
                Destroy(tile.go);
            }
        }

        board = new Tile[boardSize, boardSize];

        CalculateTileSize();

        float posy = BoardLeftBottom.y + tileSize * 0.5f;

        for (int y = 0; y < boardSize; y++) {

            float posx = BoardLeftBottom.x + tileSize * 0.5f;

            for (int x = 0; x < boardSize; x++) {

                Vector3 tilePosition = new Vector3(posx, posy, 0f);

                GameObject tileGO = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);
                tileGO.transform.localScale = Vector3.one * tileSize;

                TileBehaviour tb = tileGO.AddComponent<TileBehaviour>();
                tb.Init(x, y);

                Tile tile = new Tile(tileGO, x, y);
                board[x, y] = tile;


                posx += tileSize + spacing;
            }
        }

    }


    public void MarkTile(int indexX, int indexY)
    {
        board[indexX, indexY].marked = true;

        IEnumerator<Vector2Int> matches;
        if(FindMatchBreadthFirst(indexX, indexY,out matches)) {
            //Destroy match
            while (matches.MoveNext()) {

                Vector2Int match = matches.Current;
                UnmarkTile(match);
            }

            //increment match count in ui
            matchCounter++;
            UIManager.Instance.SetMatchCountText(matchCounter);
        }
    }

    void UnmarkTile(Vector2Int index)
    {
        Tile tile = board[index.x, index.y];
        tile.marked = false;
        tile.go.GetComponent<TileBehaviour>().Unmark();
    }

    // O(n x log(n)) complexity
    bool FindMatchBreadthFirst(int x, int y, out IEnumerator<Vector2Int> matchedTiles)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, int> visited = new Dictionary<Vector2Int, int>();

        queue.Enqueue(new Vector2Int(x, y));
        visited.Add(new Vector2Int(x, y), 0);

        while (queue.Count > 0) {
            Vector2Int item = queue.Dequeue();

            foreach (Tile neighbourTile in GetMarkedNeighbours(item.x, item.y)) {

                Vector2Int neighbourIndex = new Vector2Int(neighbourTile.boardIndexX, neighbourTile.boardIndexY);

                int visitNumber = 0;
                if (!visited.TryGetValue(neighbourIndex, out visitNumber)) {

                    int limit = visited[item] + 1;

                    queue.Enqueue(neighbourIndex);
                    visited.Add(neighbourIndex, limit);

                    if (visited.Count == matchAcceptLimit ) {
                        matchedTiles = visited.Keys.GetEnumerator();
                        return true;
                    }
                }
                
            }
        }
        matchedTiles = visited.Keys.GetEnumerator();
        return false;
    }

    IEnumerable GetNeighbourIndexes(int x, int y)
    {
        yield return new Vector2Int(x, y + 1);
        yield return new Vector2Int(x, y - 1);
        yield return new Vector2Int(x + 1, y);
        yield return new Vector2Int(x - 1, y);
    }

    IEnumerable GetMarkedNeighbours(int x, int y)
    {
        foreach (Vector2Int nei in GetNeighbourIndexes(x, y)) {
            if (nei.x >= 0 && nei.x < boardSize) {
                if (nei.y >= 0 && nei.y < boardSize) {
                    if (board[nei.x, nei.y].marked) {
                        yield return board[nei.x, nei.y];
                    }
                }
            }
        }

    }

    IEnumerable GetNeighbours(int x, int y)
    {
        foreach (Vector2Int nei in GetNeighbourIndexes(x, y)) {
            if (nei.x >= 0 && nei.x < boardSize) {
                if (nei.y >= 0 && nei.y < boardSize) {
                    yield return board[nei.x, nei.y];
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawSphere(BoardLeftBottom, 0.2f);

        HelperFs.GizmosDrawRect(new Rect(BoardLeftBottom, CameraWidth * 2 * Vector2.one));


        Gizmos.color = Color.grey;

        CalculateTileSize();

        float posy = BoardLeftBottom.y + tileSize * 0.5f;

        for (int y = 0; y < boardSize; y++) {
            float posx = BoardLeftBottom.x + tileSize * 0.5f;

            for (int x = 0; x < boardSize; x++) {

                Gizmos.DrawSphere(new Vector3(posx, posy), 0.2f);

                posx += tileSize + spacing;
            }
            posy += tileSize + spacing;
        }


    }
}
