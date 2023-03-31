using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundArrow : MonoBehaviour
{

    public Arc arcVisual;
    public Transform arrowHead;

    [Range(0, 2 * Mathf.PI)]
    [SerializeField] private float angle = Mathf.PI / 3f;
    [Range(0, 20)]
    [SerializeField] private float outerRadius = 5f;
    [Range(0, 20)]
    [SerializeField] private float innerRadius = 1f;


    private void OnValidate()
    {
        arcVisual.SetParameters(angle, outerRadius, innerRadius);
        arrowHead.localPosition = new Vector3(outerRadius, arrowHead.localPosition.y, arrowHead.localPosition.z);
    }
}
