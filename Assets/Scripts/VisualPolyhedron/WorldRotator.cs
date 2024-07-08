using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;


/// <summary>
/// Whenever the rotation object is pressed, look for the closest rotation axis, and rotate around it.
/// </summary>
public class WorldRotator : MonoBehaviour
{

    // TODO: Change it to rotatable object
    [SerializeField] private VisualPolyhedronFactory factory;
    [SerializeField] private Transform rotationObject;

    [SerializeField] private float rotationTimeSec = 1f;
    private RotationOp waitingRotation = null;
    private bool isRotating;
    private NetsPlayerInput input;

    // TODO: Finally..........
    [System.Serializable]
    public class RotationOp
    {
        public RotationOp(Vector3 axis, float angle)
        {
            normalizedAxis = axis.normalized;
            this.angle = angle;
        }

        // TODO: I hate serializing objects in unity... This is a nightmare
        [SerializeField] public Vector3 normalizedAxis;
        [SerializeField] public float angle;

        public override string ToString() => $"{normalizedAxis}[{angle}]";
    }

    // TODO: I hate everything about this
    [SerializeField] private List<RotationOp> axes;

    public static List<RotationOp> GetRotationsFromPolyhedron(AbstractGroupPolyhedron groupPoly)
    {
        List<RotationOp> axes = new List<RotationOp>();

        float verAngle = 360 / groupPoly.vertexDegree;
        foreach (var vertex in groupPoly.GetVertices())
        {
            axes.Add(new RotationOp(vertex, verAngle));
        }

        foreach (var face in groupPoly.GetFaces())
        {
            Vector3 center = Vector3.zero;
            // TODO: If vertex count is always the same, consider fixing this.
            int verCount = 0;
            foreach (Vector3 v in groupPoly.GetVerticesAt(face))
            {
                verCount += 1;
                center += v;
            }
            center /= 4;
            axes.Add(new RotationOp(center, 360 / verCount));
        }

        return axes;
    }

    void Awake()
    {
        input = new NetsPlayerInput();
        input.Player.Enable();

        SetRotationOp(GetRotationsFromPolyhedron(AbstractGroupPolyhedron.Cube(1)));
    }

    private void Start()
    {
        if (factory != null) {
            SetRotationOp(factory.GetRotations());
            factory.OnPolyhedroneGenerated += OnPolyhedroneGenerated;
        }
    }

    private void OnPolyhedroneGenerated(object sender, AbstractGroupPolyhedron absGroupPoly)
    {
        SetRotationOp(GetRotationsFromPolyhedron(absGroupPoly));
    }

    public void SetRotationOp(List<RotationOp> rotations)
    {
        if (rotations != null)
        {
            axes = rotations;
        }

    }


    #region -------------------- Action Event Rotation --------------------

    private void OnEnable()
    {
        RaycastSelector.Instance.OnObjectPressedPlus += Rotate;
    }

    private void OnDisable()
    {
        RaycastSelector.Instance.OnObjectPressedPlus -= Rotate;
    }

    private RotationOp ClosestRotation(Vector3 dir) {
        dir.Normalize();

        float dist = 1000; // > 2
        RotationOp closestRotOp = new RotationOp(Vector3.up, 0);
        foreach (RotationOp currentRotOp in axes)
        {
            float d = Vector3.Distance(dir, currentRotOp.normalizedAxis);
            if (d < dist)
            {
                closestRotOp = currentRotOp;
                dist = d;
            }
        }
        return closestRotOp;
    }

    private void Rotate(object sender, (Transform, RaycastHit) e)
    {
        if (e.Item1 == rotationObject)
        {
            Vector3 dir = e.Item2.point - transform.position;
            RotateAround(ClosestRotation(dir), rotationTimeSec);
        }
    }

    #endregion


    #region -------------------- Rotation --------------------

    private void FinishedRotating()
    {
        isRotating = false;
        if (waitingRotation != null)
        {
            RotationOp nextRotation = waitingRotation;
            waitingRotation = null;
            RotateAround(nextRotation, rotationTimeSec);
        }
    }

    private void RotateAround(RotationOp rotation, float rotationTimeSec)
    {
        if (waitingRotation!=null)
        {
            return;
        }
        if (isRotating) // TODO: Consider saving the action, only if near the end of the previous action.
        {
            waitingRotation = rotation;
            return;
        }
        isRotating = true;

        Quaternion targetRotation = Quaternion.AngleAxis(rotation.angle, rotation.normalizedAxis) * transform.rotation;

        transform.DORotateQuaternion(targetRotation, rotationTimeSec)
            .SetRelative(false)
            .OnComplete(FinishedRotating);        
    }

    #endregion

}
