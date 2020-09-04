using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
    public void LoadNextScene()
    {
        SceneManager.LoadScene(0);
    }
}