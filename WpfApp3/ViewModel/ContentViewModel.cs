using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApp3.Model;
using WpfApp3.MVVM;
using WpfApp3.View;


namespace WpfApp3.ViewModel
{
    internal class ContentViewModel:ViewModelBase
    {
        private const string Json = "Resources\\login.json";

        public ContentViewModel() {

            Auth auth = new(Json);

            ConnectorSQL = new ConnectAPI(auth);

            ConnectButtonLabel = "Connect";

            ConnectorSQL.connection.StateChange += Connection_StateChange;

        }

        private ConnectAPI ConnectorSQL;

        private EditWindow editWindow;

        private ObservableCollection<QuestionItem> originalItemsCatalogue;
        //private QuestionItem originalQuestionItem;

        #region Bound Properties
        private ObservableCollection<QuestionItem> questionItemsCatalogue;

        public ObservableCollection<QuestionItem> QuestionItemsCatalogue
        {
            get => questionItemsCatalogue;
            set { questionItemsCatalogue = value; OnPropertyChanged(); }
        }

        private int selectedChoiceIndex;

        public int SelectedChoiceIndex
        {
            get => selectedChoiceIndex;
            set { selectedChoiceIndex = value; OnPropertyChanged(); }
        }

        private QuestionItem selectedQuestionItem;

        public QuestionItem SelectedQuestionItem
        {
            get { return selectedQuestionItem; }
            set {
                selectedQuestionItem = value;
                OnPropertyChanged();
            }
        }
        private bool isItemChanged;

        public bool IsItemChanged
        {
            get { return isItemChanged; }
            set {
                isItemChanged = value; OnPropertyChanged(); }
        }

        private string selectedChoice;

        public string SelectedChoice
        {
            get { return selectedChoice; }
            set { selectedChoice = value; OnPropertyChanged(); }
        }

        private string connectButtonLabel;

        public string ConnectButtonLabel
        {
            get => connectButtonLabel;
            set { connectButtonLabel = value; OnPropertyChanged(); }
        }


        private string statusMessage;

        public string StatusMessage { 
            get => statusMessage;
            set { statusMessage = value; OnPropertyChanged(); }
        }

        #endregion


        private bool CheckQuestionItemChange()
        {

            if (selectedQuestionItem != null && selectedChoiceIndex >= 0)
            {
                QuestionItem questionItem = originalItemsCatalogue[selectedChoiceIndex];
                return !questionItem.Equals(selectedQuestionItem);
            }
            else 
                return false;
        }
        private void Connection_StateChange(object sender, StateChangeEventArgs e)
        {
            StatusMessage = ConnectorSQL.statusMessage;
        }


        #region Relay Commands

        public RelayCommand EstablishConnectionCommand => new RelayCommand(
            _execute => EstablishConnection(), 
            _canExecuteFunc=> ConnectorSQL!=null);
        private void EstablishConnection()
        {

            if (ConnectorSQL.isConnected)
            {
                ConnectorSQL.Close();
            }

            else
            {
                ConnectorSQL.Connect();
            }

            RefreshUIElements();
        }


        private void RefreshUIElements()
        {

            if (ConnectorSQL.isConnected)
            {
                ConnectButtonLabel = "Disconnect";
                QuestionItemsCatalogue = ConnectorSQL.QuestionsList;
                originalItemsCatalogue = new(QuestionItemsCatalogue);
            }

            else
            {
                ConnectButtonLabel = "Connect";
                QuestionItemsCatalogue.Clear();
                QuestionItemsCatalogue = null;
                originalItemsCatalogue.Clear();

            }
        }
        public RelayCommand AddChoiceCommand => new RelayCommand(_execute => AddChoice(), _canExecute => selectedQuestionItem != null);

        private void AddChoice()
        {
            selectedQuestionItem.Choices.Add("new choice");
        }
        
        public RelayCommand DeleteChoiceCommand => new RelayCommand(_execute => DeleteChoice(), _canExecute => selectedChoice != null);

        private void DeleteChoice()
        {
            selectedQuestionItem.Choices.Remove(selectedChoice);
        }

              
        
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

        public RelayCommand AddItemCommand => new RelayCommand(_execute => AddItem(), _canExecuteCommand => ConnectorSQL.isConnected);

        private void AddItem()
        {
            ObservableCollection<string> strings = ["new choice"];
            QuestionItem questionItem = new QuestionItem(id: QuestionItemsCatalogue.Count+1, 
                date: DateTime.Now, 
                question: "new question", 
                choices: strings, 
                solution: 0);

            QuestionItemsCatalogue.Add(questionItem);
            ConnectorSQL.UpdateList(questionItem);

        }

        public RelayCommand RemoveItemCommand => new RelayCommand(_execute => RemoveItem(),_canExecuteCommand => SelectedQuestionItem!= null);
        private void RemoveItem()
        {
            ConnectorSQL.RemoveItem(selectedQuestionItem);
            QuestionItemsCatalogue.Remove(selectedQuestionItem);
        }


        public RelayCommand SaveItemCommand => new RelayCommand( _execute => SaveItem(), _canExecute => SelectedQuestionItem != null);


        private void SaveItem()
        {
            SelectedQuestionItem.Date = DateTime.Now;
            ConnectorSQL.UpdateList(selectedQuestionItem);
        }

        #endregion
    }
}

