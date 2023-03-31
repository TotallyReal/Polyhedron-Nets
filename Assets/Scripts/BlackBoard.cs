using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class BlackBoard : MonoBehaviour
{

    [Header("Dimension")]
    [Range(0,100)]
    [SerializeField] private float width = 40f;
    [Range(0, 100)]
    [SerializeField] private float height = 25f;

    [Header("Components")]
    [SerializeField] private Transform board;
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;

    // Start is called before the first frame update
    void Start()
    {
        UpdateDimension();
    }

    private void OnValidate()
    {
        UpdateDimension();
    }

    // Update is called once per frame
    void UpdateDimension()
    {
        board.localScale  = new Vector3(width, height, 1f);
        top.localScale    = new Vector3(width + 1f, 1f, 1.5f);
        bottom.localScale = new Vector3(width + 1f, 1f, 1.5f);
        left.localScale   = new Vector3(1f, height + 1f, 1.5f);
        right.localScale  = new Vector3(1f, height + 1f, 1.5f);

        top.localPosition    = new Vector3(0f, height / 2f);
        bottom.localPosition = new Vector3(0f, -height / 2f);
        left.localPosition   = new Vector3(width / 2f, 0f);
        right.localPosition  = new Vector3(-width / 2f, 0f);
    }
}
