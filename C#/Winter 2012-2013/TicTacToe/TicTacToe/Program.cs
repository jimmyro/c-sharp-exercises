using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/* GOAL:

 X | . | O 
-----------     
 . | O | X      
-----------     
 O | . | .      

*/

//Jan. 7, 2013 class
namespace TicTacToe
{
    class Program
    {
        static char[,] board; //two-dim array of chars

        static void Main(string[] args)
        {
            //INITIALIZATION OF THE BOARD
            board = new char[3, 3]; //define dimensions of the array

            for (int row = 0; row < 3; row++) //initialize the board
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = '-';
                    //Console.WriteLine(row + ", " + col); 
                }
            }

            PrintBoard();

            //GAME MECHANICS
            bool isXTurn = true; //keep track of whose turn it is (X always goes first)
            while (true)
            {
                if (isXTurn) //prompt user for X or O
                {
                    Console.WriteLine("Position for X: (row col)");
                }
                else
                {
                    Console.WriteLine("Position for O: (row col)");
                }

                string position = Console.ReadLine(); //input looks like: 0 0
                string[] positions = position.Split(' ');

                bool validCoordinatesEntered = true;

                if (positions.Count() != 2) //check for right number of inputs
                {
                    validCoordinatesEntered = false;
                }

                int row;
                if (!int.TryParse(positions[0], out row)) //index 0 will be the row; index 1, the column
                {
                    validCoordinatesEntered = false;
                }

                int col;
                if (!int.TryParse(positions[1], out col))
                {
                    validCoordinatesEntered = false;
                }

                if (validCoordinatesEntered && row < 3 && col < 3 && row >= 0 && col >= 0 && board[row, col] == '-') 
                    //check for out of range before checking for '-'
                    //&& is a "short circuit and" which stops checking conditions as soon as one returns false
                {
                    if (isXTurn)
                    {
                        board[row, col] = 'X';
                    }
                    else
                    {
                        board[row, col] = 'O';
                    }

                    isXTurn = !isXTurn; //toggle turn
                }
                else
                {
                    Console.WriteLine("Invalid position.  Press enter to try again.");
                    Console.ReadLine();
                }

                Console.Clear();
                PrintBoard();

            }

            Console.ReadLine(); //pause
        }

        //prints the board state onto the screen
        static void PrintBoard()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    Console.Write(board[row, col]);
                }
                Console.WriteLine(); //add an enter bar after three elements
            }
        }
    }
}
