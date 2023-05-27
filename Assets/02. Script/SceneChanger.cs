using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

  void Start()
  {
        AudioManager.Instance.Title();
  }

  void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            AudioManager.Instance.Game();
            SceneManager.LoadScene("GameScene");
        }
    }
}
