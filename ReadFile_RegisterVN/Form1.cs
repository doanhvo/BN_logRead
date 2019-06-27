using DataBase;
using ReadFile_RegisterVN.NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadFile_RegisterVN
{
    public partial class Form1 : Form
    {
        #region Init
        string[] files;
        SqlConnection _connection = null;
        SqlCommand _command = null;
        String _connectionString = "";
        public List<SoatVeTransactionModel> lstTran = new List<SoatVeTransactionModel>();

        #endregion
        public Form1()
        {
            InitializeComponent();
            _connectionString = @"Data Source=;Initial Catalog=ETC_RFID;Persist Security Info=True;User ID=sa;Password=";
            //_connectionString = @"Data Source=HUYTO\MSSQLSERVER2K16;Initial Catalog=ETC_RFID;Persist Security Info=True;User ID=sa;Password=Abcd1234";
            txtLog.Text = @"2019 - 06 - 27 14:14:47.024 : [ProcessingVehicleQueue - _vehicleQueue.Count: 1, Vehicle type: 1, TransID: 20190627141440914 ]
2019-06-27 14:14:47.149 : [ProcessBarcodeTicket - OPEN BARIE BARCODE: 0 (Lan)...TID: B116010034051 SX: 30E59590 Barrier Status: OpenAuto ]
2019-06-27 14:14:47.211 : [ProcessBarcodeTicket - Vehicle '30E59590 - B116010034051' Done]";
        }

        private void btnReadFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.DBView.Rows.Clear();
                //lstTran.Clear();
                if (string.IsNullOrEmpty(this.txtpathfile.Text))
                {
                    MessageBox.Show("Vui lòng chọn file trước !");
                }
                else if (!File.Exists(this.txtpathfile.Text))
                {
                    MessageBox.Show("File không tồn tại !");
                }
                else
                {
                    using (FileStream fs = File.Open(this.txtpathfile.Text, FileMode.Open))
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            List<SoatVeTransactionModel> lstTemp = new List<SoatVeTransactionModel>();
                            string dataLine;
                            while ((dataLine = sr.ReadLine()) != null)
                            {
                                if (dataLine.Contains("---------------") || string.IsNullOrEmpty(dataLine))
                                {

                                }
                                else
                                {
                                    var stringArr = dataLine.Split('-');
                                    var TransactionType = stringArr[0].Split('[')[1];
                                    var stringArr1 = stringArr[1].Split('|');

                                    SoatVeTransactionModel item = SoatVeTransactionModel.CreateTransactionbyString2(TransactionType, stringArr1);
                                    if (item != null)
                                        lstTemp.Add(item);
                                }
                            }
                            this.lstTran = lstTemp;
                        }
                        fs.Close();
                    }
                    MessageBox.Show("Read file succees");
                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(this.GetType() + "." + System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message); //STORED_ONCE_TICKET, ex.Message);
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    files = Directory.GetFiles(fbd.SelectedPath);
                    txtpathfile.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            foreach (SoatVeTransactionModel item in lstTran)
            {
                switch (item.TypeTransaction.Trim())
                {
                    case "SaveOnceTicket":
                        item.SaveOnceTicket(item);
                        break;
                    case "SaveMonthQuarterTicket":
                        //item.SaveMonthQuarterTicket(item);
                        break;
                    case "Add_ForceOpen":
                        //item.SaveForceOpen(item);
                        break;
                    case "EtagTransaction":
                        item.CommitRFID(item);
                        break;
                }
            }
            MessageBox.Show("Insert suceess");
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void BtnReadLog_Click(object sender, EventArgs e)
        {
            try
            {
               // this.DBView.Rows.Clear();
                SoatVeTransactionModel item = new SoatVeTransactionModel();
                item.ID = Guid.NewGuid();
                item.LoginID = txtMaNV.Text.Trim();
                item.ShiftID = txtMaCa.Text.Trim();
                item.StationID = txtMaTram.Text.Trim();
                item.LaneID = txtLan.Text.Trim();

                //string type = "";
                var log = txtLog.Lines;
                string line = log[0];
                if (line.Contains("SendBackEndProxyCheckIn"))// Etag
                {
                    item.TypeTransaction = "EtagTransaction";
                }
                else // mtc
                {

                    string tranid = line.Substring(line.LastIndexOf("2019"), 17);
                    string ngaysoat = log[1].Substring(0, 23);
                    //string checker = ngaysoat.Substring(11, 8);
                    DateTime date = DateTime.Parse(ngaysoat);
                    double checkTime = date.TimeOfDay.TotalSeconds;
                    string checker = date.ToString("hh:mm:ss tt");
                    string sx = log[1].Substring(log[1].IndexOf("SX:")).Split(' ')[1];
                    string TID = log[1].Substring(log[1].IndexOf("TID:") + 5, 13);

                    int changeAmount = 0;
                    string maLXe = "";
                    string lx = TID.Substring(1, 1);
                    if (TID.Contains("A"))
                    {
                        maLXe = "1" + lx;
                        item.TypeTransaction = "SaveOnceTicket";
                        if(maLXe == "11")
                        {
                            changeAmount = 35000;
                        }
                        else if(maLXe == "12")
                        {
                            changeAmount = 50000;
                        }
                        else if(maLXe == "13")
                        {
                            changeAmount = 75000;
                        }
                        else if (maLXe == "14")
                        {
                            changeAmount = 140000;
                        }
                        else if (maLXe == "15")
                        {
                            changeAmount = 200000;
                        }
                    }
                    else if (TID.Contains("B"))
                    {
                        maLXe = "2" + lx;
                        item.TypeTransaction = "SaveMonthQuarterTicket";
                    }
                    else if(TID.Contains("C"))
                    {
                        maLXe = "3" + lx;
                        item.TypeTransaction = "SaveMonthQuarterTicket";
                    }

                    //common
                    item.ObuID = TID;
                    item.CheckDate = ngaysoat;
                   item.CheckTime = (int)checkTime;
                    item.BeginBalance = "0";
                    item.ChargeAmount = changeAmount.ToString();
                    item.Balance = "0";
                    item.VehicleClassID = lx;                  
                    item.RegisPlateNumber = "";
                    item.PlateType = "1";
                    item.RecogPlateNumber = sx;
                    item.IsIntelligentVeriField = "1";
                    item.IntelVerifyResult = "1";
                    item.ImageID = tranid;
                    item.ImageStatus = "1";
                    item.IsOnlineCheck = "1";
                    item.PeriodTicket = "0";
                    item.Checker = checker;
                    item.F0 = "0";
                    item.F1 = "0";
                    item.F2 = "0";
                    item.TransactionStatus = "";
                    item.TicketID = "";
                    item.CheckInDate = "";
                    item.CommitDate = "";
                    item.ETCStatus = "";


                    if(item.TypeTransaction == "SaveMonthQuarterTicket")
                    {
                        //RegisPlateNumber = soatVeTransactionModelString[8].Split(':')[1],
                    }
                    
                }

                lstTran.Add(item);
                this.DBView.DataSource = null;
                this.DBView.DataSource = lstTran;
                //this.DBView.Update();
                this.DBView.Refresh();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
