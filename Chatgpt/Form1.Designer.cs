namespace Chatgpt
{
    partial class ApiTest
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
            Search_txt = new TextBox();
            Send_btn = new Button();
            Show_txt = new TextBox();
            History_btn = new Button();
            SuspendLayout();
            // 
            // Search_txt
            // 
            Search_txt.Location = new Point(37, 39);
            Search_txt.Name = "Search_txt";
            Search_txt.Size = new Size(606, 23);
            Search_txt.TabIndex = 0;
            // 
            // Send_btn
            // 
            Send_btn.Location = new Point(679, 39);
            Send_btn.Name = "Send_btn";
            Send_btn.Size = new Size(75, 23);
            Send_btn.TabIndex = 1;
            Send_btn.Text = "送出";
            Send_btn.UseVisualStyleBackColor = true;
            Send_btn.Click += Send_btn_Click;
            // 
            // Show_txt
            // 
            Show_txt.Location = new Point(37, 90);
            Show_txt.Multiline = true;
            Show_txt.Name = "Show_txt";
            Show_txt.Size = new Size(717, 318);
            Show_txt.TabIndex = 2;
            // 
            // History_btn
            // 
            History_btn.Location = new Point(37, 423);
            History_btn.Name = "History_btn";
            History_btn.Size = new Size(75, 23);
            History_btn.TabIndex = 3;
            History_btn.Text = "歷史紀錄";
            History_btn.UseVisualStyleBackColor = true;
            History_btn.Click += History_btn_Click;
            // 
            // ApiTest
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(787, 458);
            Controls.Add(History_btn);
            Controls.Add(Show_txt);
            Controls.Add(Send_btn);
            Controls.Add(Search_txt);
            Name = "ApiTest";
            Text = "ApiTest";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Search_txt;
        private Button Send_btn;
        private TextBox Show_txt;
        private Button History_btn;
    }
}