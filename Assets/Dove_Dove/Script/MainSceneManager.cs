using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    FullScreenMode fullScreenMode;

    // Start is called before the first frame update
    public Button StartButton;

    public Button ShopButton;

    public Button OptionButton;
    public GameObject Option;

    public Button EndButton;

    public TMP_Dropdown resolutionsDropdown;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum;

    //---
    private bool optionOpen = false;

    void Start()
    {
        //���� ��ư Ŭ�� �̺�Ʈ
        StartButton.onClick.AddListener(test);

        //�� ��ư
        ShopButton.onClick.AddListener(test);

        //�� ��ư
        OptionButton.onClick.AddListener(OpenOption);

        //���� ��ư
        EndButton.onClick.AddListener(test);
        DropDownShow();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void test()
    {
        Debug.Log("�׽�Ʈ ");
    }

    void OpenOption()
    {
        if(!optionOpen)
            Option.gameObject.SetActive(true);
        else
            Option.gameObject.SetActive(false);
        optionOpen = !optionOpen;
    }

    void DropDownShow()
    {
        resolutions.Clear();  // �ߺ� ����
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
}
