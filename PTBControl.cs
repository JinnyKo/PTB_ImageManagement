using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SettingLib;
using System.IO;
using System.IO.Ports;
using CCommLib;

namespace Meta_PG
{
    public class PTBControl
    {
        CComSocket Communicator = new CComSocket(CComSocket.eType.eTCP);
        CComCommonNode.sCommInfo ComInfo = new CComCommonNode.sCommInfo();

        public static bool DeviceSet = false;
        public bool IsConnected = false;

        public static MonitorPowerData _PM = new PTBControl.MonitorPowerData();

        #region Connection
        public bool ConnectPTB(string ip)
        {
            Globals.Settings.Load("Setting.config");
            ComInfo.sLanInfo.nPort = 6000;
            ComInfo.sLanInfo.sIPAddress = ip;
            ComInfo.sLanInfo.sSendTerminator = "\r\n";
            ComInfo.sLanInfo.sRcvTerminator = "\r\n";
            if (Communicator.Connect(ComInfo) == CComCommonNode.eReturnCode.OK && ISConnect())  //실제 연결 Return 확인 
            {
                IsConnected = true;
            }
            else
            {
                MessageBox.Show("Pease Try PTB Connection Again");
                IsConnected = false;
            }
            return IsConnected;
        }
        #endregion 
        enum ReturnCode
        {
            Ok,
            ReceivedDataError,
            ConnectFail,
            OutOfRangeValue,
            NotValidInput
        }
        public struct MonitorPowerData
        {
            public string V_DVDD;
            public string I_DVDD;
            public string V_VDDI;
            public string I_VDDI;
            public string V_AVDD;
            public string I_AVDD;
            public string V_ELVDD;
            public string I_ELVDD;
            public string V_ELVSS;
            public string I_ELVSS;
            public string PMIC_SHT;
            public string FREQ_TE;
        }
        public struct Settings
        {
            public string ELVDD;
            public string ELVSS;
            public string AVDD;
            public string Brightness;
            public int Red;
            public int Green;
            public int Blue;
        }
        public struct sPTBData
        {
            public MonitorPowerData monitorpowerdata;
            public string PTBTemp;
            public string DUTTemp;
        }

        public bool ISConnect()
        {
            string Command = "$c.Version,HW";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool VersionCheck(string device, ref string _RcvData)
        {
            string Command = "$c.Version," + device;
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                _RcvData = RcvData;
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool DUTON()
        {
            string Command = "$c.DUT.powerOn,DSCMode";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool DUTOFF()
        {
            string Command = "$c.DUT.powerOFF";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool ShowImage(string compressMode)
        {
            if (int.Parse(compressMode) > 0)
            {
                string Command = "$c.ShowImage," + (int.Parse(compressMode) - 1).ToString("X");
                string RcvData = "";
                if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
                {
                    string[] list = RcvData.Split(',');
                    if (list.Length > 1 && list[1] == "0000") { return true; }
                    else return false;
                }
                else return false;
            }
            return false;
        }
        public bool SetColor(string Red, string Green, string Blue)
        {
            if (0 <= int.Parse(Red) && int.Parse(Red) <= 255 && 0 <= int.Parse(Green) && int.Parse(Green) <= 255 && 0 <= int.Parse(Blue) && int.Parse(Blue) <= 255)
            {
                string red = "0X" + int.Parse(Red).ToString("X");
                string green = "0X" + int.Parse(Green).ToString("X");
                string blue = "0X" + int.Parse(Blue).ToString("X");
                string Command = "$c.SetColor" + "," + red + "," + green + "," + blue;
                string RcvData = "";
                if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
                {
                    string[] list = RcvData.Split(',');
                    if (list.Length > 1 && list[1] == "0000") { return true; }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public void ReadRegister() { }
        public void WriteRegister() { }

        public bool GetMonitorPower(ref MonitorPowerData _PM)
        {
            string Command = "$c.Monitor.Power";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length == 14)
                {
                    _PM.V_DVDD = ListData[2];
                    _PM.I_DVDD = ListData[3];
                    _PM.V_VDDI = ListData[4];
                    _PM.I_VDDI = (ListData[5]);
                    _PM.V_AVDD = (ListData[6]);
                    _PM.I_AVDD = (ListData[7]);
                    _PM.V_ELVDD = (ListData[8]);
                    _PM.I_ELVDD = (ListData[9]);
                    _PM.V_ELVSS = (ListData[10]);
                    _PM.I_ELVSS = (ListData[11]);
                    _PM.PMIC_SHT = (ListData[12]);
                    _PM.FREQ_TE = (ListData[13]);
                    return true;
                }
                else return false;
            }
            return false;
        }
        public bool Refreshrate()
        {
            string Command = "$c.Refreshrate,DSCNode";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool GetBrightness(ref string br)
        {
            string Command = "$C.Register.READ,17,1";
            string Brighrness;
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    Brighrness = Convert.ToInt32(ListData[2].Substring(2, 4), 16).ToString();
                    br = Brighrness;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public bool GetPTBTemp(ref string refPtbTemp)
        {
            string Command = "$c.GetInfraredTemp";
            string _ptbTemp;
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    _ptbTemp = ListData[2];
                    refPtbTemp = _ptbTemp;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public bool SetDutTemp_PageChange()
        {
            string RcvData = "";
            string Command = "$C.PANEL_IIC.Write,0xFF,0x27";

            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                }
            }
            Command = "$C.PANEL_IIC.Write,0xFB,0x01";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                }
            }

            Command = "$C.PANEL_IIC.Write,0x72,0x05";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                }
            }
            return true;
        }
        public bool GetDUTTemp(ref string refDUTTemp)
        {

            string Command = "$C.PANEL_IIC.Read,0x0AC,2";
            string _ptbTemp1;
            string _ptbTemp2;
            string _ptbTemp;
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    _ptbTemp1 = Convert.ToInt32(ListData[3].Replace("0x", string.Empty), 16).ToString();
                    _ptbTemp2 = ListData[4].Replace("\r\n", string.Empty);
                    _ptbTemp2 = Convert.ToInt32(_ptbTemp2.Replace("0x", string.Empty), 16).ToString();
                    _ptbTemp = _ptbTemp1 + "." + _ptbTemp2;
                    refDUTTemp = _ptbTemp;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public bool Reboot()
        {
            string Command = "$c.Reboot";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        //SET GET
        //AVDD, ELVDD, ELVSS 
        public bool SetAVDD(string value)
        {
            int AVDD;
            AVDD = int.Parse((ConvertAVDD(value)));
            string Command = "$C.Register.Write,14,0X" + AVDD.ToString("X");
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }

        public bool GetAVDD(ref string AVDD)
        {
            string _AVDD;
            string Command = "$C.Register.READ,14,1";
            string RcvData = "";
            double decAVDD;
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    _AVDD = ListData[2];
                    decAVDD = Convert.ToInt32(_AVDD, 16);
                    decAVDD = (decAVDD + 1) / 10;
                    AVDD = decAVDD.ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
        public bool SetELVDD(string value)
        {
            int ELVDD;
            ELVDD = int.Parse((ConvertELVDD(value)));
            string Command = "$C.Register.Write,15,0X" + ELVDD.ToString("X");
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool GetELVDD(ref string ELVDD)
        {
            string _ELVDD;
            string Command = "$C.Register.READ,15,1";
            string RcvData = "";
            double decELVDD;
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    _ELVDD = ListData[2];
                    decELVDD = Convert.ToInt32(_ELVDD, 16);
                    decELVDD = (decELVDD - 83) / 10 + 4.6;
                    ELVDD = decELVDD.ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
        public bool SetELVSS(string value)
        {
            int ELVSS;
            ELVSS = int.Parse((ConvertELVSS(value)));
            string Command = "$C.Register.Write,16,0X" + ELVSS.ToString("X");
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool GetELVSS(ref string ELVSS)
        {
            string _ELVSS;
            string Command = "$C.Register.READ,16,1";
            string RcvData = "";
            double decELVSS;
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    _ELVSS = ListData[2];

                    decELVSS = Convert.ToInt32(_ELVSS, 16);

                    decELVSS = (decELVSS - 1) / 10 - 6;

                    ELVSS = decELVSS.ToString();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
        public bool SetBrightness(string value)
        {
            string ValueRange1 = int.Parse(value).ToString("X");
            string Command = "$C.Register.Write,17,0x" + ValueRange1;
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] list = RcvData.Split(',');
                if (list.Length > 1 && list[1] == "0000") { return true; }
                else return false;
            }
            else return false;
        }
        public bool SetBrightnessIIC(string value1, string value2)
        {
            if (int.Parse(value1) >= 15 && int.Parse(value2) <= 255)
            {
                string ValueRange1 = int.Parse(value1).ToString("X");
                string ValueRange2 = int.Parse(value2).ToString("X");
                string Command = "$C.PANEL_IIC.Write,0x51,0x" + ValueRange1 + "0x" + ValueRange2;
                string RcvData = "";
                if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
                {
                    string[] list = RcvData.Split(',');
                    if (list.Length > 1 && list[1] == "0000") { return true; }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        public bool GetImageFileList(ref string[] FileName, ref string TotalImgNum)
        {
            string Command = "$C.IMAGE_LIST.Get";
            string RcvData = "";
            string[,] IndexFile;
            int flag = 0;
            while (flag < 3)
            {
                if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
                {
                    //20221124 Jinny Main 측정 돌릴때 없는 이미지 에러 방지 
                    string[] data = RcvData.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length > 0 && data[data.Length - 1].Split(',')[1] == "0000")
                    {
                        TotalImgNum = data[data.Length - 1].Split(',')[2];
                        //20221128 Jinny 홀수 인덱스는 파일 Index 짝수 인덱스는 파일 이름
                        IndexFile = new string[data.Length - 1, 2];
                        FileName = new string[data.Length - 1];
                        for (int i = 0; i < data.Length-1 ; i++)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                IndexFile[i, k] = data[i].Split(',')[k + 1];
                            }
                           if( i< data.Length - 2)
                            FileName[i] = (data[i].Split(',')[2]);
                        }
                        return true;
                    }
                    // 20221124 Jinny 이경우엔 RCV 데이터 끝까지 안들어 온 것. 
                    else flag++;
                }
                return false;
            }
            return false;
        }
        //public Boolean GetImageList(ref string[] FileName, ref string TotIndex) 
        //{
        //    string Command = "$C.IMAGE_LIST.Get";
        //    string TotalIndex = "";
        //    string RcvData = "";
        //    int ConvetVal = 0;
        //    string[] FileData;
        //    string[] resData;
        //    int cnt = 0;
        //    if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
        //    {
        //        string[] ListData = RcvData.Split(',');
        //        if (ListData.Length > 1 && ListData[ListData.Length - 2] == "0000")
        //        {
        //            TotalIndex = ListData[ListData.Length - 1];
        //            TotIndex = TotalIndex;
        //            ConvetVal = Convert.ToInt32(TotIndex);
        //            if(ConvetVal != 0)
        //                {
        //                FileName = new string[Convert.ToInt32(TotIndex)];
        //                for (int i = 2; i < ListData.Length -1; i += 2)
        //                {

        //                    FileData = ListData[i].Split('$');
        //                    resData = FileData[FileData.Length - 2].Split('\\');
        //                    string TmpStr = resData[resData.Length - 1];
        //                    TmpStr = TmpStr.Trim();
        //                    FileName[cnt] = TmpStr;
        //                    cnt += 1;
        //                }
        //            }
        //        }
        //        else return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        public bool GetVoltageLimit(ref string DVDD_Min, ref string DVDD_Max, ref string VDDI_Min, ref string VDDI_Max, ref string AVDD_Min, ref string AVDD_Max, ref string ELVDD_Min, ref string ELVDD_Max, ref string ELVSS_Min, ref string ELVSS_Max)
        {
            //string _DVDD_Min;
            //string _DVDD_Max;
            //string _VDDI_Min;
            //string _VDDI_Max;
            //string _AVDD_Min;
            //string _AVDD_Max;
            //string _ELVDD_Min;
            //string _ELVDD_Max;
            //string _ELVSS_Min;
            //string _ELVSS_Max;
            string Command = "$C.VOLTAGE_Limit.Read";
            string RcvData = "";
            if (Communicator.SendToString(Command, ref RcvData) == CComCommonNode.eReturnCode.OK)
            {
                string[] ListData = RcvData.Split(',');
                if (ListData.Length > 1 && ListData[1] == "0000")
                {
                    ListData[2] = DVDD_Min;
                    ListData[3] = DVDD_Max;
                    ListData[4] = VDDI_Min;
                    ListData[5] = VDDI_Max;
                    ListData[6] = AVDD_Min;
                    ListData[7] = AVDD_Max;
                    ListData[8] = ELVDD_Min;
                    ListData[9] = ELVDD_Max;
                    ListData[10] = ELVSS_Min;
                    ListData[11] = ELVSS_Max;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }
        

        public string ConvertAVDD(string value)
        {
            double j;
            if (double.TryParse(value, out j))
            {
                int Pulse = (int)((double.Parse(value) * 10) - 1);
                return Pulse.ToString();
            }
            else return ReturnCode.NotValidInput.ToString();
        }
        public static string ConvertELVDD(string value)
        {
            double j;
            if (double.TryParse(value, out j))
            {
                int Pulse = (int)(83 + ((double.Parse(value) - 4.6) * 10));
                return Pulse.ToString();
            }
            else return ReturnCode.NotValidInput.ToString();
        }
        public static string ConvertELVSS(string value)
        {
            double j;
            if (double.TryParse(value, out j))
            {
                int Pulse = (int)(1 + (((double.Parse(value) + 6) * 10)));
                return Pulse.ToString();
            }
            else return ReturnCode.NotValidInput.ToString();
        }
    }
    //public string
  
}
    

