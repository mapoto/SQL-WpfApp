using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp3.MVVM;


namespace WpfApp3.ViewModel
{
    internal class ContentViewModel:ViewModelBase
    {
        public ObservableCollection<QuestionItem> QuestionItemsCatalogue { get; set; }

        public ContentViewModel() {

            QuestionItemsCatalogue = new ObservableCollection<QuestionItem>();

            string[] strings = ["It enhances security among users", "It minimizes interference and encourage collaboration"];
            QuestionItem questionItem = new QuestionItem(id: 25, date: DateTime.Now, question: "Why is it important to define teritoriality in social VR?", choices: strings, solution:1);

            QuestionItemsCatalogue.Add(questionItem);
        }

        private QuestionItem selectedQuestionItem;

        public QuestionItem SelectedQuestionItem
        {
            get { return selectedQuestionItem; }
            set { selectedQuestionItem = value; OnPropertyChanged(); }
        }

    }
}

