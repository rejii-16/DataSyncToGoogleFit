namespace DataSyncToGoogleFit
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnGetStep = new System.Windows.Forms.Button();
            this.BtnGetWeight = new System.Windows.Forms.Button();
            this.BtnSetWeight = new System.Windows.Forms.Button();
            this.Calendar = new System.Windows.Forms.MonthCalendar();
            this.TxtboxResult = new System.Windows.Forms.TextBox();
            this.BtnSetStep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnGetStep
            // 
            this.BtnGetStep.Location = new System.Drawing.Point(284, 44);
            this.BtnGetStep.Name = "BtnGetStep";
            this.BtnGetStep.Size = new System.Drawing.Size(137, 51);
            this.BtnGetStep.TabIndex = 0;
            this.BtnGetStep.Text = "歩数取得";
            this.BtnGetStep.UseVisualStyleBackColor = true;
            this.BtnGetStep.Click += new System.EventHandler(this.BtnGetStep_Click);
            // 
            // BtnGetWeight
            // 
            this.BtnGetWeight.Location = new System.Drawing.Point(284, 101);
            this.BtnGetWeight.Name = "BtnGetWeight";
            this.BtnGetWeight.Size = new System.Drawing.Size(137, 51);
            this.BtnGetWeight.TabIndex = 1;
            this.BtnGetWeight.Text = "体重取得";
            this.BtnGetWeight.UseVisualStyleBackColor = true;
            this.BtnGetWeight.Click += new System.EventHandler(this.BtnGetWeight_Click);
            // 
            // BtnSetWeight
            // 
            this.BtnSetWeight.Location = new System.Drawing.Point(427, 101);
            this.BtnSetWeight.Name = "BtnSetWeight";
            this.BtnSetWeight.Size = new System.Drawing.Size(137, 51);
            this.BtnSetWeight.TabIndex = 2;
            this.BtnSetWeight.Text = "体重登録";
            this.BtnSetWeight.UseVisualStyleBackColor = true;
            this.BtnSetWeight.Click += new System.EventHandler(this.BtnSetWeight_Click);
            // 
            // Calendar
            // 
            this.Calendar.Location = new System.Drawing.Point(46, 18);
            this.Calendar.Name = "Calendar";
            this.Calendar.TabIndex = 3;
            // 
            // TxtboxResult
            // 
            this.TxtboxResult.Location = new System.Drawing.Point(46, 192);
            this.TxtboxResult.Multiline = true;
            this.TxtboxResult.Name = "TxtboxResult";
            this.TxtboxResult.Size = new System.Drawing.Size(540, 250);
            this.TxtboxResult.TabIndex = 4;
            // 
            // BtnSetStep
            // 
            this.BtnSetStep.Location = new System.Drawing.Point(427, 44);
            this.BtnSetStep.Name = "BtnSetStep";
            this.BtnSetStep.Size = new System.Drawing.Size(137, 51);
            this.BtnSetStep.TabIndex = 5;
            this.BtnSetStep.Text = "歩数登録";
            this.BtnSetStep.UseVisualStyleBackColor = true;
            this.BtnSetStep.Click += new System.EventHandler(this.BtnSetStep_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 450);
            this.Controls.Add(this.BtnSetStep);
            this.Controls.Add(this.TxtboxResult);
            this.Controls.Add(this.Calendar);
            this.Controls.Add(this.BtnSetWeight);
            this.Controls.Add(this.BtnGetWeight);
            this.Controls.Add(this.BtnGetStep);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnGetStep;
        private System.Windows.Forms.Button BtnGetWeight;
        private System.Windows.Forms.Button BtnSetWeight;
        private System.Windows.Forms.MonthCalendar Calendar;
        private System.Windows.Forms.TextBox TxtboxResult;
        private System.Windows.Forms.Button BtnSetStep;
    }
}

