using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;

[SelectionBase]
public class NumberedCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [Range(0,100)]
    [SerializeField] private int number;

    private void OnValidate()
    {
        SetNumber(number);
    }

    public void SetPosition(Vector3 center, Vector3 direction)
    {
        transform.position = center;
        transform.forward = direction;
    }

    public void SetRadius(float radius)
    {
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public void SetNumber(int n)
    {
        int digits = (n == 0) ? 1 : 1 + Mathf.FloorToInt(Mathf.Log10(n));
        text.text = "" + n;
        switch (digits)
        {
            case 1:
                text.fontSize = 36;
                break;
            case 2:
                text.fontSize = 26;
                break;
            case 3:
                text.fontSize = 18;
                break;
        }

    }
}
