using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWPF
{
    public class Question : INotifyPropertyChanged
    {
        private static List<Question> _questions = new List<Question>();
        public static List<Question> Questions
        {
            get { return _questions; }
        }

        public int WhoFirstPress = 0;

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        private int _firstTeamStatus = 0;
        public int FirstTeamStatus
        {
            get { return _firstTeamStatus; }
            set { _firstTeamStatus = value; NotifyPropertyChanged("FirstTeamStatus"); }
        }

        private int _secondTeamStatus = 0;
        public int SecondTeamStatus
        {
            get { return _secondTeamStatus; }
            set { _secondTeamStatus = value; NotifyPropertyChanged("SecondTeamStatus"); }
        }

        private bool _isFinished = false;
        public bool IsFinished
        {
            get { return _isFinished; }
            set { _isFinished = value; NotifyPropertyChanged("IsFinished"); }
        }

        public Question()
        { }

        public Question(string name)
        {
            _name = name;
            _id = Guid.NewGuid();
            _questions.Add(this);
        }

        public override string ToString()
        {
            return Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string type)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(type));
            }
        }
    }
}
