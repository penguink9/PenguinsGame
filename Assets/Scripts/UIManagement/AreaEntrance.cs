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
            UIFade.Instance.FadeToClear();
        }
        
    }
}
