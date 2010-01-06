﻿namespace QueryAnalyzer
{
    partial class FormRebuildTable
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelTableName = new System.Windows.Forms.Label();
            this.labelDbAdapterName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownDocIdFrom = new System.Windows.Forms.NumericUpDown();
            this.labelIndexOnly = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxRebuildWholeTable = new System.Windows.Forms.CheckBox();
            this.labelCurrentCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonRebuild = new System.Windows.Forms.Button();
            this.numericUpDownStep = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDownImportCount = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDocIdFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportCount)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Table Name:";
            // 
            // labelTableName
            // 
            this.labelTableName.AutoSize = true;
            this.labelTableName.Location = new System.Drawing.Point(132, 27);
            this.labelTableName.Name = "labelTableName";
            this.labelTableName.Size = new System.Drawing.Size(33, 13);
            this.labelTableName.TabIndex = 1;
            this.labelTableName.Text = "name";
            // 
            // labelDbAdapterName
            // 
            this.labelDbAdapterName.AutoSize = true;
            this.labelDbAdapterName.Location = new System.Drawing.Point(132, 79);
            this.labelDbAdapterName.Name = "labelDbAdapterName";
            this.labelDbAdapterName.Size = new System.Drawing.Size(33, 13);
            this.labelDbAdapterName.TabIndex = 3;
            this.labelDbAdapterName.Text = "name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "DBAdapter Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "DocId from:";
            // 
            // numericUpDownDocIdFrom
            // 
            this.numericUpDownDocIdFrom.Location = new System.Drawing.Point(135, 135);
            this.numericUpDownDocIdFrom.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDownDocIdFrom.Name = "numericUpDownDocIdFrom";
            this.numericUpDownDocIdFrom.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownDocIdFrom.TabIndex = 5;
            // 
            // labelIndexOnly
            // 
            this.labelIndexOnly.AutoSize = true;
            this.labelIndexOnly.Location = new System.Drawing.Point(132, 54);
            this.labelIndexOnly.Name = "labelIndexOnly";
            this.labelIndexOnly.Size = new System.Drawing.Size(32, 13);
            this.labelIndexOnly.TabIndex = 7;
            this.labelIndexOnly.Text = "False";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "IndexOnly";
            // 
            // checkBoxRebuildWholeTable
            // 
            this.checkBoxRebuildWholeTable.AutoSize = true;
            this.checkBoxRebuildWholeTable.Location = new System.Drawing.Point(36, 109);
            this.checkBoxRebuildWholeTable.Name = "checkBoxRebuildWholeTable";
            this.checkBoxRebuildWholeTable.Size = new System.Drawing.Size(120, 17);
            this.checkBoxRebuildWholeTable.TabIndex = 8;
            this.checkBoxRebuildWholeTable.Text = "RebuildWholeTable";
            this.checkBoxRebuildWholeTable.UseVisualStyleBackColor = true;
            this.checkBoxRebuildWholeTable.CheckedChanged += new System.EventHandler(this.checkBoxRebuildWholeTable_CheckedChanged);
            // 
            // labelCurrentCount
            // 
            this.labelCurrentCount.AutoSize = true;
            this.labelCurrentCount.Location = new System.Drawing.Point(132, 248);
            this.labelCurrentCount.Name = "labelCurrentCount";
            this.labelCurrentCount.Size = new System.Drawing.Size(13, 13);
            this.labelCurrentCount.TabIndex = 10;
            this.labelCurrentCount.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Rebuilding records:";
            // 
            // buttonRebuild
            // 
            this.buttonRebuild.Location = new System.Drawing.Point(36, 285);
            this.buttonRebuild.Name = "buttonRebuild";
            this.buttonRebuild.Size = new System.Drawing.Size(75, 23);
            this.buttonRebuild.TabIndex = 11;
            this.buttonRebuild.Text = "Rebuild";
            this.buttonRebuild.UseVisualStyleBackColor = true;
            this.buttonRebuild.Click += new System.EventHandler(this.buttonRebuild_Click);
            // 
            // numericUpDownStep
            // 
            this.numericUpDownStep.Location = new System.Drawing.Point(135, 171);
            this.numericUpDownStep.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStep.Name = "numericUpDownStep";
            this.numericUpDownStep.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownStep.TabIndex = 13;
            this.numericUpDownStep.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Step";
            // 
            // numericUpDownImportCount
            // 
            this.numericUpDownImportCount.Location = new System.Drawing.Point(135, 210);
            this.numericUpDownImportCount.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numericUpDownImportCount.Name = "numericUpDownImportCount";
            this.numericUpDownImportCount.Size = new System.Drawing.Size(120, 20);
            this.numericUpDownImportCount.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Import count";
            // 
            // FormRebuildTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 325);
            this.Controls.Add(this.numericUpDownImportCount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDownStep);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonRebuild);
            this.Controls.Add(this.labelCurrentCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBoxRebuildWholeTable);
            this.Controls.Add(this.labelIndexOnly);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDownDocIdFrom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelDbAdapterName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelTableName);
            this.Controls.Add(this.label1);
            this.Name = "FormRebuildTable";
            this.Text = "Rebuild Table";
            this.Load += new System.EventHandler(this.FormRebuildTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDocIdFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownImportCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTableName;
        private System.Windows.Forms.Label labelDbAdapterName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownDocIdFrom;
        private System.Windows.Forms.Label labelIndexOnly;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxRebuildWholeTable;
        private System.Windows.Forms.Label labelCurrentCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonRebuild;
        private System.Windows.Forms.NumericUpDown numericUpDownStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownImportCount;
        private System.Windows.Forms.Label label7;
    }
}