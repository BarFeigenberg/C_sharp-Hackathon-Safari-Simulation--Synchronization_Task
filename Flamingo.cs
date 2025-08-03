using System;

namespace SafariSimulator //Connect the proj
{
    // Represents a Flamingo that requires adjacent space and occupies 1 slot
    public class Flamingo : Animal
    {
        // Constructor: assigns random lake, size = 1, average drink time = 3500ms
        public Flamingo(int lakesCount)
            : base(AnimalType.Flamingo, GetRandomLakeNum(lakesCount), 1, 3500)
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