using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private float screenDelay = 1.5f;
    [SerializeField] private int maxBubbleCount;
    [SerializeField] Transform bubbleSpawnPoint;
    [SerializeField] GameObject bubblePrefab;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [Header("UI")]
    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject FailScreen;
    [SerializeField] TextMeshProUGUI bubleCounterText;
    [SerializeField] TextMeshProUGUI timerText;
    [Header("Win UI")]
    [SerializeField] TextMeshProUGUI finalText;

    private int savedBubbleCount = 0;
    private FanController fan;
    private float gameTime;
    private float finalTime;

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

        bubleCounterText.text = savedBubbleCount + "/" + maxBubbleCount;
        //DontDestroyOnLoad(gameObject); // Keep this object across scenes
    }


    private void Update()
    {
        if (currentState == GameState.GameOn)
            gameTime += Time.deltaTime;

        timerText.text = FormatTime(gameTime);

        if (Input.GetMouseButton(0))
        {
            //if (currentState == GameState.GameStart)
            //{
            //    currentState = GameState.GameOn;
            //    SpawnBubble();
            //}
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
            finalTime = 0f + gameTime;
            ChangeState(GameState.Win);
            finalText.text = FormatTime(finalTime);

        }
        if (currentState == GameState.GameOn) StartCoroutine(DelayedSpawnBubble());
    }

    public void FailBubble()
    {
        Debug.Log("bubble faild");
        StartCoroutine(DelayedSpawnBubble());

    }

    public void StartGame()
    {
        currentState = GameState.GameOn;
        SpawnBubble();
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
                Time.timeScale = 1;
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
        Time.timeScale = 0;
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
