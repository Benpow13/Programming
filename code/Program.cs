using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FootballConsoleApp
{

    
       public abstract class Person // Abstract person in the system
    {
        private string _name;
        private int _age;

        
        protected string Role { get; set; } = string.Empty; // Protected property to indicate Player "Coach etc

        
        protected Person(string name, int age) // Constructor for Person, assigning name and age
        {
            _name = name;
            _age = age;
        }

        
        public string Name => _name; // Read-only properties 
        public int Age => _age;

        
        public abstract void DisplayInfo(); //implement to display their info.
    }




    public class Player : Person //player inherits from Person and represents
    {
        private string _position;
        private double _height;

        
        public Player(string name, int age, string position, double height) // Constructor for player
            : base(name, age)
        {
            Role = "Player"; // Overriding the protected role property
            _position = position;
            _height = height;
        }

        
        public string Position => _position;  // Read-only properties for the player's position + height
        public double Height => _height;

        
        
        public override void DisplayInfo() // shows player infp
        {
            Console.WriteLine($"[Player Info] Name: {Name}, Age: {Age}, Position: {Position}, Height: {Height}m");
        }
    }




  public class Team
    {
        
        private string _teamName = string.Empty; // Private field for storin team name

        
        private List<Player> _players = new List<Player>(); // list of players

        
        public Team(string teamName)
        {
            _teamName = teamName;
        }

        
        public string TeamName
        {
            get { return _teamName; }
            set { _teamName = value; }
        }

        
        public IReadOnlyList<Player> Players => _players; // get an iread

        
        public int Points { get; set; } // looks after team player

        
        public void AddPlayer(Player player) // add new player
        {
            _players.Add(player);
        }
    }

    


     public class Fixture //gets planned match date
    {
        
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        
        public DateTime MatchDate { get; set; } // The date/time of match

        
        public bool IsCompleted { get; set; } // results entered?

        
        public Fixture(Team homeTeam, Team awayTeam, DateTime matchDate)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            MatchDate = matchDate;
            IsCompleted = false;
        }

        
        public override string ToString() //string returning descrition
        {
            string completionNote = IsCompleted ? " [Result Entered]" : "";
            return $"{HomeTeam.TeamName} vs {AwayTeam.TeamName} on {MatchDate:dd-MM-yyyy}{completionNote}";
        }
    }

   class Program
    {
        
        static Dictionary<string, Player> allPlayers = new Dictionary<string, Player>(); // player dictionary

        
        static List<Team> allTeams = new List<Team>();

        
        static List<Fixture> allFixtures = new List<Fixture>();

        static void Main(string[] args)
        {
            
            foreach (var arg in args)
            {
                if (arg.StartsWith("log="))
                {
                    string logValue = arg.Substring("log=".Length);
                    Console.WriteLine($"Logging level set to: {logValue}");
                }
            }

            bool exit = false;

            
            while (!exit) // continue until exsit
            {
                Console.WriteLine();
                Console.WriteLine("===== FOOTBALL CONSOLE APP =====");
                Console.WriteLine("1. Add Team");
                Console.WriteLine("2. View Teams");
                Console.WriteLine("3. Add Player to Team");
                Console.WriteLine("4. View Players in a Team");
                Console.WriteLine("5. Add Fixture");
                Console.WriteLine("6. View Fixtures");
                Console.WriteLine("7. Enter Match Result");
                Console.WriteLine("8. Display League Table");
                Console.WriteLine("9. Exit");
                Console.Write("Select an option: ");

                
                string? choice = Console.ReadLine(); // reads console
                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddTeam();
                            break;
                        case "2":
                            ViewTeams();
                            break;
                        case "3":
                            AddPlayerToTeam();
                            break;
                        case "4":
                            ViewPlayersInTeam();
                            break;
                        case "5":
                            AddFixture();
                            break;
                        case "6":
                            ViewFixtures();
                            break;
                        case "7":
                            EnterMatchResult();
                            break;
                        case "8":
                            DisplayLeagueTable();
                            break;
                        case "9":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"An error occurred: {ex.Message}"); // Catches expections
                }
            }

           
            Console.WriteLine("Application closed. Press any key to exit...");  // last message shwon before end
            Console.ReadKey();
        }

        
        static void AddTeam() // add team to list
        {
            Console.Write("Enter the team name: ");
            string? teamName = Console.ReadLine();

            
            if (string.IsNullOrWhiteSpace(teamName)) // checks empty space
            {
                Console.WriteLine("Team name cannot be empty.");
                return;
            }

            
            if (allTeams.Any(t => t.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase))) // Check name exsist
            {
                Console.WriteLine($"Team '{teamName}' already exists!");
                return;
            }



            
            Team newTeam = new Team(teamName); // new team object
            allTeams.Add(newTeam);
            Console.WriteLine($"Team '{teamName}' added successfully.");
        }

        
        static void ViewTeams() //show all teams
        {
            if (!allTeams.Any())
            {
                Console.WriteLine("No teams available.");
                return;
            }

            Console.WriteLine("Teams:");
            foreach (var team in allTeams)
            {
                Console.WriteLine($"- {team.TeamName} (Points: {team.Points})");
            }
        }

        
        static void AddPlayerToTeam() // gets users details then attributes
        {
            Console.Write("Enter the team name: ");
            string? teamName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(teamName))
            {
                Console.WriteLine("Invalid team name.");
                return;
            }

            var team = allTeams.FirstOrDefault(t =>
                t.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase));
            if (team == null)
            {
                Console.WriteLine($"Team '{teamName}' not found.");
                return;
            }

            Console.Write("Enter player name: ");
            string? playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                Console.WriteLine("Invalid player name.");
                return;
            }

            Console.Write("Enter player age: ");
            string? ageInput = Console.ReadLine();
            if (!int.TryParse(ageInput, out int age))
            {
                Console.WriteLine("Invalid age.");
                return;
            }

            Console.Write("Enter player position: ");
            string? position = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(position))
            {
                Console.WriteLine("Invalid position.");
                return;
            }

            Console.Write("Enter player height (in meters): ");
            string? heightInput = Console.ReadLine();
            if (!double.TryParse(heightInput, out double height))
            {
                Console.WriteLine("Invalid height.");
                return;
            }

           
            Player newPlayer = new Player(playerName, age, position, height);  // create new player then adds to team
            team.AddPlayer(newPlayer);

            
            if (!allPlayers.ContainsKey(playerName)) //if not in dicsonary add then
            {
                allPlayers.Add(playerName, newPlayer);
            }

            Console.WriteLine($"Player '{playerName}' added to team '{teamName}'.");
        }

        
        static void ViewPlayersInTeam() // shows players called
        {
            Console.Write("Enter the team name: ");
            string? teamName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(teamName))
            {
                Console.WriteLine("Invalid team name.");
                return;
            }

            var team = allTeams.FirstOrDefault(t =>
                t.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase));

            if (team == null)
            {
                Console.WriteLine($"Team '{teamName}' not found.");
                return;
            }

            if (!team.Players.Any())
            {
                Console.WriteLine($"No players in team '{teamName}'.");
                return;
            }

            Console.WriteLine($"Players in {teamName}:");
            foreach (var player in team.Players)
            {
                
                player.DisplayInfo();
            }
        }

        
        static void AddFixture() // create new fixture 
        {
            Console.Write("Enter Home Team name: ");
            string? homeTeamName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(homeTeamName))
            {
                Console.WriteLine("Invalid home team name.");
                return;
            }

            Console.Write("Enter Away Team name: ");
            string? awayTeamName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(awayTeamName))
            {
                Console.WriteLine("Invalid away team name.");
                return;
            }

            
            var homeTeam = allTeams.FirstOrDefault(t => // serach teams
                t.TeamName.Equals(homeTeamName, StringComparison.OrdinalIgnoreCase));
            var awayTeam = allTeams.FirstOrDefault(t =>
                t.TeamName.Equals(awayTeamName, StringComparison.OrdinalIgnoreCase));

            if (homeTeam == null || awayTeam == null)
            {
                Console.WriteLine("One or both teams not found.");
                return;
            }

            Console.Write("Enter match date (yyyy-mm-dd): ");
            string? dateInput = Console.ReadLine();

            
            if (!DateTime.TryParse(dateInput, out DateTime matchDate)) // validate input date.
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            
            Fixture newFixture = new Fixture(homeTeam, awayTeam, matchDate); // Creates new fixture then adds to list
            allFixtures.Add(newFixture);

            Console.WriteLine($"Fixture '{newFixture}' added successfully.");
        }

        
        static void ViewFixtures() // displays all fixtures
        {
            if (!allFixtures.Any())
            {
                Console.WriteLine("No fixtures scheduled.");
                return;
            }

            Console.WriteLine("Fixtures:");
            
            var sortedFixtures = allFixtures.OrderBy(f => f.MatchDate).ToList(); // orders fixtures by date
            foreach (var fixture in sortedFixtures)
            {
                Console.WriteLine(fixture.ToString());
            }
        }

        
        static void EnterMatchResult()
        {
            
            if (!allFixtures.Any()) // check exsisting fixtures
            {
                Console.WriteLine("No fixtures found. Please add fixtures first.");
                return;
            }

            
            var incompleteFixtures = allFixtures.Where(f => !f.IsCompleted).ToList();
            if (!incompleteFixtures.Any())
            {
                Console.WriteLine("All fixtures have results entered or none exist.");
                return;
            }

            
            Console.WriteLine("Existing Fixtures (not completed):"); // only displays unvaild fixtures
            for (int i = 0; i < incompleteFixtures.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {incompleteFixtures[i]}");
            }

            Console.Write("Select a fixture number to enter result: ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int index) || index < 1 || index > incompleteFixtures.Count)
            {
                Console.WriteLine("Invalid selection.");
                return;
            }

            
            Fixture selectedFixture = incompleteFixtures[index - 1]; // gets fixture which needs updating
            Console.WriteLine($"You've chosen: {selectedFixture}");

            // Ask which team won.
            Console.Write("Who won? Enter 'Home' or 'Away': ");
            string? winner = Console.ReadLine();

            
            if (winner?.Equals("Home", StringComparison.OrdinalIgnoreCase) == true) // if home =3 away =0 and revsere
            {
                selectedFixture.HomeTeam.Points += 3;
                Console.WriteLine($"{selectedFixture.HomeTeam.TeamName} awarded 3 points.");
            }
            else if (winner?.Equals("Away", StringComparison.OrdinalIgnoreCase) == true)
            {
                selectedFixture.AwayTeam.Points += 3;
                Console.WriteLine($"{selectedFixture.AwayTeam.TeamName} awarded 3 points.");
            }
            else
            {
                Console.WriteLine("Invalid winner input. No points awarded.");
                return;
            }

            
            selectedFixture.IsCompleted = true; // mark as completed
            Console.WriteLine("Match result entered successfully!");
        }

       
        static void DisplayLeagueTable()  // sort by desending price
        {
            if (!allTeams.Any())
            {
                Console.WriteLine("No teams have been added yet.");
                return;
            }

            
            var sortedTeams = allTeams.OrderByDescending(t => t.Points).ToList(); // order by points desending

            Console.WriteLine("===== LEAGUE TABLE =====");  
            Console.WriteLine("Pos | Team          | Points");
            Console.WriteLine("-----------------------------");

            int position = 1;
            foreach (var team in sortedTeams)
            {
                Console.WriteLine($"{position,2}. | {team.TeamName,-13} | {team.Points,3}");
                position++;
            }
        }
    }
}


https://youtu.be/dkcWUMmJHdQ
