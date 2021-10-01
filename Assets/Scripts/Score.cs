using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        GameController.Instance.Score.SubscribeToText(text).AddTo(this);
    }
}
