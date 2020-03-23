using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static Random r = new Random();
        public ObservableCollection<Player> allPlayers;
        public ObservableCollection<Player> selectedPlayers;
        int selectedGoalkeepers, selectedDefenders, selectedMidfielders, selectedForwards;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            allPlayers = new ObservableCollection<Player>();
            selectedPlayers = new ObservableCollection<Player>();

            allPlayers = CreatePlayers();
            RefreshDisplay();//Updates display with list of players

            cbxFormation.ItemsSource = new string[] { "4-4-2", "4-3-3", "4-5-1" };
            cbxFormation.SelectedIndex = 0;

        }

        //Method creates  a list of players
        private ObservableCollection<Player> CreatePlayers()
        {
            string[] firstNames = {
                "Adam", "Amelia", "Ava", "Chloe", "Conor", "Daniel", "Emily",
                "Emma", "Grace", "Hannah", "Harry", "Jack", "James",
                "Lucy", "Luke", "Mia", "Michael", "Noah", "Sean", "Sophie"};

            string[] lastNames = {
                "Brennan", "Byrne", "Daly", "Doyle", "Dunne", "Fitzgerald", "Kavanagh",
                "Kelly", "Lynch", "McCarthy", "McDonagh", "Murphy", "Nolan", "O'Brien",
                "O'Connor", "O'Neill", "O'Reilly", "O'Sullivan", "Ryan", "Walsh"
            };


            Position currentPosition = Position.Goalkeeper;
            ObservableCollection<Player> players = new ObservableCollection<Player>();

            for (int i = 0; i < 18; i++)
            {

                //block is used to change position based on numbers required
                //2 x Goalkeepers, 6 x Defenders, 6 x Midfielders, 4 x Forwards
                switch (i)
                {
                    case 2:
                        currentPosition = Position.Defender;
                        break;
                    case 8:
                        currentPosition = Position.Midfielder;
                        break;
                    case 14:
                        currentPosition = Position.Forward;
                        break;
                }

                Player p = new Player()
                {
                    FirstName = firstNames[r.Next(20)],
                    Surname = lastNames[r.Next(20)],
                    DateOfBirth = GetRandomDate(DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-20)), //player in range 20-30 years
                    PreferredPosition = currentPosition
                };

                players.Add(p);


            }

            return players;
        }

        //Method gets a random DateTime between two dates
        private DateTime GetRandomDate(DateTime startDate, DateTime endDate)
        {

            TimeSpan t = endDate - startDate;
            int numberOfDays = t.Days;
            DateTime newDate = startDate.AddDays(r.Next(numberOfDays));
            return newDate;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPlayers.Count > 10) //only allow 11 players on the team
                MessageBox.Show("You have too many players selected", "Player Number Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            else
            {
                Player selectedPlayer = lbxAllPlayers.SelectedItem as Player;

                if (selectedPlayer != null)
                {

                    //check if allowed by formation
                    string message = CheckFormation(selectedPlayer);

                    if (message.Equals("valid"))
                    {
                        allPlayers.Remove(selectedPlayer);
                        selectedPlayers.Add(selectedPlayer);
                    }

                    else
                        MessageBox.Show(message, "Formation Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);


                }

                RefreshDisplay();
            }
        }

        private void CbxFormation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var p in selectedPlayers)
            {
                allPlayers.Add(p);
            }

            selectedPlayers.Clear();

            selectedGoalkeepers = 0;
            selectedDefenders = 0;
            selectedMidfielders = 0;
            selectedForwards = 0;
            RefreshDisplay();
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            Player selectedPlayer = lbxSelectedPlayers.SelectedItem as Player;

            if (selectedPlayer != null)
            {
                allPlayers.Add(selectedPlayer);
                selectedPlayers.Remove(selectedPlayer);

                switch(selectedPlayer.PreferredPosition)
                {
                    case Position.Goalkeeper:
                        selectedGoalkeepers--;
                        break;

                    case Position.Defender:
                        selectedDefenders--;
                        break;

                    case Position.Midfielder:
                        selectedMidfielders--;
                        break;

                    case Position.Forward:
                        selectedForwards--;
                        break;

                }
            }

            RefreshDisplay();
        }

        private void RefreshDisplay()
        {
            lbxAllPlayers.ItemsSource = allPlayers.OrderBy(p => p.PreferredPosition).ThenBy(p => p.FirstName);
            lbxSelectedPlayers.ItemsSource = selectedPlayers.OrderBy(p => p.PreferredPosition).ThenBy(p => p.FirstName);

            tblkRemainingSpaces.Text = (11 - selectedPlayers.Count).ToString();
        }

        private string CheckFormation(Player player)
        {
            string message = "valid";

            //read formation - 4-4-2
            string selectedFormation = cbxFormation.SelectedItem as string;
            string[] formation = selectedFormation.Split('-');

            //determine allowed numbers for positions
            int allowedGoalkeepers = 1;
            int allowedDefenders = int.Parse(formation[0]);
            int allowedMidfielders = int.Parse(formation[1]);
            int allowedForwards = int.Parse(formation[2]);

            //check selected against allowed
            switch (player.PreferredPosition)
            {
                case Position.Goalkeeper:
                    if (selectedGoalkeepers < allowedGoalkeepers)
                    {
                        selectedGoalkeepers++;

                    }
                    else
                    {
                        message = "Cannot add this player. Too many goalkeepers";
                    }
                    break;

                case Position.Defender:
                    if (selectedDefenders < allowedDefenders)
                    {
                        selectedDefenders++;

                    }
                    else
                    {
                        message = "Cannot add this player. Too many defenders";
                    }
                    break;


                case Position.Midfielder:
                    if (selectedMidfielders < allowedMidfielders)
                    {
                        selectedMidfielders++;

                    }
                    else
                    {
                        message = "Cannot add this player. Too many midfielders";
                    }
                    break;
                case Position.Forward:
                    if (selectedForwards < allowedForwards)
                    {
                        selectedForwards++;

                    }
                    else
                    {
                        message = "Cannot add this player. Too many forwards";
                    }
                    break;

            }//end of switch

            return message;

        }
    }


}
