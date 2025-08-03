using System;
using System.Collections.Generic;
using System.Threading;

namespace SafariSimulator
{
    /// <summary>
    /// Represents a lake that manages animal entry, placement, and cleanup in a simulated safari environment.
    /// </summary>
    public class Lake
    {
        private readonly SemaphoreSlim semaphore;
        private readonly Queue<Animal> entranceQueue = new();
        private readonly Animal[] lake;
        private readonly int lakeSize;
        private bool is_hepo = false;
        private int lakeIndex;

        public Animal[] AnimalsInRawArray => lake;

        public Lake(int index, int size)
        {
            lakeSize = size;
            lake = new Animal[size];
            semaphore = new SemaphoreSlim(size);

            StartGarbageCollector();
            Thread handler = new Thread(() => TryEnterLake());
            handler.IsBackground = true;
            handler.Start();
            lakeIndex = index;
        }

        public void TryAddAnimal(Animal animal)
        {
            if ((int)animal.Type == 2) // Hippo
            {
                AddHipo(animal);
            }
            else
            {
                lock (entranceQueue)
                {
                    entranceQueue.Enqueue(animal);
                }
            }
        }

        private void AddHipo(Animal hipo)
        {
            for (int i = 0; i < lakeSize; i++)
                lake[i] = null;

            lake[0] = hipo;
            is_hepo = true;
            Console.WriteLine("Hippopotamus entered the lake!");

            hipo.Drink();

            new Thread(() =>
            {
                while (!hipo.Dead) Thread.Sleep(200);
                is_hepo = false;
                Console.WriteLine("Hippopotamus left the lake.");
            }).Start();
        }

        private void TryEnterLake()
        {
            while (true)
            {
                if (is_hepo)
                {
                    Thread.Sleep(300);
                    continue;
                }

                Animal animal = null;

                lock (entranceQueue)
                {
                    if (entranceQueue.Count == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    animal = entranceQueue.Peek();
                }

                bool inserted = false;

                if (animal.Type == AnimalType.Flamingo)
                {
                    bool flamingoExists = false;
                    for (int i = 0; i < lakeSize; i++)
                    {
                        if (lake[i]?.Type == AnimalType.Flamingo)
                        {
                            flamingoExists = true;
                            break;
                        }
                    }

                    if (flamingoExists)
                    {
                        for (int i = 0; i < lakeSize; i++)
                        {
                            if (lake[i] == null)
                            {
                                // Circular check: first and last are considered adjacent
                                bool leftIsFlamingo = (i > 0 && lake[i - 1]?.Type == AnimalType.Flamingo) || (i == 0 && lake[lakeSize - 1]?.Type == AnimalType.Flamingo);
                                bool rightIsFlamingo = (i < lakeSize - 1 && lake[i + 1]?.Type == AnimalType.Flamingo) || (i == lakeSize - 1 && lake[0]?.Type == AnimalType.Flamingo);

                                if (leftIsFlamingo || rightIsFlamingo)
                                {
                                    lake[i] = animal;
                                    inserted = true;
                                    Console.WriteLine("Flamingo entered near another flamingo (with circular check).");
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        List<int> emptySlots = new();
                        Random rand = new();

                        for (int i = 0; i < lakeSize; i++)
                        {
                            if (lake[i] == null)
                                emptySlots.Add(i);
                        }

                        if (emptySlots.Count > 0)
                        {
                            int index = rand.Next(emptySlots.Count);
                            lake[emptySlots[index]] = animal;
                            inserted = true;
                            Console.WriteLine("Flamingo entered randomly (first one).");
                        }
                    }
                }
                else if (animal.Type == AnimalType.Zebra)
                {
                    for (int i = 0; i < lakeSize - 1; i++)
                    {
                        if (lake[i] == null && lake[i + 1] == null)
                        {
                            lake[i] = animal;
                            lake[i + 1] = animal;
                            inserted = true;
                            Console.WriteLine("Zebra entered lake.");
                            break;
                        }
                    }
                }

                if (inserted)
                {
                    animal.Drink();

                    lock (entranceQueue)
                    {
                        entranceQueue.Dequeue();
                    }
                }

                Thread.Sleep(100);
            }
        }

        private void StartGarbageCollector()
        {
            Thread gcThread = new Thread(() =>
            {
                while (true)
                {
                    for (int i = 0; i < lakeSize; i++)
                    {
                        if (lake[i]?.Dead == true)
                        {
                            if ((int)lake[i].Type == 2) is_hepo = false;
                            lake[i] = null;
                        }
                    }

                    Thread.Sleep(100);
                }
            });

            gcThread.IsBackground = true;
            gcThread.Start();
        }

        public int Capacity => lakeSize;

        public List<Animal> Animals
        {
            get
            {
                List<Animal> list = new();
                for (int i = 0; i < lakeSize; i++)
                {
                    if (lake[i] != null && !list.Contains(lake[i]))
                        list.Add(lake[i]);
                }
                return list;
            }
        }

        public IEnumerable<Animal> WaitingQueue
        {
            get
            {
                lock (entranceQueue)
                {
                    return entranceQueue.ToArray();
                }
            }
        }
    }
}