using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour
{

    [SerializeField] private Face face1;
    [SerializeField] private Face face2;
    [SerializeField] private Transform visual;
    private Vector3 direction, worldDirection;
    private MouseTarget target;
    private float angle;
    private float anglePerSecond = 125f;

    private float scale = 1f;

    void Start()
    {
        target = GetComponentInChildren<MouseTarget>();
    }

    public void SetFace1(Face face)
    {
        if (face == face1)
            return;
        if (face1 != null)
            face1.edges.Remove(this);
        if (face != null)
            face.edges.Add(this);
        face1 = face;
    }

    public Face GetFace1()
    {
        return face1;
    }

    public void SetFace2(Face face)
    {
        if (face == face2)
            return;
        if (face2 != null)
            face2.edges.Remove(this);
        if (face != null)
            face.edges.Add(this);
        face2 = face;
    }

    public Face GetFace2()
    {
        return face2;
    }

    public void SetVisual(Vector3 from, Vector3 to)
    {
        transform.position = Vector3.Lerp(from, to, 0.5f);
        Vector3 dir = from - to;
        MathTools.RotateToMatch(transform, transform.up, dir);
        scale = dir.magnitude / 2;

        visual.transform.localScale = new Vector3(0.5f, dir.magnitude / 2, 0.5f);
    }

    public bool IsSelected()
    {
        return target.IsSelected();
    }

    public Face GetOtherFace(Face face)
    {
        if (IsSelected())
            return null;
        Face otherFace = (face1 != face) ? face1 : face2;
        if (otherFace == null)
            return null;
        return otherFace.gameObject.activeSelf ? otherFace : null;
    }

    public void Update()
    {
        if (direction == null || angle == 0)
            return;
        //Debug.Log(angle);
        float dAngle = Mathf.Min(Mathf.Abs(angle), anglePerSecond * Time.deltaTime);
        if (angle > 0)
        {
            angle -= dAngle;
            transform.Rotate(direction, dAngle, Space.Self);
        }
        if (angle < 0)
        {
            angle += dAngle;
            transform.Rotate(direction, -dAngle, Space.Self);
        }
    }

    public void StartRotating(Face fixedFace)
    {
        if (face1 == null || face2 == null)
            return;
        Face otherFace = GetOtherFace(fixedFace);
        Vector3 v = fixedFace.Normal();
        Vector3 u = otherFace.Normal();
        worldDirection = Vector3.Cross(face1.Normal(), face2.Normal());
        if (worldDirection.sqrMagnitude < 0.005f)
            return;
        direction = transform.InverseTransformDirection(worldDirection);
        angle = -Vector3.SignedAngle(v, u, worldDirection);
        
    }

    internal void Disconnect()
    {
        face1.edges.Remove(this);
        face2.edges.Remove(this);
    }

    public void SetFaces(Face face1, Face face2)
    {
        face1.edges.Add(this);
        face2.edges.Add(this);
    }

    public void SetMaterial(Material material)
    {
        visual.GetComponent<Renderer>().material = material;
    }
}
