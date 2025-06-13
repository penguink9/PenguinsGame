using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    [SerializeField] private NormalPenguinController player;

    private void Start()
    {
       

        
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            player.transform.position = this.transform.position;
            UIFade.Instance.FadeToClear();
        }
        
    }
}
