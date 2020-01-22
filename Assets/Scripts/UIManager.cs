using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Text Text_MatchCount = null;

    [SerializeField]
    private Button Button_Rebuild = null;

    [SerializeField]
    private InputField Input_BoardSize = null;

    [SerializeField]
    private Slider Slider_TileSpacing = null;

    private void Start()
    {
        Input_BoardSize.text = BoardManager.Instance.GetBoardSize().ToString();
    }

    private void OnEnable()
    {
        Button_Rebuild.onClick.AddListener(RebuildBoard);
        Input_BoardSize.onValueChanged.AddListener(BoardSizeChanged);
        Slider_TileSpacing.onValueChanged.AddListener(TileSpacingChanged);
    }

    private void OnDisable()
    {
        Button_Rebuild.onClick.RemoveListener(RebuildBoard);
        Input_BoardSize.onValueChanged.RemoveListener(BoardSizeChanged);
        Slider_TileSpacing.onValueChanged.RemoveListener(TileSpacingChanged);

    }

    private void RebuildBoard()
    {
        int newSize = int.Parse(Input_BoardSize.text);

        if(newSize < 1 || newSize > 50) {
            return;
        }

        BoardManager.Instance.SetBoardSize(newSize);
        BoardManager.Instance.Rebuild();
    }

    private void BoardSizeChanged(string newSize)
    {
        if (newSize.Length < 1) {
            Input_BoardSize.text = "0";
            return;
        }

        if (!char.IsDigit(newSize[newSize.Length - 1])) {
            Input_BoardSize.text = newSize.Substring(0, newSize.Length - 1);
        }
    }

    private void TileSpacingChanged(float newValue)
    {
        BoardManager.Instance.SetTileSpacing(newValue);
    }

    public void SetMatchCountText(int matchCount)
    {
        Text_MatchCount.text = "Match Count : " + matchCount;
    }
   

}
