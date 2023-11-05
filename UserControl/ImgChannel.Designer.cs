
namespace Meta_PG
{
    partial class ImgChannel
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
            this.checkBox_ChName = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_ChName
            // 
            this.checkBox_ChName.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_ChName.AutoSize = true;
            this.checkBox_ChName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox_ChName.Location = new System.Drawing.Point(0, 0);
            this.checkBox_ChName.Name = "checkBox_ChName";
            this.checkBox_ChName.Size = new System.Drawing.Size(136, 36);
            this.checkBox_ChName.TabIndex = 0;
            this.checkBox_ChName.UseVisualStyleBackColor = true;
            this.checkBox_ChName.CheckedChanged += new System.EventHandler(this.checkBox_ChName_CheckedChanged);
            // 
            // ImgChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_ChName);
            this.Name = "ImgChannel";
            this.Size = new System.Drawing.Size(136, 36);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox checkBox_ChName;
    }
}
