using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WorldRotator;

public class ExampleFactoryStep : VisualPolyhedronFactoryStep
{

    [SerializeField] private VisualPolyhedronProperties polyhedronProperties;
    [Header("Polyhedron position and orientation")]
    [SerializeField] private Transform positionTransform;
    enum PolyhedronPosition
    {
        CENTERED,
        ROOT
    }
    [SerializeField] private PolyhedronPosition polyhedronPosition;
    [SerializeField] private bool rootDownward = true;

    [SerializeField] private bool worldRotator = true;

    [Header("Random Open")]
    [Tooltip("Don't use in editor mode")]
    [SerializeField] private bool randomOpen = false;
    [SerializeField] private int seed = -1;

    public override void FactoryStep(VisualPolyhedron visualPolyhedron)
    {
        if (polyhedronProperties!=null)
            visualPolyhedron.SetVisualProperties(polyhedronProperties);

        // TODO: Find a way to implement it using events that are sequential, and can be edited in the inspector
        if (rootDownward)
            OrientRootDownwards(visualPolyhedron);

        if (positionTransform != null)
            SetPosition(visualPolyhedron);

        if (worldRotator)
            AddWorldRotator(visualPolyhedron);

        if (randomOpen)
            RandomOpen(visualPolyhedron);
    }


    public void OrientRootDownwards(VisualPolyhedron visualPolyhedron)
    {
        Vector3 normalDir = visualPolyhedron.RootFace.Normal();
        Vector3 rotationAxis = Vector3.Cross(normalDir, visualPolyhedron.transform.up);
        float rotationAngle = 180 - Vector3.Angle(normalDir, visualPolyhedron.transform.up);

        visualPolyhedron.transform.RotateAround(Vector3.zero, rotationAxis, -rotationAngle);

        // TODO: Try to remember why I rotated each face and edge, instead of the whole polyhedron
        // rotate the polyhedron, so that the root face will be at the bottom.
        /*foreach (PolyhedronEdge axis in visualPolyhedron.GetEdges())
        {
            axis.transform.RotateAround(Vector3.zero, rotationAxis, -rotationAngle);
        }
        foreach (Face face in visualPolyhedron.GetFaces())
        {
            face.transform.RotateAround(Vector3.zero, rotationAxis, -rotationAngle);
        }*/
    }

    private void SetPosition(VisualPolyhedron visualPolyhedron)
    {
        // TODO: consider making the polyhedronPosition the parent of this polyhedron.
        if (polyhedronPosition == PolyhedronPosition.CENTERED)
        {
            visualPolyhedron.transform.position = positionTransform.position;
        }
        if (polyhedronPosition == PolyhedronPosition.ROOT)
        {
            FaceMesh rootFace = visualPolyhedron.RootFace;
            Vector3 rootPosition = rootFace.transform.TransformPoint(rootFace.Center);

            visualPolyhedron.transform.position +=
                (positionTransform.position - rootPosition);
        }
    }

    // TODO: move somewhere else
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

    private void AddWorldRotator(VisualPolyhedron visualPolyhedron)
    {
        AbstractPolyhedron abstractPolyhedron = visualPolyhedron.GetAbstractPolyhedron();
        if (abstractPolyhedron is AbstractGroupPolyhedron)
        {
            ClickableWorldRotator worldRotator = visualPolyhedron.gameObject.AddComponent<ClickableWorldRotator>();
            List<RotationOp> rotationOps = GetRotationsFromPolyhedron((AbstractGroupPolyhedron)abstractPolyhedron);

            Quaternion localRotation = visualPolyhedron.transform.localRotation;
            //localRotation.w *= -1;

            foreach (RotationOp rotationOp in rotationOps)
            {
                rotationOp.normalizedAxis = localRotation * rotationOp.normalizedAxis;
            }

            worldRotator.SetRotationOp(rotationOps);

            // capture mouse presses at the full polyhedron level
            Rigidbody rigidBody = visualPolyhedron.gameObject.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
            rigidBody.useGravity = false;
        }
    }

    public void RandomOpen(VisualPolyhedron visualPolyhedron)
    {
        if (seed > -1)
        {
            Random.InitState(seed);
        }
        FaceGraph faceGraph = visualPolyhedron.GetComponent<FaceGraph>();
        // TODO: change the CreateRandomGraph function so it can work in editor mode?
        faceGraph.CreateRandomGraph();
    }
}
