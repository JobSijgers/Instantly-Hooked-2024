using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject normalUI;
    [SerializeField] private GameObject pauseUI;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            normalUI.SetActive(!normalUI.activeInHierarchy);
            pauseUI.SetActive(!pauseUI.activeInHierarchy);
            Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        }
    }
    public void Resume()
    {
        normalUI.SetActive(true);
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
