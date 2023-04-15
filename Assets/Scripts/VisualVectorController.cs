using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualVectorController : MonoBehaviour
{
    [SerializeField] private Vector3 from = Vector3.zero;
    [SerializeField] private Vector3 to = Vector3.up;

    private void OnValidate()
    {
        SetVisual(from, to);
    }

    public void SetVisualFrom(Vector3 fromPosition)
    {
        SetVisual(fromPosition, this.to);
    }

    public void SetVisualTo(Vector3 toPosition)
    {
        SetVisual(this.from, toPosition);
    }

    public void SetVisual(Vector3 from, Vector3 to)
    {
        this.from = from;
        this.to = to;
        transform.position = Vector3.Lerp(from, to, 0.5f);
        Vector3 dir = from - to;
        MathTools.RotateToMatch(transform, transform.up, dir);

        transform.localScale = new Vector3(0.5f, dir.magnitude / 2, 0.5f);
    }
}
