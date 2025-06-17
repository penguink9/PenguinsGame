using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    private void Start()
    { 
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerManager.Instance.GetActivePlayer().transform.position = transform.position;
            GameObject confiner = GameObject.FindGameObjectWithTag("Confiner");
            CameraManager.Instance.SetConfiner(confiner.GetComponent<PolygonCollider2D>());
            TrackCurrentMap.Instance.UpdateLevelText();
            UIFade.Instance.FadeToClear();
        }
        
    }
}
