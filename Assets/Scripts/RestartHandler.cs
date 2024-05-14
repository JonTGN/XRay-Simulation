using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartHandler : MonoBehaviour
{
    // this script just handles a reboot with the same scenario that it started with

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
