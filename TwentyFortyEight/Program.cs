using System;
using System.Collections.Generic;
using System.Linq;

namespace TwentyFortyEight {
    /// <summary>
    ///   Runs the game 2048
    /// </summary>
    /// <author>Khanh Duy Nguyen</author>
    /// <student_id>n9784616</student_id>
    public static class Program {
        /// <summary>
        /// Specifies possible moves in the game
        /// </summary>
        public enum Move { Up, Left, Down, Right, Restart, Quit };

        /// <summary>
        /// Generates random numbers
        /// </summary>
        static Random numberGenerator = new Random();

        /// <summary>
        /// Number of initial digits on a new 2048 board
        /// </summary>
        const int NUM_STARTING_DIGITS = 2;

        /// <summary>
        /// The chance of a two spawning
        /// </summary>
        const float CHANCE_OF_TWO = 0.9f; // 90% chance of a two; 10% chance of a four

        /// <summary>
        /// The size of the 2048 board
        /// </summary>
        const int BOARD_SIZE = 4; // 4x4

		/// <summary>
		/// The choice of user to quit or not
		/// </summary>
		static bool exit = false;

		/// <summary>
		/// The choice of user to restart or not
		/// </summary>
		static bool restart = false;

        /// <summary>
        /// Runs the game of 2048
        /// </summary>
        static void Main() {

			// Declare bool variables
			bool boardFull = false;
			bool gameOver = false;
			bool validMove;

			// Make the 2048 board
			int[,] theBoard = MakeBoard();

			// Repeat until player presses quit
			do {
				// Clear the board
				Console.Clear();

				// Display welcome message
				Console.WriteLine("2048 - Join the numbers and get to the 2048 tile!\n");
				
				// Display and update the 2048 board
				DisplayBoard(theBoard);
				
				// Display the possible moves
				Console.WriteLine("\nw: up        a:left" +
					"\ns: down      d: right" +
					"\n\nr: restart" +
					"\nq: quit");

				// Check if the game is over
				gameOver = CheckGameOver(theBoard);

				// If the game is over
				if (gameOver == true) {
					// Acknowledge the player and show options
					Console.WriteLine("\nGAME OVER" +
						"\nPlease choose restart or quit");
				}

				// Check the move of player and apply it 
				validMove = MakeMove(ChooseMove(), theBoard);

				// If the move of user is valid
				if (validMove == true){
					// Add value to an empty cell
					PopulateAnEmptyCell(theBoard);
				}

				// If the user press restart button
				if (restart == true){
					// Make a new board
					theBoard = MakeBoard();
					// Set the restart choice back to false
					restart = false;
				}
			} while (exit == false);
			// Repeat as long as the player does not press quit

			// Say goodbye when player presses quit
			Console.WriteLine("\nSee you later");
        }

        /// <summary>
        /// Generates a new 2048 board
        /// </summary>
        /// <returns>A new 2048 board</returns>
        public static int[,] MakeBoard() {
            // Make a BOARD_SIZExBOARD_SIZE array of integers (filled with zeros)
            int[,] board = new int[BOARD_SIZE, BOARD_SIZE];

            // Populate some random empty cells
            for (int i = 0; i < NUM_STARTING_DIGITS; i++) {
                PopulateAnEmptyCell(board);
            }

            return board;
        }

        /// <summary>
        /// Display the given 2048 board
        /// </summary>
        /// <param name="board">The 2048 board to display</param>
        public static void DisplayBoard(int[,] board) {
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int column = 0; column < board.GetLength(1); column++) {
                    Console.Write("{0,4}", board[row, column] == 0 ? "-" : board[row, column].ToString());
                }
                Console.WriteLine();
            }
        }

		/// <summary>
		/// Check if the game is over
		/// </summary>
		/// <param name="board">The 2048 board to check</param>
		/// <returns>True if the game is over, false otherwise</returns>
		public static bool CheckGameOver(int[,] board) {

			// Declares variables
			bool boardFull = IsFull(board);
			bool gameOver = true;
			int moveTotal = Enum.GetNames(typeof(Move)).Length;
			bool checkMove;

			// When the board is full
			if (boardFull == true) {
				// Create a copy of the board
				int[,] testBoard = board;

				// Check Up, Down, Left, Right moves
				for (int i = 0; i < moveTotal - 2; i++) {
					checkMove = MakeMove((Move)i, testBoard);

					// If those moves are possible
					if (checkMove == true) {
						// The game is not over
						gameOver = false;
					}
				}
			// Or if the board is not full
			} else if (boardFull == false) {
				// The game is not over
				gameOver = false;
			}
			return gameOver;
		}

        /// <summary>
        /// If the board is not full, choose a random empty cell and add a two or a four.
        /// There should be a 90% chance of adding a two, and a 10% chance of adding a four.
        /// </summary>
        /// <param name="board">The board to add a new number to</param>
        /// <returns>False if the board is already full; true otherwise</returns>
        public static bool PopulateAnEmptyCell(int[,] board) {
			
			// Declare constants
            const int FULL_PERCENTAGE = 100;
            const int ONE_PERCENTAGE = 1;
            const float PERCENTAGE_OF_TWO = FULL_PERCENTAGE * CHANCE_OF_TWO;

			// Declare variables
            int row;
            int column;
            bool boardFull = IsFull(board);
            bool boardNotFull;

			// If the board is not full
            if (boardFull == false) {

				// Find a random position that has the value of 0
                do {
                    row = numberGenerator.Next(board.GetLength(0));
                    column = numberGenerator.Next(board.GetLength(1));
                } while (board[row, column] != 0);

				// Generate a random number between 1 and 100
                int num = numberGenerator.Next(ONE_PERCENTAGE, FULL_PERCENTAGE + ONE_PERCENTAGE);

				// 90% of the possible outcomes
                if (ONE_PERCENTAGE <= num && num <= PERCENTAGE_OF_TWO) {
					// Change the value of the random position to 2
                    board[row, column] = 2;
				// 10% remains
                } else {
					// Change the value of the random position to 4
                    board[row, column] = 4;
                }
				// The board is not full
                boardNotFull = true; 
			// Otherwise the board  is full
            } else {
                boardNotFull = false;
            }
            return boardNotFull;

        }

        /// <summary>
        /// Returns true if the given 2048 board is full (contains no zeros)
        /// </summary>
        /// <param name="board">A 2048 board to check</param>
        /// <returns>True if the board is full; false otherwise</returns>
        public static bool IsFull(int[,] board) {

			// Declare variable and assume that the board is full
            bool boardFull = true;

			// Check the whole board
            for (int row = 0; row < board.GetLength(0); row++) {
                for (int column = 0; column < board.GetLength(1); column++) {
					// If there is a position which has the value of 0
                    if (board[row, column] == 0 ) {
						// The board is not full
                        boardFull = false;
                    }
                }
            }
            return boardFull;

        }

        /// <summary>
        /// Get a Move from the user (such as UP, LEFT, DOWN, RIGHT, RESTART or QUIT)
        /// </summary>
        /// <returns>The chosen Move</returns>
        public static Move ChooseMove() {

			// Take the input from player
            string userInput = Console.ReadKey().KeyChar.ToString();

			// If the player presses w
            if (userInput == "w") {
				// The move is Up
                return Move.Up;

			// If the player presses a
            } else if (userInput == "a") {
				//The move is Left
                return Move.Left;

			// If the player presses d
            } else if (userInput == "d") {
				// The move is Right
                return Move.Right;

			// If the player presses s
            } else if (userInput == "s") {
				// The move is Down
                return Move.Down;

			// If the player presses q
            } else if (userInput == "q") {
				// The move is Quit
                return Move.Quit;
			
			// If the player press r
            } else if (userInput == "r") {
				// The move is Restart
                return Move.Restart;
			
			// Otherwise the move is nothing
            } else {
                return (Move)6;
            }
        }


        /// <summary>
        /// Applies the chosen Move on the given 2048 board
        /// </summary>
        /// <param name="move">A move such as UP, LEFT, RIGHT or DOWN</param>
        /// <param name="board">A 2048 board</param>
        /// <returns>True if the move had an affect on the game; false otherwise</returns>
        public static bool MakeMove(Move move, int[,] board) {

			// Declare variable
			bool moveAffected = false;

			// If the move is Up
			if (move == Move.Up) {
				// Repeat to affect every column of the board
				for (int i = 0; i < BOARD_SIZE; i++) {
					// Take the column out of the board
					int[] column = MatrixExtensions.GetCol(board, i);
					// Shift and combine the column
					bool affectMove = ShiftCombineShift(column, true);
					// Put it back into the board
					MatrixExtensions.SetCol(board, i, column);
					// If the column changed
					if (affectMove == true) { 
						moveAffected = true;
					}
				}
			// If the move is Down
			} else if (move == Move.Down) {
				// Repeat to affect every column of the board
				for (int i = 0; i < BOARD_SIZE; i++) {
					// Take the column out of the board
					int[] column = MatrixExtensions.GetCol(board, i);
					// Shift and combine the column
					bool affectMove = ShiftCombineShift(column, false);
					// Put it back into the board
					MatrixExtensions.SetCol(board, i, column);
					// If the column changed
					if (affectMove == true) {
						moveAffected = true;
					}
				}
			// If the move is Left
			} else if (move == Move.Left) {
				// Repeat to affect every row of the board
				for (int i = 0; i < BOARD_SIZE; i++) {
					// Take the row out of the board
					int[] row = MatrixExtensions.GetRow(board, i);
					// Shift and combine the row
					bool affectMove = ShiftCombineShift(row, true);
					// Put it back into the board
					MatrixExtensions.SetRow(board, i, row);
					// If the row changed
					if (affectMove == true) {
						moveAffected = true;
					}
				}
			// If the move is Right
			} else if (move == Move.Right) {
				// Repeat to affect every row of the board
				for (int i = 0; i < BOARD_SIZE; i++) {
					// Take the row out of the board
					int[] row = MatrixExtensions.GetRow(board, i);
					// Shift and combine the row
					bool affectMove = ShiftCombineShift(row, false);
					// Put it back into the board
					MatrixExtensions.SetRow(board, i, row);
					// If the row changed
					if (affectMove == true) {
						moveAffected = true;
					}
				}
			// If the move is Restart
			} else if (move == Move.Restart) {
				// Change the choice to restart
				restart = true;

				moveAffected = false;

			// If the move is Quit
			} else if (move == Move.Quit) {
				// Change the choice to quit
				exit = true;

				moveAffected = false;

			// The move is invalid
			} else {
				moveAffected = false;
			}
			return moveAffected;
        }

		/// <summary>
		/// Shifts the non-zero integers in the given 1D array to the left
		/// </summary>
		/// <param name="nums">A 1D array of integers</param>
		/// <returns>True if shifting had an effect; false otherwise</returns>
		public static bool ShiftLeft(int[] nums) {

			// Declare variables
			bool leftShifted = false;
			bool containZero = false;
			int counter = 0;

			// Convert the array to a list
			List<int> numsList = nums.ToList();

			// Check every element in the array
			for (int i = 0; i + 1 < nums.Length; i++) {
				// If the array can be shifted to the left
				if (nums[i] == 0 && nums[i + 1] != 0) {
					leftShifted = true;
				}
			}

			// If the array can be shifted to the left
			if (leftShifted == true) {
				// Remove all the zeros out of the list
				do {
					// Assume that there is no zero
					containZero = false;
					// Detect zeros
					for (int i = 0; i < numsList.Count; i++) {
						if (numsList[i] == 0) {
							containZero = true;
							// Remove it
							numsList.RemoveAt(i);
							// Count it
							counter++;
						}
					}
				// Make sure there is no zero left
				} while (containZero == true);

				// Insert all the removed zeros to then end of the list
				for (int i = 0; i < counter; i++) {
					numsList.Insert(numsList.Count, 0);
				}
			}
			
			// Convert back to array
			int[] numsArray = numsList.ToArray();
			Array.Copy(numsArray, nums, nums.Length);


			return leftShifted;
        }

        /// <summary>
        /// Combines identical, non-zero integers that are adjacent to one another by summing 
        /// them in the left integer, and replacing the right-most integer with a zero
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <returns>True if combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3  }
        ///   It will be modified to:
        ///       { 0, 4, 0, 8, 0, 0, 0, 16, 0, 5, 3  }
        /// </example>
        public static bool CombineLeft(int[] nums) {

			// Declare variable
			bool leftCombined = false;

			// Check every element in the array
			for (int i = 0; i + 1 < nums.Length; i++) {
				// If the array can be combined to the left
				if (nums[i] != 0 && nums[i + 1] == nums[i]) {
					leftCombined = true;

					// Combine the array
					nums[i] = nums[i] + nums[i + 1];
					nums[i + 1] = 0;
				}
			}

			return leftCombined;
		}

        /// <summary>
        /// Shifts the numbers in the array in the specified direction, then combines them, then 
        /// shifts them again.
        /// </summary>
        /// <param name="nums">A 1D array of integers</param>
        /// <param name="left">True if numbers should be shifted to the left; false otherwise</param>
        /// <returns>True if shifting and combining had an effect; false otherwise</returns>
        /// <example>
        ///   If nums has the values below, and shiftLeft is true:
        ///       { 0, 2, 2,  4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 4, 8, 16, 5, 3, 0, 0, 0, 0, 0, 0  }
        ///       
        ///   If nums has the values below, and shiftLeft is false:
        ///       { 0, 2, 2, 4, 4, 0, 0, 8,  8, 5, 3 }
        ///   It will be modified to:
        ///       { 0, 0, 0, 0, 0, 0, 2, 8, 16, 5, 3 }
        /// </example>
        public static bool ShiftCombineShift(int[] nums, bool shiftLeft) {

			// Declare variables
			bool shiftedAndCombined = false;

			// If shift to left
			if (shiftLeft == true) {
				// Shift and combine to left
				bool shift = ShiftLeft(nums);
				bool combine = CombineLeft(nums);
				bool secondShift = ShiftLeft(nums);
				// Check if there is an effect
				if (shift == true || combine == true
					|| secondShift == true)	{
					shiftedAndCombined = true;
				}

			// If shift to the right
			} else {
				// Reverse the array and shift and combine
				Array.Reverse(nums);
				bool shift = ShiftLeft(nums);
				bool combine = CombineLeft(nums);
				bool secondShift = ShiftLeft(nums);
				// Reverse to the right
				Array.Reverse(nums);
				// Check if there is an effect
				if(shift == true || combine == true
					|| secondShift == true) {
					shiftedAndCombined = true;
				}
			}
			return shiftedAndCombined;
		}


    }
}