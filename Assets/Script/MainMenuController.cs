using UnityEngine;
using UnityEngine.SceneManagement;

//this code was made by myself. it for sample control to load scene
// taojin sha 
//2024.10.18

public class MainMenuController : MonoBehaviour
{
    public GameObject ControlInfo;
    public GameObject CreditsInfo;
    public GameObject MainInfo;

    void Start()
    {
        if (ControlInfo == null)
        {
            Debug.LogWarning("ControlInfo not found");
        }
        else
        {
            ControlInfo.SetActive(false);
        }
        if (CreditsInfo == null)
        {
            Debug.LogWarning("CreditsInfo not found");
        }
        else
        {
          CreditsInfo.SetActive(false);
        }
        if (MainInfo == null)
        {
            Debug.LogWarning("MainInfo not found");
        }
        else
        {
            MainInfo.SetActive(true);
        }
    }

    public void NewGame()
    {
        
        SceneManager.LoadScene("Overworld");
    }

    public void ContinueGame()
    {
        
        SceneManager.LoadScene("Overworld"); 
    }

    public void Credits()
    {
       //turn off all and open hided credits info
       MainInfo.gameObject.SetActive(false);
        CreditsInfo.gameObject.SetActive(true);
        Debug.Log("credits clicked");
    }

    public void Control()
    {
        //turn off all and open hided control info
        MainInfo.gameObject.SetActive(false);
        ControlInfo.gameObject.SetActive(true);
        Debug.Log("control clicked");
    }

    public void BackToMain()
    {
        CreditsInfo.gameObject.SetActive(false);
        ControlInfo.gameObject.SetActive(false);
        MainInfo.SetActive(true);
        Debug.Log("BackToMain clicked");
    }


    public void ExitGame()
    {
       
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
            Application.Quit(); // Exits the build
#endif
    }
}
