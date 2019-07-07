namespace RailSiteDataGrabber
{
    partial class frm_rwmms
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
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("مشخصات شرکتهای همکار ");
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("اظهارنامه ثبت ناوگان ");
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("مشخصات ویژه واگن", new System.Windows.Forms.TreeNode[] {
            treeNode28,
            treeNode29});
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("درخت قطعات");
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("قطعات مصرفی استاندارد ", new System.Windows.Forms.TreeNode[] {
            treeNode31});
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("دریافت قطعات");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("تحویل به پیمانکار ");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("قطعات", new System.Windows.Forms.TreeNode[] {
            treeNode33,
            treeNode34});
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("اطلاعات پایه", new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode32,
            treeNode35});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_getInfo = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode28.Name = "node_coworkerCompanies";
            treeNode28.Text = "مشخصات شرکتهای همکار ";
            treeNode29.Name = "node_wagonDecleration";
            treeNode29.Text = "اظهارنامه ثبت ناوگان ";
            treeNode30.Name = "node_wagonSpecialInfo";
            treeNode30.Text = "مشخصات ویژه واگن";
            treeNode31.Name = "node_partsTree";
            treeNode31.Text = "درخت قطعات";
            treeNode32.Name = "node_standardsParts";
            treeNode32.Text = "قطعات مصرفی استاندارد ";
            treeNode33.Name = "node_partsDelivery";
            treeNode33.Text = "دریافت قطعات";
            treeNode34.Name = "node_contractorDelivery";
            treeNode34.Text = "تحویل به پیمانکار ";
            treeNode35.Name = "node_parts";
            treeNode35.Text = "قطعات";
            treeNode36.Name = "node_base";
            treeNode36.Text = "اطلاعات پایه";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode36});
            this.treeView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.treeView1.RightToLeftLayout = true;
            this.treeView1.Size = new System.Drawing.Size(700, 403);
            this.treeView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_getInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 403);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 47);
            this.panel1.TabIndex = 1;
            // 
            // btn_getInfo
            // 
            this.btn_getInfo.Location = new System.Drawing.Point(10, 7);
            this.btn_getInfo.Name = "btn_getInfo";
            this.btn_getInfo.Size = new System.Drawing.Size(143, 32);
            this.btn_getInfo.TabIndex = 0;
            this.btn_getInfo.Text = "دریافت اطلاعات";
            this.btn_getInfo.UseVisualStyleBackColor = true;
            this.btn_getInfo.Click += new System.EventHandler(this.btn_getInfo_Click);
            // 
            // frm_rwmms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 450);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Name = "frm_rwmms";
            this.Text = "RWMMS";
            this.Load += new System.EventHandler(this.frm_rwmms_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_getInfo;
    }
}