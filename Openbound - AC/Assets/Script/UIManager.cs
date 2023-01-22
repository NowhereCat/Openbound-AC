using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("TAB MENU")]
    public GameObject tabMenuPanel;
    public float[] tabMenuPositions;
    public float tabMenuSpeed = 10f;
    bool tabMenu = false;

    [Header("LOCATION PANEL")]
    public RectTransform locationPanel;
    public float[] locationPositions;
    public float locationPanelDuration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        ToggleTabMenu(true);

        StartCoroutine(ShowLocation(true));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleTabMenu(bool onlyCheck = false)
    {
        if (!onlyCheck) tabMenu = !tabMenu;

        if (tabMenu)
        {
            tabMenuPanel.transform.LeanMoveX(tabMenuPositions[0], tabMenuSpeed);
        }
        else
        {
            tabMenuPanel.transform.LeanMoveX(tabMenuPositions[1], tabMenuSpeed);
        }
    }

    public IEnumerator ShowLocation(bool _bool)
    {
        if (_bool)
        {
            locationPanel.transform.LeanMoveY(locationPositions[0], locationPanelDuration/3).setOnComplete(() => { StartCoroutine(ShowLocation(false)); });
        }
        else
        {
            yield return new WaitForSeconds(locationPanelDuration/3);
            locationPanel.transform.LeanMoveY(locationPositions[1], locationPanelDuration / 3);
        }
    }

}
