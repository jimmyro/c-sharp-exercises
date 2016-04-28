//Jan. 9, 2013 class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    class Card
    {
        private int value; //numerical value of card
        private string suit;
        //private string name;
        private string symbol; //e.g. K, A, 10

        public Card(int myValue, string mySuit) //constructor (e.g. Card myCard = new Card();)
        {
            value = myValue;
            suit = mySuit;

            symbol = value.ToString();
            if (value == 1)
            {
                symbol = "Ace";
            }
            else if (value == 11)
            {
                symbol = "Jack";
            }
            else if (value == 12)
            {
                symbol = "Queen";
            }
            else if (value == 13)
            {
                symbol = "King";
            }
        }

        public string ToString() //override the built-in ToString method automatically attached to all objects
        {
            return symbol + " of " + suit;
        }

        //getters (we won't have setters b/c they aren't necessary => "immutable data type")
        public int Value()
        {
            return value;
        }

        public string Suit()
        {
            return suit;
        }

        public string Symbol()
        {
            return symbol;
        }
    } //create a card object with attributes, methods, etc.

    class Program
    {
        static void Main(string[] args)
        {
            List<Card> deck = CreateDeck();

            Shuffle(deck);

            /* //test whether deck has been initialized correctly
            foreach (Card c in deck)
            {
                Console.WriteLine(c.ToString());
            } */

            List<Card> myHand = new List<Card>();

            myHand.Add(Deal(deck));
            myHand.Add(Deal(deck));

            foreach (Card c in myHand)
            {
                Console.WriteLine(c.ToString());
            }
            Console.WriteLine("Your hand's max value = " + CalculateMaxValue(myHand));

            Console.ReadLine(); //pause
        }

        static int CalculateMaxValue(List<Card> hand)
        {
            int handValue = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].Value() == 1) //let all aces count for 11
                {
                    handValue += 11;
                }
                else if (hand[i].Value() <= 10)
                {
                    handValue += hand[i].Value();
                }
                else //handle 10 and face cards
                {
                    handValue += 10;
                }
            }
            return handValue;
        } //returns the value of a hand

        static Card Deal(List<Card> deck) //returns the top card
        {
            Card card = deck[0]; //remove the top card
            deck.Remove(card);

            return card;
        }

        static void Shuffle(List<Card> deck) //Knuth shuffle
        {
            Random rng = new Random();
            for (int i = deck.Count - 1; i >= 0; i--) //deck.Count - 1 because the List<Card> begins at index 0
            {
                int rand = rng.Next(i+1); //random number
                                        //from 0..i-1

                Card tmp = deck[i];
                deck[i] = deck[rand];
                deck[rand] = tmp;
            }
        }

        static List<Card> CreateDeck() //create a deck of our Card objects using nested 'for' loops
        {
            List<Card> deck = new List<Card>();

            for (int val = 1; val <= 13; val++)
            {
                for (int i = 0; i < 4; i++)
                {
                    //clubs, hearts, diamonds, spades
                    string suit = "";
                    if (i == 0)
                    {
                        suit = "clubs";
                    }
                    else if (i == 1)
                    {
                        suit = "hearts";
                    }
                    else if (i == 2)
                    {
                        suit = "diamonds";
                    }
                    else
                    {
                        suit = "spades";
                    }

                    Card myCard = new Card(val, suit);
                    deck.Add(myCard); //if this weren't here, the compiler's garbage collector would annihilate it
                }
            }

            return deck;
        }
    }
}
