using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StopUi : MonoBehaviour
{

    public Button reStartButton;

    public Button resetButton;

    //public Button OptionButton;

    public Button EixtButton;
    public Animator animator;



    void Start()
    {
        reStartButton.onClick.AddListener(restart);
        resetButton.onClick.AddListener(resetGame);
        EixtButton.onClick.AddListener(Exit);

        animator = GetComponent<Animator>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    // Update is called once per frame
    void Update()
    {


    }

    void restart()
    {
        GameManager.Instance.GameReStart();
    }
    void resetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Exit()
    {
        Application.Quit();
    }

    public void moveUi(bool move)
    {
        animator.SetBool("MoveUI", move);
    }

    public void EndAnime()
    {
        GameObject ui;
        ui = GameObject.Find("UI_Canvas");
        ui.GetComponent<UIManager>().ActiveStopPanel(false);
    }
}
