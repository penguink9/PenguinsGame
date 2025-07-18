﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{

    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {         
            SceneManagement.Instance.SetTransitionName(sceneTransitionName);
            int mapIndex = TrackCurrentMap.Instance.map;
            MapStateManager.Instance.SaveMapState(mapIndex);
            UIFade.Instance.FadeToBlack();
            StartCoroutine(LoadSceneRoutine());
        }

    }

    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
