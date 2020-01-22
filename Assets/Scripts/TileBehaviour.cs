using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public int indexX { get; private set; }
    public int indexY { get; private set; }

    public bool marked { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int x,int y)
    {
        indexX = x;
        indexY = y;
    }
    
    private void OnMouseDown()
    {
        if(!marked)
            Mark();
    }

    private void OnMouseEnter()
    {
        // BoardManager.Instance.MouseOverTile = this;
        // BoardManager.Instance.ShowNeighbours();

        if (!marked) {
            GetComponent<SpriteRenderer>().color = BoardManager.Instance.TileMouseOverColor;
        }
        
    }

    private void OnMouseExit()
    {
        // BoardManager.Instance.HideNeighbours();
        // BoardManager.Instance.MouseOverTile = null;

        if (!marked) {
            GetComponent<SpriteRenderer>().color = BoardManager.Instance.TileColor;
        }
    }

    private void Mark()
    {
        marked = true;
        // Activate Mark Sprite
        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().color = BoardManager.Instance.TileColor;

        BoardManager.Instance.MarkTile(indexX, indexY);
    }

    public void Unmark()
    {
        marked = false;

        transform.GetChild(0).gameObject.SetActive(false);
    }
}
