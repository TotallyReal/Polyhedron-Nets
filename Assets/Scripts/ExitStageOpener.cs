using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitStageOpener : MonoBehaviour
{
    [SerializeField] private VisualPolyhedronFactory mainFactory;
    [SerializeField] private VisualPolyhedronFactory shadowFactory;
    [SerializeField] private PolyhedronKeyboardInput keyboardInput;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform player;

    [Header("Colors")]
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color failedColor;
    [SerializeField] private Color passedColor;

    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        material.color = neutralColor;

        SetState(StageState.NEUTRAL);
    }

    private void Start()
    {
        keyboardInput.OnPolyhedronUnfolded += KeyboardInput_OnPolyhedronUnfolded;
        keyboardInput.OnRestart += KeyboardInput_OnRestart;
    }

    private void KeyboardInput_OnRestart(object sender, System.EventArgs e)
    {
        SetState(StageState.NEUTRAL);
    }

    private enum StageState
    {
        NEUTRAL,
        PASS,
        FAIL
    }

    private StageState state;

    private void SetState(StageState state) {
        this.state = state;
        switch (state)
        {
            case StageState.NEUTRAL:
                material.color = neutralColor;
                break;
            case StageState.PASS:
                material.color = passedColor;
                break;
            case StageState.FAIL:
                material.color = failedColor;
                break;
        }
    }

    private void KeyboardInput_OnPolyhedronUnfolded(object sender, bool fullyUnfolded)
    {
        if (fullyUnfolded)
        {
            FaceGraph mainFaceGraph = mainFactory.GetVisualPolyhedron().GetFaceGraph();
            FaceGraph shadowFaceGraph = shadowFactory.GetVisualPolyhedron().GetFaceGraph();

            if (mainFaceGraph.CompareTo(shadowFaceGraph))
            {
                SetState(StageState.PASS);
            } else
            {
                SetState(StageState.FAIL);
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (state)
        {
            case StageState.PASS:
                EscapeRoomManager.Instance.RestartPolyhedrons(restartShadow: true);
                SetState(StageState.NEUTRAL);

                // move to spawn
                CharacterController controller = player.GetComponent<CharacterController>();
                controller.enabled = false;
                player.position = spawnPosition.position;
                controller.enabled = true;
                break;
            case StageState.FAIL:
                EscapeRoomManager.Instance.RestartPolyhedrons(restartShadow: false);
                SetState(StageState.NEUTRAL);
                break;
        }
    }
}
