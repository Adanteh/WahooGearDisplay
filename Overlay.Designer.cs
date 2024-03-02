namespace WahooShift
{
    partial class Overlay
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ConnectionLabel = new Label();
            button1 = new Button();
            BottomField = new TextBox();
            SuspendLayout();
            // 
            // ConnectionLabel
            // 
            ConnectionLabel.Anchor = AnchorStyles.None;
            ConnectionLabel.AutoSize = true;
            ConnectionLabel.Font = new Font("Segoe UI", 16.2F, FontStyle.Regular, GraphicsUnit.Point);
            ConnectionLabel.ForeColor = Color.White;
            ConnectionLabel.Location = new Point(34, 24);
            ConnectionLabel.Name = "ConnectionLabel";
            ConnectionLabel.Size = new Size(176, 38);
            ConnectionLabel.TabIndex = 2;
            ConnectionLabel.Text = "Connecting...";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(222, 0);
            button1.Name = "button1";
            button1.Size = new Size(25, 25);
            button1.TabIndex = 3;
            button1.Text = "X";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // BottomField
            // 
            BottomField.Anchor = AnchorStyles.Bottom;
            BottomField.BackColor = Color.Black;
            BottomField.BorderStyle = BorderStyle.None;
            BottomField.ForeColor = Color.White;
            BottomField.Location = new Point(-6, 120);
            BottomField.Name = "BottomField";
            BottomField.Size = new Size(257, 20);
            BottomField.TabIndex = 4;
            BottomField.Text = "UP";
            BottomField.TextAlign = HorizontalAlignment.Center;
            // 
            // Overlay
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = Color.Black;
            CausesValidation = false;
            ClientSize = new Size(248, 145);
            ControlBox = false;
            Controls.Add(BottomField);
            Controls.Add(button1);
            Controls.Add(ConnectionLabel);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Overlay";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "Wahoo Gear Display";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ConnectionLabel;
        private Button button1;
        private TextBox BottomField;
    }
}