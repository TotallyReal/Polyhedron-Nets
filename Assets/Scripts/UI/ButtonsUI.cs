using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUI : MonoBehaviour
{

    [SerializeField] private FaceGraph faceGraph;
    [SerializeField] private Button unfoldButton;
    [SerializeField] private Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
        unfoldButton.onClick.AddListener(() => { faceGraph.CreateTransformGraph(); });
        restartButton.onClick.AddListener(() => { VisualPolyhedronFactory.Instance.CreatePolyhedron(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
