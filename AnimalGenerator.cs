using System;
using System.Threading;

namespace SafariSimulator
{
    /// <summary>
    /// Responsible for generating and assigning animals to lakes at randomized intervals.
    /// </summary>
    public class AnimalGenerator
    {
        private Lake[] lakes;
        private static Random rand = new Random();


        /// <summary>
        /// Initializes the animal generator with the specified lakes.
        /// </summary>
        /// <param name="lakeArray">Array of lakes where animals can be added.</param>
        public void Init(Lake[] lakeArray)
        {
            lakes = lakeArray;
        }


        /// <summary>
        /// Starts the simulation by launching threads to generate different animal types.
        /// </summary>
        public void Start()
        {
            StartAnimalThread(() => new Flamingo(lakes.Length), 2000);
            StartAnimalThread(() => new Zebra(lakes.Length), 3000);
            StartAnimalThread(() => new Hippopotamus(lakes.Length), 10000);
        }


        /// <summary>
        /// Launches a background thread that continuously generates animals at a normally distributed interval.
        /// </summary>
        /// <param name="createAnimal">A delegate that creates a new animal instance.</param>
        /// <param name="arrivalMean">The mean time (in milliseconds) between animal arrivals.</param>
        private void StartAnimalThread(Func<Animal> createAnimal, int arrivalMean)
        {
            Thread thread = new Thread(() =>
            {
                while (true) // continue to generate animails while the program run
                {
                    Animal cur = createAnimal();
                    int lakeIndex = cur.DesiredLakeNum;

                    if (lakeIndex >= 0 && lakeIndex < lakes.Length)
                    {
                        lakes[lakeIndex].TryAddAnimal(cur);
                    }

                    int delay = GetNormallyDistributedDelay(arrivalMean, 1000);
                    Thread.Sleep(delay);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }


        /// <summary>
        /// Generates a delay time based on a normal distribution.
        /// </summary>
        /// <param name="mean">The mean of the distribution (in milliseconds).</param>
        /// <param name="stddev">The standard deviation of the distribution (in milliseconds).</param>
        /// <returns>A delay time in milliseconds, constrained to a minimum of 100 ms.</returns>
        private int GetNormallyDistributedDelay(int mean, int stddev)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            double randNormal = mean + stddev * randStdNormal;

            return Math.Max(100, (int)Math.Round(randNormal));
        }
    }
}