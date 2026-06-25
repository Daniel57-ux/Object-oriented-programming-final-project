using System;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller game = new Controller();
            game.StartGame();
        }
    }

    
    class Controller
    {
        private Model model;
        private View view;

        private HumanPlayer player1;
        private HumanPlayer player2;

        private HumanPlayer currentPlayer;

        public Controller()
        {
            model = new Model();
            view = new View();
        }

        public void StartGame()
        {
            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();

                model.InitializeBoard();

                Console.Write("Enter Player 1 name: ");
                string name1 = Console.ReadLine();

                Console.Write("Enter Player 2 name: ");
                string name2 = Console.ReadLine();

                player1 = new HumanPlayer(name1, 'X');
                player2 = new HumanPlayer(name2, 'O');

                currentPlayer = player1;

                bool gameOver = false;

                while (!gameOver)
                {
                    Console.Clear();
                    view.DisplayBoard(model.Board);

                    Console.WriteLine();
                    Console.WriteLine(currentPlayer.Name + "'s Turn");
                    Console.WriteLine("Your symbol is: " + currentPlayer.Symbol);

                    int column = view.GetColumnChoice();

                    while (!model.DropPiece(column, currentPlayer.Symbol))
                    {
                        Console.WriteLine("That column is full. Try another.");
                        column = view.GetColumnChoice();
                    }

                    if (model.CheckWinner(currentPlayer.Symbol))
                    {
                        Console.Clear();
                        view.DisplayBoard(model.Board);

                        Console.WriteLine();
                        Console.WriteLine(currentPlayer.Name + " Wins!");
                        gameOver = true;
                    }
                    else if (model.CheckDraw())
                    {
                        Console.Clear();
                        view.DisplayBoard(model.Board);

                        Console.WriteLine();
                        Console.WriteLine("It's a Draw!");
                        gameOver = true;
                    }
                    else
                    {
                        SwitchPlayer();
                    }
                }

                playAgain = view.PlayAgain();
            }

            Console.WriteLine("Thanks for playing! And happy Holidays!");
            Console.ReadKey();
        }

        private void SwitchPlayer()
        {
            if (currentPlayer == player1)
                currentPlayer = player2;
            else
                currentPlayer = player1;
        }
    }

    
    abstract class Player
    {
        public string Name { get; set; }
        public char Symbol { get; set; }
    }

    
    class HumanPlayer : Player
    {
        public HumanPlayer(string name, char symbol)
        {
            Name = name;
            Symbol = symbol;
        }
    }

    class Model
    {
        public char[,] Board { get; private set; }

        public Model()
        {
            Board = new char[6, 7];
        }

        public void InitializeBoard()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Board[row, col] = '.';
                }
            }
        }

        public bool DropPiece(int column, char symbol)
        {
            column--; 

            for (int row = 5; row >= 0; row--)
            {
                if (Board[row, column] == '.')
                {
                    Board[row, column] = symbol;
                    return true;
                }
            }

            return false;
        }

        public bool CheckDraw()
        {
            for (int col = 0; col < 7; col++)
            {
                if (Board[0, col] == '.')
                    return false;
            }

            return true;
        }

        public bool CheckWinner(char symbol)
        {
            return CheckHorizontal(symbol) ||
                   CheckVertical(symbol) ||
                   CheckDiagonalRight(symbol) ||
                   CheckDiagonalLeft(symbol);
        }

        private bool CheckHorizontal(char symbol)
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (Board[row, col] == symbol &&
                        Board[row, col + 1] == symbol &&
                        Board[row, col + 2] == symbol &&
                        Board[row, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckVertical(char symbol)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (Board[row, col] == symbol &&
                        Board[row + 1, col] == symbol &&
                        Board[row + 2, col] == symbol &&
                        Board[row + 3, col] == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckDiagonalRight(char symbol)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (Board[row, col] == symbol &&
                        Board[row + 1, col + 1] == symbol &&
                        Board[row + 2, col + 2] == symbol &&
                        Board[row + 3, col + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckDiagonalLeft(char symbol)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    if (Board[row, col] == symbol &&
                        Board[row + 1, col - 1] == symbol &&
                        Board[row + 2, col - 2] == symbol &&
                        Board[row + 3, col - 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    
    class View
    {
        public void DisplayBoard(char[,] board)
        {
            Console.WriteLine(" 1 2 3 4 5 6 7");
            Console.WriteLine();

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    Console.Write(board[row, col] + " ");
                }

                Console.WriteLine();
            }
        }

        public int GetColumnChoice()
        {
            int column;

            while (true)
            {
                Console.WriteLine();
                Console.Write("Choose a column (1-7): ");

                string input = Console.ReadLine();

                if (int.TryParse(input, out column))
                {
                    if (column >= 1 && column <= 7)
                        return column;
                }

                Console.WriteLine("Invalid input. Please enter a number from 1 to 7.");
            }
        }

        public bool PlayAgain()
        {
            Console.WriteLine();
            Console.Write("Play again? (Y/N): ");

            string answer = Console.ReadLine().ToUpper();

            return answer == "Y";
        }
    }
}
