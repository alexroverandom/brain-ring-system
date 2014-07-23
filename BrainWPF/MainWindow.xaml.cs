using RawInput_dll;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BrainWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Game> _gameSteps = new List<Game>();
        Timer MainTimer = new Timer();
        Timer HelpTimer = new Timer();
        public int PointsFirstTeam;
        public int PointsSecondTeam;
        int _afterStart = 0;
        int _meterSeconds = 60;
        int _numberOfQuestion = 0;
        int CountQuestions;
        bool _isFallStart = true;
        DateTime _timeTick;
        DateTime _timeStop;
        Timer TimerGlob;
        Random _r = new Random();
        bool _isNoWinner = false;

        string _firstTeamKeyboardType;
        string _secondTeamKeyboardType;
        /*RawStuff.InputDevice id;
        int NumberOfKeyboards;
        Message message = new Message();*/
        private RawInput rawinput;
        private List<string> _kb = new List<string>();
        System.Media.SoundPlayer musicStart = new System.Media.SoundPlayer(Properties.Resources.Start);
        System.Media.SoundPlayer musicRing = new System.Media.SoundPlayer(Properties.Resources.Ring);
        System.Media.SoundPlayer musicBleep = new System.Media.SoundPlayer(Properties.Resources.Bleep);
        System.Media.SoundPlayer musicAnswer = new System.Media.SoundPlayer(Properties.Resources.Answer);
        System.Media.SoundPlayer musicFallStart = new System.Media.SoundPlayer(Properties.Resources.FallStart);

        Game game;
        bool _continue = false;
        bool _AddingKeyboards = true;
        bool _isTest = false;
        bool _isFirstPress = true;
        int _stage
        { get { return _gameSteps.Count; } }

        public MainWindow()
        {
            InitializeComponent();
            try
            { Game.Load(); }
            catch { }
            lbGames.ItemsSource = Game.Games;

            
        }
        #region RawStaff

        /*private void _KeyPressed(object sender, RawStuff.InputDevice.KeyControlEventArgs e)
        {
            string[] tokens = e.Keyboard.Name.Split(';');
            string token = tokens[1];
            lblFKey.Visibility = System.Windows.Visibility.Visible;
            lblFKey.Content = e.Keyboard.deviceHandle.ToString();
        }

        void StartWndProcHandler()
        {
            IntPtr hwnd = IntPtr.Zero;
            Window myWin = System.Windows.Application.Current.MainWindow;

            try
            {
                hwnd = new WindowInteropHelper(this).Handle;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //Get the Hwnd source   
            HwndSource source = HwndSource.FromHwnd(hwnd);
            //Win32 queue sink
            source.AddHook(new HwndSourceHook(WndProc));

            id = new RawStuff.InputDevice(source.Handle);
            NumberOfKeyboards = id.EnumerateDevices();
            id.KeyPressed += new RawStuff.InputDevice.DeviceEventHandler(_KeyPressed);
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (id != null)
            {
                // I could have done one of two things here.
                // 1. Use a Message as it was used before.
                // 2. Changes the ProcessMessage method to handle all of these parameters(more work).
                //    I opted for the easy way.

                //Note: Depending on your application you may or may not want to set the handled param.

                message.HWnd = hwnd;
                message.Msg = msg;
                message.LParam = lParam;
                message.WParam = wParam;

                id.ProcessMessage(message);
            }
            return IntPtr.Zero;
        }*/
#endregion

        private void CreateNewGame()
        {
            game = new Game(new Team(tbFirstTeamName.Text), new Team(tbSecondTeamName.Text), Convert.ToInt32(Sld.Value));
            this.DataContext = game;
            dgQuestions.DataContext = game.GameQuestions;
            _firstTeamKeyboardType = lblFKey.Content.ToString();
            _secondTeamKeyboardType = lblSKey.Content.ToString();
            CountQuestions = game.CountOfQuestions;
        }

        #region LabelVisualizations
        private void lblNew_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblNew.Foreground = Brushes.Yellow;
            lblNew.BorderThickness = new Thickness(2.0);
        }

        private void lblNew_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblNew.Foreground = Brushes.White;
            lblNew.BorderThickness = new Thickness(0);
        }

        private void lblStopAndSave_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblStopAndSave.Foreground = Brushes.Yellow;
            lblStopAndSave.BorderThickness = new Thickness(2.0);
        }

        private void lblStopAndSave_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblStopAndSave.Foreground = Brushes.White;
            lblStopAndSave.BorderThickness = new Thickness(0);
        }

        private void lblTestSystem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblTestSystem.Foreground = Brushes.Yellow;
            lblTestSystem.BorderThickness = new Thickness(2.0);
        }

        private void lblTestSystem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblTestSystem.Foreground = Brushes.White;
            lblTestSystem.BorderThickness = new Thickness(0);
        }

        private void lblClearGames_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblClearGames.Foreground = Brushes.Yellow;
            lblClearGames.BorderThickness = new Thickness(2.0);
        }

        private void lblClearGames_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblClearGames.Foreground = Brushes.White;
            lblClearGames.BorderThickness = new Thickness(0);
        }

        private void lblStart_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblStart.Foreground = Brushes.Yellow;
            lblStart.BorderThickness = new Thickness(2.0);
        }

        private void lblStart_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblStart.Foreground = Brushes.White;
            lblStart.BorderThickness = new Thickness(0);
        }

        private void lblAddKeyboards_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblAddKeyboards.Foreground = Brushes.Yellow;
            lblAddKeyboards.BorderThickness = new Thickness(2.0);
        }

        private void lblAddKeyboards_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblAddKeyboards.Foreground = Brushes.White;
            lblAddKeyboards.BorderThickness = new Thickness(0);
        }

        private void lblNextQuestion_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblNextQuestion.Foreground = Brushes.Yellow;
            lblNextQuestion.BorderThickness = new Thickness(2.0);
        }

        private void lblNextQuestion_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            lblNextQuestion.Foreground = Brushes.White;
            lblNextQuestion.BorderThickness = new Thickness(0);
        }
        #endregion

        private void lblNew_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _continue = false;
            lblNew.IsEnabled = false;
            lblStart.IsEnabled = true;
            tbFirstTeamName.Visibility = System.Windows.Visibility.Visible;
            tbSecondTeamName.Visibility = System.Windows.Visibility.Visible;
            lblFKey.Visibility = System.Windows.Visibility.Visible;
            lblSKey.Visibility = System.Windows.Visibility.Visible;
            lbKeyboards.Visibility = System.Windows.Visibility.Visible;
            btnFirstKeyboard.Visibility = System.Windows.Visibility.Visible;
            btnSecondKeyboard.Visibility = System.Windows.Visibility.Visible;
            Sld.Visibility = System.Windows.Visibility.Visible;
            l1.Visibility = l5.Visibility = l8.Visibility = l11.Visibility = System.Windows.Visibility.Visible;
        }

        private void lblStopAndSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
                if (System.Windows.MessageBox.Show("Is this game over?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    game.IsOver = true;
            game.NumberOfQuestion = _numberOfQuestion;
            try { TimerGlob.Stop(); }
            catch { }
            ResultsRefresh();
            game.Step = _afterStart;
            Game.Save();
            game = null;
            this.DataContext = game;
            lblStopAndSave.IsEnabled = false;
            lblNew.IsEnabled = true;
            btnStart.Visibility = System.Windows.Visibility.Hidden;
            btnStart.IsEnabled = true;
            lblTestSystem.IsEnabled = false;
            lbGamesRefresh();
            lblNextQuestion.IsEnabled = false;
            btnBackStep.Visibility = System.Windows.Visibility.Hidden;
        }

        private void lblTestSystem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_isTest)
            {
                lblTestSystem.Background = Brushes.Orange;
                _isTest = true;
                btnBackStep.IsEnabled = false;
            }
            else
            {
                _isTest = false;
                lblTestSystem.Background = lblFirstPoint.Background;
                ResultsRefresh();
                btnBackStep.IsEnabled = true;
            }
        }

        private void lblNextQuestion_MouseDown(object sender, MouseButtonEventArgs e)
        {
            game.GameQuestions.Add(new Question((game.GameQuestions.Count + 1).ToString()));
            game.CountOfQuestions += 1;
            btnStart.IsEnabled = true;
            lblStopAndSave.IsEnabled = false;
            lblNextQuestion.IsEnabled = false;
            dgQuestions.DataContext = null;
            dgQuestions.DataContext = game.GameQuestions;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Game.Save();
        }

        private void lblClearGames_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lbGames.SelectedItems.Count == 0)
            { System.Windows.MessageBox.Show("No items to remove!", "Question", MessageBoxButton.OK); }
            else
            {
                if (System.Windows.MessageBox.Show("Do you realy want to delete selected games?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    foreach (Game x in lbGames.SelectedItems)
                        Game.Games.Remove(x);
                    if (game != null)
                    {
                        if (Game.Games.Count == 0 || Game.Games.Where(x => x.Id == game.Id).Count() == 0)
                            Game.AddLast(game);
                    }
                    lbGamesRefresh();
                }
            }
        }

        private void lbGamesRefresh()
        {
            lbGames.ItemsSource = null;
            lbGames.ItemsSource = Game.Games;
        }

        
        private void lblStart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(_firstTeamKeyboardType) || string.IsNullOrEmpty(_secondTeamKeyboardType))
            { System.Windows.MessageBox.Show("Select keyboards, please", "Error", MessageBoxButton.OK); }
            else
            {
                if (_continue)
                {
                    game = (Game)lbGames.SelectedItem;
                    this.DataContext = null;
                    this.DataContext = game;
                    dgQuestions.DataContext = game.GameQuestions;
                    _numberOfQuestion = game.NumberOfQuestion;
                    CountQuestions = game.CountOfQuestions;
                    btnStart.IsEnabled = true;
                    btnStart.Visibility = System.Windows.Visibility.Visible;
                    lblFKey.Visibility = System.Windows.Visibility.Hidden;
                    lblSKey.Visibility = System.Windows.Visibility.Hidden;
                    lbKeyboards.Visibility = System.Windows.Visibility.Hidden;
                    btnFirstKeyboard.Visibility = System.Windows.Visibility.Hidden;
                    btnSecondKeyboard.Visibility = System.Windows.Visibility.Hidden;
                    _meterSeconds = 60;
                    ResultsRefresh();
                    _isFallStart = true;
                    _isFirstPress = true;
                    _afterStart = 0;
                    lblTestSystem.IsEnabled = true;
                    btnBackStep.Visibility = System.Windows.Visibility.Visible;
                    btnBackStep.IsEnabled = true;
                    lblStopAndSave.IsEnabled = true;
                    lblStart.IsEnabled = false;
                }
                else
                {
                    CreateNewGame();
                    _afterStart = 0;
                    _isFallStart = true;
                    _isNoWinner = false;
                    _meterSeconds = 60;
                    _isTest = false;
                    _numberOfQuestion = 0;
                    lblStart.IsEnabled = false;
                    lblStopAndSave.IsEnabled = true;
                    lblTestSystem.IsEnabled = true;
                    tbFirstTeamName.Visibility = System.Windows.Visibility.Hidden;
                    tbSecondTeamName.Visibility = System.Windows.Visibility.Hidden;
                    lblFKey.Visibility = System.Windows.Visibility.Hidden;
                    lblSKey.Visibility = System.Windows.Visibility.Hidden;
                    lbKeyboards.Visibility = System.Windows.Visibility.Hidden;
                    btnFirstKeyboard.Visibility = System.Windows.Visibility.Hidden;
                    btnSecondKeyboard.Visibility = System.Windows.Visibility.Hidden;
                    Sld.Visibility = System.Windows.Visibility.Hidden;
                    l1.Visibility = l5.Visibility = l8.Visibility = l11.Visibility = System.Windows.Visibility.Hidden;
                    lbGamesRefresh();
                    btnStart.Visibility = System.Windows.Visibility.Visible;
                    btnBackStep.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void lblAddKeyboards_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_AddingKeyboards)
            {
                lblAddKeyboards.Content = "OK";
                //StartWndProcHandler();

                IntPtr Handle = new WindowInteropHelper(this).Handle;
                rawinput = new RawInput(Handle);
                rawinput.CaptureOnlyIfTopMostWindow = true;    // Otherwise default behavior is to capture always
                rawinput.AddMessageFilter();                   // Adding a message filter will cause keypresses to be handled

                Win32.DeviceAudit();
                _AddingKeyboards = false;
                rawinput.KeyPressed += OnKeyPressed;
            }
            else
            {
                rawinput.KeyPressed -= OnKeyPressed;
                lblAddKeyboards.Visibility = System.Windows.Visibility.Hidden;
                lblAddKeyboards.Content = "Add keyboards";
                lbKeyboardsRefresh();
                _AddingKeyboards = true;
                lbKeyboards.Visibility = System.Windows.Visibility.Hidden;
                lblNew.IsEnabled = true;
                lbGames.IsEnabled = true;
            }
        }

        private void OnKeyPressed(object sender, InputEventArg e)
        {
            if (_AddingKeyboards)
            {
                rawinput.KeyPressed -= OnKeyPressed;
                if (!_isTest)
                {
                    switch (_afterStart)
                    {
                        case 0:
                            if (e.KeyPressEvent.DeviceHandle.ToString() == _firstTeamKeyboardType)
                            {
                                if (_isFallStart)
                                {
                                    MainTimer.Stop();
                                    musicFallStart.Play();
                                    _afterStart = 1;
                                    game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress = 1;
                                    _isFirstPress = false;
                                    lblFirstTeamFallStart();
                                    game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = -1;
                                    btnStart.IsEnabled = true;
                                    chb60seconds.Visibility = System.Windows.Visibility.Visible;
                                    try { TimerGlob.Stop(); }
                                    catch { }
                                }
                                else
                                {
                                    _afterStart = 3;
                                    _timeStop = DateTime.Now;
                                    musicStart.Stop();
                                    musicAnswer.Play();
                                    game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress = 1;
                                    lblFirstTeamPress.Content = (_timeStop - _timeTick);//Seconds.ToString() + "," + (_timeStop - _timeTick).Milliseconds.ToString();
                                    TimerGlob.Stop();
                                    _isFirstPress = true;
                                    lblFirstTeamPress.Background = Brushes.Green;
                                    btnTrue.Visibility = System.Windows.Visibility.Visible;
                                    btnFalse.Visibility = System.Windows.Visibility.Visible;
                                    //btnTrue.Visibility = System.Windows.Visibility.Visible;
                                    //btnFalse.Visibility = System.Windows.Visibility.Visible;
                                }
                            }
                            else if (e.KeyPressEvent.DeviceHandle.ToString() == _secondTeamKeyboardType)
                            {
                                if (_isFallStart)
                                {
                                    MainTimer.Stop();
                                    musicFallStart.Play();
                                    _afterStart = 2;
                                    game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress = 2;
                                    _isFirstPress = false;
                                    lblSecondTeamFallStart();
                                    game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = -1;
                                    btnStart.IsEnabled = true;
                                    chb60seconds.Visibility = System.Windows.Visibility.Visible;
                                    try { TimerGlob.Stop(); }
                                    catch { }
                                }
                                else
                                {
                                    _afterStart = 4;
                                    _timeStop = DateTime.Now;
                                    musicStart.Stop();
                                    musicAnswer.Play();
                                    game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress = 2;
                                    lblSecondTeamPress.Content = (_timeStop - _timeTick);//.Seconds.ToString() + "," + (_timeStop - _timeTick).Milliseconds.ToString();
                                    TimerGlob.Stop();
                                    _isFirstPress = true;
                                    lblSecondTeamPress.Background = Brushes.Green;
                                    btnTrue.Visibility = System.Windows.Visibility.Visible;
                                    btnFalse.Visibility = System.Windows.Visibility.Visible;
                                    //btnFalse.Visibility = System.Windows.Visibility.Visible;
                                    //btnTrue.Visibility = System.Windows.Visibility.Visible;
                                }
                            }
                            else { rawinput.KeyPressed += OnKeyPressed; }
                            break;
                        case 1:
                            if (e.KeyPressEvent.DeviceHandle.ToString() == _secondTeamKeyboardType)
                                PressSecondTeamAfretOpponent();
                            else
                                rawinput.KeyPressed += OnKeyPressed;
                            break;
                        case 2:
                            if (e.KeyPressEvent.DeviceHandle.ToString() == _firstTeamKeyboardType)
                                PressFirstTeamAfretOpponent();
                            else
                                rawinput.KeyPressed += OnKeyPressed;
                            break;
                        case 3:
                            if (e.KeyPressEvent.DeviceHandle.ToString() == _secondTeamKeyboardType)
                                PressSecondTeamAfretOpponent();
                            else
                                rawinput.KeyPressed += OnKeyPressed;
                            break;
                        case 4:
                            if (e.KeyPressEvent.DeviceHandle.ToString() == _firstTeamKeyboardType)
                                PressFirstTeamAfretOpponent();
                            else
                                rawinput.KeyPressed += OnKeyPressed;
                            break;
                    }
                }
                else
                {
                    if (e.KeyPressEvent.DeviceHandle.ToString() == _firstTeamKeyboardType)
                    {
                        lblTestSystem.IsEnabled = true;
                        if (_isFallStart)
                        {
                            MainTimer.Stop();
                            musicFallStart.Play();
                            lblFirstTeamFallStart();
                            btnStart.IsEnabled = true;
                        }
                        else
                        {
                            _timeStop = DateTime.Now;
                            musicStart.Stop();
                            musicAnswer.Play();
                            lblFirstTeamPress.Content = (_timeStop - _timeTick);//.Seconds.ToString() + "," + (_timeStop - _timeTick).Milliseconds.ToString();
                            lblFirstTeamPress.Background = Brushes.Green;
                            btnStart.IsEnabled = true;
                        }
                    }
                    else if (e.KeyPressEvent.DeviceHandle.ToString() == _secondTeamKeyboardType)
                    {
                        lblTestSystem.IsEnabled = true;
                        if (_isFallStart)
                        {
                            MainTimer.Stop();
                            musicFallStart.Play();
                            lblSecondTeamFallStart();
                            btnStart.IsEnabled = true;
                        }
                        else
                        {
                            _timeStop = DateTime.Now;
                            musicStart.Stop();
                            musicAnswer.Play();
                            lblSecondTeamPress.Content = (_timeStop - _timeTick);//.Seconds.ToString() + "," + (_timeStop - _timeTick).Milliseconds.ToString();
                            lblSecondTeamPress.Background = Brushes.Green;
                            btnStart.IsEnabled = true;
                        }
                    }
                    else { rawinput.KeyPressed += OnKeyPressed; }
                }
            }
            else
            {
                if (IsNewKeyboard(e.KeyPressEvent.DeviceHandle.ToString()))
                {
                    _kb.Add(e.KeyPressEvent.DeviceHandle.ToString());
                    lbKeyboardsRefresh();
                }
            }
        }

        private void PressSecondTeamAfretOpponent()
        {
            TimerGlob.Stop();
            _timeStop = DateTime.Now;
            musicStart.Stop();
            musicAnswer.Play();
            _afterStart = 4;
            _isFirstPress = false;
            lblSecondTeamPress.Content = (_timeStop - _timeTick);//.Seconds.ToString() + "," + (_timeStop - _timeTick).Milliseconds.ToString();
            lblSecondTeamPress.Background = Brushes.Green;
            btnTrue.Visibility = System.Windows.Visibility.Visible;
            btnFalse.Visibility = System.Windows.Visibility.Visible;
            //btnStart.IsEnabled = false;
        }

        private void PressFirstTeamAfretOpponent()
        {
            TimerGlob.Stop();
            _timeStop = DateTime.Now;
            musicStart.Stop();
            musicAnswer.Play();
            _afterStart = 3;
            _isFirstPress = false;
            lblFirstTeamPress.Content = (_timeStop - _timeTick);//.Seconds.ToString() + "," + (_timeStop - _timeTick).Milliseconds.ToString();
            lblFirstTeamPress.Background = Brushes.Green;
            btnTrue.Visibility = System.Windows.Visibility.Visible;
            btnFalse.Visibility = System.Windows.Visibility.Visible;
            btnStart.IsEnabled = false;
        }

        private void lblFirstTeamFallStart()
        {
            lblFirstTeamPress.Content = "Фальстарт!";
            lblFirstTeamPress.Background = Brushes.Orange;
        }

        private void lblSecondTeamFallStart()
        {
            lblSecondTeamPress.Content = "Фальстарт!";
            lblSecondTeamPress.Background = Brushes.Orange;
        }

        private void lbKeyboardsRefresh()
        {
            lbKeyboards.ItemsSource = null;
            lbKeyboards.ItemsSource = _kb;
        }

        private void btnFirstKeyboard_Click(object sender, RoutedEventArgs e)
        {
            if (lbKeyboards.SelectedItem != null)
            {
                _firstTeamKeyboardType = lbKeyboards.SelectedItem as string;
                lblFKey.Content = lbKeyboards.SelectedItem as string;
                _kb.Remove(lbKeyboards.SelectedItem as string);
                lbKeyboardsRefresh();
                btnFirstKeyboard.IsEnabled = false;
            }
        }

        private void btnSecondKeyboard_Click(object sender, RoutedEventArgs e)
        {
            if (lbKeyboards.SelectedItem != null)
            {
                _secondTeamKeyboardType = lbKeyboards.SelectedItem as string;
                lblSKey.Content = lbKeyboards.SelectedItem as string;
                _kb.Remove(lbKeyboards.SelectedItem as string);
                lbKeyboardsRefresh();
                btnSecondKeyboard.IsEnabled = false;
            }
        }

        private void ResultsRefresh()
        {
            lblFirstTeamPress.Content = string.Empty;
            lblSecondTeamPress.Content = string.Empty;
            lblFirstTeamPress.Background = Brushes.White;
            lblSecondTeamPress.Background = Brushes.White;
        }

        public void OnTick(object sender, EventArgs e)
        {
            if (!_isTest)
            {
                musicStart.Play();
                _timeTick = DateTime.Now;
                _isFallStart = false;
                MainTimer.Stop();
                TimerGlob = new Timer();
                TimerGlob.Tick += new EventHandler(OnGlobTick);
                TimerGlob.Interval = 1000;
                _meterSeconds = 60;
                TimerGlob.Start();
            }
            else
            {
                musicStart.Play();
                _timeTick = DateTime.Now;
                _isFallStart = false;
                MainTimer.Stop();
            }
        }

        public void OnTickHelp(object sender, EventArgs e)
        {
            rawinput.KeyPressed += OnKeyPressed;
            HelpTimer.Stop();
            btnBackStep.IsEnabled = true;
        }

        public void OnGlobTick(object sender, EventArgs e)
        {
            _meterSeconds -= 1;
            if (_meterSeconds > 9)
            {
                lblTimer.Content = "00:" + _meterSeconds.ToString();
                if (_meterSeconds == 10)
                {
                    musicBleep.Play();
                }
            }
            else
            {
                lblTimer.Content = "00:0" + _meterSeconds.ToString();
                if (_meterSeconds != 0 && _meterSeconds < 6)
                    musicBleep.Play();
            }
            if (_meterSeconds == 0)
            {
                rawinput.KeyPressed -= OnKeyPressed;
                game.GameQuestions.ElementAt(_numberOfQuestion).IsFinished = true;
                TimerGlob.Stop();
                musicRing.Play();
                ResultsRefresh();
                btnStart.IsEnabled = true;
                _isFallStart = true;
                _afterStart = 0;
                _numberOfQuestion += 1;
                _meterSeconds = 60;
                lblTestSystem.IsEnabled = true;
                btnBackStep.IsEnabled = true;
                if ((CountQuestions == _numberOfQuestion && PointsFirstTeam == PointsSecondTeam) || _isNoWinner)
                {
                    _isNoWinner = true;
                    lblNextQuestion.IsEnabled = true;
                    lblStopAndSave.IsEnabled = true;
                    btnStart.IsEnabled = false;
                }
                else
                    if (CountQuestions == _numberOfQuestion)
                        lblStopAndSave.IsEnabled = true;
                    else
                        btnStart.IsEnabled = true;
                if (Math.Abs(PointsFirstTeam - PointsSecondTeam) > (CountQuestions - _numberOfQuestion))
                { lblStopAndSave.IsEnabled = true; }
            }
        }

        private void btnTrue_Click(object sender, RoutedEventArgs e)
        {
            switch (_afterStart)
            {
                case 3:
                    game.FirstTeam.Point += 1;
                    game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 2;
                    game.GameQuestions.ElementAt(_numberOfQuestion).IsFinished = true;
                    //AddImageFirstTeam(Images.w128h1281372334742check);
                    break;
                case 4:
                    game.SecondTeam.Point += 1;
                    game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 2;
                    game.GameQuestions.ElementAt(_numberOfQuestion).IsFinished = true;
                    //AddImageSecondTeam(Images.w128h1281372334742check);
                    break;
            }
            _isFirstPress = true;
            btnFalse.Visibility = System.Windows.Visibility.Hidden;
            btnTrue.Visibility = System.Windows.Visibility.Hidden;
            btnBackStep.IsEnabled = true;
            ResultsRefresh();
            _isFallStart = true;
            _afterStart = 0;
            _numberOfQuestion += 1;
            _meterSeconds = 60;
            lbGamesRefresh();
            lblTestSystem.IsEnabled = true;
            try { TimerGlob.Stop(); }
            catch { }
            if (_isNoWinner)
            {
                lblNextQuestion.IsEnabled = true;
                lblStopAndSave.IsEnabled = true;
                btnStart.IsEnabled = false;
            }
            else
            {
                if ((CountQuestions == _numberOfQuestion && PointsFirstTeam == PointsSecondTeam) || _isNoWinner)
                {
                    _isNoWinner = true;
                    lblNextQuestion.IsEnabled = true;
                    lblStopAndSave.IsEnabled = true;
                    btnStart.IsEnabled = false;
                }
                else
                    if (CountQuestions == _numberOfQuestion)
                        lblStopAndSave.IsEnabled = true;
                    else
                        btnStart.IsEnabled = true;
            }
            if (Math.Abs(PointsFirstTeam - PointsSecondTeam) > (CountQuestions - _numberOfQuestion))
            {
                lblStopAndSave.IsEnabled = true;
            }
        }

        private void TimeForOtherTeam(int time)
        {
            rawinput.KeyPressed += OnKeyPressed;
            musicStart.Play();
            _timeTick = DateTime.Now;
            _isFallStart = false;
            TimerGlob = new Timer();
            TimerGlob.Tick += new EventHandler(OnGlobTick);
            TimerGlob.Interval = 1000;
            _meterSeconds = time;
            TimerGlob.Start();
            btnStart.IsEnabled = false;
            btnTrue.Visibility = System.Windows.Visibility.Hidden;
            btnFalse.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnFalse_Click(object sender, RoutedEventArgs e)
        {
            if (_isFirstPress)
            {
                switch (_afterStart)
                {
                    //case 1:
                    //    TimeForOtherTeam(20);
                    //    //btn20Seconds.Enabled = false;
                    //    break;
                    //case 2:
                    //    TimeForOtherTeam(20);
                    //    //btn20Seconds.Enabled = false;
                    //    break;
                    case 3:
                        game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 1;
                        TimeForOtherTeam(20);
                        lblFirstTeamPress.Content = string.Empty;
                        lblFirstTeamPress.Background = Brushes.Red;
                        //AddImageFirstTeam(Images._11);
                        //btn20Seconds.Enabled = false;
                        break;
                    case 4:
                        game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 1;
                        TimeForOtherTeam(20);
                        lblSecondTeamPress.Content = string.Empty;
                        lblSecondTeamPress.Background = Brushes.Red;
                        //AddImageSecondTeam(Images._11);
                        //btn20Seconds.Enabled = false;
                        break;
                }
            }
            else
            {
                switch (_afterStart)
                {
                    case 3:
                        game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 1;
                        game.GameQuestions.ElementAt(_numberOfQuestion).IsFinished = true;
                        //AddImageFirstTeam(Images._11);
                        break;
                    case 4:
                        game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 1;
                        game.GameQuestions.ElementAt(_numberOfQuestion).IsFinished = true;
                        //AddImageSecondTeam(Images._11);
                        break;
                }
                btnFalse.Visibility = System.Windows.Visibility.Hidden;
                btnTrue.Visibility = System.Windows.Visibility.Hidden;
                ResultsRefresh();
                btnBackStep.IsEnabled = true;
                _isFallStart = true;
                _isFirstPress = true;
                _afterStart = 0;
                _numberOfQuestion += 1;
                _meterSeconds = 60;
                lblTestSystem.IsEnabled = true;
                try { TimerGlob.Stop(); }
                catch { }
                if ((CountQuestions == _numberOfQuestion && PointsFirstTeam == PointsSecondTeam) || _isNoWinner)
                {
                    _isNoWinner = true;
                    lblStopAndSave.IsEnabled = true;
                    lblNextQuestion.IsEnabled = true;
                    btnStart.IsEnabled = false;
                }
                else
                    if (CountQuestions == _numberOfQuestion)
                        lblStopAndSave.IsEnabled = true;
                    else
                        btnStart.IsEnabled = true;
                if (Math.Abs(PointsFirstTeam - PointsSecondTeam) > (CountQuestions - _numberOfQuestion))
                    lblStopAndSave.IsEnabled = true; 
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try { TimerGlob.Stop(); }
            catch { }
            if (!_isTest)
            {
                lblTestSystem.IsEnabled = false;
                switch (_afterStart)
                {
                    case 0:
                        game.NumberOfQuestion = _numberOfQuestion + 1;
                        if (_numberOfQuestion > game.GameQuestions.Count)
                        { }
                        else
                        {
                            btnBackStep.IsEnabled = false;
                            MainTimer = new Timer();
                            MainTimer.Tick += new EventHandler(OnTick);
                            MainTimer.Interval = Convert.ToInt32(1000 + 3000 * _r.NextDouble());
                            HelpTimer = new Timer();
                            HelpTimer.Tick += new EventHandler(OnTickHelp);
                            HelpTimer.Interval = 800;
                            MainTimer.Start();
                            HelpTimer.Start();
                            btnStart.IsEnabled = false;
                        }
                        break;
                    case 1:
                        chb60seconds.Visibility = System.Windows.Visibility.Hidden;
                        if (chb60seconds.IsChecked == true)
                            TimeForOtherTeam(60);
                        else
                            TimeForOtherTeam(20);
                        btnFalse.Visibility = System.Windows.Visibility.Hidden;
                        btnTrue.Visibility = System.Windows.Visibility.Hidden;
                        btnStart.IsEnabled = false;
                        break;
                    case 2:
                        chb60seconds.Visibility = System.Windows.Visibility.Hidden;
                        if (chb60seconds.IsChecked == true)
                            TimeForOtherTeam(60);
                        else
                            TimeForOtherTeam(20);
                        btnFalse.Visibility = System.Windows.Visibility.Hidden;
                        btnTrue.Visibility = System.Windows.Visibility.Hidden;
                        btnStart.IsEnabled = false;
                        break;
                    case 3:
                        TimeForOtherTeam(20);
                        lblFirstTeamPress.Background = Brushes.Red;
                        break;
                    case 4:
                        TimeForOtherTeam(20);
                        lblSecondTeamPress.Background = Brushes.Red;
                        break;
                }
            }
            else
            {
                lblTestSystem.IsEnabled = false;
                _isFallStart = true;
                ResultsRefresh();
                MainTimer = new Timer();
                MainTimer.Tick += new EventHandler(OnTick);
                MainTimer.Interval = Convert.ToInt32(1000 + 3000 * _r.NextDouble());
                HelpTimer = new Timer();
                HelpTimer.Tick += new EventHandler(OnTickHelp);
                HelpTimer.Interval = 800;
                MainTimer.Start();
                HelpTimer.Start();
                btnStart.IsEnabled = false;
            }
        }

        private void ActiveQuestionChange()
        {
            
            
        }

        private void btnBackStep_Click(object sender, RoutedEventArgs e)
        {
            switch (_afterStart)
            {
                case 0:
                    if (!btnStart.IsEnabled)
                    {
                        rawinput.KeyPressed -= OnKeyPressed;
                        TimerGlob.Stop();
                        _meterSeconds = 60;
                        lblTestSystem.IsEnabled = false;
                        btnStart.IsEnabled = true;
                        game.NumberOfQuestion -= 1; 
                    }
                    else 
                    {
                        if (game.GameQuestions.Where(x => x.IsFinished == true).Count() == 0)
                        { }
                        else
                        {
                            _numberOfQuestion -= 1;
                            game.GameQuestions.ElementAt(_numberOfQuestion).IsFinished = false;
                            if (game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress == 0)
                            {
                                btnStart.IsEnabled = true;
                            }
                            else
                            {
                                if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus != 0 || game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus != 0)
                                {
                                    switch (game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress)
                                    {
                                        case 1:
                                            if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 2)
                                            {
                                                game.FirstTeam.Point -= 1;
                                                game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                                                _afterStart = 3;
                                                _isFirstPress = true;
                                                lblFirstTeamPress.Background = Brushes.Green;
                                                btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                btnStart.IsEnabled = false;
                                            }
                                            else
                                                if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 1)
                                                {
                                                    if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 2)
                                                    {
                                                        game.SecondTeam.Point -= 1;
                                                        game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                                                        _afterStart = 4;
                                                        _isFirstPress = false;
                                                        lblSecondTeamPress.Background = Brushes.Green;
                                                        lblFirstTeamPress.Background = Brushes.Red;
                                                        btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                        btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                        btnStart.IsEnabled = false;
                                                    }
                                                    else
                                                        if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 1)
                                                        {
                                                            game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                                                            _afterStart = 4;
                                                            _isFirstPress = false;
                                                            lblSecondTeamPress.Background = Brushes.Green;
                                                            lblFirstTeamPress.Background = Brushes.Red;
                                                            btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                            btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                            btnStart.IsEnabled = false;
                                                        }
                                                        else
                                                        {
                                                            _afterStart = 3;
                                                            _isFirstPress = true;
                                                            lblFirstTeamPress.Background = Brushes.Red;
                                                            btnStart.IsEnabled = true;
                                                        }
                                                }
                                                else
                                                {
                                                    if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 2)
                                                    {
                                                        game.SecondTeam.Point -= 1;
                                                        game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                                                        _afterStart = 4;
                                                        _isFirstPress = false;
                                                        lblSecondTeamPress.Background = Brushes.Green;
                                                        lblFirstTeamPress.Background = Brushes.Orange;
                                                        btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                        btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                        btnStart.IsEnabled = false;
                                                    }
                                                    else
                                                        if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 1)
                                                        {
                                                            game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                                                            _afterStart = 4;
                                                            _isFirstPress = false;
                                                            lblSecondTeamPress.Background = Brushes.Green;
                                                            lblFirstTeamPress.Background = Brushes.Orange;
                                                            btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                            btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                            btnStart.IsEnabled = false;
                                                        }
                                                        else
                                                        {
                                                            _afterStart = 1;
                                                            _isFirstPress = true;
                                                            lblFirstTeamPress.Background = Brushes.Orange;
                                                            btnStart.IsEnabled = true;
                                                        }
                                                }
                                            break;
                                        case 2:
                                            if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 2)
                                            {
                                                game.SecondTeam.Point -= 1;
                                                game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                                                _afterStart = 4;
                                                _isFirstPress = true;
                                                lblSecondTeamPress.Background = Brushes.Green;
                                                btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                btnFalse.Visibility = System.Windows.Visibility.Visible;
                                            }
                                            else
                                                if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 1)
                                                {
                                                    if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 2)
                                                    {
                                                        game.FirstTeam.Point -= 1;
                                                        game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                                                        _afterStart = 3;
                                                        _isFirstPress = false;
                                                        lblFirstTeamPress.Background = Brushes.Green;
                                                        lblSecondTeamPress.Background = Brushes.Red;
                                                        btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                        btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                        btnStart.IsEnabled = false;
                                                    }
                                                    else
                                                        if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 1)
                                                        {
                                                            game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                                                            _afterStart = 3;
                                                            _isFirstPress = false;
                                                            lblFirstTeamPress.Background = Brushes.Green;
                                                            lblSecondTeamPress.Background = Brushes.Red;
                                                            btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                            btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                            btnStart.IsEnabled = false;
                                                        }
                                                        else
                                                        {
                                                            _afterStart = 4;
                                                            _isFirstPress = true;
                                                            lblSecondTeamPress.Background = Brushes.Red;
                                                            btnStart.IsEnabled = true;
                                                        }
                                                }
                                                else
                                                {
                                                    if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 2)
                                                    {
                                                        game.FirstTeam.Point -= 1;
                                                        game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                                                        _afterStart = 3;
                                                        _isFirstPress = false;
                                                        lblFirstTeamPress.Background = Brushes.Green;
                                                        lblSecondTeamPress.Background = Brushes.Orange;
                                                        btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                        btnFalse.Visibility = System.Windows.Visibility.Visible;
                                                        btnStart.IsEnabled = false;
                                                    }
                                                    else
                                                        if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 1)
                                                        {
                                                            game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                                                            _afterStart = 3;
                                                            _isFirstPress = false;
                                                            lblFirstTeamPress.Background = Brushes.Green;
                                                            lblSecondTeamPress.Background = Brushes.Orange;
                                                            btnTrue.Visibility = System.Windows.Visibility.Visible;
                                                            btnFalse.Visibility = System.Windows.Visibility.Visible;

                                                            btnStart.IsEnabled = false;
                                                        }
                                                        else
                                                        {
                                                            _afterStart = 2;
                                                            _isFirstPress = true;
                                                            lblSecondTeamPress.Background = Brushes.Orange;
                                                            btnStart.IsEnabled = true;
                                                        }
                                                }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    if (!btnStart.IsEnabled)
                    {
                        rawinput.KeyPressed -= OnKeyPressed;
                        TimerGlob.Stop();
                        btnStart.IsEnabled = true;
                        chb60seconds.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        ResultsRefresh();
                        _afterStart = 0;
                        _isFallStart = true;
                        _isFirstPress = true;
                        game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress = 0;
                        game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                        btnStart.IsEnabled = true;
                        chb60seconds.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                case 2:
                    if (!btnStart.IsEnabled)
                    {
                        rawinput.KeyPressed -= OnKeyPressed;
                        TimerGlob.Stop();
                        btnStart.IsEnabled = true;
                        chb60seconds.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        ResultsRefresh();
                        _afterStart = 0;
                        _isFallStart = true;
                        _isFirstPress = true;
                        game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                        game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress = 0;
                        btnStart.IsEnabled = true;
                        chb60seconds.Visibility = System.Windows.Visibility.Hidden;
                    }
                    break;
                case 3:
                    if (game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress == 1)
                    {
                        if (btnTrue.Visibility == System.Windows.Visibility.Hidden)
                        {
                            rawinput.KeyPressed -= OnKeyPressed;
                            TimerGlob.Stop();
                            game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus = 0;
                            btnTrue.Visibility = System.Windows.Visibility.Visible;
                            btnFalse.Visibility = System.Windows.Visibility.Visible;
                            lblFirstTeamPress.Background = Brushes.Green;
                        }
                        else
                        {
                            _afterStart = 0;
                            btnTrue.Visibility = System.Windows.Visibility.Hidden;
                            btnFalse.Visibility = System.Windows.Visibility.Hidden;
                            btnStart.IsEnabled = true;
                            _isFirstPress = true;
                            ResultsRefresh();
                        }
                    }
                    else
                    {
                        if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == 1)
                        {
                            _afterStart = 4;
                            btnTrue.Visibility = System.Windows.Visibility.Hidden;
                            btnFalse.Visibility = System.Windows.Visibility.Hidden;
                            btnStart.IsEnabled = true;
                            ResultsRefresh();
                            lblSecondTeamPress.Background = Brushes.Red;
                        }
                        else
                            if (game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus == -1)
                            {
                                _afterStart = 2;
                                btnTrue.Visibility = System.Windows.Visibility.Hidden;
                                btnFalse.Visibility = System.Windows.Visibility.Hidden;
                                btnStart.IsEnabled = true;
                                ResultsRefresh();
                                lblSecondTeamPress.Background = Brushes.Orange;
                                chb60seconds.Visibility = System.Windows.Visibility.Visible;
                            }
                    }
                    break;
                case 4:
                    if (game.GameQuestions.ElementAt(_numberOfQuestion).WhoFirstPress == 2)
                    {
                        if (btnTrue.Visibility == System.Windows.Visibility.Hidden)
                        {
                            rawinput.KeyPressed -= OnKeyPressed;
                            TimerGlob.Stop();
                            game.GameQuestions.ElementAt(_numberOfQuestion).SecondTeamStatus = 0;
                            btnTrue.Visibility = System.Windows.Visibility.Visible;
                            btnFalse.Visibility = System.Windows.Visibility.Visible;
                            lblSecondTeamPress.Background = Brushes.Green;
                        }
                        else
                        {
                            _afterStart = 0;
                            btnTrue.Visibility = System.Windows.Visibility.Hidden;
                            btnFalse.Visibility = System.Windows.Visibility.Hidden;
                            btnStart.IsEnabled = true;
                            _isFirstPress = true;
                            ResultsRefresh();
                        }
                    }
                    else
                    {
                        if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == 1)
                        {
                            _afterStart = 3;
                            btnTrue.Visibility = System.Windows.Visibility.Hidden;
                            btnFalse.Visibility = System.Windows.Visibility.Hidden;
                            btnStart.IsEnabled = true;
                            ResultsRefresh();
                            lblFirstTeamPress.Background = Brushes.Red;
                        }
                        else
                            if (game.GameQuestions.ElementAt(_numberOfQuestion).FirstTeamStatus == -1)
                            {
                                _afterStart = 1;
                                btnTrue.Visibility = System.Windows.Visibility.Hidden;
                                btnFalse.Visibility = System.Windows.Visibility.Hidden;
                                btnStart.IsEnabled = true;
                                ResultsRefresh();
                                lblFirstTeamPress.Background = Brushes.Orange;
                                chb60seconds.Visibility = System.Windows.Visibility.Visible;
                            }
                    }
                    break;
            }
        }

        private void lbGames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbGames.SelectedItem != null)
            {
                if (System.Windows.MessageBox.Show("Do you want to open this game?", "Question", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (!((Game)lbGames.SelectedItem).IsOver)
                    {
                        if (string.IsNullOrEmpty(_firstTeamKeyboardType) || string.IsNullOrEmpty(_secondTeamKeyboardType))
                        { 
                            System.Windows.MessageBox.Show("Select keyboards, please", "Error", MessageBoxButton.OK);
                            btnFirstKeyboard.Visibility = System.Windows.Visibility.Visible;
                            btnSecondKeyboard.Visibility = System.Windows.Visibility.Visible;
                            lbKeyboards.Visibility = System.Windows.Visibility.Visible;
                            lblFKey.Visibility = System.Windows.Visibility.Visible;
                            lblSKey.Visibility = System.Windows.Visibility.Visible;
                            _continue = true;
                            lblStart.IsEnabled = true;
                            tbFirstTeamName.Visibility = System.Windows.Visibility.Hidden;
                            tbSecondTeamName.Visibility = System.Windows.Visibility.Hidden;
                            Sld.Visibility = System.Windows.Visibility.Hidden;
                            l1.Visibility = l5.Visibility = l8.Visibility = l11.Visibility = System.Windows.Visibility.Hidden;
                        }
                        else
                        {
                            game = (Game)lbGames.SelectedItem;
                            this.DataContext = null;
                            this.DataContext = game;
                            dgQuestions.DataContext = game.GameQuestions;
                            _numberOfQuestion = game.NumberOfQuestion;
                            CountQuestions = game.CountOfQuestions;
                            btnStart.IsEnabled = true;
                            btnStart.Visibility = System.Windows.Visibility.Visible;
                            _meterSeconds = 60;
                            ResultsRefresh();
                            _isFallStart = true;
                            _isFirstPress = true;
                            _afterStart = 0;
                            lblTestSystem.IsEnabled = true;
                            btnBackStep.Visibility = System.Windows.Visibility.Visible;
                            btnBackStep.IsEnabled = true;
                            lblStopAndSave.IsEnabled = true;
                            tbFirstTeamName.Visibility = System.Windows.Visibility.Hidden;
                            tbSecondTeamName.Visibility = System.Windows.Visibility.Hidden;
                            Sld.Visibility = System.Windows.Visibility.Hidden;
                            l1.Visibility = l5.Visibility = l8.Visibility = l11.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }
                    else 
                    { 
                        System.Windows.MessageBox.Show("This game is over", "Error", MessageBoxButton.OK);
                        game = (Game)lbGames.SelectedItem;
                        this.DataContext = null;
                        this.DataContext = game;
                        dgQuestions.DataContext = game.GameQuestions;
                        btnBackStep.Visibility = System.Windows.Visibility.Hidden;
                        tbFirstTeamName.Visibility = System.Windows.Visibility.Hidden;
                        tbSecondTeamName.Visibility = System.Windows.Visibility.Hidden;
                        Sld.Visibility = System.Windows.Visibility.Hidden;
                        l1.Visibility = l5.Visibility = l8.Visibility = l11.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
        }

        private bool IsNewKeyboard(string keyboardType)
        {
            bool flag = true;
            if (_kb.Count == 0)
                flag = true;
            else 
            {
                foreach (var x in _kb)
                    if (x == keyboardType)
                        flag = false;
            }
            return flag;
        }
    }
}