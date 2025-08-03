# Safari Simulator 🦓🦩🦛

A multi-threaded Windows Forms application that simulates a safari environment where different animals come to drink from lakes, each with unique behaviors and rules. This project demonstrates complex synchronization using C# to manage concurrent processes in a real-time visualization.

## 📸 Screenshots

Here are a few moments captured from the simulation, showcasing the dynamic behavior and rules of the different animals.

***

<br>

![A view of the safari with three active lakes.](http://googleusercontent.com/file_content/22)

**Hippo's Exclusive Access and Waiting Queues**
* **Hippo Rule:** In the leftmost lake, a **Hippopotamus** has taken exclusive access, forcing all other animals out.
* **Waiting Queues:** You can see animals (represented by the penguins from Madagascar in the buses) waiting in the queues above the lakes. They cannot enter the lake on the left because the hippo is present.
* **Concurrent Activity:** The other two lakes continue to operate independently, with zebras and a flamingo drinking simultaneously.

***

<br>

![A close-up view of a single, active lake.](http://googleusercontent.com/file_content/21)

**Flamingo Clustering and Zebra Placement**
* **Flamingo Rule:** This image clearly shows the flamingo placement rule. Several **Flamingos** are clustered together in a group around the lake.
* **Zebra Rule:** Two **Zebras** are also present, each occupying two "slots" in the lake. This demonstrates that multiple animal types can coexist peacefully when a hippo is not around.

***

<br>

![A wide view of the safari with five active lakes.](http://googleusercontent.com/file_content/20)

**Scalability and Dynamic Distribution**
* **Scalable Design:** This view demonstrates the application's ability to handle a variable number of lakes, showcasing its scalable architecture.
* **Dynamic State:** Each lake and queue is in a unique state, reflecting the random and concurrent nature of the simulation at any given moment.

## 📝 Original Task Description

> ### Hackathon Safari
>
> **Background:** In this assignment, you will simulate a Safari environment where various animals come to drink water from the lakes. Your task is to model and implement proper synchronization between multiple threads representing animals, using Mutexes and Semaphores to ensure correct and deadlock-free behavior.
>
> **Lakes:** There are 3 lakes with different capacities (measured in Flamingo units): Lake A: 5 slots, Lake B: 7 slots, Lake C: 10 slots. Each lake can be used simultaneously by animals unless a hippopotamus arrives, in which case all animals must vacate and no one else may enter.
>
> **Animal Types & Rules:**
>
> | Animal      | Arrival Time (μ) | Drink Time (μ) | Rules                                                                                                           |
> | :---------- | :--------------- | :------------- | :-------------------------------------------------------------------------------------------------------------- |
> | Flamingo    | 2.0 sec (normal) | 3.5 sec (normal) | If other flamingos are present, it must occupy an adjacent spot. If it's the first flamingo, it can take any empty spot. |
> | Zebra       | 3.0 sec (normal) | 5.0 sec (normal) | Can occupy any open space. Each Zebra takes space equivalent to 2 Flamingos.                                    |
> | Hippopotamus| 10.0 sec (normal)| 5.0 sec (normal) | Requires full exclusive access to the lake. All others must vacate. No concurrent Hippos.                       |

>
> Arrival and drinking times follow a normal distribution. Upon finishing drinking, animals leave the lake. Once an animal arrives, it is randomly allocated to the lake where it will drink.

## ✨ Key Features

* **Three Distinct Animal Types:** Flamingos, Zebras, and Hippos, each with unique behaviors and resource requirements.
* **Complex Placement Logic:** Implements specific rules, such as flamingos clustering together and hippos claiming exclusive lake access.
* **Concurrent & Asynchronous Operations:** Uses multi-threading to allow all lakes and animal generators to operate simultaneously and independently.
* **Dynamic Simulation:** Animal arrival and drinking times are based on a normal distribution for more realistic, non-deterministic behavior.
* **Real-time Visualization:** A Windows Forms GUI provides a live view of the animals at the lakes and in the waiting queues.
* **Scalable Design:** The application is architected to handle a variable number of lakes without requiring changes to the core logic.

## 🛠️ Technical Deep Dive & Architecture

The project is architected with a clear separation between the simulation logic (backend) and the user interface (frontend).

### Core Components

* `Animal`: An **abstract base class** that defines the core properties of all animals (e.g., `Type`, `DrinkTimeMs`, `Dead` flag).
* `Flamingo`, `Zebra`, `Hippopotamus`: **Concrete classes** inheriting from `Animal`, each implementing its specific characteristics.
* `AnimalGenerator`: Acts as the **producer** in a producer-consumer pattern. It runs on independent threads (one for each animal type) and continuously creates new animal instances, assigning them to a random lake.
* `Lake`: The heart of the simulation logic. Each `Lake` instance is a self-managing **controller** responsible for:
    * Managing its own waiting queue (`entranceQueue`).
    * Enforcing animal placement rules.
    * Cleaning up animals that have finished drinking.
* `Form1`: The **View** of the application. It is responsible only for rendering the state of the simulation. It uses a `System.Windows.Forms.Timer` to periodically read the public state of the `Lake` objects and update the display.

### Concurrency & Synchronization

This project heavily relies on multi-threading to simulate concurrent activities.

* **Producer-Consumer Pattern:** The `AnimalGenerator` produces `Animal` objects and places them into a `Lake`'s queue. The `Lake`'s internal `TryEnterLake` thread consumes these animals from the queue, decoupling animal creation from animal placement.
* **Thread-per-Lake Model:** Each `Lake` instance spawns its own two management threads:
    1.  **Entry Thread (`TryEnterLake`):** An infinite loop that processes the waiting queue and applies placement logic.
    2.  **Cleanup Thread (`StartGarbageCollector`):** An infinite loop that scans the lake for "dead" animals (those that have finished drinking) and removes them.
    This design ensures that each lake operates independently and in parallel, making the simulation highly concurrent and scalable.
* **Thread Safety:**
    * **`lock (entranceQueue)`:** The `lock` keyword is used to ensure thread-safe modifications to a lake's `entranceQueue`. This prevents race conditions, for example, where two `AnimalGenerator` threads might try to add an animal to the same lake's queue simultaneously.
    * **`IsBackground = true`:** All threads running infinite loops (in `AnimalGenerator` and `Lake`) are set as background threads. This is a critical setting that allows the main application to shut down cleanly without waiting for these infinite loops to complete.

## 🚀 How to Run the Project

1.  Clone the repository to your local machine.
2.  Open the `Safari.sln` file in Visual Studio.
3.  Build the solution (this will restore any necessary packages).
4.  Run the project by pressing F5 or the "Start" button.

## 🏗️ Built With

* **C#**
* **.NET Framework**
* **Windows Forms**
