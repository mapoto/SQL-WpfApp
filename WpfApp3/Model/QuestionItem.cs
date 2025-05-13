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

    public QuestionItem(QuestionItem o) {
        this.id = o.id;
        this.date = o.date;
        this.question = o.question;
        this.choices = o.choices;
        this.solution = o.solution;

    }

    //public QuestionItem() {
    //    this.id = 0;
    //    this.date = DateTime.Now;
    //    this.question = "new questions";
    //    this.choices = new ObservableCollection<string>();
    //    this.solution = 0;
    //}

    public int Id { get => id; set => id = value; }
    public DateTime Date { get => date; set => date = value; }
    public string Question { get => question; set => question = value; }
    public ObservableCollection<string> Choices { get => choices; set => choices = value; }
    public int Solution { get => solution; set => solution = value; }


    // override object.Equals
    public override bool Equals(Object? obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        QuestionItem a = (QuestionItem)obj;
        
        if (a.id != this.id ||
            a.question != this.question ||
            a.choices != this.choices ||
            a.solution != this.solution)
        {
            return false;
        }

        return true;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return id.GetHashCode();
    }

}
