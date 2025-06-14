using UnityEngine;

public class EnemyTargetProvider : Singleton<EnemyTargetProvider>
{
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public Transform GetTarget()
    {
        return target;
    }
}
