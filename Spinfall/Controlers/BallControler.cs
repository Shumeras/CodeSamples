using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControler : UnitySingleton<BallControler> {

    #region Editor properties

    [SerializeField] private Color ballColor;
    [SerializeField] private Color splashColor;

    [SerializeField] private int activateFloorSmashAfterFloors = 5;

    #endregion

    #region Private properties

    private Vector3 ballStartPosition;
    private TrailRenderer ballTrail;
    private Animator ballAnimator;
    private ParticleSystem ballParticleSystem;
    private Renderer ballRenderer;

    public int floorsFalen { get; private set; }

    public bool isBreakingThroughFloor = false;

    #endregion

    #region OnUnityEvents

    private void Start()
    {
        EventManager.LevelStertedEvent += OnLevelStarted;
        EventManager.LevelEndedEvent += OnLevelEnded;
        EventManager.GameEndedEvent += OnGameEnded;
        EventManager.GameResumedEvent += OnGameResumed;
        EventManager.GamePausedEvent += OnGamePaused;
        EventManager.BallPassedFloorEvent += OnBallPassedFloor;

        ballStartPosition = this.transform.position;
        ballTrail = this.GetComponent<TrailRenderer>();
        ballAnimator = this.GetComponent<Animator>();
        ballParticleSystem = this.GetComponent<ParticleSystem>();
        ballRenderer = this.GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        EventManager.RaiseBallCollidedEvent(collision);

        if (!ballAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bounce"))
        {
            ballAnimator.Play("Bounce");
            ballParticleSystem.Stop();
        }

        if (floorsFalen >= activateFloorSmashAfterFloors)
        {
            FloorControler.instance.BallSmashedThroughFloor();
            Destroy(collision.gameObject.transform.parent.GetComponentInChildren<FloorPassedScript>().gameObject);
            floorsFalen = 0;
            return;
        }
        else
        {
            floorsFalen = 0;
        }

        if (collision.gameObject.tag == "BadSegment")
        {
            EventManager.RaiseGameEndedEvent();
        }
        else if (collision.gameObject.tag == "EndLevelSegment")
        {
            EventManager.RaiseLevelEndedEvent();
        }
        else
        {

            //ballBody.velocity = new Vector3(ballBody.velocity.x, bounceForce, ballBody.velocity.z);
        }
    }

    #endregion

    #region OnCustomEvent Handlers

    void OnLevelStarted()
    {
        this.transform.position = ballStartPosition;
        ballRenderer.material.color = ballColor;
        ballTrail.startColor = ballColor;
        ballAnimator.speed = 1;
        floorsFalen = 0;
    }

    void OnGameEnded()
    {
        ballAnimator.speed = 0;
    }

    void OnBallPassedFloor()
    {
        if(!ballParticleSystem.isPlaying)
            ballParticleSystem.Play();

        floorsFalen++;

    }

    void OnLevelEnded()
    {
        ballAnimator.speed = 0;
    }

    void OnGamePaused()
    {
        ballAnimator.speed = 0;
    }

    void OnGameResumed()
    {
        ballAnimator.speed = 1;
    }

    #endregion
}
