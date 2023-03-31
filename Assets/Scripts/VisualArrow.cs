using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualArrow : MonoBehaviour
{

    [SerializeField] private Transform arrowHead;
    [SerializeField] private Transform arrowLine;

    [SerializeField] private Vector3 headPoint;
    [SerializeField] private Vector3 tailPoint;

    private float headLength = 3f;


    public void SetVisual(Vector3 from, Vector3 to)
    {
        transform.position = Vector3.Lerp(from, to, 0.5f);
        transform.position = to;
        Vector3 dir = from - to;
        Vector3 normal = dir.normalized;
        MathTools.RotateToMatch(transform, transform.up, dir);
        //scale = dir.magnitude / 2;

        arrowLine.transform.position = Vector3.Lerp(from, to + normal * headLength, 0.5f);
        arrowLine.transform.localScale = new Vector3(0.5f, (dir.magnitude - headLength )/ 2, 0.5f);
    }

    public void SetHeadPoint(Vector3 pt)
    {
        this.headPoint = pt;
        SetVisual(tailPoint, headPoint);
    }
    public void SetTailPoint(Vector3 pt)
    {
        this.tailPoint = pt;
        SetVisual(tailPoint, headPoint);
    }

    private void OnValidate()
    {
        SetVisual(tailPoint, headPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
