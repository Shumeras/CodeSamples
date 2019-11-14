using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    #region Events

    public static event Action GameStartedEvent = OnGameStarted;
    public static event Action GameEndedEvent = OnGameEnded;

    public static event Action LevelStertedEvent = OnLevelStarted;
    public static event Action LevelEndedEvent = OnLevelEnded;

    public static event Action BallPassedFloorEvent = OnBallPassedFloor;
    public static event Action<Collision> BallCollidedEvent = OnBallCollided;

    public static event Action GamePausedEvent = OnGamePaused;
    public static event Action GameResumedEvent = OnGameResumed;

    #endregion

    #region Event Raisers

    public static void RaiseGameStartedEvent()
    {
        GameStartedEvent();
    }

    public static void RaiseGameEndedEvent()
    {
        GameEndedEvent();
    }

    public static void RaiseBallPassedFloorEvent()
    {
        BallPassedFloorEvent();
    }

    public static void RaiseBallCollidedEvent(Collision collision)
    {
        BallCollidedEvent(collision);
    }

    public static void RaiseLevelStartedEvent()
    {
        LevelStertedEvent();
    }

    public static void RaiseLevelEndedEvent()
    {
        LevelEndedEvent();
    }

    public static void RaiseGamePausedEvent()
    {
        GamePausedEvent();
    }

    public static void RaiseGameResumedEvent()
    {
        GameResumedEvent();
    }
    #endregion

    #region OnEvent Handlers

    static void OnGameStarted()
    {
        Debug.Log("Game Started");
    }

    static void OnGameEnded()
    {
        Debug.Log("Game Ended");
    }

    static void OnBallPassedFloor()
    {
        Debug.Log("Ball passed floor");
    }

    static void OnBallCollided(Collision collision)
    {
       // Debug.Log("Ball collided at: " + collision.contacts[0].point.ToString());
    }

    static void OnLevelStarted()
    {
        Debug.Log("Level Started");
    }

    static void OnLevelEnded()
    {
        Debug.Log("Level Ended");
    }

    static void OnGamePaused()
    {
        Debug.Log("Game Paused");
    }

    static void OnGameResumed()
    {
        Debug.Log("Game resumed");
    }
    #endregion
}
