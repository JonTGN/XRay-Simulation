using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class sceneIntroductionManager : MonoBehaviour
{
    // this script simply hanles the initial transition at the beginning of the scene
    [SerializeField] public SceneTypes loadScene;
    [SerializeField] private bool dontUseJSHook = true;
    [SerializeField] private JavascriptHook jsHook;
    [SerializeField] private GameObject characterManager;
    [SerializeField] private CanvasGroup introUIGroup;
    [SerializeField] private TMP_Text introTextTMPObj;
    public string introText = "";
    [SerializeField] private CinemachineVirtualCamera introVCam;
    public GameObject selectedScenario;

    // existing UI
    [SerializeField] private CanvasGroup menuUIGroup;
    [SerializeField] private CanvasGroup firstUIGroup;
    [SerializeField] private GameObject menuGO;
    [SerializeField] private GameObject firstUIGO;

    // animators
    [SerializeField] private Animator doorAnim;
    [SerializeField] private AudioSource doorOpenSFX;

    // private vars
    bool init = false;
    bool startTransition = false;
    bool doneFadeOut = false;
    bool notActivated = true;
    bool timeToFadeInNormalUI = false;

    void Start()
    {
        if (dontUseJSHook)
        {
            jsHook.LoadScenario(loadScene.ToString());
        }
        introTextTMPObj.text = introText;
    }

    public void SceneLoaded()
    {
        introTextTMPObj.text = introText;

        // find GO correlating with sceneType and enable it, **GO name will always == Enum**
        selectedScenario = characterManager.transform.Find(loadScene.ToString()).gameObject; // will use this later to manage different variations of the character
        selectedScenario.SetActive(true);
    }

    void Update()
    {
        if ((Input.anyKeyDown || Input.GetMouseButtonDown(0)) && !init)
        {
            init = true;
            StartCoroutine(WaitForDoor(0.5f));
            doorAnim.SetBool("Start", true);
            doorOpenSFX.Play();
        }

        if (startTransition)
        {
            if (introUIGroup.alpha >= 0)
            {
                introUIGroup.alpha -= Time.deltaTime;
                if (introUIGroup.alpha <= 0)
                {
                    doneFadeOut = true;
                }
            }
        }

        if (doneFadeOut && notActivated)
        {
            // fade in normal UI
            notActivated = false;
            StartCoroutine(WaitToActivateUI(1)); // wait to enable UI for a bit

            // transition vcams
            introVCam.Priority = 0; // this will auto-transition into the first camera

            // can open door here too???
        }

        if (timeToFadeInNormalUI)
        {
            if (menuUIGroup.alpha < 1)
            {
                menuUIGroup.alpha += Time.deltaTime;
            }

            if (firstUIGroup.alpha < 1)
            {
                firstUIGroup.alpha += Time.deltaTime;
            }

            if (menuUIGroup.alpha >= 1 && firstUIGroup.alpha >= 1)
            {
                //Debug.Log("Done with intro!");
                this.gameObject.SetActive(false); // turn this off so we dont run update the rest of the sim
            }
        }
    }

    private void activateUI()
    {
        menuGO.SetActive(true);
        firstUIGO.SetActive(true);

        menuUIGroup.alpha = 0;
        firstUIGroup.alpha = 0;

        timeToFadeInNormalUI = true;

    }

    IEnumerator WaitToActivateUI(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        activateUI(); // activate user UI after coroutine
    }

    IEnumerator WaitForDoor(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        startTransition = true; // activate user UI after coroutine
    }
}
