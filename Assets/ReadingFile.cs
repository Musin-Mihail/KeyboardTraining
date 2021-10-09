using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class ReadingFile : MonoBehaviour
{
    int number = 0;
    public Text currentTextBox;
    public Text writtenTextBox;
    string currentText;
    bool error = false;
    string[] AllSymbol;
    List<string> newAllSymbol;
    public GameObject statistics;
    public Text statisticsTextBox;
    void Start()
    {
        List<string> tempNewAllSymbol = new List<string>();
        newAllSymbol = new List<string>();
        AllSymbol = File.ReadAllLines("AllSymbol.txt");
        foreach (var symbol in AllSymbol)
        {
            var parse = symbol.Split(char.Parse(";"));
            if(parse.Length == 1)
            {
                tempNewAllSymbol.Add(parse[0] + ";" + 0);
                newAllSymbol.Add(parse[0]);
            }
            else
            {
                tempNewAllSymbol.Add(parse[0] + ";" + parse[1]);
                int correctAnswers = int.Parse(parse[1]);
                if(correctAnswers < 10)
                {
                    newAllSymbol.Add(parse[0]);
                }
            }
        }
        File.WriteAllLines("AllSymbol.txt", tempNewAllSymbol);
        RandomSymbol();
    }
    void Update()
    {
        if(Input.anyKeyDown)
        {
            string currentSymbol = Input.inputString;
            if (currentSymbol.Length > 0)
            {
                if ((int)currentText[number] == (int)currentSymbol[0])
                {
                    if(error == true)
                    {
                        writtenTextBox.text = writtenTextBox.text.Remove(writtenTextBox.text.Length - 24);
                        error = false;
                    }
                    writtenTextBox.text += $"<color=#00FF00>{ Input.inputString }</color>";
                    CreateNewFile(currentText[number].ToString(), true);
                    number++;
                    if(number >= currentText.Length)
                    {
                        number = 0;
                        RandomSymbol();
                    }
                }
                else
                {
                    if (error == true)
                    {
                        writtenTextBox.text = writtenTextBox.text.Remove(writtenTextBox.text.Length - 24);
                    }
                    writtenTextBox.text += $"<color=#FF0000>{ Input.inputString }</color>";
                    CreateNewFile(currentText[number].ToString(), false);
                    error = true;
                }
            }
        }
    }
    void RandomSymbol()
    {
        currentText = "";
        int random = Random.Range(0, newAllSymbol.Count);
        currentText += newAllSymbol[random];
        int numberSymbol = newAllSymbol[random].Length;
        while(numberSymbol < 18)
        {
            random = Random.Range(0, newAllSymbol.Count);
            currentText += newAllSymbol[random];
            numberSymbol ++;
        }
        currentTextBox.text = currentText;
        writtenTextBox.text = "";
    }
    void CreateNewFile(string correctsymbol, bool СorrectAnswer)
    {
        List<string> tempNewAllSymbol = new List<string>();
        AllSymbol = File.ReadAllLines("AllSymbol.txt");
        foreach (var symbol in AllSymbol)
        {
            var parse = symbol.Split(char.Parse(";"));
            if(parse[0] == correctsymbol)
            {
                int correctAnswers = int.Parse(parse[1]);
                if(СorrectAnswer)
                {
                    correctAnswers++;
                }
                else
                {
                    correctAnswers--;
                }
                tempNewAllSymbol.Add(parse[0] + ";" + correctAnswers.ToString());
            }
            else
            {
                tempNewAllSymbol.Add(symbol);
            }
        }
        File.WriteAllLines("AllSymbol.txt", tempNewAllSymbol);
    }
    public void OpenStatistics()
    {
        statisticsTextBox.text = "";
        statistics.SetActive(true);
        AllSymbol = File.ReadAllLines("AllSymbol.txt");
        foreach (var symbol in AllSymbol)
        {
            var parse = symbol.Split(char.Parse(";"));
            int correctAnswers = int.Parse(parse[1]);
            if(correctAnswers < 0)
            {
                statisticsTextBox.text += $"<color=#FF0000>{ parse[0] }</color><color=#FF0000>({ parse[1] })</color>  ";
            }
            else if (correctAnswers > 10)
            {
                statisticsTextBox.text += $"<color=#00FF00>{ parse[0] }</color><color=#00FF00>({ parse[1] })</color>  ";
            }
            else
            {
                statisticsTextBox.text += parse[0] + "(" + parse[1] + ")  ";
            }
        }
    }
    public void CloseStatistics()
    {
        statistics.SetActive(false);
    }
    public void ResetStatistics()
    {
        List<string> tempNewAllSymbol = new List<string>();
        statisticsTextBox.text = "";
        statistics.SetActive(true);
        AllSymbol = File.ReadAllLines("AllSymbol.txt");
        foreach (var symbol in AllSymbol)
        {
            var parse = symbol.Split(char.Parse(";"));
            statisticsTextBox.text += parse[0] + "(0)  ";
            tempNewAllSymbol.Add(parse[0] + ";" + 0);
        }
        File.WriteAllLines("AllSymbol.txt", tempNewAllSymbol);
    }
    public void Exit()
    {
        Application.Quit();
    }
}