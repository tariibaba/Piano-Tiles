using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector2.down * GameController.Instance.noteSpeed * Time.deltaTime);
    }

    public void Played()
    {
        animator.Play("Played");
    }
}
