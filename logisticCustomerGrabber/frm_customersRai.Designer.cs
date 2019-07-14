namespace logisticCustomerGrabber
{
    partial class frm_customersRai
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
            this.btn_trackWagons = new System.Windows.Forms.Button();
            this.txtbx_wagonNoStart = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtbx_wagonNoEnd = new System.Windows.Forms.TextBox();
            this.txtbx_get30DaysBeforeWagonNo = new System.Windows.Forms.TextBox();
            this.date_get30DaysBefore = new FarsiLibrary.Win.Controls.FADatePicker();
            this.txtbx_history_wagonNo = new System.Windows.Forms.TextBox();
            this.date_history_fromDate = new FarsiLibrary.Win.Controls.FADatePicker();
            this.date_history_toDate = new FarsiLibrary.Win.Controls.FADatePicker();
            this.numeric_cycleNumber = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkbx_trackWagons_get30DaysAgo = new System.Windows.Forms.CheckBox();
            this.btn_get30DaysBefore = new System.Windows.Forms.Button();
            this.btn_history = new System.Windows.Forms.Button();
            this.btn_billOfLadingTracking = new System.Windows.Forms.Button();
            this.txtbx_billOfLadingTracking_numbers = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_passengersLastStatus = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_cycleNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_trackWagons
            // 
            this.btn_trackWagons.Location = new System.Drawing.Point(12, 41);
            this.btn_trackWagons.Name = "btn_trackWagons";
            this.btn_trackWagons.Size = new System.Drawing.Size(228, 33);
            this.btn_trackWagons.TabIndex = 0;
            this.btn_trackWagons.Text = "ردیابی واگنها به صورت کلی";
            this.btn_trackWagons.UseVisualStyleBackColor = true;
            this.btn_trackWagons.Click += new System.EventHandler(this.btn_trackWagons_Click);
            // 
            // txtbx_wagonNoStart
            // 
            this.txtbx_wagonNoStart.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_wagonNoStart.Location = new System.Drawing.Point(246, 43);
            this.txtbx_wagonNoStart.Name = "txtbx_wagonNoStart";
            this.txtbx_wagonNoStart.Size = new System.Drawing.Size(100, 28);
            this.txtbx_wagonNoStart.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtbx_wagonNoStart, "Start WagonNo");
            this.txtbx_wagonNoStart.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_wagonNoEnd
            // 
            this.txtbx_wagonNoEnd.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_wagonNoEnd.Location = new System.Drawing.Point(352, 43);
            this.txtbx_wagonNoEnd.Name = "txtbx_wagonNoEnd";
            this.txtbx_wagonNoEnd.Size = new System.Drawing.Size(100, 28);
            this.txtbx_wagonNoEnd.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtbx_wagonNoEnd, "End WagonNo");
            this.txtbx_wagonNoEnd.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_get30DaysBeforeWagonNo
            // 
            this.txtbx_get30DaysBeforeWagonNo.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_get30DaysBeforeWagonNo.Location = new System.Drawing.Point(246, 81);
            this.txtbx_get30DaysBeforeWagonNo.Name = "txtbx_get30DaysBeforeWagonNo";
            this.txtbx_get30DaysBeforeWagonNo.Size = new System.Drawing.Size(100, 28);
            this.txtbx_get30DaysBeforeWagonNo.TabIndex = 28;
            this.toolTip1.SetToolTip(this.txtbx_get30DaysBeforeWagonNo, "WagonNo");
            this.txtbx_get30DaysBeforeWagonNo.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // date_get30DaysBefore
            // 
            this.date_get30DaysBefore.Location = new System.Drawing.Point(352, 85);
            this.date_get30DaysBefore.Name = "date_get30DaysBefore";
            this.date_get30DaysBefore.Size = new System.Drawing.Size(120, 20);
            this.date_get30DaysBefore.TabIndex = 29;
            this.toolTip1.SetToolTip(this.date_get30DaysBefore, "Check 30 days before this date");
            this.date_get30DaysBefore.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_history_wagonNo
            // 
            this.txtbx_history_wagonNo.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_history_wagonNo.Location = new System.Drawing.Point(246, 121);
            this.txtbx_history_wagonNo.Name = "txtbx_history_wagonNo";
            this.txtbx_history_wagonNo.Size = new System.Drawing.Size(100, 28);
            this.txtbx_history_wagonNo.TabIndex = 31;
            this.toolTip1.SetToolTip(this.txtbx_history_wagonNo, "WagonNo");
            this.txtbx_history_wagonNo.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // date_history_fromDate
            // 
            this.date_history_fromDate.Location = new System.Drawing.Point(352, 125);
            this.date_history_fromDate.Name = "date_history_fromDate";
            this.date_history_fromDate.Size = new System.Drawing.Size(120, 20);
            this.date_history_fromDate.TabIndex = 32;
            this.toolTip1.SetToolTip(this.date_history_fromDate, "Start date (from date)");
            this.date_history_fromDate.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // date_history_toDate
            // 
            this.date_history_toDate.Location = new System.Drawing.Point(478, 125);
            this.date_history_toDate.Name = "date_history_toDate";
            this.date_history_toDate.Size = new System.Drawing.Size(120, 20);
            this.date_history_toDate.TabIndex = 33;
            this.toolTip1.SetToolTip(this.date_history_toDate, "End date (to date)");
            this.date_history_toDate.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // numeric_cycleNumber
            // 
            this.numeric_cycleNumber.Location = new System.Drawing.Point(352, 9);
            this.numeric_cycleNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_cycleNumber.Name = "numeric_cycleNumber";
            this.numeric_cycleNumber.Size = new System.Drawing.Size(120, 23);
            this.numeric_cycleNumber.TabIndex = 25;
            this.numeric_cycleNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_cycleNumber.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(246, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 24;
            this.label1.Text = "CycleNumber";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkbx_trackWagons_get30DaysAgo
            // 
            this.chkbx_trackWagons_get30DaysAgo.Location = new System.Drawing.Point(459, 45);
            this.chkbx_trackWagons_get30DaysAgo.Name = "chkbx_trackWagons_get30DaysAgo";
            this.chkbx_trackWagons_get30DaysAgo.Size = new System.Drawing.Size(206, 24);
            this.chkbx_trackWagons_get30DaysAgo.TabIndex = 26;
            this.chkbx_trackWagons_get30DaysAgo.Text = "دریافت اطلاعات 30 روز گذشته";
            this.chkbx_trackWagons_get30DaysAgo.UseVisualStyleBackColor = true;
            // 
            // btn_get30DaysBefore
            // 
            this.btn_get30DaysBefore.Location = new System.Drawing.Point(12, 80);
            this.btn_get30DaysBefore.Name = "btn_get30DaysBefore";
            this.btn_get30DaysBefore.Size = new System.Drawing.Size(228, 33);
            this.btn_get30DaysBefore.TabIndex = 27;
            this.btn_get30DaysBefore.Text = "اطلاعات ۳۰ روز گذشته";
            this.btn_get30DaysBefore.UseVisualStyleBackColor = true;
            this.btn_get30DaysBefore.Click += new System.EventHandler(this.btn_get30DaysBefore_Click);
            // 
            // btn_history
            // 
            this.btn_history.Location = new System.Drawing.Point(12, 119);
            this.btn_history.Name = "btn_history";
            this.btn_history.Size = new System.Drawing.Size(228, 33);
            this.btn_history.TabIndex = 30;
            this.btn_history.Text = "تاریخچه";
            this.btn_history.UseVisualStyleBackColor = true;
            this.btn_history.Click += new System.EventHandler(this.btn_history_Click);
            // 
            // btn_billOfLadingTracking
            // 
            this.btn_billOfLadingTracking.Location = new System.Drawing.Point(12, 158);
            this.btn_billOfLadingTracking.Name = "btn_billOfLadingTracking";
            this.btn_billOfLadingTracking.Size = new System.Drawing.Size(228, 33);
            this.btn_billOfLadingTracking.TabIndex = 34;
            this.btn_billOfLadingTracking.Text = "رهگیری بارنامه ها";
            this.btn_billOfLadingTracking.UseVisualStyleBackColor = true;
            this.btn_billOfLadingTracking.Click += new System.EventHandler(this.btn_billOfLadingTracking_Click);
            // 
            // txtbx_billOfLadingTracking_numbers
            // 
            this.txtbx_billOfLadingTracking_numbers.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_billOfLadingTracking_numbers.Location = new System.Drawing.Point(246, 160);
            this.txtbx_billOfLadingTracking_numbers.Name = "txtbx_billOfLadingTracking_numbers";
            this.txtbx_billOfLadingTracking_numbers.Size = new System.Drawing.Size(352, 28);
            this.txtbx_billOfLadingTracking_numbers.TabIndex = 35;
            this.toolTip1.SetToolTip(this.txtbx_billOfLadingTracking_numbers, "شماره های بارنامه");
            this.txtbx_billOfLadingTracking_numbers.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(246, 191);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 42);
            this.label2.TabIndex = 36;
            this.label2.Text = "۱- ۱۰ (بارنامه های ۱ تا ۱۰)\r\n12,15,16(بارنامه های 12,15,16)\r\n";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_passengersLastStatus
            // 
            this.btn_passengersLastStatus.Location = new System.Drawing.Point(12, 233);
            this.btn_passengersLastStatus.Name = "btn_passengersLastStatus";
            this.btn_passengersLastStatus.Size = new System.Drawing.Size(228, 33);
            this.btn_passengersLastStatus.TabIndex = 37;
            this.btn_passengersLastStatus.Text = "آخرین وضعیت قطارهای مسافری";
            this.btn_passengersLastStatus.UseVisualStyleBackColor = true;
            this.btn_passengersLastStatus.Click += new System.EventHandler(this.btn_passengersLastStatus_Click);
            // 
            // frm_customersRai
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.btn_passengersLastStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbx_billOfLadingTracking_numbers);
            this.Controls.Add(this.btn_billOfLadingTracking);
            this.Controls.Add(this.date_history_toDate);
            this.Controls.Add(this.date_history_fromDate);
            this.Controls.Add(this.txtbx_history_wagonNo);
            this.Controls.Add(this.btn_history);
            this.Controls.Add(this.date_get30DaysBefore);
            this.Controls.Add(this.txtbx_get30DaysBeforeWagonNo);
            this.Controls.Add(this.btn_get30DaysBefore);
            this.Controls.Add(this.chkbx_trackWagons_get30DaysAgo);
            this.Controls.Add(this.numeric_cycleNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbx_wagonNoEnd);
            this.Controls.Add(this.txtbx_wagonNoStart);
            this.Controls.Add(this.btn_trackWagons);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Name = "frm_customersRai";
            this.Text = "Customers Rai";
            this.Load += new System.EventHandler(this.frm_customersRai_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numeric_cycleNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_trackWagons;
        private System.Windows.Forms.TextBox txtbx_wagonNoStart;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtbx_wagonNoEnd;
        private System.Windows.Forms.NumericUpDown numeric_cycleNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkbx_trackWagons_get30DaysAgo;
        private System.Windows.Forms.Button btn_get30DaysBefore;
        private System.Windows.Forms.TextBox txtbx_get30DaysBeforeWagonNo;
        private FarsiLibrary.Win.Controls.FADatePicker date_get30DaysBefore;
        private System.Windows.Forms.Button btn_history;
        private System.Windows.Forms.TextBox txtbx_history_wagonNo;
        private FarsiLibrary.Win.Controls.FADatePicker date_history_fromDate;
        private FarsiLibrary.Win.Controls.FADatePicker date_history_toDate;
        private System.Windows.Forms.Button btn_billOfLadingTracking;
        private System.Windows.Forms.TextBox txtbx_billOfLadingTracking_numbers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_passengersLastStatus;
    }
}