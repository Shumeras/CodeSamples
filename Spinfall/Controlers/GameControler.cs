using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : UnitySingleton<GameControler> {

    #region Private properties

    public bool gameIsPaused { get; private set; }
    public bool gameEnded { get; private set; }

    public bool levelEnded { get; private set; }

    public int score { get; private set; }
    public int highScore { get; private set; }

    public int floorsFellThrough { get; private set; }

    public int totalFloorsFallen { get; private set; }
    #endregion

    #region OnUnityEvents

    private void Start()
    {
        EventManager.LevelStertedEvent += OnLevelStarted;
        EventManager.LevelEndedEvent += OnLevelEnded;
        EventManager.GameEndedEvent += OnGameEnded;
        EventManager.GamePausedEvent += OnGamePaused;
        EventManager.GameResumedEvent += OnGameResumed;
        EventManager.BallPassedFloorEvent += OnBallPassedFloor;
        EventManager.BallCollidedEvent += OnBallCollided;

        EventManager.RaiseGameStartedEvent();
        EventManager.RaiseLevelStartedEvent();
    }

    #endregion

    #region OnCustomEvents

    void OnLevelStarted()
    {
        levelEnded = false;
        gameEnded = false;
        score = 0;
        UIControler.instance.SetScore(score);
    }

    void OnLevelEnded()
    {
        levelEnded = true;
    }

    void OnGameEnded()
    {
        gameEnded = true;
        if(score > highScore)
        {
            highScore = score;
            UIControler.instance.SetHighScore(highScore);
        }
    }

    void OnGamePaused()
    {
        gameIsPaused = true;
    }

    void OnGameResumed()
    {
        gameIsPaused = false;
    }

    void OnBallPassedFloor()
    {
        floorsFellThrough++;
        score += floorsFellThrough;
        UIControler.instance.SetScore(score);
    }

    void OnBallCollided(Collision collision)
    {
        floorsFellThrough = 0;
    }

    #endregion

}
