using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Animator animator;

    private bool visible;
    public bool Visible
    {
        get => visible;
        set
        {
            visible = value;
            if (!visible) animator.Play("Invisible");
        }
    }

    public bool Played { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameController.Instance.GameStarted.Value && !GameController.Instance.GameOver.Value)
        {
            transform.Translate(Vector2.down * GameController.Instance.noteSpeed * Time.deltaTime);
        }
    }

    public void Play()
    {
        if (Visible)
        {
            if (!Played)
            {
                Played = true;
                animator.Play("Played");
            }
        }
        else
        {
            GameController.Instance.GameOver.Value = true;
            animator.Play("Missed");
        }
    }

    public void OutOfScreen()
    {
        if (Visible && !Played)
        {
            GameController.Instance.GameOver.Value = true;
            animator.Play("Missed");
        }
    }
}
