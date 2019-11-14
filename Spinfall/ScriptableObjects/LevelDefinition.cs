using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDefinition", menuName = "Level Definition", order = 2)]
public class LevelDefinition : ScriptableObject {

    public List<FloorDefinition> floors;

}
