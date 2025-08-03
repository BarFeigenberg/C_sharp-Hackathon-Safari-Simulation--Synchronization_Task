using System;

namespace SafariSimulator //Connect the proj
{
    // Represents a Zebra that occupies 2 flamingo units in a lake
    public class Zebra : Animal
    {
        // Constructor: assigns random lake, size = 2, average drink time = 5000ms
        public Zebra(int lakesCount)
            : base(AnimalType.Zebra, GetRandomLakeNum(lakesCount), 2, 5000)
        {
        }

        // Returns a random lake number from available lakes
        private static int GetRandomLakeNum(int total)
        {
            var rand = new Random();
            return rand.Next(total);
        }
    }
}