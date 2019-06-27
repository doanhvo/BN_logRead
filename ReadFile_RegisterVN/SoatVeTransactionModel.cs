using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using DataBase;
using NLog.LogReceiverService;

namespace ReadFile_RegisterVN
{
    public class SoatVeTransactionModel
    {
        //public string _connectionString =
        //    @"Data Source=HUYTO\MSSQLSERVER2K16;Initial Catalog=ETC_RFID;Persist Security Info=True;User ID=sa;Password=Abcd1234";

        public string _connectionString = @"Data Source=;Initial Catalog=ETC_RFID;Persist Security Info=True;User ID=sa;Password=";

        private NLogEvents log = new NLogEvents();
        public Guid ID { get; set; }

        public string TypeTransaction { get; set; }

        public string ObuID { get; set; }

        public string CheckDate { get; set; }

        public int CheckTime { get; set; }

        public string BeginBalance { get; set; }

        public string ChargeAmount { get; set; }

        public string Balance { get; set; }

        public string VehicleClassID { get; set; }

        public string LoginID { get; set; }

        public string LaneID { get; set; }

        public string ShiftID { get; set; }

        public string StationID { get; set; }

        public string RegisPlateNumber { get; set; }

        public string PlateType { get; set; }

        public string RecogPlateNumber { get; set; }

        public string IsIntelligentVeriField { get; set; }

        public string IntelVerifyResult { get; set; }

        public string ImageID { get; set; }

        public string ImageStatus { get; set; }

        public string IsOnlineCheck { get; set; }

        public string PeriodTicket { get; set; }

        public string Checker { get; set; }

        public string F0 { get; set; }

        public string F1 { get; set; }

        public string F2 { get; set; }

        public string TransactionStatus { get; set; }

        public string TicketID { get; set; }

        public string CheckInDate { get; set; }

        public string CommitDate { get; set; }

        public string ETCStatus { get; set; }

        public int SequenceID { get; set; }

        public string TID { get; set; }

        public static SoatVeTransactionModel CreateTransactionbyString(string SoatVeTransactionModelString)
        {
            return new SoatVeTransactionModel
            {
                ID = Guid.NewGuid(),
                ObuID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[0],
                CheckDate = CONVERTddMMyyyyHHmmssTOyyyyMMddHHmmss(SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[1].Replace("/", "-")),
                CheckTime = int.Parse(SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[2]),
                BeginBalance = "0",
                ChargeAmount = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[3],
                Balance = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[4],
                VehicleClassID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[5],
                LoginID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[6],
                LaneID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[7],
                ShiftID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[8],
                StationID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[9],
                RegisPlateNumber = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[10],
                PlateType = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[11],
                RecogPlateNumber = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[12],
                IsIntelligentVeriField = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[13],
                IntelVerifyResult = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[14],
                ImageID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[15],
                ImageStatus = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[16],
                IsOnlineCheck = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[17],
                PeriodTicket = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[18],
                Checker = CONVERTddMMyyyyHHmmssTOyyyyMMddHHmmss(SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[19].Replace("/", "-")),
                F0 = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[20],
                F1 = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[21],
                F2 = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[22],
                TransactionStatus = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[23],
                TicketID = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[24],
                CheckInDate = CONVERTddMMyyyyHHmmssTOyyyyMMddHHmmss(SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[25].Replace("/", "-")),
                CommitDate = CONVERTddMMyyyyHHmmssTOyyyyMMddHHmmss(SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[26].Replace("/", "-")),
                ETCStatus = SoatVeTransactionModelString.Split(new[]
                {
                    '|'
                })[27]
            };
        }

        public static SoatVeTransactionModel CreateTransactionbyString2(string type,
            string[] soatVeTransactionModelString)
        {
            if (type.Trim() == "SaveOnceTicket")
            {
                return new SoatVeTransactionModel
                {
                    TypeTransaction = type,
                    ID = Guid.NewGuid(),
                    ObuID = soatVeTransactionModelString[0].Split(':')[1],
                    CheckDate =
                        soatVeTransactionModelString[1].Split(' ')[0].Split(':')[1] + " " +
                        soatVeTransactionModelString[1].Split(' ')[1]
                        + " " + soatVeTransactionModelString[1].Split(' ')[2],
                    CheckTime = int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[0])*3600
                                + int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[1])*60
                                + int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[2]),
                    BeginBalance = "0",
                    ChargeAmount = soatVeTransactionModelString[16].Split(':')[1],
                    Balance = "0",
                    VehicleClassID = soatVeTransactionModelString[0].Split(':')[1].Substring(1, 1),
                    LoginID = soatVeTransactionModelString[4].Split(':')[1],
                    LaneID = soatVeTransactionModelString[5].Split(':')[1],
                    ShiftID = soatVeTransactionModelString[6].Split(':')[1],
                    StationID = soatVeTransactionModelString[7].Split(':')[1],
                    RegisPlateNumber = soatVeTransactionModelString[8].Split(':')[1],
                    PlateType = "1",
                    RecogPlateNumber = soatVeTransactionModelString[9].Split(':')[1],
                    IsIntelligentVeriField = "1",
                    IntelVerifyResult = "1",
                    ImageID = soatVeTransactionModelString[10].Split(':')[1],
                    ImageStatus = "1",
                    IsOnlineCheck = "1",
                    PeriodTicket = "0",
                    Checker =
                        soatVeTransactionModelString[11].Split(':')[1] + ":" +
                        soatVeTransactionModelString[11].Split(':')[2] + ":" +
                        soatVeTransactionModelString[11].Split(':')[3],
                    F0 = soatVeTransactionModelString[14].Split(':')[1],
                    F1 = soatVeTransactionModelString[15].Split(':')[1],
                    F2 = "0",
                    TransactionStatus = "",
                    TicketID = soatVeTransactionModelString[0].Split(':')[1],
                    CheckInDate = "",
                    CommitDate = "",
                    ETCStatus = ""
                };
            }
            if (type.Trim() == "EtagTransaction")
            {
                return new SoatVeTransactionModel
                {
                    TypeTransaction = type,
                    ID = Guid.NewGuid(),
                    ObuID = soatVeTransactionModelString[0].Split(':')[1],
                    CheckDate =
                        soatVeTransactionModelString[1].Split(' ')[0].Split(':')[1] + " " +
                        soatVeTransactionModelString[1].Split(' ')[1] + " "
                        + soatVeTransactionModelString[1].Split(' ')[2],
                    CheckTime = int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[0])*3600
                                + int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[1])*60
                                + int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[2]),
                    BeginBalance = "0",
                    ChargeAmount = soatVeTransactionModelString[25].Split(':')[1],
                    Balance = "0",
                    VehicleClassID = soatVeTransactionModelString[3].Split(':')[1],
                    LoginID = soatVeTransactionModelString[4].Split(':')[1],
                    LaneID = soatVeTransactionModelString[5].Split(':')[1],
                    ShiftID = soatVeTransactionModelString[6].Split(':')[1],
                    StationID = soatVeTransactionModelString[7].Split(':')[1],
                    RegisPlateNumber = soatVeTransactionModelString[8].Split(':')[1],
                    PlateType = soatVeTransactionModelString[9].Split(':')[1],
                    RecogPlateNumber = soatVeTransactionModelString[10].Split(':')[1],
                    IsIntelligentVeriField = soatVeTransactionModelString[11].Split(':')[1],
                    IntelVerifyResult = soatVeTransactionModelString[12].Split(':')[1],
                    ImageID = soatVeTransactionModelString[13].Split(':')[1],
                    ImageStatus = soatVeTransactionModelString[14].Split(':')[1],
                    IsOnlineCheck = "1",
                    PeriodTicket = soatVeTransactionModelString[15].Split(':')[1],
                    Checker =
                        soatVeTransactionModelString[16].Split(':')[1] + ":" +
                        soatVeTransactionModelString[16].Split(':')[2] + ":" +
                        soatVeTransactionModelString[16].Split(':')[3],
                    F0 = soatVeTransactionModelString[17].Split(':')[1],
                    F1 = soatVeTransactionModelString[18].Split(':')[1],
                    F2 = "0",
                    TransactionStatus = soatVeTransactionModelString[19].Split(':')[1],
                    TicketID = soatVeTransactionModelString[20].Split(':')[1],
                    CheckInDate =
                        soatVeTransactionModelString[21].Split(' ')[0].Split(':')[1] + " " +
                        soatVeTransactionModelString[21].Split(' ')[1] + " " +
                        soatVeTransactionModelString[21].Split(' ')[2],
                    CommitDate =
                        soatVeTransactionModelString[22].Split(' ')[0].Split(':')[1] + " " +
                        soatVeTransactionModelString[22].Split(' ')[1] + " " +
                        soatVeTransactionModelString[22].Split(' ')[2],
                    ETCStatus = soatVeTransactionModelString[23].Split(':')[1],
                };
            }
            if (type.Trim() == "SaveMonthQuarterTicket")
            {
                return new SoatVeTransactionModel
                {
                    TypeTransaction = type,
                    ID = Guid.NewGuid(),
                    ObuID = soatVeTransactionModelString[0].Split(':')[1],
                    CheckDate =
                        soatVeTransactionModelString[1].Split(' ')[0].Split(':')[1] + " " +
                        soatVeTransactionModelString[1].Split(' ')[1] + " " +
                        soatVeTransactionModelString[1].Split(' ')[2],
                    CheckTime = int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[0])*3600
                                + int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[1])*60
                                + int.Parse(soatVeTransactionModelString[1].Split(' ')[1].Split(':')[2]),
                    BeginBalance = "0",
                    ChargeAmount = soatVeTransactionModelString[16].Split(':')[1],
                    Balance = "0",
                    VehicleClassID = soatVeTransactionModelString[0].Split(':')[1].Substring(1, 1),
                    LoginID = soatVeTransactionModelString[4].Split(':')[1],
                    LaneID = soatVeTransactionModelString[5].Split(':')[1],
                    ShiftID = soatVeTransactionModelString[6].Split(':')[1],
                    StationID = soatVeTransactionModelString[7].Split(':')[1],
                    RegisPlateNumber = soatVeTransactionModelString[8].Split(':')[1],
                    PlateType = "1",
                    RecogPlateNumber = soatVeTransactionModelString[9].Split(':')[1],
                    IsIntelligentVeriField = "1",
                    IntelVerifyResult = "1",
                    ImageID = soatVeTransactionModelString[10].Split(':')[1],
                    ImageStatus = "1",
                    IsOnlineCheck = "1",
                    PeriodTicket = "0",
                    Checker =
                        soatVeTransactionModelString[11].Split(':')[1] + ":" +
                        soatVeTransactionModelString[11].Split(':')[2] + ":" +
                        soatVeTransactionModelString[11].Split(':')[3],
                    F0 = soatVeTransactionModelString[14].Split(':')[1],
                    F1 = soatVeTransactionModelString[15].Split(':')[1],
                    F2 = "0",
                    TransactionStatus = "",
                    TicketID = soatVeTransactionModelString[0].Split(':')[1],
                    CheckInDate = "",
                    CommitDate = "",
                    ETCStatus = ""
                };
            }
            return null;
        }

        public static string CONVERTddMMyyyyHHmmssTOyyyyMMddHHmmss(string date)
        {
            string result = string.Empty;
            result = result + date.Split(new[]
            {
                ' '
            })[0].Split(new[]
            {
                '-'
            })[2] + "-";
            result = result + date.Split(new[]
            {
                ' '
            })[0].Split(new[]
            {
                '-'
            })[1] + "-";
            result = result + date.Split(new[]
            {
                ' '
            })[0].Split(new[]
            {
                '-'
            })[0] + " ";
            result = result + date.Split(new[]
            {
                ' '
            })[1].Split(new[]
            {
                ':'
            })[0] + ":";
            result = result + date.Split(new[]
            {
                ' '
            })[1].Split(new[]
            {
                ':'
            })[1] + ":";
            return result + date.Split(new[]
            {
                ' '
            })[1].Split(new[]
            {
                ':'
            })[2];
        }

        public void InsertDB(ref DAL da)
        {
            using (DataTable dt = da.selectDataBase("TRP_tblCheckObuAccount_RFID", "count(*)", string.Concat(new[]
            {
                "ImageID = '",
                ImageID,
                "' and LaneID = '",
                LaneID,
                "'"
            }), ""))
            {
                if (dt.Rows[0][0].ToString().Equals("0"))
                {
                    da.insertDataBase("TRP_tblCheckObuAccount_RFID",
                        "([ObuID],[PrepaidAccountID],[CheckDate],[CheckTime],[BeginBalance],[ChargeAmount],[Balance],[VehicleClassID],[LoginID],[LaneID],[ShiftID],[StationID] ,[RegisPlateNumber] ,[PlateType],[RecogPlateNumber],[IsIntelligentVerified],[IntelVerifyResult],[ImageID],[ImageStatus],[PeriodTicket],[Checker],[SupervisionStatus],[PreSupervisionStatus],[F0],[F1],[F2],[SyncStatus],[ModifyDate],[SyncReturnMsg],[FP],[FC],[TransactionStatus],[TicketID],[CheckInDate],[CommitDate],[ETCStatus],[FeeType],[FeeChargeType],[FeeChargeInfo],[Notes],[SyncEtcMtc],[SyncFeBe],[IsOnlineCheck])",
                        string.Concat(new object[]
                        {
                            "('",
                            ObuID,
                            "',N'','",
                            CheckDate,
                            "','",
                            CheckTime,
                            "','",
                            BeginBalance,
                            "','",
                            ChargeAmount,
                            "','",
                            Balance,
                            "','",
                            VehicleClassID,
                            "','",
                            LoginID,
                            "','",
                            LaneID,
                            "','",
                            ShiftID,
                            "','",
                            StationID,
                            "','",
                            RegisPlateNumber,
                            "','",
                            PlateType,
                            "','",
                            RecogPlateNumber,
                            "','",
                            IsIntelligentVeriField,
                            "','",
                            IntelVerifyResult,
                            "','",
                            ImageID,
                            "','",
                            ImageStatus,
                            "','",
                            PeriodTicket,
                            "','",
                            Checker,
                            "',N'',N'','",
                            F0,
                            "','",
                            F1,
                            "','",
                            F2,
                            "','",
                            0,
                            "','",
                            CheckInDate,
                            "',N'',N'',N'','",
                            TransactionStatus,
                            "','",
                            TicketID,
                            "','",
                            CheckInDate,
                            "','",
                            CommitDate,
                            "','",
                            ETCStatus,
                            "',N'','0',N'','",
                            string.Empty,
                            "','",
                            0,
                            "','",
                            0,
                            "','",
                            IsOnlineCheck,
                            "')"
                        }));
                }
            }
        }

        public static SqlCommand InitCommand(string pStoredName)
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = pStoredName;
            return cmd;
        }

        public void SaveOnceTicket(SoatVeTransactionModel item)
        {
            try
            {
                using (SqlCommand cmd = InitCommand("Add_SoatVeLuot"))
                {
                    // Add param 
                    cmd.Parameters.Add("@sTID", SqlDbType.VarChar, 20).Value = item.ObuID;
                    cmd.Parameters.Add("@sMSLane", SqlDbType.VarChar, 3).Value = item.LaneID;
                    cmd.Parameters.Add("@sGiaVe", SqlDbType.Int).Value = item.ChargeAmount;
                    cmd.Parameters.Add("@sGioSoat", SqlDbType.Int).Value = item.CheckTime;
                    cmd.Parameters.Add("@sNgaySoat", SqlDbType.DateTime).Value =
                       
                    new DateTime(int.Parse(item.CheckDate.Split('/')[2].Split(' ')[0]),
                          int.Parse(item.CheckDate.Split('/')[1]), int.Parse(item.CheckDate.Split('/')[0]));

                    cmd.Parameters.Add("@sLogin", SqlDbType.VarChar, 20).Value = item.LoginID;
                    cmd.Parameters.Add("@sCa", SqlDbType.VarChar, 3).Value = item.ShiftID;
                    cmd.Parameters.Add("@sMSLoaiVe", SqlDbType.VarChar, 2).Value = "1" + item.VehicleClassID;
                    cmd.Parameters.Add("@sMSLoaiXe", SqlDbType.VarChar, 2).Value = "";
                    //biến checker hiện giờ dùng chứa datestring @
                    cmd.Parameters.Add("@sChecker", SqlDbType.VarChar, 20).Value = item.Checker;
                    cmd.Parameters.Add("@sSoXe", SqlDbType.VarChar, 20).Value = item.RecogPlateNumber;
                    cmd.Parameters.Add("@sF0", SqlDbType.VarChar, 1).Value = item.F0;
                    cmd.Parameters.Add("@sF1", SqlDbType.VarChar, 1).Value = item.F1;
                    cmd.Parameters.Add("@sF2", SqlDbType.VarChar, 1).Value = item.F2;
                    cmd.Parameters.Add("@sImageId", SqlDbType.NVarChar, 20).Value = item.ImageID;
                    //if (this.SFTPUsername.Equals("station0075"))
                    //{
                    //   cmd.Parameters.Add("@sMSTram", SqlDbType.VarChar, 1).Value = this.StationID;
                    //    cmd.Parameters.Add("@sKhuHoi", SqlDbType.VarChar, 1).Value = 0;
                    // Soat tại trạm dau vào nên la 0
                    // cmd.Parameters.Add("@IsSGVT", SqlDbType.Int).Value = IsSGVT;
                    // cmd.Parameters.Add("@IsVTSG", SqlDbType.Int).Value = IsVTSG;
                    // }
                    //Tam add
                    cmd.Parameters.Add("@sEtagID", SqlDbType.VarChar, 24).Value = "";

                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;

                    // Thuc thi stored
                    ExecuteNonQuery(cmd);

                    // Kiem tra ket qua 
                    int retValue = int.Parse(cmd.Parameters["@Result"].Value.ToString());
                    string strReturnMsg = string.Empty;

                    // Ghi log database
                    //CommitDatabaseLog("SaveOnceTicket", this, retValue);

                    //return (retValue > 0) ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(GetType() + "." + MethodBase.GetCurrentMethod().Name,
                    ex.Message); //STORED_ONCE_TICKET, ex.Message);
            }

            //return 0;
        }

        public int SaveForceOpen(SoatVeTransactionModel item)
        {
            try
            {
                using (SqlCommand cmd = TKUtils.InitCommand("Add_ForceOpen"))
                {
                    // Add param                
                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@sTID", SqlDbType.VarChar, 20).Value = TicketID;
                    cmd.Parameters.Add("@sMSLane", SqlDbType.VarChar, 3).Value = LaneID;
                    cmd.Parameters.Add("@sLogin", SqlDbType.VarChar, 20).Value = LoginID;
                    cmd.Parameters.Add("@sCa", SqlDbType.VarChar, 3).Value = ShiftID;
                    cmd.Parameters.Add("@sGioMo", SqlDbType.Int).Value = CheckTime;
                    cmd.Parameters.Add("@sNgaySoat", SqlDbType.DateTime).Value =
                        new DateTime(int.Parse(item.CheckDate.Split('/')[2].Split(' ')[0]),
                            int.Parse(item.CheckDate.Split('/')[0]), int.Parse(item.CheckDate.Split('/')[1]));
                    cmd.Parameters.Add("@sGioDong", SqlDbType.Int).Value = CheckTime;
                    cmd.Parameters.Add("@sNgayDong", SqlDbType.DateTime).Value =
                        new DateTime(int.Parse(item.CheckDate.Split('/')[2].Split(' ')[0]),
                            int.Parse(item.CheckDate.Split('/')[0]), int.Parse(item.CheckDate.Split('/')[1]));
                    //cmd.Parameters.Add("@sTramID", SqlDbType.Int).Value = StationID;
                    //cmd.Parameters.Add("@sReason", SqlDbType.Int).Value = Reason;
                    //cmd.Parameters.Add("@sTicketType", SqlDbType.Int).Value = Reason; // (int)Category;????
                    //Biến checker chỗ này hiện giờ dùng chứa datestring
                    cmd.Parameters.Add("@sChecker", SqlDbType.VarChar, 20).Value = item.Checker;
                    cmd.Parameters.Add("@sSoXe", SqlDbType.VarChar, 20).Value = item.RecogPlateNumber;
                    cmd.Parameters.Add("@sF0", SqlDbType.VarChar, 1).Value = F0;
                    cmd.Parameters.Add("@sF1", SqlDbType.VarChar, 1).Value = F1;
                    cmd.Parameters.Add("@sF2", SqlDbType.VarChar, 1).Value = F2;
                    cmd.Parameters.Add("@sImageId", SqlDbType.NVarChar, 20).Value = item.ImageID;
                    //if (this.SFTPUsername.Equals("station0075"))
                    //{
                    //    cmd.Parameters.Add("@sMSTram", SqlDbType.VarChar, 1).Value = StationID;
                    //}
                    //Tam add
                    cmd.Parameters.Add("@sEtagID", SqlDbType.VarChar, 24).Value = "";

                    // Thuc thi stored
                    ExecuteNonQuery(cmd);

                    // Kiem tra ket qua 
                    int retValue = int.Parse(cmd.Parameters["@Result"].Value.ToString());

                    return (retValue > 0) ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(GetType() + "." + MethodBase.GetCurrentMethod().Name,
                    ex.Message); //STORED_FORCE_TICKET+Category.ToString(), ex.Message);
            }
            return 0;
        }

        public int SaveMonthQuarterTicket(SoatVeTransactionModel item)
        {
            try
            {
                using (SqlCommand cmd = TKUtils.InitCommand("Add_SoatVeThangQui"))
                {
                    // Add param                
                    cmd.Parameters.Add("@Result", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@sGioSoat", SqlDbType.Int).Value = CheckTime;
                    cmd.Parameters.Add("@sNgaySoat", SqlDbType.DateTime).Value =
                        new DateTime(int.Parse(item.CheckDate.Split('/')[2].Split(' ')[0]),
                            int.Parse(item.CheckDate.Split('/')[0]), int.Parse(item.CheckDate.Split('/')[1]));
                    cmd.Parameters.Add("@sMSLane", SqlDbType.VarChar, 3).Value = LaneID;
                    cmd.Parameters.Add("@sLogin", SqlDbType.VarChar, 20).Value = LoginID;
                    cmd.Parameters.Add("@sCa", SqlDbType.VarChar, 3).Value = ShiftID;
                    cmd.Parameters.Add("@sMSLoaiXe", SqlDbType.VarChar, 2).Value = "";
                    //biến checker hiện giờ dùng chứa datestring
                    cmd.Parameters.Add("@sChecker", SqlDbType.VarChar, 20).Value = item.CheckDate;
                    cmd.Parameters.Add("@sSoXe", SqlDbType.VarChar, 20).Value = item.RecogPlateNumber;

                    cmd.Parameters.Add("@sF0", SqlDbType.VarChar, 1).Value = F0;
                    cmd.Parameters.Add("@sF1", SqlDbType.VarChar, 1).Value = F1;
                    cmd.Parameters.Add("@sF2", SqlDbType.VarChar, 1).Value = F2;
                    cmd.Parameters.Add("@sImageId", SqlDbType.NVarChar, 20).Value = item.ImageID;
                    cmd.Parameters.Add("@sTID", SqlDbType.VarChar, 20).Value = TicketID;

                    if (item.TicketID.Substring(0, 1) == "B")
                        cmd.Parameters.Add("@sMSLoaiVe", SqlDbType.VarChar, 2).Value = "2" + item.VehicleClassID;
                    else if (item.TicketID.Substring(0, 1) == "C")
                        cmd.Parameters.Add("@sMSLoaiVe", SqlDbType.VarChar, 2).Value = "3" + item.VehicleClassID;

                    cmd.Parameters.Add("@sGiaVe", SqlDbType.Int).Value = item.ChargeAmount;
                    //if (this.SFTPUsername.Equals("station0075"))
                    //{
                    //    cmd.Parameters.Add("@sMSTram", SqlDbType.VarChar, 1).Value = StationID;
                    //}
                    //Tam add
                    cmd.Parameters.Add("@sEtagID", SqlDbType.VarChar, 24).Value = "";

                    // Thuc thi stored
                    ExecuteNonQuery(cmd);

                    // Kiem tra ket qua 
                    int retValue = int.Parse(cmd.Parameters["@Result"].Value.ToString());

                    return (retValue > 0) ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(GetType() + "." + MethodBase.GetCurrentMethod().Name,
                    ex.Message); //STORED_MONTH_QUARTER_TICKET, ex.Message);
                return 0;
            }
        }

        public int CommitRFID(SoatVeTransactionModel item)
        {
            int res = 0;
            SqlCommand cmd = null;
            try
            {
                using (cmd = TKUtils.InitCommand("TRP_spCheckObuAccount_RFID"))
                {
                    cmd.Parameters.Add("@Activity", SqlDbType.NVarChar, 50).Value = "Save";
                    cmd.Parameters.Add("@ReturnMsg", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ReturnValue", SqlDbType.Int).Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ObuID", SqlDbType.NVarChar).Value = item.ObuID;
                    var dt = new DateTime(int.Parse(item.ImageID.Substring(0, 4)),
                        int.Parse(item.ImageID.Substring(4, 2)), int.Parse(item.ImageID.Substring(6, 2)),
                        int.Parse(item.ImageID.Substring(8, 2)), int.Parse(item.ImageID.Substring(10, 2)),
                        int.Parse(item.ImageID.Substring(12, 2)), int.Parse(item.ImageID.Substring(14, 3)));
                    cmd.Parameters.Add("@CheckDate", SqlDbType.DateTime).Value = dt;

                    cmd.Parameters.Add("@CheckTime", SqlDbType.Int).Value = item.CheckTime;
                    cmd.Parameters.Add("@BeginBalance", SqlDbType.Int).Value = item.BeginBalance;
                    cmd.Parameters.Add("@ChargeAmount", SqlDbType.Int).Value = item.ChargeAmount; //
                    cmd.Parameters.Add("@Balance", SqlDbType.Int).Value = 0;
                    cmd.Parameters.Add("@VehicleClassID", SqlDbType.Int).Value = item.VehicleClassID; //vehicle type
                    if (string.IsNullOrEmpty(item.LoginID))
                    {
                        cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar).Value = "0000";
                        //nhan vien mac dinh, chua login
                    }
                    else
                    {
                        cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar).Value = item.LoginID;
                    }
                    cmd.Parameters.Add("@LaneID", SqlDbType.NVarChar).Value = item.LaneID;
                    cmd.Parameters.Add("@ShiftID", SqlDbType.NVarChar).Value = item.ShiftID;
                    cmd.Parameters.Add("@StationID", SqlDbType.NVarChar).Value = item.StationID;
                    cmd.Parameters.Add("@RegisPlateNumber", SqlDbType.NVarChar).Value = item.RegisPlateNumber;
                    cmd.Parameters.Add("@PlateType", SqlDbType.SmallInt).Value = item.PlateType;
                    cmd.Parameters.Add("@RecogPlateNumber", SqlDbType.NVarChar).Value = item.RecogPlateNumber;
                    cmd.Parameters.Add("@IsIntelligentVeriField", SqlDbType.Bit).Value = false;
                    cmd.Parameters.Add("@IntelVerifyResult", SqlDbType.SmallInt).Value = item.IntelVerifyResult;
                    cmd.Parameters.Add("@ImageID", SqlDbType.NVarChar).Value = item.ImageID;
                    cmd.Parameters.Add("@ImageStatus", SqlDbType.Int).Value = ImageStatus; //VDH 30july
                    cmd.Parameters.Add("@IsOnlineCheck", SqlDbType.Int).Value = item.IsOnlineCheck; //VDH 30july
                    cmd.Parameters.Add("@PeriodTicket", SqlDbType.Int).Value = item.PeriodTicket;
                    cmd.Parameters.Add("@Checker", SqlDbType.VarChar, 20).Value = item.Checker;
                    cmd.Parameters.Add("@F0", SqlDbType.VarChar, 1).Value = item.F0;
                    cmd.Parameters.Add("@F1", SqlDbType.VarChar, 1).Value = item.F1;

                    #region [20160916] HUYTO

                    cmd.Parameters.Add("@F2", SqlDbType.VarChar, 1).Value = item.F2;

                    #endregion

                    cmd.Parameters.Add("@TransactionStatus", SqlDbType.Int, 1).Value = TransactionStatus;
                    cmd.Parameters.Add("@TicketID", SqlDbType.NVarChar, 20).Value = item.TicketID;
                    cmd.Parameters.Add("@CheckInDate", SqlDbType.DateTime).Value =
                        new DateTime(int.Parse(item.CheckInDate.Split('/')[2].Split(' ')[0]),
                            int.Parse(item.CheckInDate.Split('/')[0]), int.Parse(item.CheckInDate.Split('/')[1]),
                            int.Parse(item.CheckInDate.Split(' ')[1].Split(':')[0]),
                            int.Parse(item.CheckInDate.Split(' ')[1].Split(':')[1]),
                            int.Parse(item.CheckInDate.Split(' ')[1].Split(':')[2]));
                    cmd.Parameters.Add("@CommitDate", SqlDbType.DateTime).Value =
                        new DateTime(int.Parse(item.CommitDate.Split('/')[2].Split(' ')[0]),
                            int.Parse(item.CommitDate.Split('/')[0]), int.Parse(item.CommitDate.Split('/')[1]),
                            int.Parse(item.CommitDate.Split(' ')[1].Split(':')[0]),
                            int.Parse(item.CommitDate.Split(' ')[1].Split(':')[1]),
                            int.Parse(item.CommitDate.Split(' ')[1].Split(':')[2]));
                    cmd.Parameters.Add("@ETCStatus", SqlDbType.Char, 1).Value = item.ETCStatus;

                    //cmd.Parameters.Add("@SequenceID", SqlDbType.BigInt).Value = item.SequenceID;

                    cmd.Parameters.Add("@TID", SqlDbType.VarChar, 48).Value = "";

                    //else
                    //{
                    //    cmd.Parameters.Add("@SequenceID", SqlDbType.BigInt).Value = 0;
                    //}
                    // Thuc thi stored
                    int numRows = ExecuteNonQuery(cmd);

                    // Kiem tra ket qua  
                    if (numRows < 1)
                    {
                        res = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(GetType() + "." + MethodBase.GetCurrentMethod().Name,
                    ex.ToString());
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Connection.Close();
                    cmd.Dispose();
                }
                cmd = null;
            }
            return res;
        }

        public int ExecuteNonQuery(SqlCommand pCommand)
        {
            int numRows = 0;
            SqlConnection connection = null;
            try
            {
                using (connection = new SqlConnection(_connectionString))
                {
                    pCommand.Connection = connection;
                    pCommand.Connection.Open();
                    numRows = pCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(GetType() + "." + MethodBase.GetCurrentMethod().Name,
                    ex.Message); //STORED_ONCE_TICKET, ex.Message);
            }

            return numRows;
        }
    }

    /// <summary>
    /// VD: 2019-06-27 00:57:20.381 : [ProcessingVehicleQueue - _vehicleQueue.Count: 1, Vehicle type: 2, TransID: 20190627005715021 ]
    /// </summary>
    public class LogFileModel
    {
        /// <summary>
        /// Phần thời gian: 2019-06-27 00:57:20.381
        /// </summary>
        public DateTime TimeLog { get; set; }

        /// <summary>
        /// Tên hàm: "ProcessingVehicleQueue"
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// Phần nội dung sau dấu "-": _vehicleQueue.Count: 1, Vehicle type: 2, TransID: 20190627005715021
        /// </summary>
        public string Content { get; set; }
    }
}