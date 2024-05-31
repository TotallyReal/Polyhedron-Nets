using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyhedronEdge : MonoBehaviour
{

    [SerializeField] private Face face1;
    [SerializeField] private Face face2;
    [SerializeField] private Transform visual;

    private MouseTarget target;
    private float anglePerSecond = 125f;

    private RotationAxis rotationAxis;

    void Awake()
    {
        target = GetComponentInChildren<MouseTarget>();
        rotationAxis = GetComponent<RotationAxis>();
    }

    public MouseTarget GetMouseTarget()
    {
        return target;
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

    /// <summary>
    /// Returns a tuple (i,j) representing this edge, where i, j are the IDs of the faces
    /// touching this edge in increasing order (i<j).
    /// </summary>
    /// <param name="increasingOrder"></param>
    /// <returns></returns>
    public (int, int) AsTuple(bool increasingOrder = true)
    {
        int n1 = GetFace1().ID;
        int n2 = GetFace2().ID;
        if (increasingOrder && n2 < n1)
            return (n2, n1);
        return (n1, n2);
    }

    #endregion

    #region ------------------ rotating process ------------------

    /// <summary>
    /// Set angle in DEGREES. Rotate the non fixed face
    /// </summary>
    public void SetAngle(Face fixedFace, float angle)
    {
        Face otherFace = GetOtherFace(fixedFace);
        float angleBetweenFaces = rotationAxis.GetAngle(fixedFace.GetGlobalCenter(), otherFace.GetGlobalCenter());
        rotationAxis.RotateAround(otherFace.transform, angle - angleBetweenFaces);
    }

    /// <summary>
    /// Start a rotating animation of the other face (than the given one) so it 
    /// would be eventually at 180 from the fixed face.
    /// </summary>
    /// <param name="fixedFace"></param>
    public void StartRotating(Face fixedFace)
    {
        Face otherFace = GetOtherFace(fixedFace);
        float angleBetweenFaces = rotationAxis.GetAngle(otherFace.GetGlobalCenter(), fixedFace.GetGlobalCenter());
        float angleToRotate = angleBetweenFaces - 180;
        if (angleBetweenFaces < 0)
            angleToRotate = angleBetweenFaces + 180;
        rotationAxis.AddRotationProcess(otherFace.transform, angleToRotate, anglePerSecond);
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


    public void SetVisual(Vector3 from, Vector3 to, float radius = 0.5f)
    {
        transform.position = Vector3.Lerp(from, to, 0.5f);
        Vector3 dir = from - to;
        MathTools.RotateToMatch(transform, transform.up, dir);

        visual.transform.localScale = new Vector3(radius, dir.magnitude / 2, radius);

        rotationAxis.SetAxis(from, to);
    }

    public void SetMaterial(Material material)
    {
        visual.GetComponent<Renderer>().material = material;
    }

    public void SetVisible(bool visible)
    {
        visual.gameObject.SetActive(visible);
    }
}
