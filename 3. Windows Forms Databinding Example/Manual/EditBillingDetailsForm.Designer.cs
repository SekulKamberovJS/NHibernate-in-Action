namespace NHibernateInAction.DataBinding.Manual
{
	partial class EditBillingDetailsForm
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
			this.editBankOrExpYear = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.editOwnerName = new System.Windows.Forms.TextBox();
			this.editNumber = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// editBankOrExpYear
			// 
			this.editBankOrExpYear.Location = new System.Drawing.Point(122, 99);
			this.editBankOrExpYear.Name = "editBankOrExpYear";
			this.editBankOrExpYear.Size = new System.Drawing.Size(150, 20);
			this.editBankOrExpYear.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(42, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Owner name";
			// 
			// editOwnerName
			// 
			this.editOwnerName.Location = new System.Drawing.Point(122, 19);
			this.editOwnerName.Name = "editOwnerName";
			this.editOwnerName.Size = new System.Drawing.Size(150, 20);
			this.editOwnerName.TabIndex = 3;
			// 
			// editNumber
			// 
			this.editNumber.Location = new System.Drawing.Point(122, 57);
			this.editNumber.Name = "editNumber";
			this.editNumber.Size = new System.Drawing.Size(150, 20);
			this.editNumber.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(42, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Number";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(42, 106);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "BankOrExpYear";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(109, 146);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// EditBillingDetailsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 182);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.editBankOrExpYear);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.editNumber);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.editOwnerName);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "EditBillingDetailsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Billing Details";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox editOwnerName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox editNumber;
		private System.Windows.Forms.TextBox editBankOrExpYear;
		private System.Windows.Forms.Button btnOK;
	}
}