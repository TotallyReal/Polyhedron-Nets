using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsUI : MonoBehaviour
{
    [SerializeField] private Button unfoldButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button compareButton;
    [SerializeField] private VisualPolyhedronFactory polyhedronFactory;

    // Start is called before the first frame update
    void Start()
    {
        unfoldButton.onClick.AddListener(() => { polyhedronFactory.CreateTransformGraph(); });
        restartButton.onClick.AddListener(() => { polyhedronFactory.CreatePolyhedron(); });
        compareButton.onClick.AddListener(() => { polyhedronFactory.CompareUnfolding(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
