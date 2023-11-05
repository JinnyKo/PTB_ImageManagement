
namespace Meta_PG
{
    partial class Client_IP
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.ClientName = new System.Windows.Forms.GroupBox();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ClientName.SuspendLayout();
            this.SuspendLayout();
            // 
            // ClientName
            // 
            this.ClientName.Controls.Add(this.textBox_IP);
            this.ClientName.Controls.Add(this.label1);
            this.ClientName.Location = new System.Drawing.Point(3, 3);
            this.ClientName.Name = "ClientName";
            this.ClientName.Size = new System.Drawing.Size(185, 63);
            this.ClientName.TabIndex = 0;
            this.ClientName.TabStop = false;
            this.ClientName.Text = "groupBox1";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(32, 25);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.Size = new System.Drawing.Size(147, 21);
            this.textBox_IP.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // Client_IP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ClientName);
            this.Name = "Client_IP";
            this.Size = new System.Drawing.Size(191, 69);
            this.ClientName.ResumeLayout(false);
            this.ClientName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ClientName;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Label label1;
    }
}
