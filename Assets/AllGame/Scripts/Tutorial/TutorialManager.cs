using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Text tutorialText;
    private int tutorialStep = 0;
    private string[] tutorialMessages = {
        "Bienvenido al juego!",
        "Usa el joystick izquierdo para moverte.",
        "¡Buena suerte y diviértete!"
    };

    void Start()
    {
        if (!PlayerPrefs.HasKey("HasSeenTutorial"))
        {
            ShowTutorial();
        }
        else
        {
            tutorialPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (tutorialPanel.activeSelf && Input.GetButtonDown("Submit"))
        {
            ShowNextMessage();
        }
    }

    void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
        ShowNextMessage();
    }

    public void ShowNextMessage()
    {
        if (tutorialStep < tutorialMessages.Length)
        {
            tutorialText.text = tutorialMessages[tutorialStep];
            tutorialStep++;
        }
        else
        {
            tutorialPanel.SetActive(false);
            PlayerPrefs.SetInt("HasSeenTutorial", 1);
            PlayerPrefs.Save();
        }
    }

    public void SkipTutorial()
    {
        tutorialPanel.SetActive(false);
        PlayerPrefs.SetInt("HasSeenTutorial", 1);
        PlayerPrefs.Save();
    }
}
