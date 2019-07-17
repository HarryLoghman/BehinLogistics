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
            this.components = new System.ComponentModel.Container();
            this.date_toDate = new FarsiLibrary.Win.Controls.FADatePicker();
            this.date_fromDate = new FarsiLibrary.Win.Controls.FADatePicker();
            this.txtbx_wagonNos = new System.Windows.Forms.TextBox();
            this.btn_grab = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbx_fromDate = new System.Windows.Forms.ComboBox();
            this.chkbx_startFromLastPage = new System.Windows.Forms.CheckBox();
            this.cmbx_toDate = new System.Windows.Forms.ComboBox();
            this.cmbx_wagonNos = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numeric_threadCount = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_threadCount)).BeginInit();
            this.SuspendLayout();
            // 
            // date_toDate
            // 
            this.date_toDate.Location = new System.Drawing.Point(28, 12);
            this.date_toDate.Name = "date_toDate";
            this.date_toDate.Size = new System.Drawing.Size(120, 20);
            this.date_toDate.TabIndex = 37;
            // 
            // date_fromDate
            // 
            this.date_fromDate.Location = new System.Drawing.Point(375, 14);
            this.date_fromDate.Name = "date_fromDate";
            this.date_fromDate.Size = new System.Drawing.Size(120, 20);
            this.date_fromDate.TabIndex = 36;
            // 
            // txtbx_wagonNos
            // 
            this.txtbx_wagonNos.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_wagonNos.Location = new System.Drawing.Point(28, 42);
            this.txtbx_wagonNos.Name = "txtbx_wagonNos";
            this.txtbx_wagonNos.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtbx_wagonNos.Size = new System.Drawing.Size(467, 28);
            this.txtbx_wagonNos.TabIndex = 35;
            this.toolTip1.SetToolTip(this.txtbx_wagonNos, "برای مشخص کردن شماره واگنها از ; و - استفاده کنید. به عنوان مثال 11000;12000 یعنی" +
        " ۲ تا واگن 11000و 12000 Grab شوند و 11000-12000 مشخص می کند که 1000 واکن Grab شو" +
        "ند امکان استفاده ترگکیبی نیز وجود دارد");
            // 
            // btn_grab
            // 
            this.btn_grab.Location = new System.Drawing.Point(25, 169);
            this.btn_grab.Name = "btn_grab";
            this.btn_grab.Size = new System.Drawing.Size(228, 33);
            this.btn_grab.TabIndex = 34;
            this.btn_grab.Text = "دریافت";
            this.btn_grab.UseVisualStyleBackColor = true;
            this.btn_grab.Click += new System.EventHandler(this.btn_grab_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(25, 209);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(682, 23);
            this.progressBar1.TabIndex = 38;
            this.progressBar1.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(646, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 28);
            this.label1.TabIndex = 39;
            this.label1.Text = "از :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(302, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 28);
            this.label2.TabIndex = 40;
            this.label2.Text = "تا :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(646, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 28);
            this.label3.TabIndex = 41;
            this.label3.Text = "شماره واگن :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbx_fromDate
            // 
            this.cmbx_fromDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbx_fromDate.FormattingEnabled = true;
            this.cmbx_fromDate.Items.AddRange(new object[] {
            "تاریخ انتخابی",
            "بیشترین تاریخ دریافت شده",
            "کمترین تاریخ دریافت شده"});
            this.cmbx_fromDate.Location = new System.Drawing.Point(502, 12);
            this.cmbx_fromDate.Name = "cmbx_fromDate";
            this.cmbx_fromDate.Size = new System.Drawing.Size(147, 24);
            this.cmbx_fromDate.TabIndex = 43;
            this.cmbx_fromDate.SelectedIndexChanged += new System.EventHandler(this.cmbx_fromDate_SelectedIndexChanged);
            // 
            // chkbx_startFromLastPage
            // 
            this.chkbx_startFromLastPage.Location = new System.Drawing.Point(12, 137);
            this.chkbx_startFromLastPage.Name = "chkbx_startFromLastPage";
            this.chkbx_startFromLastPage.Size = new System.Drawing.Size(707, 24);
            this.chkbx_startFromLastPage.TabIndex = 44;
            this.chkbx_startFromLastPage.Text = "شروع Grab از آخرین صفحه دریافت شده(توجه داشته باشید که تغییر بازه تاریخی می تواند" +
    " توالی صفحات را تغییر دهد)";
            this.chkbx_startFromLastPage.UseVisualStyleBackColor = true;
            // 
            // cmbx_toDate
            // 
            this.cmbx_toDate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbx_toDate.FormattingEnabled = true;
            this.cmbx_toDate.Items.AddRange(new object[] {
            "تاریخ انتخابی",
            "بیشترین تاریخ دریافت شده",
            "کمترین تاریخ دریافت شده"});
            this.cmbx_toDate.Location = new System.Drawing.Point(155, 10);
            this.cmbx_toDate.Name = "cmbx_toDate";
            this.cmbx_toDate.Size = new System.Drawing.Size(147, 24);
            this.cmbx_toDate.TabIndex = 42;
            this.cmbx_toDate.SelectedIndexChanged += new System.EventHandler(this.cmbx_toDate_SelectedIndexChanged);
            // 
            // cmbx_wagonNos
            // 
            this.cmbx_wagonNos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbx_wagonNos.DropDownWidth = 200;
            this.cmbx_wagonNos.FormattingEnabled = true;
            this.cmbx_wagonNos.Items.AddRange(new object[] {
            "همه واگنها",
            "واگنهای باقی مانده",
            "ورود شماره واگنها به صورت دستی"});
            this.cmbx_wagonNos.Location = new System.Drawing.Point(502, 44);
            this.cmbx_wagonNos.Name = "cmbx_wagonNos";
            this.cmbx_wagonNos.Size = new System.Drawing.Size(147, 24);
            this.cmbx_wagonNos.TabIndex = 45;
            this.cmbx_wagonNos.SelectedIndexChanged += new System.EventHandler(this.cmbx_wagonNos_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(617, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 17);
            this.label4.TabIndex = 46;
            this.label4.Text = "ماکزیمم TPS :";
            // 
            // numeric_threadCount
            // 
            this.numeric_threadCount.Location = new System.Drawing.Point(491, 175);
            this.numeric_threadCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_threadCount.Name = "numeric_threadCount";
            this.numeric_threadCount.Size = new System.Drawing.Size(120, 23);
            this.numeric_threadCount.TabIndex = 47;
            this.numeric_threadCount.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Info;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Tahoma", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(28, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(467, 61);
            this.label5.TabIndex = 48;
            this.label5.Text = "11000;12000 واگنهای 11000 و 12000 \r\n11000-12000 واگنهای از 11000 تا 12000\r\n11000-" +
    "12000;13000 واگنهای از 11000 تا 12000 و واگن 13000";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frm_customerGrabber
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 240);
            this.Controls.Add(this.date_fromDate);
            this.Controls.Add(this.chkbx_startFromLastPage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numeric_threadCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbx_wagonNos);
            this.Controls.Add(this.cmbx_fromDate);
            this.Controls.Add(this.cmbx_toDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.date_toDate);
            this.Controls.Add(this.txtbx_wagonNos);
            this.Controls.Add(this.btn_grab);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_customerGrabber";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "Customer Grabber History";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_threadCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FarsiLibrary.Win.Controls.FADatePicker date_toDate;
        private FarsiLibrary.Win.Controls.FADatePicker date_fromDate;
        private System.Windows.Forms.TextBox txtbx_wagonNos;
        private System.Windows.Forms.Button btn_grab;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbx_fromDate;
        private System.Windows.Forms.CheckBox chkbx_startFromLastPage;
        private System.Windows.Forms.ComboBox cmbx_toDate;
        private System.Windows.Forms.ComboBox cmbx_wagonNos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numeric_threadCount;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
    }
}