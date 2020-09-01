using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject terranButton;
    public GameObject quitButton;
    public GameObject loadButton;
    public GameObject saveButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (Time.timeScale == 1f)
            {
                resumeButton.SetActive(true);
                terranButton.SetActive(true);
                quitButton.SetActive(true);
                loadButton.SetActive(true);
                saveButton.SetActive(true);
                Time.timeScale = 0f;
            }
            
        }
    }
    public void Resume()
    {
        if (Time.timeScale == 0f)
        {
            resumeButton.SetActive(false);
            terranButton.SetActive(false);
            quitButton.SetActive(false);
            loadButton.SetActive(false);
            saveButton.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void LoadTerranPlanet()
    {
        FindObjectOfType<PlayerChange>().NewScene();
        SceneManager.LoadScene("Terran");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
