using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfScreenTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            var note = collision.gameObject.GetComponent<Note>();
            note.OutOfScreen();
        }
    }
}
