using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BrainWPF
{
    public class Game : INotifyPropertyChanged
    {
        private static List<Game> _games = new List<Game>();
        public static List<Game> Games { get { return _games; } }

        private Guid _idFirstTeam;
        public Team FirstTeam
        {
            get { return Team.Teams.Where(x => x.Id == _idFirstTeam).First(); }
            set 
            {
                _idFirstTeam = value.Id;
                NotifyPropertyChanged("FirstTeam");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string type)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(type));
            }
        }

        private Guid _idSecondTeam;
        public Team SecondTeam
        {
            get { return Team.Teams.Where(x => x.Id == _idSecondTeam).First(); }
            set { _idSecondTeam = value.Id; NotifyPropertyChanged("SecondTeam"); }
        }

        public Guid Id { get; set; }
        public bool IsOver = false;
        public int Step;
        public DateTime Date { get; set; }
        private int _countOfQuestions;
        public int CountOfQuestions 
        {
            get { return _countOfQuestions; }
            set { _countOfQuestions = value; NotifyPropertyChanged("CountOfQuestions"); }
        }

        private int _numberOfQuestion;
        public int NumberOfQuestion 
        {
            get { return _numberOfQuestion; }
            set { _numberOfQuestion = value; NotifyPropertyChanged("NumberOfQuestion"); }
        }

        private List<Question> _gameQuestions = new List<Question>();
        public List<Question> GameQuestions
        {
            get { return _gameQuestions; }
            set { _gameQuestions = value; NotifyPropertyChanged("GameQuestions"); }
        }

        public string Result
        { get { return FirstTeam.Name + "-" + SecondTeam.Name + "  " + FirstTeam.Point + ":" + SecondTeam.Point; } }

        public Game()
        {
            Id = Guid.NewGuid();
            _games.Add(this);
        }

        public Game(Team firstTeam, Team secondTeam)
        {
            FirstTeam = firstTeam;
            SecondTeam = secondTeam;
            Date = DateTime.Now;
            Id = Guid.NewGuid();
            _games.Add(this);
        }

        public Game(Team firstTeam, Team secondTeam, int numQuestion)
        {
            FirstTeam = firstTeam;
            SecondTeam = secondTeam;
            Date = DateTime.Now;
            CountOfQuestions = numQuestion;
            for (int i = 0; i < this.CountOfQuestions; i++)
            {
                var q = new Question((i + 1).ToString());
                this._gameQuestions.Add(q);
            }     
            Id = Guid.NewGuid();
            _games.Add(this);
        }

        public static void Save()
        {
            FileStream stream;

            try { stream = new FileStream("Game.xml", FileMode.Truncate); }
            catch (FileNotFoundException) { stream = new FileStream("Game.xml", FileMode.Create); }
            var serializer = new XmlSerializer(typeof(List<Game>));
            serializer.Serialize(stream, Games);
            stream.Close();
        }

        public static void Load()
        {
            try
            {
                var stream = new FileStream("Game.xml", FileMode.OpenOrCreate);
                var serializer = new XmlSerializer(typeof(List<Game>));
                List<Game> listVal = null;
                if (stream.Length != 0) { listVal = (List<Game>)serializer.Deserialize(stream); }
                stream.Close();
            }
            catch { }
        }

        public override string ToString()
        {
            if (IsOver == false)
            {
                return "... " + Result + " - " + Date.ToShortDateString();
            }
            else
                return Result + " - " + Date.ToShortDateString();
        }

        public static void AddLast(Game game)
        { _games.Add(game); }

        public static Game Clone(Game game)
        {
            FileStream stream;
            try { stream = new FileStream("GameTemp.xml", FileMode.Truncate); }
            catch (FileNotFoundException) { stream = new FileStream("GameTemp.xml", FileMode.Create); }
            var serializer = new XmlSerializer(typeof(Game));
            serializer.Serialize(stream, game);
            stream.Close();
            Game g = new Game();
            Game._games.Remove(g);
                stream = new FileStream("GameTemp.xml", FileMode.OpenOrCreate);
                serializer = new XmlSerializer(typeof(Game));
                if (stream.Length != 0) { g = (Game)serializer.Deserialize(stream); }
                stream.Close();
            
            return g;
        }
    }
}
