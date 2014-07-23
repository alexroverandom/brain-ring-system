using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWPF
{
    public class Team : INotifyPropertyChanged
    {
        private static List<Team> _teams = new List<Team>();
        public static List<Team> Teams { get { return _teams; } }

        private string _name;
        public string Name 
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }

        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }

        private int _point;
        public int Point 
        {
            get { return _point; }
            set { _point = value; NotifyPropertyChanged("Point"); }
        }
        public Guid Id { get; set; }

        public Team()
        {
            Point = 0;
            Id = Guid.NewGuid();
            _teams.Add(this);
        }

        public Team(string name)
        {
            Name = name;
            Point = 0;
            Id = Guid.NewGuid();
            _teams.Add(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
