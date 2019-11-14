using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionControler : UnitySingleton<InteractionControler> {

    #region Editor properties

    [SerializeField, Range(0f, 10f)] private float moveSensitivity;

    #endregion

    #region Private properties

    private Vector2 firstScreenPoint;
    private Vector2 secondScreenPoint;
    private Vector2 screenPointDelta;

    #endregion

    private void FixedUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            firstScreenPoint = Input.mousePosition;
            secondScreenPoint = Input.mousePosition;

            if (GameControler.instance.gameIsPaused)
            {
                EventManager.RaiseGameResumedEvent();
            }

            if(GameControler.instance.gameEnded)
            {
                EventManager.RaiseLevelStartedEvent();
            }

        }
        else if(Input.GetMouseButton(0))
        {
            firstScreenPoint = secondScreenPoint;
            secondScreenPoint = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            secondScreenPoint = firstScreenPoint;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameControler.instance.gameIsPaused || GameControler.instance.gameEnded)
            {
                Application.Quit();
            }
            else
            {
                EventManager.RaiseGamePausedEvent();
            }
        }

        if (!firstScreenPoint.Equals(secondScreenPoint) && !GameControler.instance.gameIsPaused)
        {
            screenPointDelta = firstScreenPoint - secondScreenPoint;
            FloorControler.instance.transform.Rotate(Vector3.up, screenPointDelta.x*moveSensitivity*Time.fixedDeltaTime);
        }
        
    }
}
