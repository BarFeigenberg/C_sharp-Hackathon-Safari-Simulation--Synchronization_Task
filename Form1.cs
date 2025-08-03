// Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;


namespace SafariSimulator
{
    public partial class Form1 : Form
    {
        private List<Lake> lakes = new();                         // List of lake objects
        private List<PictureBox> lakeBoxes = new();               // UI containers for each lake
        private List<PictureBox> busBoxes = new();                // UI containers for queues (buses)
        private Random rand = new();
        private System.Windows.Forms.PictureBox sun;              // Sun image display

        private List<int> lakeSizes = new List<int> { 1, 1, 3, 8 }; // Size of each lake
        private AnimalGenerator animalGenerator = new AnimalGenerator();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            InitializeLakes(lakeSizes);
            animalGenerator.Init(lakes.ToArray());
            animalGenerator.Start();

            guiTimer.Interval = 1000;                 // Refresh every second
            guiTimer.Tick += (s, ev) => UpdateGui();
            guiTimer.Start();
        }

        private void InitializeLakes(List<int> sizes)
        {
            lakes.Clear();
            for (int i = 0; i < sizes.Count; i++)
            {
                lakes.Add(new Lake(i, sizes[i]));     // Create lakes with index and size
            }

            // Add sun image to the background
            this.sun = new System.Windows.Forms.PictureBox();
            this.sun.Location = new System.Drawing.Point(1500, 0);
            this.sun.Size = new System.Drawing.Size(400, 400);
            this.sun.Image = Image.FromFile("Images/sun.gif");
            this.sun.SizeMode = PictureBoxSizeMode.StretchImage;
            this.sun.BackColor = Color.Transparent;
            this.sun.Name = "sun";
            this.sun.TabIndex = 1;
            this.sun.TabStop = false;
            this.Controls.Add(this.sun);

            CreateLakeControls(); // Create lake and bus PictureBoxes
        }

        private void CreateLakeControls()
        {
            // Remove previous controls
            foreach (var box in lakeBoxes) this.Controls.Remove(box);
            foreach (var box in busBoxes) this.Controls.Remove(box);
            lakeBoxes.Clear();
            busBoxes.Clear();

            int totalLakes = lakes.Count;
            int spacing = this.ClientSize.Width / totalLakes;
            int lakeHeight = 300;
            int lakeTop = this.ClientSize.Height - lakeHeight - 50;
            int busHeight = 120;
            int busOffsetY = 50;

            for (int i = 0; i < totalLakes; i++)
            {
                var lake = lakes[i];
                int lakeWidth = Math.Min(120 + lake.Capacity * 25, spacing - 40);
                int left = i * spacing + (spacing - lakeWidth) / 2;

                // Add lake image
                var lakeBox = new PictureBox
                {
                    Location = new Point(left, lakeTop),
                    Size = new Size(lakeWidth, lakeHeight),
                    Image = Image.FromFile("Images/lake.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };
                lakeBoxes.Add(lakeBox);
                this.Controls.Add(lakeBox);

                // Add bus image (animal queue)
                var busBox = new PictureBox
                {
                    Location = new Point(left, lakeTop - busHeight - busOffsetY),
                    Size = new Size(lakeWidth, busHeight),
                    Image = Image.FromFile("Images/bus.png"),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent
                };
                busBoxes.Add(busBox);
                this.Controls.Add(busBox);
            }
        }

        private void UpdateGui()
        {
            // Refresh visual state for each lake
            for (int i = 0; i < lakes.Count; i++)
            {
                UpdateLakeBox(lakeBoxes[i], busBoxes[i], lakes[i]);
            }
        }

        private void UpdateLakeBox(PictureBox lakeBox, PictureBox busBox, Lake lake)
        {
            lakeBox.Controls.Clear(); // Clear previous lake view
            busBox.Controls.Clear();  // Clear previous bus queue

            int centerX = lakeBox.Width / 2;
            int centerY = lakeBox.Height / 2;
            int radiusX = lakeBox.Width / 2 - 45;
            int radiusY = lakeBox.Height / 2 - 45;

            int capacity = lake.Capacity;

            for (int i = 0; i < capacity; i++)
            {
                var animal = lake.AnimalsInRawArray[i]; // Direct access by index
                if (animal == null) continue;

                if (animal.Type == AnimalType.Zebra)
                {
                    i += 1; // Skip second cell of a zebra (each one takes 2 slots)
                }

                int multiplier = animal.Type switch
                {
                    AnimalType.Flamingo => 2,
                    AnimalType.Zebra => 2,
                    AnimalType.Hippopotamus => 3,
                    _ => 1
                };

                double angle = 2 * Math.PI * i / capacity; // Distribute animals around circle
                int animalX = (int)(centerX + radiusX * Math.Cos(angle)) - (25 * multiplier);
                int animalY = (int)(centerY + radiusY * Math.Sin(angle)) - (25 * multiplier);

                PictureBox pb = new PictureBox
                {
                    Image = Image.FromFile(animal.ImagePath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(50 * multiplier, 50 * multiplier),
                    Location = new Point(animalX, animalY),
                    BackColor = Color.Transparent,
                    Parent = lakeBox
                };
                lakeBox.Controls.Add(pb);
                this.Controls.SetChildIndex(lakeBox, 0);
            }

            // Draw waiting animals in the queue above lake
            int stackX = 10;
            int stackY = 50;
            foreach (var waiting in lake.WaitingQueue)
            {
                PictureBox pb = new PictureBox
                {
                    Image = Image.FromFile(waiting.ImagePath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(60, 60),
                    Location = new Point(stackX, stackY),
                    BackColor = Color.Transparent,
                    Parent = busBox
                };
                busBox.Controls.Add(pb);
                stackX += 40;
            }
        }
    }
}
