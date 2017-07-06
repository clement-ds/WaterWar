using UnityEngine;
using System.Collections;

public abstract class ACutscene : MonoBehaviour {

    public bool startImmediately = false;
    private bool running = false;
    public Camera mainCamera;
    public Camera cutsceneCamera;

    void Start()
    {
        if (startImmediately) StartCutscene();
    }

    void Update()
    {
        if (running) UpdateCutscene();
    }

    public virtual void StartCutscene()
    {
        running = true;
        SwitchCamera();
    }

    private void SwitchCamera()
    {
        mainCamera.enabled = !mainCamera.enabled;
        cutsceneCamera.enabled = !cutsceneCamera.enabled;

    }

    public abstract void UpdateCutscene();

    public virtual void StopCutscene()
    {
        running = false;
        SwitchCamera();
    }
}
