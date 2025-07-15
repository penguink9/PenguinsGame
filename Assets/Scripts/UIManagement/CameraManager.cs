using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    private CinemachineCamera virtualCamera;
    private CinemachineConfiner2D confiner;

    protected override void Awake()
    {
        base.Awake();

        virtualCamera = GetComponent<CinemachineCamera>();
        confiner = GetComponent<CinemachineConfiner2D>();

        if (virtualCamera == null)
            Debug.LogError("CameraManager: Missing CinemachineCamera component!");

        if (confiner == null)
            Debug.LogError("CameraManager: Missing CinemachineConfiner2D component!");
    }
    private void Start()
    {
        GameObject confiner = GameObject.FindGameObjectWithTag("Confiner");
        if (confiner != null)
        {
            SetConfiner(confiner.GetComponent<Collider2D>());
        }
        else
        {
            Debug.LogWarning("CameraManager: No confiner found with tag 'Confiner'.");
        }
    }
    public void SetFollow(Transform target)
    {
        virtualCamera.Follow = target;
    }

    public void SetConfiner(Collider2D newConfiner)
    {
        if (confiner != null)
        {
            confiner.BoundingShape2D = newConfiner;
            confiner.InvalidateBoundingShapeCache();
        }
    }
}
