using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    FullScreenMode fullScreenMode;

    // Start is called before the first frame update
    public Button StartButton;

    public Button CreateButton;
    public GameObject Create;

    public Button OptionButton;
    public GameObject Option;

    public Button EndButton;

    public TMP_Dropdown resolutionsDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;

    //---
    private bool optionOpen = false;
    private bool itemCreateOpen =false;

    void Start()
    {
        //시작 버튼 클릭 이벤트
        StartButton.onClick.AddListener(StartGame);

        //제작 버튼
        CreateButton.onClick.AddListener(OpenCreateItem);

        //샵 버튼
        OptionButton.onClick.AddListener(OpenOption);

        //종료 버튼
        EndButton.onClick.AddListener(GameExit);
        DropDownShow();
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Map1");
    }

    void OpenOption()
    {
        if(!optionOpen)
            Option.gameObject.SetActive(true);
        else
            Option.gameObject.SetActive(false);
        optionOpen = !optionOpen;
    }

    void OpenCreateItem()
    {
        if (!itemCreateOpen)
        {
            Create.gameObject.SetActive(true);
            Create.GetComponent<CreateItemUI>().OpneCreateItem();
        }

        else
            Create.gameObject.SetActive(false);
        itemCreateOpen = !itemCreateOpen;

    }

    void DropDownShow()
    {
        resolutions.Clear();  // 중복 방지
        resolutions.AddRange(Screen.resolutions);
        resolutionsDropdown.options.Clear();
        int optionNum = 0;

        foreach (Resolution res in resolutions)
        {
            string optionText = $"{res.width} x {res.height} ";
            resolutionsDropdown.options.Add(new TMP_Dropdown.OptionData(optionText));

            if (res.width == Screen.width && res.height == Screen.height)
                resolutionsDropdown.value = optionNum;
            optionNum++;
        }

        resolutionsDropdown.RefreshShownValue();
    }

    public void DropBoxChange(int x)
    {
        resolutionNum = x;

    }

    public void onBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height
            , fullScreenMode);
    }

    private void GameExit()
    {
        Application.Quit();
    }

}
