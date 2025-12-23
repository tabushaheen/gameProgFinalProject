using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Assign from Hierarchy")]
    public GameObject menuCanvas;
    public GameObject optionsPanel; 

    [Header("Scene To Load")]
    public string firstLevelSceneName = "DungeonRoom";

    void Awake()
    {
        Time.timeScale = 1f;
        ShowMain();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void ShowOptions()
    {
        if (menuCanvas == null || optionsPanel == null) return;

        optionsPanel.SetActive(true);

        Transform root = menuCanvas.transform;
        for (int i = 0; i < root.childCount; i++)
        {
            GameObject child = root.GetChild(i).gameObject;
            if (child == optionsPanel) continue;
            child.SetActive(false);
        }
    }

    public void ShowMain()
    {
        if (menuCanvas == null || optionsPanel == null) return;

        Transform root = menuCanvas.transform;
        for (int i = 0; i < root.childCount; i++)
        {
            root.GetChild(i).gameObject.SetActive(true);
        }

        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
