using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorControler : UnitySingleton<FloorControler> {

    #region Editor properties

    [SerializeField] private GameObject floorPrefab;

    [Header("Floor options")]
    [SerializeField, Range(1f, 10f)] private float floorHeight = 2f;
    [SerializeField, Range(0.01f, 0.5f)] private float floorSwitchTime;
    [SerializeField] private int loadedFloors = 6;
    [SerializeField] private bool randomiseRotation = false;

    #endregion

    #region Private properties

    private Queue<Floor> floors = new Queue<Floor>();
    private int floorsToMove;
    private float floorHeightMoved = 0;

    #endregion

    #region OnUnityEvents 

    void Start()
    {
        EventManager.BallPassedFloorEvent += OnBallPassedFloor;
        EventManager.LevelStertedEvent += OnLevelStarted;
    }

    void FixedUpdate()
    {
        if(floorsToMove > 0)
        {  
            float heightToMove = (1/floorSwitchTime) * Time.fixedDeltaTime;

            if (heightToMove + floorHeightMoved > floorHeight)
            {
                heightToMove = floorHeight - floorHeightMoved;
                floorsToMove--;
                RemoveTopFloor();
                LoadNextFloor(loadedFloors);
                floorHeightMoved = 0;
            }
            else
            {
                floorHeightMoved += heightToMove;
            }
            
            foreach(var floor in floors)
            {
                floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y + heightToMove, floor.transform.position.z) ;
            }
            
        }
    }

    #endregion

    #region OnCustomEvents

    void OnLevelStarted()
    {
        while(floors.Count > 0)
        {
            floors.Dequeue().DestroyFloor();
        }

        for(int buildingFloorNumber = 0; buildingFloorNumber <= loadedFloors; buildingFloorNumber++)
        {
         //   Debug.Log(buildingFloorNumber);
            LoadNextFloor(buildingFloorNumber);
        }

    }

    void OnBallPassedFloor()
    {
        floorsToMove++;
    }

    #endregion

    Floor LoadNextFloor(int position)
    {
        GameObject go = Instantiate(floorPrefab, new Vector3(this.transform.position.x, -floorHeight*position, this.transform.position.z), floorPrefab.transform.rotation, this.transform);
        if (randomiseRotation)
        {
            go.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, Random.Range(0, 360), transform.rotation.eulerAngles.z);
        }
        Floor tempFloorReference = go.GetComponent<Floor>();
        tempFloorReference.CreateSegmentsFromList(LevelControler.instance.GetNextFloor());
        floors.Enqueue(tempFloorReference);
        return tempFloorReference;
    }

    void RemoveTopFloor()
    {
        floors.Dequeue().Explode();
    }

    public void BallSmashedThroughFloor()
    {
        floorsToMove++;
    }

}
