using SettingLib;
using System.ComponentModel;
using System.Windows.Forms;

namespace Meta_PG
{
    public partial class Client_IP : UserControl
    {
        public Client_IP()
        {
            InitializeComponent();
        }
        public string IPName
        {
            get 
            {
                return ClientName.Text; 
            }
            set
            { 
                ClientName.Text = value; 
            }
        }
        public string IPAddress_Text
        {
            get
            {
                return textBox_IP.Text;
            }
            set
            {
                textBox_IP.Text = value;
            }
        }
    }
    public class IP_Address : BaseSetting
    {

        [ID("ChName: ")]
        [Browsable(true)]
        public string ChNum
        {
            get;
            set;
        }

        [ID("IP: ")]
        [Browsable(true)]
        public string IP
        {
            get;
            set;
        }
    }
}
