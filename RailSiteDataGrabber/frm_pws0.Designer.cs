namespace RailSiteDataGrabber
{
    partial class frm_pws0
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
            this.btn_getAgencies = new System.Windows.Forms.Button();
            this.btn_getStations = new System.Windows.Forms.Button();
            this.btn_getStates = new System.Windows.Forms.Button();
            this.btn_getCities = new System.Windows.Forms.Button();
            this.btn_getTrainPassengers = new System.Windows.Forms.Button();
            this.btn_getTrainBarries = new System.Windows.Forms.Button();
            this.btn_getTrainBarryLocomotives = new System.Windows.Forms.Button();
            this.btn_getTrainBarryBillOfLadings = new System.Windows.Forms.Button();
            this.btn_findTrainBarryWagon = new System.Windows.Forms.Button();
            this.btn_findTrainBarry = new System.Windows.Forms.Button();
            this.txtbx_stateCode = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtbx_locomotives_F15Rec_ID = new System.Windows.Forms.TextBox();
            this.txtbx_BillOfLading_F15Rec_ID = new System.Windows.Forms.TextBox();
            this.txtbx_BillOfLading_Train_No = new System.Windows.Forms.TextBox();
            this.txtbx_wagon_no = new System.Windows.Forms.TextBox();
            this.txtbx_train_no = new System.Windows.Forms.TextBox();
            this.txtbx_barnameh_no = new System.Windows.Forms.TextBox();
            this.chkbx_getAllCities = new System.Windows.Forms.CheckBox();
            this.chkbx_getAllLocomotives = new System.Windows.Forms.CheckBox();
            this.chkbx_getAllBillOfLadings = new System.Windows.Forms.CheckBox();
            this.btn_findBillOfLading = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numeric_cycleNumber = new System.Windows.Forms.NumericUpDown();
            this.chkbx_getTrainBarryBillOfLadings = new System.Windows.Forms.CheckBox();
            this.chkbx_getTrainBarryLocomotives = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_cycleNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_getAgencies
            // 
            this.btn_getAgencies.Location = new System.Drawing.Point(2, 47);
            this.btn_getAgencies.Name = "btn_getAgencies";
            this.btn_getAgencies.Size = new System.Drawing.Size(261, 33);
            this.btn_getAgencies.TabIndex = 0;
            this.btn_getAgencies.Text = "get Agencies";
            this.btn_getAgencies.UseVisualStyleBackColor = true;
            this.btn_getAgencies.Click += new System.EventHandler(this.btn_getAgencies_Click);
            // 
            // btn_getStations
            // 
            this.btn_getStations.Location = new System.Drawing.Point(2, 86);
            this.btn_getStations.Name = "btn_getStations";
            this.btn_getStations.Size = new System.Drawing.Size(261, 33);
            this.btn_getStations.TabIndex = 1;
            this.btn_getStations.Text = "get Stations";
            this.btn_getStations.UseVisualStyleBackColor = true;
            this.btn_getStations.Click += new System.EventHandler(this.btn_getStations_Click);
            // 
            // btn_getStates
            // 
            this.btn_getStates.Location = new System.Drawing.Point(2, 125);
            this.btn_getStates.Name = "btn_getStates";
            this.btn_getStates.Size = new System.Drawing.Size(261, 33);
            this.btn_getStates.TabIndex = 2;
            this.btn_getStates.Text = "get States";
            this.btn_getStates.UseVisualStyleBackColor = true;
            this.btn_getStates.Click += new System.EventHandler(this.btn_getStates_Click);
            // 
            // btn_getCities
            // 
            this.btn_getCities.Location = new System.Drawing.Point(2, 164);
            this.btn_getCities.Name = "btn_getCities";
            this.btn_getCities.Size = new System.Drawing.Size(261, 33);
            this.btn_getCities.TabIndex = 3;
            this.btn_getCities.Text = "get Cities";
            this.btn_getCities.UseVisualStyleBackColor = true;
            this.btn_getCities.Click += new System.EventHandler(this.btn_getCities_Click);
            // 
            // btn_getTrainPassengers
            // 
            this.btn_getTrainPassengers.Location = new System.Drawing.Point(442, 47);
            this.btn_getTrainPassengers.Name = "btn_getTrainPassengers";
            this.btn_getTrainPassengers.Size = new System.Drawing.Size(261, 33);
            this.btn_getTrainPassengers.TabIndex = 5;
            this.btn_getTrainPassengers.Text = "get Train Passengers";
            this.btn_getTrainPassengers.UseVisualStyleBackColor = true;
            this.btn_getTrainPassengers.Click += new System.EventHandler(this.btn_getTrainPassengers_Click);
            // 
            // btn_getTrainBarries
            // 
            this.btn_getTrainBarries.Location = new System.Drawing.Point(442, 86);
            this.btn_getTrainBarries.Name = "btn_getTrainBarries";
            this.btn_getTrainBarries.Size = new System.Drawing.Size(261, 33);
            this.btn_getTrainBarries.TabIndex = 6;
            this.btn_getTrainBarries.Text = "get Train Barries";
            this.btn_getTrainBarries.UseVisualStyleBackColor = true;
            this.btn_getTrainBarries.Click += new System.EventHandler(this.btn_getTrainBarries_Click);
            // 
            // btn_getTrainBarryLocomotives
            // 
            this.btn_getTrainBarryLocomotives.Location = new System.Drawing.Point(442, 124);
            this.btn_getTrainBarryLocomotives.Name = "btn_getTrainBarryLocomotives";
            this.btn_getTrainBarryLocomotives.Size = new System.Drawing.Size(261, 33);
            this.btn_getTrainBarryLocomotives.TabIndex = 7;
            this.btn_getTrainBarryLocomotives.Text = "get Train Barry Locomotives";
            this.btn_getTrainBarryLocomotives.UseVisualStyleBackColor = true;
            this.btn_getTrainBarryLocomotives.Click += new System.EventHandler(this.btn_getTrainBarryLocomotives_Click);
            // 
            // btn_getTrainBarryBillOfLadings
            // 
            this.btn_getTrainBarryBillOfLadings.Location = new System.Drawing.Point(442, 164);
            this.btn_getTrainBarryBillOfLadings.Name = "btn_getTrainBarryBillOfLadings";
            this.btn_getTrainBarryBillOfLadings.Size = new System.Drawing.Size(261, 33);
            this.btn_getTrainBarryBillOfLadings.TabIndex = 8;
            this.btn_getTrainBarryBillOfLadings.Text = "get Train Barry BillOfLadings";
            this.btn_getTrainBarryBillOfLadings.UseVisualStyleBackColor = true;
            this.btn_getTrainBarryBillOfLadings.Click += new System.EventHandler(this.btn_getTrainBarryBillOfLadings_Click);
            // 
            // btn_findTrainBarryWagon
            // 
            this.btn_findTrainBarryWagon.Location = new System.Drawing.Point(442, 258);
            this.btn_findTrainBarryWagon.Name = "btn_findTrainBarryWagon";
            this.btn_findTrainBarryWagon.Size = new System.Drawing.Size(261, 33);
            this.btn_findTrainBarryWagon.TabIndex = 9;
            this.btn_findTrainBarryWagon.Text = "find Train Barry Wagon";
            this.btn_findTrainBarryWagon.UseVisualStyleBackColor = true;
            this.btn_findTrainBarryWagon.Click += new System.EventHandler(this.btn_findTrainBarryWagon_Click);
            // 
            // btn_findTrainBarry
            // 
            this.btn_findTrainBarry.Location = new System.Drawing.Point(442, 335);
            this.btn_findTrainBarry.Name = "btn_findTrainBarry";
            this.btn_findTrainBarry.Size = new System.Drawing.Size(261, 33);
            this.btn_findTrainBarry.TabIndex = 10;
            this.btn_findTrainBarry.Text = "find Train Barry";
            this.btn_findTrainBarry.UseVisualStyleBackColor = true;
            this.btn_findTrainBarry.Click += new System.EventHandler(this.btn_findTrainBarry_Click);
            // 
            // txtbx_stateCode
            // 
            this.txtbx_stateCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_stateCode.Location = new System.Drawing.Point(269, 167);
            this.txtbx_stateCode.Name = "txtbx_stateCode";
            this.txtbx_stateCode.Size = new System.Drawing.Size(100, 27);
            this.txtbx_stateCode.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txtbx_stateCode, "StateCode");
            this.txtbx_stateCode.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_locomotives_F15Rec_ID
            // 
            this.txtbx_locomotives_F15Rec_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_locomotives_F15Rec_ID.Location = new System.Drawing.Point(713, 127);
            this.txtbx_locomotives_F15Rec_ID.Name = "txtbx_locomotives_F15Rec_ID";
            this.txtbx_locomotives_F15Rec_ID.Size = new System.Drawing.Size(100, 27);
            this.txtbx_locomotives_F15Rec_ID.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtbx_locomotives_F15Rec_ID, "F15Rec_ID");
            this.txtbx_locomotives_F15Rec_ID.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_BillOfLading_F15Rec_ID
            // 
            this.txtbx_BillOfLading_F15Rec_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_BillOfLading_F15Rec_ID.Location = new System.Drawing.Point(713, 167);
            this.txtbx_BillOfLading_F15Rec_ID.Name = "txtbx_BillOfLading_F15Rec_ID";
            this.txtbx_BillOfLading_F15Rec_ID.Size = new System.Drawing.Size(100, 27);
            this.txtbx_BillOfLading_F15Rec_ID.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtbx_BillOfLading_F15Rec_ID, "F15Rec_ID");
            this.txtbx_BillOfLading_F15Rec_ID.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_BillOfLading_Train_No
            // 
            this.txtbx_BillOfLading_Train_No.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_BillOfLading_Train_No.Location = new System.Drawing.Point(820, 167);
            this.txtbx_BillOfLading_Train_No.Name = "txtbx_BillOfLading_Train_No";
            this.txtbx_BillOfLading_Train_No.Size = new System.Drawing.Size(100, 27);
            this.txtbx_BillOfLading_Train_No.TabIndex = 14;
            this.toolTip1.SetToolTip(this.txtbx_BillOfLading_Train_No, "F15Rec_ID");
            this.txtbx_BillOfLading_Train_No.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_wagon_no
            // 
            this.txtbx_wagon_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_wagon_no.Location = new System.Drawing.Point(713, 260);
            this.txtbx_wagon_no.Name = "txtbx_wagon_no";
            this.txtbx_wagon_no.Size = new System.Drawing.Size(100, 27);
            this.txtbx_wagon_no.TabIndex = 18;
            this.toolTip1.SetToolTip(this.txtbx_wagon_no, "Wagon No(Empty string means all wagons)");
            this.txtbx_wagon_no.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_train_no
            // 
            this.txtbx_train_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_train_no.Location = new System.Drawing.Point(713, 338);
            this.txtbx_train_no.Name = "txtbx_train_no";
            this.txtbx_train_no.Size = new System.Drawing.Size(100, 27);
            this.txtbx_train_no.TabIndex = 19;
            this.toolTip1.SetToolTip(this.txtbx_train_no, "Train No");
            this.txtbx_train_no.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // txtbx_barnameh_no
            // 
            this.txtbx_barnameh_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.txtbx_barnameh_no.Location = new System.Drawing.Point(713, 299);
            this.txtbx_barnameh_no.Name = "txtbx_barnameh_no";
            this.txtbx_barnameh_no.Size = new System.Drawing.Size(100, 27);
            this.txtbx_barnameh_no.TabIndex = 21;
            this.toolTip1.SetToolTip(this.txtbx_barnameh_no, "Barnameh No");
            this.txtbx_barnameh_no.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // chkbx_getAllCities
            // 
            this.chkbx_getAllCities.AutoSize = true;
            this.chkbx_getAllCities.Checked = true;
            this.chkbx_getAllCities.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbx_getAllCities.Location = new System.Drawing.Point(269, 131);
            this.chkbx_getAllCities.Name = "chkbx_getAllCities";
            this.chkbx_getAllCities.Size = new System.Drawing.Size(110, 21);
            this.chkbx_getAllCities.TabIndex = 15;
            this.chkbx_getAllCities.Text = "Get All Cities";
            this.chkbx_getAllCities.UseVisualStyleBackColor = true;
            // 
            // chkbx_getAllLocomotives
            // 
            this.chkbx_getAllLocomotives.AutoSize = true;
            this.chkbx_getAllLocomotives.Checked = true;
            this.chkbx_getAllLocomotives.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbx_getAllLocomotives.Location = new System.Drawing.Point(713, 93);
            this.chkbx_getAllLocomotives.Name = "chkbx_getAllLocomotives";
            this.chkbx_getAllLocomotives.Size = new System.Drawing.Size(140, 21);
            this.chkbx_getAllLocomotives.TabIndex = 16;
            this.chkbx_getAllLocomotives.Text = "Get All Lomotives";
            this.chkbx_getAllLocomotives.UseVisualStyleBackColor = true;
            // 
            // chkbx_getAllBillOfLadings
            // 
            this.chkbx_getAllBillOfLadings.AutoSize = true;
            this.chkbx_getAllBillOfLadings.Checked = true;
            this.chkbx_getAllBillOfLadings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbx_getAllBillOfLadings.Location = new System.Drawing.Point(859, 93);
            this.chkbx_getAllBillOfLadings.Name = "chkbx_getAllBillOfLadings";
            this.chkbx_getAllBillOfLadings.Size = new System.Drawing.Size(159, 21);
            this.chkbx_getAllBillOfLadings.TabIndex = 17;
            this.chkbx_getAllBillOfLadings.Text = "Get All BillOfLadings";
            this.chkbx_getAllBillOfLadings.UseVisualStyleBackColor = true;
            // 
            // btn_findBillOfLading
            // 
            this.btn_findBillOfLading.Location = new System.Drawing.Point(442, 297);
            this.btn_findBillOfLading.Name = "btn_findBillOfLading";
            this.btn_findBillOfLading.Size = new System.Drawing.Size(261, 33);
            this.btn_findBillOfLading.TabIndex = 20;
            this.btn_findBillOfLading.Text = "find BillOfLading";
            this.btn_findBillOfLading.UseVisualStyleBackColor = true;
            this.btn_findBillOfLading.Click += new System.EventHandler(this.btn_findBillOfLading_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(439, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 22;
            this.label1.Text = "CycleNumber";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numeric_cycleNumber
            // 
            this.numeric_cycleNumber.Location = new System.Drawing.Point(545, 9);
            this.numeric_cycleNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_cycleNumber.Name = "numeric_cycleNumber";
            this.numeric_cycleNumber.Size = new System.Drawing.Size(120, 22);
            this.numeric_cycleNumber.TabIndex = 23;
            this.numeric_cycleNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numeric_cycleNumber.Enter += new System.EventHandler(this.control_gotFocused);
            // 
            // chkbx_getTrainBarryBillOfLadings
            // 
            this.chkbx_getTrainBarryBillOfLadings.AutoSize = true;
            this.chkbx_getTrainBarryBillOfLadings.Checked = true;
            this.chkbx_getTrainBarryBillOfLadings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbx_getTrainBarryBillOfLadings.Location = new System.Drawing.Point(935, 341);
            this.chkbx_getTrainBarryBillOfLadings.Name = "chkbx_getTrainBarryBillOfLadings";
            this.chkbx_getTrainBarryBillOfLadings.Size = new System.Drawing.Size(140, 21);
            this.chkbx_getTrainBarryBillOfLadings.TabIndex = 25;
            this.chkbx_getTrainBarryBillOfLadings.Text = "Get BillOfLadings";
            this.chkbx_getTrainBarryBillOfLadings.UseVisualStyleBackColor = true;
            // 
            // chkbx_getTrainBarryLocomotives
            // 
            this.chkbx_getTrainBarryLocomotives.AutoSize = true;
            this.chkbx_getTrainBarryLocomotives.Checked = true;
            this.chkbx_getTrainBarryLocomotives.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkbx_getTrainBarryLocomotives.Location = new System.Drawing.Point(816, 341);
            this.chkbx_getTrainBarryLocomotives.Name = "chkbx_getTrainBarryLocomotives";
            this.chkbx_getTrainBarryLocomotives.Size = new System.Drawing.Size(121, 21);
            this.chkbx_getTrainBarryLocomotives.TabIndex = 24;
            this.chkbx_getTrainBarryLocomotives.Text = "Get Lomotives";
            this.chkbx_getTrainBarryLocomotives.UseVisualStyleBackColor = true;
            // 
            // frm_pws0
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 381);
            this.Controls.Add(this.chkbx_getTrainBarryBillOfLadings);
            this.Controls.Add(this.chkbx_getTrainBarryLocomotives);
            this.Controls.Add(this.numeric_cycleNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbx_barnameh_no);
            this.Controls.Add(this.btn_findBillOfLading);
            this.Controls.Add(this.txtbx_train_no);
            this.Controls.Add(this.txtbx_wagon_no);
            this.Controls.Add(this.chkbx_getAllBillOfLadings);
            this.Controls.Add(this.chkbx_getAllLocomotives);
            this.Controls.Add(this.chkbx_getAllCities);
            this.Controls.Add(this.txtbx_BillOfLading_Train_No);
            this.Controls.Add(this.txtbx_BillOfLading_F15Rec_ID);
            this.Controls.Add(this.txtbx_locomotives_F15Rec_ID);
            this.Controls.Add(this.txtbx_stateCode);
            this.Controls.Add(this.btn_findTrainBarry);
            this.Controls.Add(this.btn_findTrainBarryWagon);
            this.Controls.Add(this.btn_getTrainBarryBillOfLadings);
            this.Controls.Add(this.btn_getTrainBarryLocomotives);
            this.Controls.Add(this.btn_getTrainBarries);
            this.Controls.Add(this.btn_getTrainPassengers);
            this.Controls.Add(this.btn_getCities);
            this.Controls.Add(this.btn_getStates);
            this.Controls.Add(this.btn_getStations);
            this.Controls.Add(this.btn_getAgencies);
            this.Name = "frm_pws0";
            this.Text = "PWS0";
            ((System.ComponentModel.ISupportInitialize)(this.numeric_cycleNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_getAgencies;
        private System.Windows.Forms.Button btn_getStations;
        private System.Windows.Forms.Button btn_getStates;
        private System.Windows.Forms.Button btn_getCities;
        private System.Windows.Forms.Button btn_getTrainPassengers;
        private System.Windows.Forms.Button btn_getTrainBarries;
        private System.Windows.Forms.Button btn_getTrainBarryLocomotives;
        private System.Windows.Forms.Button btn_getTrainBarryBillOfLadings;
        private System.Windows.Forms.Button btn_findTrainBarryWagon;
        private System.Windows.Forms.Button btn_findTrainBarry;
        private System.Windows.Forms.TextBox txtbx_stateCode;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtbx_locomotives_F15Rec_ID;
        private System.Windows.Forms.TextBox txtbx_BillOfLading_F15Rec_ID;
        private System.Windows.Forms.TextBox txtbx_BillOfLading_Train_No;
        private System.Windows.Forms.CheckBox chkbx_getAllCities;
        private System.Windows.Forms.CheckBox chkbx_getAllLocomotives;
        private System.Windows.Forms.CheckBox chkbx_getAllBillOfLadings;
        private System.Windows.Forms.TextBox txtbx_wagon_no;
        private System.Windows.Forms.TextBox txtbx_train_no;
        private System.Windows.Forms.TextBox txtbx_barnameh_no;
        private System.Windows.Forms.Button btn_findBillOfLading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numeric_cycleNumber;
        private System.Windows.Forms.CheckBox chkbx_getTrainBarryBillOfLadings;
        private System.Windows.Forms.CheckBox chkbx_getTrainBarryLocomotives;
    }
}

