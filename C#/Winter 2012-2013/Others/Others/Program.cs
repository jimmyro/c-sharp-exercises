using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            /* PROBLEMS 1-3-13
             * second one (mode = 2) takes too long to figure
             * 
             */

            //list modes and ask for input
            string[] modes = {"Add all the natural numbers below 1000 that are multiples of 3 or 5.",
                              "Find the largest prime factor of a given composite number n = 600851475143.",
                              "Find the only Pythagorean triplet, {a,b,c}, for which a + b + c = 1000.",
                              "Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.",
                              "What is the first term in the Fibonacci sequence to contain 1000 digits?"};

            Console.WriteLine("Please input the number for one of the following modes:");

            for (int i = 1; i <= modes.Length; i++)
            {
                    Console.WriteLine(i + " | " + modes[i-1]);
            }

            while (true)
            {
                int mode;

                //interpret input and so on
                if (int.TryParse(Console.ReadLine(), out mode) && 0 < mode && mode <= modes.Length)
                {
                    switch (mode)
                    {
                        case 1:
                            //it doesn't matter whether zero is a natural number or not
                            int sum = 0;

                            for (int i = 0; i < 1000; i++)
                            {
                                if (i % 3 == 0 || i % 5 == 0)
                                {
                                    sum += i;
                                }
                            }

                            Console.WriteLine(sum);
                            break;

                        case 2:
                            //step 1, wait for a factor and test for a prime each time
                            long highestPrime = 0;

                            for (long i = 2; i < 600851475143; i++)
                            {
                                if (600851475143 % i == 0) //found a factor
                                {
                                    bool prime = true;

                                    for (long j = 2; j < i; j++)
                                    {
                                        if (i % j == 0) { prime = false; break; }
                                    }

                                    if (prime) { highestPrime = i; } else { continue; }
                                }
                            }

                            //step 2, report findings
                            Console.WriteLine(highestPrime);
                            break;
                        
                        case 3:
                            //no idea
                            Console.WriteLine("Can't help you there, buddy");

                            break;
                        case 4:
                            //assume that zero is NOT a natural number
                            int sum_of_sqs = 0; int sum2 = 0;
                            
                            for (int i = 1; i <= 100; i++)
                            {
                                sum_of_sqs = sum_of_sqs + (i ^ 2);
                                sum2 = sum2 + i;
                            }

                            int answer = sum_of_sqs - (sum2 ^ 2);

                            Console.WriteLine(sum_of_sqs);
                            Console.WriteLine(sum2);
                            Console.WriteLine(answer);
                            break;
                        
                        case 5:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Please input an integer within range");
                    Console.WriteLine();
                    continue;
                }
            }
        }
    }
}
