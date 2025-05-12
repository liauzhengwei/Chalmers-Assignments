namespace LargeListsExample
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
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.sizeLabel = new System.Windows.Forms.ToolStripLabel();
            this.dataSetSizeTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.generateDataButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.numberOfCallsTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.generateStatisticsButton = new System.Windows.Forms.ToolStripButton();
            this.resultsTextBox = new System.Windows.Forms.TextBox();
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
            this.menuStrip1.Size = new System.Drawing.Size(849, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeLabel,
            this.dataSetSizeTextBox,
            this.generateDataButton,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.numberOfCallsTextBox,
            this.generateStatisticsButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(849, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // sizeLabel
            // 
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(27, 22);
            this.sizeLabel.Text = "Size";
            // 
            // dataSetSizeTextBox
            // 
            this.dataSetSizeTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dataSetSizeTextBox.Name = "dataSetSizeTextBox";
            this.dataSetSizeTextBox.Size = new System.Drawing.Size(70, 25);
            this.dataSetSizeTextBox.Text = "100000";
            // 
            // generateDataButton
            // 
            this.generateDataButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.generateDataButton.Image = ((System.Drawing.Image)(resources.GetObject("generateDataButton.Image")));
            this.generateDataButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.generateDataButton.Name = "generateDataButton";
            this.generateDataButton.Size = new System.Drawing.Size(84, 22);
            this.generateDataButton.Text = "Generate data";
            this.generateDataButton.Click += new System.EventHandler(this.generateDataButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(94, 22);
            this.toolStripLabel2.Text = "Number of calls:";
            // 
            // numberOfCallsTextBox
            // 
            this.numberOfCallsTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numberOfCallsTextBox.Name = "numberOfCallsTextBox";
            this.numberOfCallsTextBox.Size = new System.Drawing.Size(70, 25);
            this.numberOfCallsTextBox.Text = "10000";
            // 
            // generateStatisticsButton
            // 
            this.generateStatisticsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.generateStatisticsButton.Enabled = false;
            this.generateStatisticsButton.Image = ((System.Drawing.Image)(resources.GetObject("generateStatisticsButton.Image")));
            this.generateStatisticsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.generateStatisticsButton.Name = "generateStatisticsButton";
            this.generateStatisticsButton.Size = new System.Drawing.Size(106, 22);
            this.generateStatisticsButton.Text = "Generate statistics";
            this.generateStatisticsButton.Click += new System.EventHandler(this.generateStatisticsButton_Click);
            // 
            // resultsTextBox
            // 
            this.resultsTextBox.BackColor = System.Drawing.Color.Black;
            this.resultsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.resultsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsTextBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsTextBox.ForeColor = System.Drawing.Color.Lime;
            this.resultsTextBox.Location = new System.Drawing.Point(0, 49);
            this.resultsTextBox.Multiline = true;
            this.resultsTextBox.Name = "resultsTextBox";
            this.resultsTextBox.Size = new System.Drawing.Size(849, 250);
            this.resultsTextBox.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 299);
            this.Controls.Add(this.resultsTextBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Large lists example";
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
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton generateDataButton;
        private System.Windows.Forms.TextBox resultsTextBox;
        private System.Windows.Forms.ToolStripLabel sizeLabel;
        private System.Windows.Forms.ToolStripTextBox dataSetSizeTextBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox numberOfCallsTextBox;
        private System.Windows.Forms.ToolStripButton generateStatisticsButton;
    }
}

