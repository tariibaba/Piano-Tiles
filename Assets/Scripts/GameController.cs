using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform lastSpawnedNote;
    private static float noteHeight;
    private static float noteWidth;
    public Note notePrefab;
    private Vector3 noteLocalScale;
    private float noteSpawnStartPosX;
    public float noteSpeed = 5f;
    public const int NotesToSpawn = 20;
    private int prevRandomIndex = -1;
    public static GameController Instance { get; private set; }
    public Transform noteContainer;
    public ReactiveProperty<bool> GameStarted { get; set; }
    public ReactiveProperty<bool> GameOver { get; set; }
    public ReactiveProperty<int> Score { get; set; }
    private int lastNoteId = 1;
    public int LastPlayedNoteId { get; set; } = 0;
    public AudioSource audioSource;
    private Coroutine playSongSegmentCoroutine;
    private float songSegmentLength = 0.8f;
    private bool lastNote = false;
    private bool lastSpawn = false;
    public ReactiveProperty<bool> ShowGameOverScreen { get; set; }
    public bool PlayerWon { get; set; } = false;

    private void Awake()
    {
        Instance = this;
        GameStarted = new ReactiveProperty<bool>();
        GameOver = new ReactiveProperty<bool>();
        Score = new ReactiveProperty<int>();
        ShowGameOverScreen = new ReactiveProperty<bool>();
    }

    void Start()
    {
        SetDataForNoteGeneration();
        SpawnNotes();
    }

    private void Update()
    {
        DetectNoteClicks();
        DetectStart();
    }

    private void DetectStart()
    {
        if (!GameController.Instance.GameStarted.Value && Input.GetMouseButtonDown(0))
        {
            GameController.Instance.GameStarted.Value = true;
        }
    }

    private void DetectNoteClicks()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(origin, Vector2.zero);
            if (hit)
            {
                var gameObject = hit.collider.gameObject;
                if (gameObject.CompareTag("Note"))
                {
                    var note = gameObject.GetComponent<Note>();
                    note.Play();
                }
            }
        }
    }


    private void SetDataForNoteGeneration()
    {
        var topRight = new Vector3(Screen.width, Screen.height, 0);
        var topRightWorldPoint = Camera.main.ScreenToWorldPoint(topRight);
        var bottomLeftWorldPoint = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var screenWidth = topRightWorldPoint.x - bottomLeftWorldPoint.x;
        var screenHeight = topRightWorldPoint.y - bottomLeftWorldPoint.y;
        noteHeight = screenHeight / 4;
        noteWidth = screenWidth / 4;
        var noteSpriteRenderer = notePrefab.GetComponent<SpriteRenderer>();
        noteLocalScale = new Vector3(
               noteWidth / noteSpriteRenderer.bounds.size.x * noteSpriteRenderer.transform.localScale.x,
               noteHeight / noteSpriteRenderer.bounds.size.y * noteSpriteRenderer.transform.localScale.y, 1);
        var leftmostPoint = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height / 2));
        var leftmostPointPivot = leftmostPoint.x + noteWidth / 2;
        noteSpawnStartPosX = leftmostPointPivot;
    }

    public void SpawnNotes()
    {
        if (lastSpawn) return;

        var noteSpawnStartPosY = lastSpawnedNote.position.y + noteHeight;
        Note note = null;
        var timeTillEnd = audioSource.clip.length - audioSource.time;
        int notesToSpawn = NotesToSpawn;
        if (timeTillEnd < NotesToSpawn)
        {
            notesToSpawn = Mathf.CeilToInt(timeTillEnd);
            lastSpawn = true;
        }
        for (int i = 0; i < notesToSpawn; i++)
        {
            var randomIndex = GetRandomIndex();
            for (int j = 0; j < 4; j++)
            {
                note = Instantiate(notePrefab, noteContainer.transform);
                note.transform.localScale = noteLocalScale;
                note.transform.position = new Vector2(noteSpawnStartPosX + noteWidth * j, noteSpawnStartPosY);
                note.Visible = (j == randomIndex);
                if (note.Visible)
                {
                    note.Id = lastNoteId;
                    lastNoteId++;
                }
            }
            noteSpawnStartPosY += noteHeight;
            if (i == NotesToSpawn - 1) lastSpawnedNote = note.transform;
        }
    }

    private int GetRandomIndex()
    {
        var randomIndex = Random.Range(0, 4);
        while (randomIndex == prevRandomIndex) randomIndex = Random.Range(0, 4);
        prevRandomIndex = randomIndex;
        return randomIndex;
    }

    public void PlaySomeOfSong()
    {
        if (!audioSource.isPlaying && !lastNote)
        {
            audioSource.Play();
        }
        if (audioSource.clip.length - audioSource.time <= songSegmentLength)
        {
            lastNote = true;
        }
        if (playSongSegmentCoroutine != null) StopCoroutine(playSongSegmentCoroutine);
        playSongSegmentCoroutine = StartCoroutine(PlaySomeOfSongCoroutine());
    }

    private IEnumerator PlaySomeOfSongCoroutine()
    {
        yield return new WaitForSeconds(songSegmentLength);
        audioSource.Pause();
        if (lastNote)
        {
            PlayerWon = true;
            StartCoroutine(EndGame());
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator EndGame()
    {
        GameOver.Value = true;
        yield return new WaitForSeconds(1);
        ShowGameOverScreen.Value = true;
    }
}
