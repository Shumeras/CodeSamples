using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControler : UnitySingleton<UIControler> {

    #region Editor properties

    [SerializeField] private GameObject gameStartedLayer;
    [SerializeField] private GameObject gameEndedLayer;
    [SerializeField] private GameObject levelEndedLayer;
    [SerializeField] private GameObject pausedLayer;
    [SerializeField] private GameObject playingLayer;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    #endregion

    #region Private properties

    public GameObject currentlyActiveLayer { get; private set; }

    #endregion

    #region OnUnityEvents

    void Start()
    {
        EventManager.LevelStertedEvent += OnLevelStarted;
        EventManager.LevelEndedEvent += OnLevelEnded;
        EventManager.GamePausedEvent += OnGamePaused;
        EventManager.GameResumedEvent += OnGameResumed;
        EventManager.GameEndedEvent += OnGameEnded;
    }

    #endregion

    #region OnCustomEvents

    void OnLevelStarted()
    {
        gameEndedLayer.SetActive(false);
        levelEndedLayer.SetActive(false);

        currentlyActiveLayer = playingLayer;
        playingLayer.SetActive(true);
    }

    void OnLevelEnded()
    {
        levelEndedLayer.SetActive(true);
    }

    void OnGameEnded()
    {
        gameEndedLayer.SetActive(true);
    }

    void OnGamePaused()
    {
        currentlyActiveLayer = pausedLayer;
        pausedLayer.SetActive(true);
    }

    void OnGameResumed()
    {
        currentlyActiveLayer = playingLayer;
        pausedLayer.SetActive(false);
    }

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetHighScore(int score)
    {
        highScoreText.text = score.ToString();
    }

    #endregion

}
