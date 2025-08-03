using System;

namespace SafariSimulator //Connect the proj
{
    // Represents a Hippopotamus that requires exclusive access to a lake
    public class Hippopotamus : Animal
    {
        // Constructor: selects a random lake and sets size to MaxValue (requires full lake)
        public Hippopotamus(int lakesCount)
            : base(AnimalType.Hippopotamus, GetRandomLakeNum(lakesCount), int.MaxValue, 5000)
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