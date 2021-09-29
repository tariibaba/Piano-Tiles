using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNotesTrigger : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameController.Instance.lastSpawnedNote.gameObject)
        {
            GameController.Instance.SpawnNotes();
        }
    }
}
