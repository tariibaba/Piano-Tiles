using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameOverScreen : MonoBehaviour
{
    public Text score;
    private CanvasElementVisibility visibility;
    public CanvasElementVisibility winnerPraise;

    void Start()
    {
        visibility = GetComponent<CanvasElementVisibility>();
        GameController.Instance.ShowGameOverScreen.Where((value) => value).Subscribe((value) =>
        {
            visibility.Visible = true;
            score.text = GameController.Instance.Score.Value.ToString();
            winnerPraise.Visible = GameController.Instance.PlayerWon;
        }).AddTo(this);
    }
}
