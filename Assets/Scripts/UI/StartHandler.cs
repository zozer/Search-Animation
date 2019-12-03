using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartHandler : MonoBehaviour
{

    // Start is called before the first frame update
    public void StartButton() {
        SceneManager.LoadScene("Instruction");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
