using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyNoteTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            Destroy(collision.gameObject);
        }
    }
}
