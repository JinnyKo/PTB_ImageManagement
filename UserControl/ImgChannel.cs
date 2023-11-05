using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Meta_PG
{
    public partial class ImgChannel : UserControl
    {
        //ImageUpload img = new ImageUpload();
        //public int count = 0; 
        public ImgChannel()
        {
            InitializeComponent();
        }
        public string ButtonName
        {
            get
            {
                return checkBox_ChName.Text;
            }
            set
            {
                checkBox_ChName.Text = value;
            }
        }
        private void checkBox_ChName_CheckedChanged(object sender, EventArgs e)
        {
                if (!checkBox_ChName.Checked)
                    checkBox_ChName.BackColor = Color.Gray;
                else checkBox_ChName.BackColor = Color.LightGray;
            
        }
    }
}
