namespace SafariSimulator
{
    partial class Form1
    {
        // Container for components (used by the designer)
        private System.ComponentModel.IContainer components = null;

        // Timer to refresh or update GUI periodically
        private System.Windows.Forms.Timer guiTimer;

        // Disposes resources used by the form
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        // Initializes the GUI components and layout
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.guiTimer = new System.Windows.Forms.Timer(this.components);

            this.SuspendLayout();

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1600, 900);  // Set form size
            this.Name = "Form1";                                 // Set form name
            this.Text = "Safari Simulator";                     // Set form title
            this.Load += new System.EventHandler(this.Form1_Load);  // Register Load event

            // Set background image and layout style
            this.BackgroundImage = System.Drawing.Image.FromFile("Images/sand_background.jpg");
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            // Add visual element (e.g., sun image) to the form
            this.Controls.Add(this.sun);

            this.ResumeLayout(false);
        }
    }
}