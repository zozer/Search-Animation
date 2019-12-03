#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HelpHandler : MonoBehaviour
{
    [SerializeField] Text question1;
    [SerializeField] Text question2;
    [SerializeField] Text question3;
    [SerializeField] Text question4;
    [SerializeField] Text answer1;
    [SerializeField] Text answer2;
    [SerializeField] Text answer3;
    [SerializeField] Text answer4;

    string question1Text = "How to create a node";
    string question2Text = "How to link two nodes together";
    string question3Text = "3...";
    string question4Text = "4...";
    string question5Text = "5...";

    string answer1Text = "Clicks on node button and a blue node will show up. Places the blue node on the drawing space.";
    string answer2Text = "Clicks on link button. Clicks on one node and then another node.";
    string answer3Text = "3...";
    string answer4Text = "4...";
    string answer5Text = "5...";


    List<string> questionList = new List<string>();
    List<string> answerList = new List<string>();

    private int pageNumber;

    void Start()
    {
        pageNumber = 0;

        questionList.Add(question1Text);
        questionList.Add(question2Text);
        questionList.Add(question3Text);
        questionList.Add(question4Text);
        questionList.Add(question5Text);

        answerList.Add(answer1Text);
        answerList.Add(answer2Text);
        answerList.Add(answer3Text);
        answerList.Add(answer4Text);
        answerList.Add(answer5Text);

        showQA();
        pageNumber = pageNumber + 3;
    }

    public void PreviousButton()
    {

        pageNumber--;
        
        int excess = questionList.Count % 4;

        if (excess == 1)
        {
            pageNumber = pageNumber - 6;
        }
        else if (excess == 2)
        {
            pageNumber = pageNumber - 5;
        }
        else if (excess == 3)
        {
            pageNumber = pageNumber - 4;
        }

        if (pageNumber >= 0)
        {
            showQA();
            pageNumber = pageNumber + 3;
        }
        else
        {
            pageNumber++;

            if (excess == 1)
            {
                pageNumber = pageNumber + 6;
            }
            else if (excess == 2)
            {
                pageNumber = pageNumber + 5;
            }
            else if (excess == 3)
            {
                pageNumber = pageNumber + 4;
            }
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void NextButton()
    {
        pageNumber++;

        int excess = questionList.Count % 4;
        int balance = questionList.Count;

        if(excess == 1)
        {
            balance = balance + 3;
        }
        else if (excess == 2)
        {
            balance = balance + 2;
        }
        else if (excess == 3)
        {
            balance = balance + 1;
        }

        if (pageNumber < balance)
        {
            showQA();
            pageNumber = pageNumber + 3;
        }
        else
        {
            pageNumber--;
        }
    }

    private void showQA()
    {

        int question1Block = pageNumber;
        int question2Block = pageNumber + 1;
        int question3Block = pageNumber + 2;
        int question4Block = pageNumber + 3;

        addText(question1, answer1, question1Block);
        addText(question2, answer2, question2Block);
        addText(question3, answer3, question3Block);
        addText(question4, answer4, question4Block);
    }

    void addText(Text questionField, Text answerField, int index)
    {
        int questionNum = questionList.Count;

        if (index < questionNum)
        {
            questionField.text = questionList[index];
            answerField.text = answerList[index];
        }
        else
        {
            questionField.text = "";
            answerField.text = "";
        }
    }
}