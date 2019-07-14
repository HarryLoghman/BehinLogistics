namespace logisticCustomerGrabber
{
    partial class frm_customerGrabber
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
            this.date_history_toDate = new FarsiLibrary.Win.Controls.FADatePicker();
            this.date_history_fromDate = new FarsiLibrary.Win.Controls.FADatePicker();
            this.txtbx_history_wagonNo = new System.Windows.Forms.TextBox();
            this.btn_history = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // date_history_toDate
            // 
            this.date_history_toDate.Location = new System.Drawing.Point(471, 49);
            this.date_history_toDate.Name = "date_history_toDate";
            this.date_history_toDate.Size = new System.Drawing.Size(120, 20);
            this.date_history_toDate.TabIndex = 37;
            // 
            // date_history_fromDate
            // 
            this.date_history_fromDate.Location = new System.Drawing.Point(471, 20);
            this.date_history_fromDate.Name = "date_history_fromDate";
            this.date_history_fromDate.Size = new System.Drawing.Size(120, 20);
            this.date_history_fromDate.TabIndex = 36;
            // 
            // txtbx_history_wagonNo
            // 
            this.txtbx_history_wagonNo.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_history_wagonNo.Location = new System.Drawing.Point(471, 74);
            this.txtbx_history_wagonNo.Name = "txtbx_history_wagonNo";
            this.txtbx_history_wagonNo.Size = new System.Drawing.Size(120, 28);
            this.txtbx_history_wagonNo.TabIndex = 35;
            // 
            // btn_history
            // 
            this.btn_history.Location = new System.Drawing.Point(6, 69);
            this.btn_history.Name = "btn_history";
            this.btn_history.Size = new System.Drawing.Size(228, 33);
            this.btn_history.TabIndex = 34;
            this.btn_history.Text = "تاریخچه";
            this.btn_history.UseVisualStyleBackColor = true;
            this.btn_history.Click += new System.EventHandler(this.btn_history_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 109);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(682, 23);
            this.progressBar1.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(599, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 28);
            this.label1.TabIndex = 39;
            this.label1.Text = "از :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(599, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 28);
            this.label2.TabIndex = 40;
            this.label2.Text = "تا :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(599, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 28);
            this.label3.TabIndex = 41;
            this.label3.Text = "شماره واگن :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frm_customerGrabber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 144);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.date_history_toDate);
            this.Controls.Add(this.date_history_fromDate);
            this.Controls.Add(this.txtbx_history_wagonNo);
            this.Controls.Add(this.btn_history);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_customerGrabber";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "Customer Grabber";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarsiLibrary.Win.Controls.FADatePicker date_history_toDate;
        private FarsiLibrary.Win.Controls.FADatePicker date_history_fromDate;
        private System.Windows.Forms.TextBox txtbx_history_wagonNo;
        private System.Windows.Forms.Button btn_history;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}