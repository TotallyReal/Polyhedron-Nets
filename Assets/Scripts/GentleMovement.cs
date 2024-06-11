using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GentleMovement : MonoBehaviour
{

    [SerializeField] float maxY = 1f;

    private float t = 0;
    private float dt = 0;
    private Vector3 center;
    private void Start()
    {
        center = transform.position;
        t = Random.Range(0f, 100f);
    }


    void FixedUpdate()
    {
        transform.position = center + Vector3.up * Mathf.Cos(t) * maxY;
        transform.rotation = Quaternion.Euler(0, 30f * Mathf.Cos(t * 1.1245f), 0);
        t += 2f * Mathf.PI * Time.deltaTime / 4f;
    }
}
