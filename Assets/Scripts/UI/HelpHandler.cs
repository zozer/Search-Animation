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
    [SerializeField] Text pageText;

    string question1Text = "How to create a node";
    string question2Text = "How to link two nodes together";
    string question3Text = "How to name the node";
    string question4Text = "How to delete the node";
    string question5Text = "How to delete the line";
    string question6Text = "How to show the search tree";
    string question7Text = "How to play search animation";
    string question8Text = "How to pause animation";
    string question9Text = "How to resume search animation";
    string question10Text = "How to step by step shows animation";
    string question11Text = "How to swich searh model";


    string answer1Text = "Clicks on node button and a blue node will show up. Places the blue node on the drawing space.";
    string answer2Text = "Clicks on link button. Clicks on one node and then another node.";
    string answer3Text = "Clicks on the node, the node will turn yellow, then type the name of the node";
    string answer4Text = "Clicks on Delete Node button, when the button turn gray, click node which you want to delete. Note: if the node you deleted link with line, line will automatically delete. ";
    string answer5Text = "Clicks on Delete line button, when the button turn gray, click two node that the line is connected";
    string answer6Text = "After build your map, clicks on Show button";
    string answer7Text = "Clicks on Play button, the animation will automatically play";
    string answer8Text = "Clicks on pause button, the animation will pause";
    string answer9Text = "Clicks on resume button, the animation will resume";
    string answer10Text = "Clicks on Play button first, then clicks on pause button. Clicks on Next step button or previous step button to show the animation step by step";
    string answer11Text = "The defult search model is bread-first search model, if user want to switch search, click BF search button, then chose other search model.";
    List<string> questionList = new List<string>();
    List<string> answerList = new List<string>();

    private int questionNumber;
    private int pageNumber;
    private int totalPageNumber;

    void Start()
    {
        questionNumber = 0;
        pageNumber = 1;

        questionList.Add(question1Text);
        questionList.Add(question2Text);
        questionList.Add(question3Text);
        questionList.Add(question4Text);
        questionList.Add(question5Text);
        questionList.Add(question6Text);
        questionList.Add(question7Text);
        questionList.Add(question8Text);
        questionList.Add(question9Text);
        questionList.Add(question10Text);
        questionList.Add(question11Text);

        answerList.Add(answer1Text);
        answerList.Add(answer2Text);
        answerList.Add(answer3Text);
        answerList.Add(answer4Text);
        answerList.Add(answer5Text);
        answerList.Add(answer6Text);
        answerList.Add(answer7Text);
        answerList.Add(answer8Text);
        answerList.Add(answer9Text);
        answerList.Add(answer10Text);
        answerList.Add(answer11Text);

        int excess = questionList.Count % 4;
        totalPageNumber = questionList.Count / 4;
        
        if(excess != 0)
        {
            totalPageNumber++;
        }

        showQA();
        questionNumber = questionNumber + 3;
    }

    public void PreviousButton()
    {
        questionNumber = questionNumber - 7;

        if (questionNumber >= 0)
        {
            pageNumber--;
            showQA();
            questionNumber = questionNumber + 3;
        }
        else
        {
            questionNumber = questionNumber + 7;
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void NextButton()
    {
        questionNumber++;

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

        if (questionNumber < balance)
        {
            pageNumber++;
            showQA();
            questionNumber = questionNumber + 3;
        }
        else
        {
            questionNumber--;
        }
    }

    private void showQA()
    {

        int question1Block = questionNumber;
        int question2Block = questionNumber + 1;
        int question3Block = questionNumber + 2;
        int question4Block = questionNumber + 3;

        addText(question1, answer1, question1Block);
        addText(question2, answer2, question2Block);
        addText(question3, answer3, question3Block);
        addText(question4, answer4, question4Block);

        pageText.text = pageNumber + "/" + totalPageNumber;
    }

    void addText(Text questionField, Text answerField, int index)
    {
        int totalQuestions = questionList.Count;

        if (index < totalQuestions)
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