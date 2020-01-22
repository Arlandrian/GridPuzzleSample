using UnityEngine;

public class Tile
{
    public GameObject go;
    public int boardIndexX;
    public int boardIndexY;
    public bool marked = false;

    public Tile(GameObject go, int boardIndexX, int boardIndexY)
    {
        this.go = go;
        this.boardIndexX = boardIndexX;
        this.boardIndexY = boardIndexY;
    }
}