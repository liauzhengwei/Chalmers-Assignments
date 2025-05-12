
namespace BayesianClassifierApplication
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTrainingSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTestSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tokenizeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.progressListBox = new System.Windows.Forms.ListBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadTrainingSetToolStripMenuItem,
            this.loadTestSetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadTrainingSetToolStripMenuItem
            // 
            this.loadTrainingSetToolStripMenuItem.Name = "loadTrainingSetToolStripMenuItem";
            this.loadTrainingSetToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.loadTrainingSetToolStripMenuItem.Text = "Load training set";
            this.loadTrainingSetToolStripMenuItem.Click += new System.EventHandler(this.loadTrainingSetToolStripMenuItem_Click);
            // 
            // loadTestSetToolStripMenuItem
            // 
            this.loadTestSetToolStripMenuItem.Name = "loadTestSetToolStripMenuItem";
            this.loadTestSetToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.loadTestSetToolStripMenuItem.Text = "Load test set";
            this.loadTestSetToolStripMenuItem.Click += new System.EventHandler(this.loadTestSetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tokenizeButton,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tokenizeButton
            // 
            this.tokenizeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tokenizeButton.Enabled = false;
            this.tokenizeButton.Image = ((System.Drawing.Image)(resources.GetObject("tokenizeButton.Image")));
            this.tokenizeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tokenizeButton.Name = "tokenizeButton";
            this.tokenizeButton.Size = new System.Drawing.Size(57, 22);
            this.tokenizeButton.Text = "Tokenize";
            this.tokenizeButton.Click += new System.EventHandler(this.tokenizeButton_Click);
            // 
            // trainButton
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "trainButton";
            this.toolStripButton1.Size = new System.Drawing.Size(37, 22);
            this.toolStripButton1.Text = "Train";
            this.toolStripButton1.Click += new System.EventHandler(this.trainButton_Click);
            // 
            // testButton
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "testButton";
            this.toolStripButton2.Size = new System.Drawing.Size(32, 22);
            this.toolStripButton2.Text = "Test";
            this.toolStripButton2.Click += new System.EventHandler(this.testButton_Click);
            // 
            // progressListBox
            // 
            this.progressListBox.BackColor = System.Drawing.Color.Black;
            this.progressListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.progressListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressListBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressListBox.ForeColor = System.Drawing.Color.Lime;
            this.progressListBox.FormattingEnabled = true;
            this.progressListBox.ItemHeight = 11;
            this.progressListBox.Location = new System.Drawing.Point(0, 49);
            this.progressListBox.Name = "progressListBox";
            this.progressListBox.Size = new System.Drawing.Size(800, 401);
            this.progressListBox.TabIndex = 4;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressListBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bayesian classifier";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTrainingSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTestSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tokenizeButton;
        private System.Windows.Forms.ListBox progressListBox;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

