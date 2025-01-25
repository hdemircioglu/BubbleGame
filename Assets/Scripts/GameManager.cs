using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject FailScreen;
    [SerializeField] private float screenDelay = 1.5f;
    [SerializeField] private int maxBubbleCount;
    [SerializeField] Transform bubbleSpawnPoint;
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [Header("UI")]
    [SerializeField] TextMeshProUGUI bubleCounterText;
    [SerializeField] TextMeshProUGUI timerText;

    private int savedBubbleCount = 0;
    private FanController fan;
    private float gameTime;

    public GameState currentState;

    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        fan = FindAnyObjectByType<FanController>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object across scenes
    }


    private void Update()
    {
        if (currentState == GameState.GameOn)
            gameTime += Time.deltaTime;

        timerText.text = FormatTime(gameTime);

        if (Input.GetMouseButton(0))
        {
            if (currentState == GameState.GameStart)
            {
                currentState = GameState.GameOn;
                SpawnBubble();
            }
        }
    }

    public void WinBubble()
    {
        savedBubbleCount += 1;
        string bubbletxt = savedBubbleCount + "/" + maxBubbleCount;
        bubleCounterText.text = bubbletxt;
        Debug.Log(bubbletxt);
        if (savedBubbleCount >= maxBubbleCount)
        {
            ChangeState(GameState.Win);
        }
        if (currentState == GameState.GameOn) StartCoroutine(DelayedSpawnBubble());
    }

    public void FailBubble()
    {
        Debug.Log("bubble faild");
        StartCoroutine(DelayedSpawnBubble());

    }


    private IEnumerator DelayedSpawnBubble()
    {
        yield return new WaitForSeconds(0.7f);
        SpawnBubble();
    }
    private void SpawnBubble()
    {
        var bubble = Instantiate(bubblePrefab, bubbleSpawnPoint.position, Quaternion.identity);
        fan.SetTarget(bubble);
        virtualCamera.Follow = bubble.transform;
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.GameStart:
                Debug.Log("Game Starting...");
                break;

            case GameState.GameOn:
                Debug.Log("Game On!");
                Time.timeScale = 1;
                break;

            case GameState.GamePaused:
                Debug.Log("Game Paused.");
                Time.timeScale = 0;
                break;

            case GameState.Win:
                Debug.Log("You Win!");
                StartCoroutine(ActivateObjectWithDelay(WinScreen, screenDelay));
                break;

            case GameState.Fail:
                Debug.Log("You Fail!");
                StartCoroutine(ActivateObjectWithDelay(FailScreen, screenDelay));
                break;
        }
    }

    private IEnumerator ActivateObjectWithDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            obj.SetActive(true);
        }
    }

    private string FormatTime(float seconds)
    {
        int minutes = Mathf.FloorToInt(seconds / 60);
        int remainingSeconds = Mathf.FloorToInt(seconds % 60);
        return string.Format("{0}:{1:D2}", minutes, remainingSeconds);
    }
}



public enum GameState
{
    GameStart,
    GameOn,
    GamePaused,
    Win,
    Fail
}
