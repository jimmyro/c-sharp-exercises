using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackbox
{
    class Program
    {
        /* INPUT FORMAT:
         * Top------Right----Bottom---Left
         * 01001000 00003000 02003000 00002000
         * 
         * :D  1. How many light paths are there? (3)
         *      we get a string of alphanumerics
         *      we need an array of ints
         * :D  2. Create elements w/ light number, side, and position
         *      e.g. { 1-T-2, 1-T-5, 3-R-5, 2-B-2, 3-B-5, 2-L-5 }
         * 3. Use logic to determine location and orientation of the mirrors
         * 
         * 
         *    '?' = not yet determined
         *    '.' = no mirror
         *    '1', '2', 'A', etc. = candidate for that path
         * 
         */

        static int sideLength;
        static CoordPair[] coordPairs;
        static char[,] boxMap;

        static void Main(string[] args)
        {
            #region Input

            Console.WriteLine("Input scan data");
            
            string rawInput = Console.ReadLine();
            sideLength = rawInput.Length / 4;

            Console.WriteLine("Working...");

            int[] numInput = new int[rawInput.Length];
            for (int i = 0; i < rawInput.Length; i++)
                numInput[i] = Convert.ToInt32(rawInput[i].ToString(), 16);

            #endregion

            #region Arrays

            //fill coordPairs instances
            int totalPaths = numInput.Max();
            coordPairs = new CoordPair[totalPaths];
            
            for (int i = 0; i < coordPairs.Length; i++)
                coordPairs[i] = new CoordPair();

            for (int i = 0; i < numInput.Length; i++)
            {
                if (numInput[i] == 0)
                    continue;
                
                //pos (start at 1) = (i + 1) % sideLength
                //side (start at 0) = ((int)Math.Floor((i + 1d) / sideLength))

                coordPairs[numInput[i] - 1].AddCoord((i + 1) % sideLength, (Side)((int)Math.Floor((i + 1d) / sideLength)));
                    //ew. sorry about that.
            }

            //create and fill the output array
            boxMap = new char[sideLength, sideLength];

            for (int y = 0; y < sideLength; y++)
                for (int x = 0; x < sideLength; x++)
                    boxMap[x, y] = '?';

            #endregion

            #region Logic

            //eliminate 0s
            for (int i = 0; i < sideLength * 2; i++)
            {
                if (numInput[i] == numInput[i + (sideLength * 2)])
                {
                    //in theory, this is true if and only if both values are equal to 0
                    if (i < sideLength) //top
                        for (int y = 0; y < sideLength; y++)
                            boxMap[i, y] = '.';
                    else //right
                        for (int x = 0; x < sideLength; x++)
                            boxMap[x, i % sideLength] = '.';
                }
            }

            int pathNumber = 1;
            foreach (CoordPair cp in coordPairs)
            {
                //  TR or LB --> \
                //  TL or RB --> /

                switch (Math.Abs(cp.Side1 - cp.Side2))
                {
                    case 0: //same
                        
                        break;
                    case 1: //adjacent
                    case 3:
                        break;
                    case 2: //opposite
                        break;
                    default:
                        break;
                }

                pathNumber++;
            }

            #endregion

            DisplayMap();

            Console.ReadLine();
        }

        static void DisplayMap()
        {
            Console.Clear();

            for (int y = 0; y < sideLength; y++)
            {
                for (int x = 0; x < sideLength; x++)
                    Console.Write(boxMap[x, y].ToString() + " ");

                Console.WriteLine();
            }
        }
    }

    public enum Side { top, right, bottom, left, nan }

    public class CoordPair
    {
        private int pos1, pos2;
        private Side side1, side2;

        public CoordPair()
        {
            pos1 = 0;
            pos2 = 0;
            side1 = Side.nan;
            side2 = Side.nan;
        }

        public int Pos1 { get { return pos1; } }
        public int Pos2 { get { return pos2; } }
        public Side Side1 { get { return side1; } }
        public Side Side2 { get { return side2; } }

        public void AddCoord(int myPos, Side mySide)
        {
            if (pos1 == 0 && side1 == Side.nan)
            {
                pos1 = myPos;
                side1 = mySide;
            }
            else if (pos2 == 0 && side2 == Side.nan)
            {
                pos2 = myPos;
                side2 = mySide;
            }
            else
                throw new Exception("FUUUUUUU-");
        }
    }
}
