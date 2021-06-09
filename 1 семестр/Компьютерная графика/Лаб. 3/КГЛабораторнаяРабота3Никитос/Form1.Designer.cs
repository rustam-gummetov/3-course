namespace Interpolation
{
    partial class Form1
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
            this.pbGouraud = new System.Windows.Forms.PictureBox();
            this.pbBarycentric = new System.Windows.Forms.PictureBox();
            this.pbInitialImage = new System.Windows.Forms.PictureBox();
            this.pbScaledImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbGouraud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarycentric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInitialImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScaledImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbGouraud
            // 
            this.pbGouraud.Location = new System.Drawing.Point(16, 15);
            this.pbGouraud.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbGouraud.Name = "pbGouraud";
            this.pbGouraud.Size = new System.Drawing.Size(853, 665);
            this.pbGouraud.TabIndex = 0;
            this.pbGouraud.TabStop = false;
            // 
            // pbBarycentric
            // 
            this.pbBarycentric.Location = new System.Drawing.Point(921, 15);
            this.pbBarycentric.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbBarycentric.Name = "pbBarycentric";
            this.pbBarycentric.Size = new System.Drawing.Size(853, 665);
            this.pbBarycentric.TabIndex = 1;
            this.pbBarycentric.TabStop = false;
            // 
            // pbInitialImage
            // 
            this.pbInitialImage.Location = new System.Drawing.Point(16, 714);
            this.pbInitialImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbInitialImage.Name = "pbInitialImage";
            this.pbInitialImage.Size = new System.Drawing.Size(339, 298);
            this.pbInitialImage.TabIndex = 2;
            this.pbInitialImage.TabStop = false;
            // 
            // pbScaledImage
            // 
            this.pbScaledImage.Location = new System.Drawing.Point(921, 714);
            this.pbScaledImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbScaledImage.Name = "pbScaledImage";
            this.pbScaledImage.Size = new System.Drawing.Size(828, 700);
            this.pbScaledImage.TabIndex = 3;
            this.pbScaledImage.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1809, 1055);
            this.Controls.Add(this.pbScaledImage);
            this.Controls.Add(this.pbInitialImage);
            this.Controls.Add(this.pbBarycentric);
            this.Controls.Add(this.pbGouraud);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbGouraud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBarycentric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInitialImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbScaledImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGouraud;
        private System.Windows.Forms.PictureBox pbBarycentric;
        private System.Windows.Forms.PictureBox pbInitialImage;
        private System.Windows.Forms.PictureBox pbScaledImage;
    }
}

