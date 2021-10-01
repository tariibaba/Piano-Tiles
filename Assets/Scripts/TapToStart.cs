using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TapToStart : MonoBehaviour
{
    CanvasElementVisibility visibility;
    void Start()
    {
        visibility = GetComponent<CanvasElementVisibility>();
        GameController.Instance.GameStarted.Where((value) => value).Subscribe((value) =>
        {
            visibility.Visible = false;
        }).AddTo(this);
    }
}
