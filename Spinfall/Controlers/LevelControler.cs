using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControler : UnitySingleton<LevelControler> {


    #region Editor properties

    [SerializeField] private bool infiniteLevel = true;

    [SerializeField] private List<Dictionary<int, Floor.SegmentType>> floorDefinitionsForInfiniteLevels = new List<Dictionary<int, Floor.SegmentType>>();

    #endregion

    #region OnUnityEvents

    private void Start()
    {
        //Dummy data for testing
        floorDefinitionsForInfiniteLevels.Add(
            new Dictionary<int, Floor.SegmentType>
            {
                { 0,0 },
                { 1,0 },
                { 2,0 },
                { 3,0 },
                { 4,0 },
                //{ 5,0 },
                //{ 6,0 },
                //{ 7,0 },
                { 8,0 },
                { 9,0 },
                { 10,0 },
                { 11,0 },
                { 12,0 },
                { 13,0 },
                { 14,0 },
                { 15,0 },
                //{ 16,0 },
                //{ 17,0 },
                //{ 18,0 },
                //{ 19,0 },
                { 20,0 },
                { 21,0 },
                { 22,0 },
                { 23,0 }
            });
        floorDefinitionsForInfiniteLevels.Add(
            new Dictionary<int, Floor.SegmentType>
            {
                { 0,0 },
                { 1,0 },
                { 2,0 },
                { 3,0 },
                { 4,0 },
                { 5,0 },
                { 6,Floor.SegmentType.BAD },
                //{ 7,0 },
                //{ 8,0 },
                //{ 9,0 },
                //{ 10,0 },
                //{ 11,0 },
                //{ 12,0 },
                //{ 13,0 },
                //{ 14,0 },
                //{ 15,0 },
                //{ 16,0 },
                { 17, Floor.SegmentType.BAD},
                { 18,0 },
                { 19,0 },
                { 20,0 },
                { 21,0 },
                { 22,0 },
                { 23,0 }
            });
        floorDefinitionsForInfiniteLevels.Add(
            new Dictionary<int, Floor.SegmentType>
            {
                { 0,0 },
                { 1,0 },
                { 2,0 },
                { 3,0 },
                { 4,0 },
                { 5,0 },
                { 6,0 },
                //{ 7,0 },
                //{ 8,0 },
                //{ 9,0 },
                { 10,0 },
                { 11,0 },
                { 12,0 },
                { 13,0 },
                { 14,0 },
                { 15,0 },
                { 16,0 },
                { 17,0 },
                { 18,0 },
                //{ 19,0 },
                //{ 20,0 },
                //{ 21,0 },
                //{ 22,0 },
                //{ 23,0 }
            });

    }

    #endregion

    public Dictionary<int, Floor.SegmentType> GetNextFloor()
    {
        if (infiniteLevel)
            return GetRandomFloor();

        return null;
    }

    public Dictionary<int, Floor.SegmentType> GetRandomFloor()
    {
        return floorDefinitionsForInfiniteLevels[Random.Range(0,floorDefinitionsForInfiniteLevels.Count)];
    }


}
