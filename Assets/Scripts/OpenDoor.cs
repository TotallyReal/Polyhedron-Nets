using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    [SerializeField] private Transform door;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        door.gameObject.SetActive(false);
        EscapeRoomManager.Instance.RestartPolyhedrons(restartShadow: false);
    }
}
