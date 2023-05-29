using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void Retry()
    {
        GameManager.Instance.RetryGame();
    }

    public void BackMenu()
    {
        GameManager.Instance.EnterTitle();
    }
}
