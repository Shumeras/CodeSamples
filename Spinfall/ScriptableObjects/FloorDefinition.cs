using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FloorDefinition", menuName ="Floor Definition",order = 1)]
public class FloorDefinition : ScriptableObject {

    [SerializeField] public Dictionary<int, Floor.SegmentType> segments;

}
