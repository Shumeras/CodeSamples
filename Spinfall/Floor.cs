using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    public enum SegmentType
    {
        NORMAL,
        BAD,
        ENDLEVEL
    }

    #region Editor properties
    [Header("Segment prefabs")]
    [SerializeField] private GameObject normalSegmentPrefab;
    [SerializeField] private GameObject badSegmentPrefab;
    [SerializeField] private GameObject endLevelSegmentPrefab;

    [Header("Segment colors")]
    [SerializeField] private bool useCustomColors = false;
    [SerializeField] private Color normalSegmentColor = Color.black; 
    [SerializeField] private Color badSegmentColor = Color.red;
    [SerializeField] private Color endLevelSegmentColor = Color.blue;

    [Header("Removal parameters")]
    [SerializeField, Range(0f, 5f)] private float destroyTime = 1f;
    [SerializeField, Range(0f, 10f)] private float explosionForce = 1f;
    [SerializeField, Range(0f, 5f)] private float explosionUpwardForce = 1f;

    #endregion

    #region Private properties

    public Dictionary<int, GameObject> segments = new Dictionary<int, GameObject>();
    [SerializeField] private GameObject cylinderGameObject;
    [SerializeField] private GameObject triggerAreaGameObject;

    #endregion

    public void CreateSegmentsFromList(IDictionary<int, SegmentType> segmentCreationList)
    {
        foreach(var s in segmentCreationList)
        { 
            segments.Add(s.Key, CreateSegment(s.Key, s.Value));
        }
    }

    private GameObject CreateSegment(int segmentLocation, SegmentType type)
    {
        GameObject selectedPrefab;
        Color color;
        switch (type)
        {
            
            case (SegmentType.BAD):
                selectedPrefab = badSegmentPrefab;
                color = badSegmentColor;
                break;
            case (SegmentType.ENDLEVEL):
                selectedPrefab = endLevelSegmentPrefab;
                color = endLevelSegmentColor;
                break;
            case (SegmentType.NORMAL):
            default:
                selectedPrefab = normalSegmentPrefab;
                color = normalSegmentColor;
                break;
        }

        GameObject seg = Instantiate(selectedPrefab, this.transform);
        seg.transform.Rotate(Vector3.up, 15*segmentLocation);
      //  Debug.Log(seg.ToString());
        return seg;
    }

    public void Explode()
    {
        Rigidbody segmentRigidBody;

        foreach(var segment in segments)
        {
            segmentRigidBody = segment.Value.GetComponent<Rigidbody>();
            segmentRigidBody.useGravity = true;
            segmentRigidBody.isKinematic = false;
            segmentRigidBody.AddExplosionForce(explosionForce, this.transform.position, 10, explosionUpwardForce, ForceMode.Impulse);
            segment.Value.GetComponent<Collider>().enabled = false;
        }

        DestroyFloor(destroyTime);
    }

    public void DestroyFloor(float afterTime = 0)
    {
        foreach (var segment in segments)
        {
            Destroy(segment.Value, afterTime);
        }
        segments.Clear();
        Destroy(cylinderGameObject, afterTime);
        Destroy(this.gameObject, afterTime);

        Debug.Log(triggerAreaGameObject.ToString());

        DestroyFloorTrigger();
        
    }

    public void DestroyFloorTrigger()
    {
        if (triggerAreaGameObject != null)
        {
            Destroy(triggerAreaGameObject);
        }
    }

}
