using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Lab
{
    //current types: slider, on-off, enum

    interface IMenuOption
    {
        string Name { get; set; }
        string TextValue { get; }

        void Increment();
        void Decrement();
    }

    class SliderOption : IMenuOption
    {
        private string name;
        private double lower, upper, increment, value;
        private int rounding;

        public SliderOption(string myName, double myLower, double myUpper, double myIncrement, int myRounding, double myDefault)
        {
            name = myName;
            lower = myLower;
            upper = myUpper;
            increment = myIncrement;

            if ((upper - lower) % increment > 0.1)
                throw new Exception();

            rounding = myRounding;
            value = myDefault;
        }

        public string Name { get { return name; } set { name = value; } }
        public string TextValue 
        { 
            get 
            {
                string s = "├";

                double location = Math.Round(10 * (value - lower) / (upper - lower + increment), 0);

                for (int i = 0; i < 10; i++)
                {
                    if (Program.AboutEqual(i, location))
                        s += "■";
                    else
                        s += "─";
                }

                string format = "";

                for (int i = upper.ToString().Length; i > 0; i--)
                {
                    if (i <= value.ToString().Length)
                        format += "0";
                    else
                        format += " ";
                }

                format += ".";

                for (int i = 0; i < rounding; i++)
                    format += "0";

                Debug.WriteLine("I got this format {0}", format);

                s += "┤ " + value.ToString(format);
                return s;
            }
        }

        public void Increment()
        {
            if (value < upper)
            {
                value += increment;

                if (value > upper)
                    value = upper;
            }
        }

        public void Decrement()
        {
            if (value > lower)
            {
                value -= increment;

                if (value < lower)
                    value = lower;
            }
        }
    }

    class OnOffOption : IMenuOption
    {
        private string name;
        private bool value;

        public OnOffOption(string myName, bool myDefault)
        {
            name = myName;
            value = myDefault;
        }

        public string Name { get { return name; } set { name = value; } }

        public string TextValue
        {
            get
            {
                if (value)
                    return "ON";
                else
                    return "OFF";
            }
        }

        public void Increment()
        {
            value = !value;
        }

        public void Decrement()
        {
            Increment();
        }
    }

    class EnumOption : IMenuOption
    {
        string name;
        public string Name { get { return name; } set { name = value; } }
        
        public string[] options;
        int position;

        public EnumOption(string myName, int defaultIndex, params string[] myOptions)
        {
            name = myName;
            position = defaultIndex;
            options = myOptions;
        }

        public string TextValue
        {
            get
            {
                return options[position];
            }
        }

        public void Increment()
        {
            if (position < options.Length - 1)
                position++;
            else
                position = 0;
        }

        public void Decrement()
        {
            if (position > 0)
                position--;
            else
                position = options.Length - 1;
        }
    }
}
