namespace Gomoku_ChessBoard
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTm = new System.Windows.Forms.Label();
            this.textBoxTm = new System.Windows.Forms.TextBox();
            this.labelCut = new System.Windows.Forms.Label();
            this.labelSch = new System.Windows.Forms.Label();
            this.textBoxCut = new System.Windows.Forms.TextBox();
            this.textBoxSch = new System.Windows.Forms.TextBox();
            this.labelDpt = new System.Windows.Forms.Label();
            this.textBoxDpt = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Gomoku_ChessBoard.Properties.Resources.bg2;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.labelDpt);
            this.panel1.Controls.Add(this.textBoxDpt);
            this.panel1.Controls.Add(this.labelTm);
            this.panel1.Controls.Add(this.textBoxTm);
            this.panel1.Controls.Add(this.labelCut);
            this.panel1.Controls.Add(this.labelSch);
            this.panel1.Controls.Add(this.textBoxCut);
            this.panel1.Controls.Add(this.textBoxSch);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(823, 765);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // labelTm
            // 
            this.labelTm.AutoSize = true;
            this.labelTm.Location = new System.Drawing.Point(740, 373);
            this.labelTm.Name = "labelTm";
            this.labelTm.Size = new System.Drawing.Size(47, 12);
            this.labelTm.TabIndex = 5;
            this.labelTm.Text = "用时(s)";
            // 
            // textBoxTm
            // 
            this.textBoxTm.Location = new System.Drawing.Point(723, 391);
            this.textBoxTm.Name = "textBoxTm";
            this.textBoxTm.ReadOnly = true;
            this.textBoxTm.Size = new System.Drawing.Size(97, 21);
            this.textBoxTm.TabIndex = 4;
            // 
            // labelCut
            // 
            this.labelCut.AutoSize = true;
            this.labelCut.Location = new System.Drawing.Point(740, 303);
            this.labelCut.Name = "labelCut";
            this.labelCut.Size = new System.Drawing.Size(53, 12);
            this.labelCut.TabIndex = 3;
            this.labelCut.Text = "剪枝次数";
            // 
            // labelSch
            // 
            this.labelSch.AutoSize = true;
            this.labelSch.Location = new System.Drawing.Point(740, 228);
            this.labelSch.Name = "labelSch";
            this.labelSch.Size = new System.Drawing.Size(53, 12);
            this.labelSch.TabIndex = 2;
            this.labelSch.Text = "搜索次数";
            // 
            // textBoxCut
            // 
            this.textBoxCut.Location = new System.Drawing.Point(723, 321);
            this.textBoxCut.Name = "textBoxCut";
            this.textBoxCut.ReadOnly = true;
            this.textBoxCut.Size = new System.Drawing.Size(97, 21);
            this.textBoxCut.TabIndex = 1;
            // 
            // textBoxSch
            // 
            this.textBoxSch.Location = new System.Drawing.Point(723, 246);
            this.textBoxSch.Name = "textBoxSch";
            this.textBoxSch.ReadOnly = true;
            this.textBoxSch.Size = new System.Drawing.Size(97, 21);
            this.textBoxSch.TabIndex = 0;
            // 
            // labelDpt
            // 
            this.labelDpt.AutoSize = true;
            this.labelDpt.Location = new System.Drawing.Point(740, 159);
            this.labelDpt.Name = "labelDpt";
            this.labelDpt.Size = new System.Drawing.Size(53, 12);
            this.labelDpt.TabIndex = 7;
            this.labelDpt.Text = "搜索深度";
            // 
            // textBoxDpt
            // 
            this.textBoxDpt.Location = new System.Drawing.Point(723, 177);
            this.textBoxDpt.Name = "textBoxDpt";
            this.textBoxDpt.ReadOnly = true;
            this.textBoxDpt.Size = new System.Drawing.Size(97, 21);
            this.textBoxDpt.TabIndex = 6;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(822, 765);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelCut;
        private System.Windows.Forms.Label labelSch;
        private System.Windows.Forms.TextBox textBoxCut;
        private System.Windows.Forms.TextBox textBoxSch;
        private System.Windows.Forms.Label labelTm;
        private System.Windows.Forms.TextBox textBoxTm;
        private System.Windows.Forms.Label labelDpt;
        private System.Windows.Forms.TextBox textBoxDpt;
    }
}

