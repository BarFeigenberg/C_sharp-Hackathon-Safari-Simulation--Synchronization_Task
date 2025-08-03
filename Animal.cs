using System;
using System.Threading;

namespace SafariSimulator //Connect the proj
{
    // Defines the types of animals in the simulation
    public enum AnimalType { Flamingo, Zebra, Hippopotamus } //set the names (ints)

    // Abstract base class for all animals
    public abstract class Animal
    {
        // Basic animal properties
        public AnimalType Type { get; protected set; }         //Type of animal
        public int Size { get; protected set; }                // Space required in lake
        public int DrinkTimeMs { get; protected set; }         // Duration to drink
        public int DesiredLakeNum { get; protected set; }      // Preferred lake index
        public string ImagePath { get; protected set; }        // Path to animal's image
        public bool Dead { get; set; } = false;                // Flag for when drinking ends

        private static Random rand = new Random(); //generate random number

        // Constructor sets type, lake, size, and randomized drink time
        protected Animal(AnimalType type, int desiredLakeNum, int size, int avgDrinkTime)
        {
            Type = type;
            Size = size;
            DesiredLakeNum = desiredLakeNum;
            DrinkTimeMs = GetNormallyDistributedTime(avgDrinkTime, 1000);
            ImagePath = $"Images/{type.ToString().ToLower()}.gif";
        }

        // Simulates a normally distributed random duration
        private int GetNormallyDistributedTime(int mean, int stddev)
        {
            // Get two non-zero random numbers between 0.0 and 1.0.
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();

            // Apply Box-Muller transform to create a standard normal distribution (bell curve).
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            // Scale the bell curve to the desired mean and standard deviation.
            double randNormal = mean + stddev * randStdNormal;
            // Return the result as an integer, with a minimum of 1000ms.
            return Math.Max(1000, (int)Math.Round(randNormal));
        }

        // Starts a thread to simulate drinking, sets Dead = true after drink time
        public void Drink()
        {
            new Thread(() =>
            {
                Thread.Sleep(DrinkTimeMs);
                Dead = true;
            }).Start();
        }
    }
}