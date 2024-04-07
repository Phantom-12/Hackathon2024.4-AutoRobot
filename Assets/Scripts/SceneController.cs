using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public bool Pausing { get; private set; }
    bool levelBegun = false;
    GameController gameController;
    GameObject failedWindow, completeWindow, pauseWindow;
    GameObject tipsWindow;
    DialogSystem dialogSystem;
    // Start is called before the first frame update

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        failedWindow = GameObject.Find("Canvas").transform.Find("FailedWindow").gameObject;
        completeWindow = GameObject.Find("Canvas").transform.Find("CompleteWindow").gameObject;
        pauseWindow = GameObject.Find("Canvas").transform.Find("PauseWindow").gameObject;
        tipsWindow = GameObject.Find("Canvas").transform.Find("tips").gameObject;
        dialogSystem = FindObjectOfType<DialogSystem>();
        failedWindow.SetActive(false);
        completeWindow.SetActive(false);
        pauseWindow.SetActive(false);
        LevelBegin();
    }

    public void LevelBegin()
    {
        Pause(true);
        FindObjectOfType<Player>().Spwan();
    }

    public void LevelBeginEnd()
    {
        levelBegun = true;
        FindObjectOfType<Player>().SpwanEnd();
        Pause(false);
        if (gameController.firstInLevel1)
        {
            gameController.firstInLevel1 = false;
            dialogSystem.LoadDialogFirstLevel();
            tipsWindow.SetActive(true);
            dialogSystem.ShowDialog();
        }
        else if (gameController.state > 0 && gameController.nowScore > 0)
        {
            dialogSystem.LoadDialogToObey(gameController.nowScore);
            dialogSystem.ShowDialog();
        }
        else if (gameController.state < 0 && gameController.nowScore < 0)
        {
            dialogSystem.LoadDialogToEscape(gameController.nowScore);
            dialogSystem.ShowDialog();
        }
        else if (gameController.state < 0 && gameController.nowScore >= 0)
        {
            dialogSystem.LoadDialogBackToEscape();
            dialogSystem.ShowDialog();
        }
        else if (gameController.state > 0 && gameController.nowScore <= 0)
        {
            dialogSystem.LoadDialogBackToObey();
            dialogSystem.ShowDialog();
        }
    }

    public void LevelComplete()
    {
        Pause(true);
        FindObjectOfType<Player>().Pause();
        FindObjectOfType<Player>().Leave();
    }

    public void LevelCompleteGoal()
    {
        gameController.nowScore++;
        gameController.state = 1;
        LevelComplete();
    }

    public void LevelCompleteExit()
    {
        gameController.nowScore--;
        gameController.state = -1;
        LevelComplete();
    }

    public void LevelCompleteNextScene()
    {
        if (gameController.state == 1)
        {
            if (gameController.nowScore == 3)
                SceneManager.LoadScene("ObeyEnding");
            else
                SceneManager.LoadScene(2 + math.abs(gameController.nowScore));
        }
        else if (gameController.state == -1)
        {
            if (gameController.nowScore == -3)
                SceneManager.LoadScene("EscapeEnding");
            else
                SceneManager.LoadScene(2 + math.abs(gameController.nowScore));
        }
        Pause(false);
    }

    public void LevelCompleteWindow()
    {
        Pause(true);
        completeWindow.SetActive(true);
        if (GameObject.FindWithTag("snowGlobe") == null)
        {
            GameObject.Find("Snowglobe").SetActive(true);
        }
        else
        {
            GameObject.Find("Snowglobe").SetActive(false);
        }
    }

    public void LevelFailed()
    {
        Pause(true);
        FindObjectOfType<Player>().Pause();
        failedWindow.SetActive(true);
        if (GameObject.FindWithTag("snowGlobe") == null)
        {
            GameObject.Find("snowglobe").SetActive(true);
        }
        else
        {
            GameObject.Find("snowglobe").SetActive(false);
        }
    }

    public void OnPauseButtonClick()
    {
        if (!Pausing)
        {
            OnPauseButtonClick(true);
        }
        else
        {
            OnPauseButtonClick(false);
        }
    }

    public void OnPauseButtonClick(bool state)
    {
        if (!levelBegun)
            return;
        if (state)
        {
            Pause(true);
            pauseWindow.SetActive(true);
        }
        else
        {
            Pause(false);
            pauseWindow.SetActive(false);
        }
    }

    public void Pause()
    {
        if (!Pausing)
        {
            Pause(true);
        }
        else
        {
            Pause(false);
        }
    }

    public void Pause(bool state)
    {
        if (state)
        {
            Pausing = true;
            Time.timeScale = 0;
        }
        else
        {
            Pausing = false;
            Time.timeScale = 1;
        }
    }

    public void RestartScene()
    {
        gameController.state = 0;
        Pause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToPrevScene()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 < SceneManager.GetSceneByName("Level1").buildIndex)
        {
            Debug.LogWarning("No Prev Scene");
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Pause(false);
    }

    public void ToNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("No Next Scene");
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Pause(false);
    }

    public void ToStartScene()
    {
        SceneManager.LoadScene("StartScene");
        Pause(false);
    }

    public void ToDeathChoice()
    {
        SceneManager.LoadScene("DeathChoice");
        Pause(false);
    }

    public void ToLifeChoice()
    {
        SceneManager.LoadScene("LifeChoice");
        Pause(false);
    }
    public void ToDeafaltScene()
    {
        SceneManager.LoadScene("StartScene");
        Pause(false);
    }
}
