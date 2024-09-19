namespace MyProject
{
    class MainProgram
    {
        static void Main(string[] _)
        {
            Main mainProgram = new();
            mainProgram.Run();
        }

    }

    class Utils
    {
        public static void ClearCurrentLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, currentLineCursor - 1);
            Console.Write(new string(' ', Console.WindowWidth)); // Overwrite the line with spaces
            Console.SetCursorPosition(0, currentLineCursor - 1); // Move cursor back to the start of the line
        }


        public static List<int> GetLargestNumbers(List<int> numbers)
        {
            if (numbers == null || numbers.Count == 0)
                return [];

            int largest = numbers.Max();
            return numbers.Where(number => number == largest).ToList();

        }
    }

    class Main
    {
        public static CardDeck Cards = new();
        public List<Player> PlayerList = new();
        public GameBord gameBord = new();
        public int turns = 0;

        public enum PlayerGameChose
        {
            ShowCards,
            ShowGameBored,
            PlayCard
        }


        public void Run()
        {
            Console.WriteLine("Welcome to Console Card game Kariba!");
            GeneratePlayers();
            StartGame();

        }

        public void GeneratePlayers()
        {
            Console.WriteLine("GENERATION OF PLAYER");

            int amoutOfPlayer = 0;

            Console.WriteLine(@"
___________________________________
|         HOW MANY PLAYERS?       |
|---------------------------------|
| 1: 2 Players         ( Type: 1) |
| 2: 3 Players         ( Type: 2) |
| 3: 4 Players         ( Type: 3) |
|_________________________________| ");
            bool PlayerTypedWrong = false;
            while (amoutOfPlayer == 0)
            {
                Console.Write("What Option do you want? ");
                string playerAwnser = Console.ReadLine();



                switch (playerAwnser)
                {
                    case "1":
                        amoutOfPlayer = 2;
                        break;

                    case "2":
                        amoutOfPlayer = 3;
                        break;
                    case "3":
                        amoutOfPlayer = 4;
                        break;
                    default:
                        Utils.ClearCurrentLine();
                        if (!PlayerTypedWrong)
                        {
                            Console.WriteLine($"Invaid input ({playerAwnser}). Ops that menu option ({playerAwnser}) dose not exist.");
                            PlayerTypedWrong = true;
                        }
                        continue;
                }
            }

            for (int player_id = 0; player_id < amoutOfPlayer; player_id++)
            {
                Player new_player = new(player_id);
                PlayerList.Add(new_player);
            }
        }

        public void StartGame()
        {
            turns = 0;
            int cardsInPlayerHand = 0; // THE CARDS THAT ARE IN THE GAME
            List<Player> winnerPlayerList = []; // THE LIST WHERE ALL THE PLAYERS THAT HAVE THE LARGEST SCORE ARE 
            List<int> scoreOfPlayers = [];
            List<int> winnerScore = [];


            foreach (Player player in PlayerList)
            {
                cardsInPlayerHand += player.PlayerCards.Count;
            }


            // GAME LOOP 
            while (cardsInPlayerHand != 0)
            {
                turns++;
                // FULL QUEUE TURN

                foreach (Player playerInQueue in PlayerList)
                {

                    playerInQueue.State = Player.PlayerStates.Playing;

                    // GAME 
                    while (playerInQueue.State == Player.PlayerStates.Playing)
                    {
                        Console.Clear();

                        // WRITRE OUT GAME MENU
                        Console.WriteLine(@$"
___________________________________
|  Player Option | Player {playerInQueue.PlayerID + 1}       |
|---------------------------------|
| 1: Show Game Bord    ( Type: 1) |
| 2: Show Player Cards ( Type: 2) |");
                        if (playerInQueue.PlayerCards.Count == 0)
                        {
                            Console.WriteLine("| 3: Pass turn         ( Type: 3) |");
                        }
                        else
                        {
                            Console.WriteLine("| 3: Play playerCard   ( Type: 3) |");
                        }
                        Console.WriteLine(@"|_________________________________|

What option do you want to do?");



                        string playerChose = Console.ReadLine();

                        // PLAYER OPTIONS 
                        switch (playerChose)
                        {

                            // SHOW GAME BORED
                            case "1":
                                gameBord.Display();
                                Console.Write("Press any button to go back....");
                                Console.ReadLine();
                                break;

                            // SHOW PLAYER HAND
                            case "2":
                                playerInQueue.ShowPlayerHand();
                                Console.Write("Press any button to go back....");
                                Console.ReadLine();
                                break;

                            // PLACE A CARD ON THE GAME BORED 
                            case "3":
                                if (playerInQueue.PlayerCards.Count > 0)
                                {
                                    playerInQueue.PlaceCardOnBored(gameBord);
                                }
                                else
                                {
                                    playerInQueue.State = Player.PlayerStates.TurnDone;
                                }
                                break;

                            case "4":
                                if (playerInQueue.PlayerCards.Count == 0)
                                {
                                    Console.WriteLine($"Ops that menu option ({playerChose}) dose not exist \n");
                                }
                                playerInQueue.State = Player.PlayerStates.TurnDone;

                                break;

                            default:
                                Console.WriteLine($"Ops that menu option({playerChose}) dose not exist \n");
                                break;

                        }
                    }

                    // ONLY PICK UP IF THERE ARE ANY CARDS IN THE CARD PILE
                    if (Main.Cards.DeckCardStack.Count != 0)
                    {
                        playerInQueue.FillPlayerHand();
                    }
                }

                cardsInPlayerHand = 0;
                // HOW MANY CARDS ARE IN PLAYERS HAND?
                foreach (Player player in PlayerList)
                {
                    cardsInPlayerHand += player.PlayerCards.Count;
                }
            }




            // ADD ALL PLAYERS SCORE
            foreach (Player player in PlayerList)
            {
                scoreOfPlayers.Add(player.PlayerScore);
            }

            // GETTING THE LARGEST SCORE
            winnerScore = Utils.GetLargestNumbers(scoreOfPlayers);


            // ADD ALL THE PLAYERS THAT HAVES THE 
            foreach (Player player in PlayerList)
            {
                if (player.PlayerScore == winnerScore[0])
                {
                    winnerPlayerList.Add(player);
                }
            }


            // DISPLAY THE WINNERS
            Console.WriteLine($@"
___________________________________
|           PLAYER WINNERS        |
|---------------------------------|
|                                 |


");
            foreach (Player player in winnerPlayerList)
            {
                Console.WriteLine($"|      Player {player.PlayerID + 1}             |");
            }
            Console.WriteLine(@"|                                 |
|_________________________________| ");
            Console.WriteLine("Press any Key to end the program");

        }


    }



    class CardDeck
    {
        public List<int> DeckCards =
        [
           1,1,1,1,1,1,1,1,
           2,2,2,2,2,2,2,2,
           3,3,3,3,3,3,3,3,
           4,4,4,4,4,4,4,4,
           5,5,5,5,5,5,5,5,
           6,6,6,6,6,6,6,6,
           7,7,7,7,7,7,7,7,
           8,8,8,8,8,8,8,8
        ];

        /*  
         * 
        */
        public Stack<int> DeckCardStack;


        public CardDeck()
        {
            List<int> ShuffledList = Shuffle();
            DeckCardStack = new Stack<int>(ShuffledList);
        }

        public List<int> Shuffle()
        {
            Console.WriteLine("SHUFFEL");
            Random rng = new();
            return DeckCards.OrderBy(x => rng.Next()).ToList();
        }

    }

    class GameBord
    {
        // The Diffrent type of build upps in the game borad
        public List<List<int>> GameCards = [];


        public GameBord()
        {
            GameCards.AddRange(Enumerable.Repeat(new List<int>(), 8).Select(list => new List<int>()));
        }

        public int PlaceCards(List<int> cardNumberList)
        {
            int cardNumber = cardNumberList[0];

            // PLACES THE CARD ON THE GAMEBORED 
            foreach (int card in cardNumberList)
            {
                GameCards[cardNumber - 1].Add(card);
            }


            // CALULATION ON WHAT SCORE THE PLAYER GETS BY PLACEING THE NUMBER
            if (GameCards[cardNumber - 1].Count >= 3)
            {
                int plieNumber = 0;
                // LOOK IF THE 1 VS 8 RULE APLIES
                if (cardNumber != 1)
                {
                    for (int pileIndex = cardNumber; pileIndex < 0; pileIndex--)
                    {
                        if (GameCards[pileIndex].Count > 0)
                        {
                            plieNumber = pileIndex;

                            break;
                        }
                    }
                }
                else
                {
                    if (GameCards[7].Count != 0)
                    {
                        plieNumber = 7;
                    }
                }
                GameCards[plieNumber].Clear();
                return GameCards[plieNumber].Count;
            }

            return 0;
        }



        public void Display()
        {
            Console.Clear();
            for (int gameListIndex = 0; gameListIndex < GameCards.Count; gameListIndex++)
            {
                Console.WriteLine(@$"
___________________________________
|       Card List Value {gameListIndex + 1}         |
|---------------------------------|
|                                 |");

                foreach (int card in GameCards[gameListIndex])
                {
                    Console.WriteLine($"|        Card Value {card}             |");
                }

                Console.WriteLine("|_________________________________|");

            }
        }
    }

    class Player
    {

        public enum PlayerStates
        {
            TurnDone,
            Waiting,
            Playing
        }

        public int PlayerID { get; }
        private int MaxSizeHand { get; }


        public List<int> PlayerCards = [];
        public int PlayerScore = 0;

        public PlayerStates State { get; set; }

        public Player(int playerNumber, int maxSizeHand = 5)
        {
            PlayerID = playerNumber;
            MaxSizeHand = maxSizeHand;
            State = PlayerStates.Waiting;
            FillPlayerHand();
        }

        public void PlaceCardOnBored(GameBord gameBord)
        {
            int cardValue = 0;
            int valueAmount = 0;
            int amoutOfCards = 0;

            List<int> PlayerPlacedCard = [];

            bool PlayerTypedWrong = false;

            Console.Clear();
            ShowPlayerHand();


            while (true)
            {
                Console.Write("What Card do you want to place (1-5)? Card ");

                string playerChoice = Console.ReadLine();


                // WHAT CARD THE PLAYER CHOSES TO PLAY
                switch (playerChoice)
                {
                    case "1":
                        cardValue = PlayerCards[0];

                        break;

                    case "2":
                        cardValue = PlayerCards[1];
                        break;

                    case "3":
                        cardValue = PlayerCards[2];
                        break;

                    case "4":
                        cardValue = PlayerCards[3];
                        break;

                    case "5":
                        cardValue = PlayerCards[4];
                        break;

                    default:
                        Utils.ClearCurrentLine();
                        if (!PlayerTypedWrong)
                        {
                            Console.WriteLine($"Invaid input ({playerChoice}). Please select a number between 1 and {valueAmount}.");
                            PlayerTypedWrong = true;
                        }
                        continue;
                }




                // LOOK AT HOW MANY OF THAT CARD THERE ARE 
                foreach (int card in PlayerCards)
                {
                    if (card == cardValue)
                    {
                        valueAmount += 1;
                    }
                }

                // IF THE PLAYER HAVE MORE THEN 1 OF THAT CARD IN THE HAND
                if (valueAmount > 1)
                {
                    PlayerTypedWrong = false;
                    // PLAYER CHOSE IF THE PLAYER WANT TO PLAY THEN MORE 1 CARD OF THAT CARD
                    while (amoutOfCards == 0)
                    {
                        Console.Write($"How many of that card value ({cardValue}) do u want to play? (Type: 1 - {valueAmount}):  ");
                        string playerChose = Console.ReadLine();

                        switch (playerChose)
                        {

                            case "1":
                                // LOOKS IF THE AMOUT OF CARDS THE PLAYER WANT TO PLAY IS RIGHT AMOUT AND NORE MORE THEN THE PLAYERS HAND DO NOT CONTAINS
                                if (int.Parse(playerChose) <= valueAmount)
                                {
                                    amoutOfCards = 1;
                                    break;
                                }
                                else
                                {
                                    Utils.ClearCurrentLine();
                                    if (!PlayerTypedWrong)
                                    {
                                        Console.WriteLine($"Invaid input ({playerChose}). Please select a number between 1 and {valueAmount}.");
                                        PlayerTypedWrong = true;
                                    }
                                    continue;
                                }

                            case "2":
                                // LOOKS IF THE AMOUT OF CARDS THE PLAYER WANT TO PLAY IS RIGHT AMOUT AND NORE MORE THEN THE PLAYERS HAND DO NOT CONTAINS
                                if (int.Parse(playerChose) <= valueAmount)
                                {
                                    amoutOfCards = 2;
                                    break;
                                }
                                else
                                {

                                    Utils.ClearCurrentLine();
                                    if (!PlayerTypedWrong)
                                    {
                                        Console.WriteLine($"Invaid input ({playerChose}). Please select a number between 1 and {valueAmount}.");
                                        PlayerTypedWrong = true;
                                    }
                                    continue;
                                }


                            case "3":
                                // LOOKS IF THE AMOUT OF CARDS THE PLAYER WANT TO PLAY IS RIGHT AMOUT AND NORE MORE THEN THE PLAYERS HAND DO NOT CONTAINS
                                if (int.Parse(playerChose) <= valueAmount)
                                {
                                    amoutOfCards = 3;
                                    break;
                                }
                                else
                                {

                                    Utils.ClearCurrentLine();
                                    if (!PlayerTypedWrong)
                                    {
                                        Console.WriteLine($"Invaid input ({playerChose}). Please select a number between 1 and {valueAmount}.");
                                        PlayerTypedWrong = true;
                                    }
                                    continue;
                                }

                            case "4":
                                // LOOKS IF THE AMOUT OF CARDS THE PLAYER WANT TO PLAY IS RIGHT AMOUT AND NORE MORE THEN THE PLAYERS HAND DO NOT CONTAINS
                                if (int.Parse(playerChose) <= valueAmount)
                                {
                                    amoutOfCards = 4;
                                    break;
                                }
                                else
                                {

                                    Utils.ClearCurrentLine();
                                    if (!PlayerTypedWrong)
                                    {
                                        Console.WriteLine($"Invaid input ({playerChose}). Please select a number between 1 and {valueAmount}.");
                                        PlayerTypedWrong = true;
                                    }
                                    continue;
                                }

                            case "5":
                                // LOOKS IF THE AMOUT OF CARDS THE PLAYER WANT TO PLAY IS RIGHT AMOUT AND NORE MORE THEN THE PLAYERS HAND DO NOT CONTAINS
                                if (int.Parse(playerChose) <= valueAmount)
                                {
                                    amoutOfCards = 5;
                                    break;
                                }
                                else
                                {

                                    Utils.ClearCurrentLine();
                                    if (!PlayerTypedWrong)
                                    {
                                        Console.WriteLine($"Invaid input ({playerChose}). Please select a number between 1 and {valueAmount}.");
                                        PlayerTypedWrong = true;
                                    }
                                    continue;
                                }

                            default:
                                Utils.ClearCurrentLine();
                                if (!PlayerTypedWrong)
                                {
                                    Console.WriteLine($"Invaid input ({playerChose}). Please select a number between 1 and {valueAmount}.");
                                    PlayerTypedWrong = true;
                                }
                                continue;
                        }

                    }
                }
                // IF THE PLAYER DO NOT HAVE THEN MORE 1 OF THAT CARD
                else
                {
                    amoutOfCards = 1;
                }

                // ADDS THE VALUE OF THE CARD AND ADDS TO THE PLAYER PLAY HAND THE AMOUT OF MANNY CARDS THE PLAYER WANT TO PLAY
                for (int i = 0; i < amoutOfCards; i++)
                {
                    PlayerPlacedCard.Add(cardValue);
                }

                for (int cardIndex = 0; cardIndex < PlayerCards.Count; cardIndex++)
                {
                    int card = PlayerCards[cardIndex];
                    if (card == cardValue && amoutOfCards > 0)
                    {
                        PlayerCards.Remove(card);
                        amoutOfCards--;
                    }
                }

                PlayerScore += gameBord.PlaceCards(PlayerPlacedCard);

                FillPlayerHand();

                State = Player.PlayerStates.TurnDone;
                break;

            }

        }


        public void DebugPlayerHand()
        {
            Console.WriteLine($"Player ID: {PlayerID}, Player number: {PlayerID + 1}");

            Console.WriteLine($"\nPlayer Hand: {PlayerCards.Count}");

            for (int card_index = 0; card_index < PlayerCards.Count; card_index++)
            {
                Console.WriteLine($"Card ({card_index + 1}): Nummer {PlayerCards[card_index]}");
            }

        }

        // Function is done
        public void FillPlayerHand()
        {
            /*
             * IT FILLS THE PLAYER HAND TO THE MAXIMUM HAND SIZE THAT THE PLAYER IS ALLOWED TO HAVE 
            */

            while (PlayerCards.Count != MaxSizeHand && Main.Cards.DeckCardStack.Count > 0)
            {
                int DeckCard = Main.Cards.DeckCardStack.Pop();
                PlayerCards.Add(DeckCard);
            }
        }

        public void ShowPlayerHand()
        {
            Console.Clear();
            Console.WriteLine(@"
___________________________________
|           PLAYER HAND           |
|---------------------------------|
|                                 |");
            // WRITE OUT EACH OF THE CARDS
            for (int playerCardIndex = 0; playerCardIndex < PlayerCards.Count; playerCardIndex++)
            {
                int playerCard = PlayerCards[playerCardIndex];
                Console.WriteLine($"|        CARD {playerCardIndex + 1} :  Value {playerCard}        |");
            }
            Console.WriteLine(@"|                                 |
|_________________________________| ");

        }
    }


}