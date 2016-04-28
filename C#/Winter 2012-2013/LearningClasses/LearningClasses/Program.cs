using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*

the word 'private' is a security measure, 

think of the 'Person' class as a blueprint for a person, with its own variables and such.


*/

namespace LearningClasses
{
    class Person
    {
        private string name;
        private double weight;
        private double height;
        private bool isMale;

        public Person(string n, double w, double h, bool im) //this is a 'constructor'
        {
            name = n;
            weight = w;
            height = h;
            isMale = im;
        }

        public bool HasSameName(Person other)
        {
            if (name.Equals(other.GetName()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //this is a 'getter' to retrieve info safely
        public string GetName() {return name;}

        //this is a 'setter' to edit info safely
        public void SetName(string otherName) {name = otherName;}
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person Nosa = new Person("Nosa", 100, 20, true);
            Console.WriteLine(Nosa.GetName());

            Person Oliver = new Person("Oliver", 100, 20, true);
            if (!Oliver.HasSameName(Nosa))
            {
                Console.WriteLine(Oliver.GetName() + " doesn't have the same name as " + Nosa.GetName() + ".");
            }
            
            Console.ReadLine();
        }
    }
}
