
namespace AutocompleteApplication
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
            this.loadDataSetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tokenizeButton = new System.Windows.Forms.ToolStripButton();
            this.sentenceTextBox = new System.Windows.Forms.TextBox();
            this.nGramsListBox = new System.Windows.Forms.ListBox();
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
            this.loadDataSetToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // loadDataSetToolStripMenuItem
            // 
            this.loadDataSetToolStripMenuItem.Name = "loadDataSetToolStripMenuItem";
            this.loadDataSetToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.loadDataSetToolStripMenuItem.Text = "Load data set";
            this.loadDataSetToolStripMenuItem.Click += new System.EventHandler(this.loadDataSetToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tokenizeButton});
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
            this.tokenizeButton.Size = new System.Drawing.Size(172, 22);
            this.tokenizeButton.Text = "Tokenize and Genrate n-grams";
            this.tokenizeButton.Click += new System.EventHandler(this.tokenizeButton_Click);
            // 
            // sentenceTextBox
            // 
            this.sentenceTextBox.AcceptsTab = true;
            this.sentenceTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.sentenceTextBox.Enabled = false;
            this.sentenceTextBox.Location = new System.Drawing.Point(0, 49);
            this.sentenceTextBox.Name = "sentenceTextBox";
            this.sentenceTextBox.Size = new System.Drawing.Size(800, 20);
            this.sentenceTextBox.TabIndex = 2;
            this.sentenceTextBox.TabStop = false;
            // 
            // nGramsListBox
            // 
            this.nGramsListBox.BackColor = System.Drawing.Color.Black;
            this.nGramsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nGramsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nGramsListBox.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nGramsListBox.ForeColor = System.Drawing.Color.Lime;
            this.nGramsListBox.FormattingEnabled = true;
            this.nGramsListBox.ItemHeight = 11;
            this.nGramsListBox.Location = new System.Drawing.Point(0, 69);
            this.nGramsListBox.Name = "nGramsListBox";
            this.nGramsListBox.Size = new System.Drawing.Size(800, 381);
            this.nGramsListBox.TabIndex = 6;
            this.nGramsListBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.nGramsListBox);
            this.Controls.Add(this.sentenceTextBox);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Autocompletion";
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
        private System.Windows.Forms.ToolStripMenuItem loadDataSetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tokenizeButton;
        private System.Windows.Forms.TextBox sentenceTextBox;
        private System.Windows.Forms.ListBox nGramsListBox;
    }
}

