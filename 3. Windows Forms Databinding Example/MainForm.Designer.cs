namespace NHibernateInAction.DataBinding
{
	partial class MainForm
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
			this.lblSelect = new System.Windows.Forms.Label();
			this.btnManual = new System.Windows.Forms.Button();
			this.btnDirect = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblSelect
			// 
			this.lblSelect.AutoSize = true;
			this.lblSelect.Location = new System.Drawing.Point(37, 23);
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Size = new System.Drawing.Size(201, 13);
			this.lblSelect.TabIndex = 0;
			this.lblSelect.Text = "Select the way you want to see in action:";
			// 
			// btnManual
			// 
			this.btnManual.Location = new System.Drawing.Point(102, 67);
			this.btnManual.Name = "btnManual";
			this.btnManual.Size = new System.Drawing.Size(75, 23);
			this.btnManual.TabIndex = 1;
			this.btnManual.Text = "Manual";
			this.btnManual.UseVisualStyleBackColor = true;
			this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
			// 
			// btnDirect
			// 
			this.btnDirect.Location = new System.Drawing.Point(102, 108);
			this.btnDirect.Name = "btnDirect";
			this.btnDirect.Size = new System.Drawing.Size(75, 23);
			this.btnDirect.TabIndex = 2;
			this.btnDirect.Text = "Direct";
			this.btnDirect.UseVisualStyleBackColor = true;
			this.btnDirect.Click += new System.EventHandler(this.btnDirect_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.btnDirect);
			this.Controls.Add(this.btnManual);
			this.Controls.Add(this.lblSelect);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "NHibernateInAction.DataBinding";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnManual;
		private System.Windows.Forms.Button btnDirect;

	}
}