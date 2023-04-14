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

    private Vector3 _direction, _worldDirection;
    private MouseTarget target;
    private float angle;
    private float anglePerSecond = 125f;

    private float scale = 1f;

    void Awake()
    {
        target = GetComponentInChildren<MouseTarget>();
    }

    #region ------------------ faces ------------------
    // TODO : consider moving this into an abstract edge class.

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

    public void SetFaces(Face face1, Face face2)
    {
        face1.edges.Add(this);
        face2.edges.Add(this);
    }

    /// <summary>
    /// Returns the face different than the face in the parameter. 
    /// If the axis is selected, consider this edge as disconnected, so always return null.
    /// If the given face is not a face of this axis, then return the first face.
    /// If the other face is not active, then also return null.
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    public Face GetOtherFace(Face face)
    {
        if (IsSelected())
            return null;
        Face otherFace = (face1 != face) ? face1 : face2;
        if (otherFace == null)
            return null;
        return otherFace.gameObject.activeSelf ? otherFace : null;
    }

    public (int, int) AsTuple(bool increasingOrder = true)
    {
        int n1 = GetFace1().ID;
        int n2 = GetFace2().ID;
        if (increasingOrder && n2<n1)
            return (n2, n1);
        return (n1, n2);
    }

    #endregion

    #region ------------------ rotating process ------------------

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

    /// <summary>
    /// Computes the angle IN DEGREES(!!!) from face1 to face2.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception">In case the axis doesn't connect two faces</exception>
    public float GetAngle()
    {
        if (face1 == null || face2 == null)
            throw new Exception("Edge doesn't have two faces to compute the angles between them");

        Vector3 v = face1.Normal();
        Vector3 u = face2.Normal();

        worldDirection = Vector3.Cross(v, u);
        if (worldDirection.sqrMagnitude < 0.001f) 
            // In case the vectors are almost the same, just assume that the angle is zero.
            return 0;
        direction = transform.InverseTransformDirection(worldDirection);
        float angleBetweenNormals = Vector3.SignedAngle(v, u, worldDirection);
        return 180 - angleBetweenNormals;
    }

    /// <summary>
    /// Set angle in DEGREES. Rotate the non fixed face
    /// </summary>
    public void SetAngle(Face fixedFace, float angle)
    {
        float rotationAngle = GetAngle();
        if (fixedFace != face1)
            rotationAngle *= -1;
        transform.Rotate(direction, rotationAngle-angle, Space.Self);
    }

    public void StartRotating(Face fixedFace)
    {
        float rotationAngle = GetAngle();
        this.angle = (fixedFace == face1) ? -rotationAngle : rotationAngle;  
    }

    #endregion

    public bool IsSelected()
    {
        return target.IsSelected();
    }

    internal void Disconnect()
    {
        face1.edges.Remove(this);
        face2.edges.Remove(this);
    }


    public void SetVisual(Vector3 from, Vector3 to)
    {
        transform.position = Vector3.Lerp(from, to, 0.5f);
        Vector3 dir = from - to;
        _worldDirection = -dir.normalized;
        MathTools.RotateToMatch(transform, transform.up, dir);
        scale = dir.magnitude / 2;

        visual.transform.localScale = new Vector3(0.5f, dir.magnitude / 2, 0.5f);
    }

    public void SetMaterial(Material material)
    {
        visual.GetComponent<Renderer>().material = material;
    }
}
