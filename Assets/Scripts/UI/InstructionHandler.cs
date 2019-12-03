#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionHandler : MonoBehaviour
{
    [SerializeField] Text title;

    [SerializeField] Text detail;

    string tutorial1 = "Create A Node";
    string tutorial2 = "Link Nodes";
    string tutorial3 = "...";

    string tutorial1Description = "Clicks on node button and a blue node will show up. Places the blue node on the drawing space.";
    string tutorial2Description = "Clicks on link button. Clicks on one node and then another node.";
    string tutorial3Description = "...";

    List<string> titleList = new List<string>();
    List<string> descriptionList = new List<string>();

    private int pageNumber = 0;

    void Start() {
        pageNumber = 0;
        
        titleList.Add(tutorial1);
        titleList.Add(tutorial2);
        titleList.Add(tutorial3);

        descriptionList.Add(tutorial1Description);
        descriptionList.Add(tutorial2Description);
        descriptionList.Add(tutorial3Description);

        showTutorial();
    }

    public void PreviousButton() {

        pageNumber--;

        if (pageNumber < 0)
        {
            SceneManager.LoadScene("Start");
        }
        else
        {
            showTutorial();
        }
    }

    public void NextButton()
    {
        pageNumber++;

        if (pageNumber >= titleList.Count)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            showTutorial();
        }
    }

    public void SkipButton()
    {
        SceneManager.LoadScene("Main");
    }

    private void showTutorial()
    {
        title.text = titleList[pageNumber];
        detail.text = descriptionList[pageNumber];
    }
}