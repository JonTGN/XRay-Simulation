using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavascriptHook : MonoBehaviour
{
    [SerializeField] sceneIntroductionManager SIMngr;

    public void LoadScenario(string scenarioName)
    {
        // some conditionals to render appropriate scene up here ...
        SceneTypes sceneType;
        if (SceneTypes.TryParse(scenarioName, out sceneType))
        {
            switch (sceneType)
            {
                case SceneTypes.PAHand:
                    SIMngr.introText = "PA Hand";
                    break;
                case SceneTypes.PAFoot:
                    SIMngr.introText = "PA Foot (not implemented!)";
                    break;
                default:
                    SIMngr.introText = "Nothing found with that name!";
                    SIMngr.SceneLoaded();
                    break;
            }
            SIMngr.SceneLoaded();
            SIMngr.loadScene = sceneType;
        }
        else
        {
            SIMngr.introText = "Nothing found with that name!";
            SIMngr.SceneLoaded();
        }

        //SIMngr.introText = scenarioName;
        //SIMngr.SceneLoaded(); // this sets the TMP obj to the specified intro text ^
    }
}
