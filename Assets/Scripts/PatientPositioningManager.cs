using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Michsky.MUIP;
using Unity.VisualScripting;
using UnityEngine;

public class PatientPositioningManager : MonoBehaviour
{
    [SerializeField] private GameObject ListViewList;
    [SerializeField] private sceneIntroductionManager sceneIntroductionManager;

    public List<GameObject> XrayObjects = new List<GameObject>();
    public List<GameObject> VisibleObjects = new List<GameObject>();

    void Start()
    {
        // go through the list view list and add data to the relevant buttons
        //StartCoroutine(WaitToAddData(0));

        // ** Add Data To Buttons is called by JS Hook now **
    }

    public void InitPatientMenu()
    {
        AddDataToButtons();
    }

    private void AddDataToButtons()
    {
        var parent = sceneIntroductionManager.selectedScenario;
        GameObject visibleModels = parent.transform.Find("visible").gameObject;
        GameObject xrayModels = parent.transform.Find("xray").gameObject;
        List<GameObject> listViewBtns = new List<GameObject>();

        // get all the GO children in this GO
        foreach (Transform btn in ListViewList.transform)
        {
            //Debug.Log("btn: " + btn.gameObject.name);
            listViewBtns.Add(btn.gameObject);
        }

        int childCnt = visibleModels.transform.childCount; // visibleModels should have same # of children as xrayModels**
        for (int i = 0; i < childCnt; i++)
        {
            GameObject visibleChild = visibleModels.transform.GetChild(i).gameObject;
            GameObject xrayChild = xrayModels.transform.GetChild(i).gameObject;

            //Debug.Log(xrayChild.name + " vis: " + visibleChild.name);

            // neeed to loop thru listviewlist and set names accordingly
            listViewBtns[i].GetComponent<ButtonManager>().buttonText = visibleChild.name;
            listViewBtns[i].SetActive(true);

            // add each visible and xray object to corresponding list so the patient positioning buttons can enable/disable them as needed
            XrayObjects.Add(xrayChild);
            VisibleObjects.Add(visibleChild);
        }
    }

    IEnumerator WaitToAddData(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        AddDataToButtons();
    }
}
