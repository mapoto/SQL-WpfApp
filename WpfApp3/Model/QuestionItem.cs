using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

public class QuestionItem
{

    private int id;

    private DateTime date;

    private string question;

    private ObservableCollection<string> choices;

    private int solution;

    public QuestionItem(int id, DateTime date, string question, ObservableCollection<string> choices, int solution)
    {
        this.id = id;
        this.date = date;
        this.question = question;
        this.choices = choices;
        this.solution = solution;

    }

    public int Id { get => id; set => id = value; }
    public DateTime Date { get => date; set => date = value; }
    public string Question { get => question; set => question = value; }
    public ObservableCollection<string> Choices { get => choices; set => choices = value; }
    public int Solution { get => solution; set => solution = value; }

}
