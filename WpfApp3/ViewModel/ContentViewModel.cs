using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApp3.Model;
using WpfApp3.MVVM;
using WpfApp3.View;


namespace WpfApp3.ViewModel
{
    internal class ContentViewModel:ViewModelBase
    {
        private const string Json = """D:\Repositories\WpfApp3\WpfApp3\Resources\login.json""";

        public ContentViewModel() {

            Auth auth = new(Json);

            ConnectorSQL = new ConnectAPI(auth);

            QuestionItemsCatalogue = new ObservableCollection<QuestionItem>();
            //EstablishConnection();
            //ObservableCollection<string> strings = ["It enhances security among users", "It minimizes interference and encourage collaboration"];
            //QuestionItem questionItem = new QuestionItem(id: 25, date: DateTime.Now, question: "Why is it important to define teritoriality in social VR?", choices: strings, solution:1);

            //QuestionItemsCatalogue.Add(questionItem);
        }

        ConnectAPI ConnectorSQL;

        EditWindow editWindow;

        private ObservableCollection<QuestionItem> questionItemsCatalogue;

        public ObservableCollection<QuestionItem> QuestionItemsCatalogue { 
            get=> questionItemsCatalogue; 
            set { questionItemsCatalogue = value; OnPropertyChanged(); } }

        private int selectedChoiceIndex;

        public int SelectedChoiceIndex { get => selectedChoiceIndex; set { selectedChoiceIndex = value; OnPropertyChanged(); } }

        private QuestionItem selectedQuestionItem;

        public QuestionItem SelectedQuestionItem
        {
            get { return selectedQuestionItem; }
            set { selectedQuestionItem = value; OnPropertyChanged(); }
        }

        private string selectedChoice;

        public string SelectedChoice
        {
            get { return selectedChoice; }
            set { selectedChoice = value; OnPropertyChanged(); }
        }

        public RelayCommand EstablishConnectionCommand => new RelayCommand(
            _execute => EstablishConnection(), 
            _canExecuteFunc=> ConnectorSQL!=null);
        private void EstablishConnection()
        {
            ConnectorSQL.Connect();
            QuestionItemsCatalogue = ConnectorSQL.QuestionsList;
        }

        public RelayCommand AddChoiceCommand => new RelayCommand(_execute => AddChoice());

        private void AddChoice()
        {
            selectedQuestionItem.Choices.Add("new choice");
        }

        public RelayCommand AddItemCommand => new RelayCommand(_execute => AddItem());
              
        
        private void EditChoice()
        {
            editWindow = new EditWindow(selectedChoice);
            editWindow.ShowDialog();

            if (editWindow.Success)
            {
                selectedQuestionItem.Choices[selectedChoiceIndex] = editWindow.InputChoice;
            }
        }
        
        public RelayCommand EditChoiceCommand => new RelayCommand(_execute => EditChoice(), _canExecuteCommand => selectedChoice != null);
      
        private void AddItem()
        {
            ObservableCollection<string> strings = ["It enhances security among users", "It minimizes interference and encourage collaboration"];
            QuestionItem questionItem = new QuestionItem(id: 26, date: DateTime.Now, question: "Why is it important to define teritoriality in social VR?", choices: strings, solution: 1);

            QuestionItemsCatalogue.Add(questionItem);
        }

        public RelayCommand RemoveItemCommand => new RelayCommand(_execute => RemoveItem(),_canExecuteCommand => SelectedQuestionItem!= null);
        private void RemoveItem()
        {
            QuestionItemsCatalogue.Remove(selectedQuestionItem);
        }

        private RelayCommand modifyItemCommand;
        public ICommand ModifyItemCommand => modifyItemCommand ??= new RelayCommand(SaveItem);

        private void SaveItem(object commandParameter)
        {
            // send to database
        }

  
    }
}

