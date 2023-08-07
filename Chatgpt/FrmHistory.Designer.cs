namespace Chatgpt
{
    partial class FrmHistory
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
            Date_cb = new ComboBox();
            Weatherdata_dg = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)Weatherdata_dg).BeginInit();
            SuspendLayout();
            // 
            // Date_cb
            // 
            Date_cb.DropDownStyle = ComboBoxStyle.DropDownList;
            Date_cb.FormattingEnabled = true;
            Date_cb.Location = new Point(26, 23);
            Date_cb.Name = "Date_cb";
            Date_cb.Size = new Size(410, 23);
            Date_cb.TabIndex = 0;
            Date_cb.SelectedIndexChanged += Date_cb_SelectedIndexChanged;
            // 
            // Weatherdata_dg
            // 
            Weatherdata_dg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Weatherdata_dg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Weatherdata_dg.Location = new Point(26, 70);
            Weatherdata_dg.Name = "Weatherdata_dg";
            Weatherdata_dg.ReadOnly = true;
            Weatherdata_dg.RowTemplate.Height = 25;
            Weatherdata_dg.Size = new Size(410, 284);
            Weatherdata_dg.TabIndex = 1;
            // 
            // FrmHistory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(461, 386);
            Controls.Add(Weatherdata_dg);
            Controls.Add(Date_cb);
            Name = "FrmHistory";
            Text = "FrmHistory";
            Load += FrmHistory_Load;
            ((System.ComponentModel.ISupportInitialize)Weatherdata_dg).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ComboBox Date_cb;
        private DataGridView Weatherdata_dg;
    }
}