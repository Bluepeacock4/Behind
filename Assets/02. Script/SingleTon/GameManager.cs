using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;

    public bool isPause;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance => _instance == null ? null : _instance;

    public void PauseGame()
    {
        if (!isPause)
        {
            Time.timeScale = 0;
            isPause = true;
        }
        else
        {
            ContinueGame();
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        isPause = false;
    }

    public void EnterTitle()
    {
        ContinueGame();
        AudioManager.Instance.Click();
        SceneManager.LoadScene("TitleScene");
    }
}
