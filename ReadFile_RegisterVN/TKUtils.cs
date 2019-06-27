using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;

namespace ReadFile_RegisterVN
{
    public class TKUtils
    {
        #region Fields

        /// <summary>
        /// Hung add 20110716
        /// </summary>
        private static string BAT_FILE = "./SyncTimerSvr.bat";

        /// <summary>
        /// Log4Net object
        /// </summary>

        /// <summary>
        /// Log file chung
        /// </summary>
        private static string LOG_FILE = "./LogFolder/LogFile_{0}.txt";

        private static string LOG_FILE_COMMIT = "./LogFolder/LogFileCommit_{0}.txt";

        private static string LOG_FILE_TID = "./LogFolder/LogFileTID_{0}.txt";

        /// <summary>
        /// Log file chung
        /// </summary>
        private static string LOG_FILE_TRX = "./LogFolder/LogTRX_{0}.txt";

        /// <summary>
        /// Log file chup hinh, nhan dang
        /// </summary>
        private static string CAPTURE_LOG_FILE = "./LogFolder/CaptureLog_{0}.txt";

        /// <summary>
        /// Log file SQL
        /// </summary>
        private static string SQL_LOG_FILE = "./LogFolder/SQLLog_{0}.txt";

        private static string PLC_LOG_FILE = "./LogFolder/PLC_{0}.txt";

        /// <summary>
        /// Log file kiem tra ket noi
        /// </summary>
        private static string CONNECTION_LOG_FILE = "./LogFolder/ConnectionLog_{0}.txt";

        /// <summary>
        /// Log file ghi giao dich voi OBU
        /// </summary>
        private static string OBU_LOG_FILE = "./LogFolder/ObuTransLog_{0}.txt";
        //28sep2016 vdh doi obulogfolder thanh logfolder

        private const string mConfigFile = "Config.xml";

        #endregion

        #region Get Value

        /// <summary>
        /// Lấy giá trị số nguyên từ CSDL
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public static int GetIntegerValue(string pID)
        {
            return (string.IsNullOrEmpty(pID)) ? -1 : int.Parse(pID);
        }

        //Get so tien tu CSDL cat bot 1000vnd
        public static int GetIntegerValue2(string pID)
        {
            return (string.IsNullOrEmpty(pID)) ? -1 : int.Parse(pID) / 1000;
        }

        /// <summary>
        /// Lấy giá trị số nguyên từ CSDL
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public static long GetLongIntegerValue(string pID)
        {
            return (string.IsNullOrEmpty(pID)) ? -1 : long.Parse(pID);
        }

        /// <summary>
        /// Lấy giá trị số thực từ CSDL
        /// </summary>
        /// <param name="pID"></param>
        /// <returns></returns>
        public static double GetDoubleValue(string pID)
        {
            return (string.IsNullOrEmpty(pID)) ? -1 : double.Parse(pID);
        }

        /// <summary>
        /// Lấy giá trị ngày tháng từ CSDL
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeValue(object pDateTime)
        {
            return (string.IsNullOrEmpty(pDateTime.ToString())) ? DateTime.MinValue : (DateTime)pDateTime;
        }

        /// <summary>
        /// Lấy giá trị boolean từ CSDL
        /// </summary>
        /// <param name="pBool"></param>
        /// <returns></returns>
        public static bool GetBooleanValue(object pBool)
        {
            return (string.IsNullOrEmpty(pBool.ToString())) ? false : (bool)pBool;
        }

        #endregion

        #region Database

        /// <summary>
        /// Hàm khởi tạo SqlCommand
        /// </summary>
        /// <param name="pStore"></param>
        /// <param name="pActivity"></param>
        /// <returns></returns>
        public static SqlCommand InitCommand(string pStoredName, string pActivity)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = pStoredName;
            cmd.Parameters.Add("@Activity", SqlDbType.NVarChar, 50).Value = pActivity;
            return cmd;
        }


        public static long getTimeStamp(DateTime dt)
        {
            TimeZoneInfo NYTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            TimeZone localZone = TimeZone.CurrentTimeZone;
            TimeSpan span = (TimeZoneInfo.ConvertTime(dt, NYTimeZone) - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            NYTimeZone = null;
            localZone = null;
            return (long)Convert.ToDouble(span.TotalSeconds);
        }

        /// <summary>
        /// Khoi tao command khong co activity
        /// </summary>
        /// <param name="pStoredName"></param>
        /// <returns></returns>
        public static SqlCommand InitCommand(string pStoredName)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = pStoredName;
            return cmd;
        }

        /// <summary>
        /// Ham them tham so kieu integer vao command (kiem tra gia tri null)
        /// </summary>
        /// <param name="pCmd"></param>
        /// <param name="pParamName"></param>
        /// <param name="pValue"></param>
        public static void AddIntegerParam(SqlCommand pCmd, string pParamName, int pValue)
        {
            if (pValue != -1)
                pCmd.Parameters.Add(pParamName, SqlDbType.Int).Value = pValue;
            else
                pCmd.Parameters.Add(pParamName, SqlDbType.Int).Value = DBNull.Value;
        }

        public static string AntiSQLInject(string str)
        {
            if (str == null) return "";
            return str.Replace("'", "\\'");
        }

        /// <summary>
        /// Lay dong chua du lieu can tim
        /// </summary>
        /// <param name="pDataTable"></param>
        /// <param name="pColumnName"></param>
        /// <param name="pValue"></param>
        /// <returns></returns>
        public static DataRow GetDataRow(DataTable pDataTable, string pColumnName, string pValue)
        {
            foreach (DataRow dr in pDataTable.Rows)
            {
                if (dr[pColumnName].ToString() == pValue)
                    return dr;
            }

            return null;
        }

        
        public static bool CheckConnectionSQL(string SQLHost, int SQLPort, ref string msg)
        {
            try
            {
                msg = string.Empty;
                System.Net.Sockets.TcpClient TcpCli = new System.Net.Sockets.TcpClient();
                var result = TcpCli.BeginConnect(SQLHost, SQLPort, null, null);
                result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                if (!TcpCli.Connected)
                {
                    TcpCli.Close();
                    return false;
                }
                TcpCli.Close();
                result = null;
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
        }

        #endregion

        #region Zip file

        //private Ultils();
        //sourceFile = @"D:\Ritesh\standards.pdf"
        //destinationFile = @"C:\backup\standards.zip"
        public static bool ZipFile(string sourceFile, string destinationFile)
        {
            try
            {
                FileStream oldFile = File.OpenRead(sourceFile);
                FileStream newFile = File.Create(destinationFile);
                GZipStream compression = new GZipStream(newFile, CompressionMode.Compress);

                byte[] buffer = new byte[1024];
                int numberOfBytesRead = oldFile.Read(buffer, 0, buffer.Length);
                while (numberOfBytesRead > 0)
                {
                    compression.Write(buffer, 0, numberOfBytesRead);
                    numberOfBytesRead = oldFile.Read(buffer, 0, buffer.Length);
                }
                compression.Close();
                return true;
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile("ZipFile:" + sourceFile, ex.Message);
                return false;
            }
        }

        //Example data to run UnZipFile function :
        //sourceFile = @"C:\backup\standards.zip
        //destinationFile = @"C:\backup\standards.pdf"
        public static bool UnZipFile(string sourceFile, string destinationFile)
        {
            try
            {
                FileStream compressFile = File.Open(sourceFile, FileMode.Open);
                FileStream uncompressedFile = File.Create(destinationFile);
                GZipStream compression = new GZipStream(compressFile, CompressionMode.Decompress);
                int data = compression.ReadByte();
                while (data != -1)
                {
                    uncompressedFile.WriteByte((byte)data);
                    data = compression.ReadByte();
                }
                compression.Close();
                return true;
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile("UnZipFile:" + sourceFile, ex.Message);
                return false;
            }
        }

        #endregion


        public static void WriteTrackFile(string strFuncName, string strMsg)
        {
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format("./LogFolder/TrackFile_{0}.txt", strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }

#if DEBUG
            TKUtils.ShowError(strFuncName, strMsg);
#endif
        }

        public static void checkFolderSyncData()
        {
            try
            {
                //Hung add 20110717 check exist folder log
                if (System.IO.Directory.Exists("Sync") == false)
                    System.IO.Directory.CreateDirectory("Sync");
                //end check
            }
            catch
            {
            }
        }

        public static void checkFolderLogfile()
        {
            try
            {
                //Hung add 20110717 check exist folder log
                if (System.IO.Directory.Exists("LogFolder") == false)
                    System.IO.Directory.CreateDirectory("LogFolder");
                //end check
            }
            catch
            {
            }
        }

        /// <summary>
        /// Ham ghi log file chung
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile(string strFuncName, string strMsg)
        {
            //string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            //string filename = string.Format(LOG_FILE, strDate);

            //// Write log in other Thread 20-03-2017
            //new Thread(() =>
            //{
            //    try
            //    {
            //        using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
            //        {
            //            string logLine = System.String.Format("{0:G}: {1}.", System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"), "[" + strFuncName + " - " + strMsg + "] ");
            //            sw.WriteLine(logLine);
            //            sw.WriteLine("-------------------------------------------");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // throw ex;
            //    }
            //    finally
            //    {
            //        strDate = null;
            //        filename = null;
            //    }
            //}).Start();


            //#if DEBUG
            //                TKUtils.ShowError(strFuncName,strMsg);              
            //#endif
        }

        public static void WriteLogFileCommit(string strFuncName, string strMsg)
        {
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(LOG_FILE_COMMIT, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            finally
            {
                strDate = null;
                filename = null;
            }

#if DEBUG
            TKUtils.ShowError(strFuncName, strMsg);
#endif
        }

        public static void WriteLogFileTID(string strFuncName, string strMsg)
        {
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(LOG_FILE_TID, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            finally
            {
                strDate = null;
                filename = null;
            }

#if DEBUG
            TKUtils.ShowError(strFuncName, strMsg);
#endif
        }

        /// <summary>
        /// Ham ghi log file chung
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile_Efkon(string strFuncName, string strMsg)
        {
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "") + "_Efkon_Packet_Log";
            string filename = string.Format(LOG_FILE, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }

#if DEBUG
            TKUtils.ShowError(strFuncName, strMsg);
#endif
        }

        /// <summary>
        /// Ham ghi log file Efkon OBU error
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile_Efkon_Error(string strFuncName, string strMsg)
        {
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "") + "_Efkon_Log";
            string filename = string.Format(LOG_FILE, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }

            //#if DEBUG
            //                TKUtils.ShowError(strFuncName,strMsg);              
            //#endif
        }


        public static string GetLogLine(string strFuncName, string strMsg)
        {
            string logLine =
                System.String.Format("{0:G}: {1}.", System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                    "[" + strFuncName + " - " + strMsg + "] ") + Environment.NewLine;
            logLine += "-------------------------------------------" + Environment.NewLine;
            return logLine;
        }

        public static void WriteLogLine(string pLogLine)
        {
            return;
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(LOG_FILE, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    sw.Write(pLogLine);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ham ghi log file chung
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile(Exception ex)
        {
#if DEBUG
            TKUtils.ShowError(ex);
#endif
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(LOG_FILE, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string strFuncName = string.Format("{0}.{1}()", ex.TargetSite.DeclaringType.FullName,
                        ex.TargetSite.Name);
                    string logLine = System.String.Format("{0:G}: [{1}].", System.DateTime.Now, strFuncName);
                    sw.WriteLine(logLine);
                    sw.WriteLine(ex.Message);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ham ghi log file capture
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile_Capture(string strFuncName, string strMsg)
        {
#if DEBUG
            //TKUtils.ShowError(ex);
#endif
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(CAPTURE_LOG_FILE, strDate);

            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ham ghi log file OBU transaction
        /// </summary>
        /// <param name="pTransLog"></param>
        public static void WriteLogFile_OBU(string pTransLog)
        {
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(OBU_LOG_FILE, strDate);
            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    sw.WriteLine(pTransLog);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Ham open log file OBU transaction
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static StreamReader OpenLogFile_OBU()
        {
            try
            {
                string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
                string fileName = string.Format(OBU_LOG_FILE, strDate, DateTime.Now.Hour.ToString());

                return OpenLogFile_OBU(fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDateTime"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static StreamReader OpenLogFile_OBU(DateTime pDateTime, int hour)
        {
            try
            {
                string strDate = String.Format("{0:yyyy/MM/dd}", pDateTime).Replace("/", "");
                string fileName = string.Format(OBU_LOG_FILE, strDate, hour.ToString());

                return OpenLogFile_OBU(fileName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Ham open log file OBU transaction
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static StreamReader OpenLogFile_OBU(string pFileName)
        {
            StreamReader sr = null;
            try
            {
                if (File.Exists(pFileName))
                {
                    FileStream fs = new FileStream(pFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    sr = new StreamReader(fs);
                }
            }
            catch (Exception)
            {
            }

            return sr;
        }

        /// <summary>
        /// Ham ghi log file  kiem tra ket noi
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile_Connection(Exception ex)
        {
#if DEBUG
            TKUtils.ShowError(ex);
#endif
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(CONNECTION_LOG_FILE, strDate);
            System.IO.StreamWriter sw = System.IO.File.AppendText(filename);
            try
            {
                string strFuncName = string.Format("{0}.{1}()", ex.TargetSite.DeclaringType.FullName, ex.TargetSite.Name);
                string logLine = System.String.Format("{0:G}: [{1}].", System.DateTime.Now, strFuncName);
                sw.WriteLine(logLine);
                sw.WriteLine(ex.Message);
                sw.WriteLine("-------------------------------------------");
            }
            catch (Exception exx)
            {
                TKUtils.ShowError(exx.Message);
            }
            finally
            {
                sw.Close();
            }
        }


        /// <summary>
        /// Log plc hour
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile_PLC(string strFuncName, string strMsg)
        {
#if DEBUG
            //TKUtils.ShowError(ex);
#endif
            string strDate = String.Format("{0:yyyy/MM/dd/HH}", DateTime.Now).Replace("/", "");
            string filename = string.Format(PLC_LOG_FILE, strDate);

            // Write log in other thread. 20-03-2017
            new Thread(() =>
            {
                try
                {
                    using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                    {
                        string logLine = System.String.Format("{0:G}: {1}.",
                            System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                            "[" + strFuncName + " - " + strMsg + "] ");
                        sw.WriteLine(logLine);
                        sw.WriteLine("-------------------------------------------");
                    }
                }
                catch (Exception ex)
                {
                }
            }).Start();
        }

        /// <summary>
        /// Ham ghi log file  kiem tra ket noi
        /// </summary>
        /// <param name="strFuncName"></param>
        /// <param name="strMsg"></param>
        public static void WriteLogFile_SQL(string strFuncName, string strMsg)
        {
#if DEBUG
            //TKUtils.ShowError(ex);
#endif
            string strDate = String.Format("{0:yyyy/MM/dd}", DateTime.Now).Replace("/", "");
            string filename = string.Format(SQL_LOG_FILE, strDate);

            try
            {
                using (System.IO.StreamWriter sw = System.IO.File.AppendText(filename))
                {
                    string logLine = System.String.Format("{0:G}: {1}.",
                        System.DateTime.Now.ToString("dd/MM/yyy HH:mm:ss:fff"),
                        "[" + strFuncName + " - " + strMsg + "] ");
                    sw.WriteLine(logLine);
                    sw.WriteLine("-------------------------------------------");
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Them so 0 vao truoc mot so
        /// </summary>
        /// <param name="pNumber">So can them</param>
        /// <param name="pNumNumbers">So chu so</param>
        /// <returns></returns>
        public static string AddZeroBeforeNumber(int pNumber, int pNumNumbers)
        {
            string retValue = pNumber.ToString();

            for (int i = pNumber.ToString().Length; i < pNumNumbers; i++)
            {
                retValue = "0" + retValue;
            }
            return retValue;
        }

        /// <summary>
        /// Tao chuoi datestring cho moi luot soat ve
        /// </summary>
        /// <param name="pForObu">De biet transid nay co phai dung cho obu hay khong</param>
        /// <returns></returns>
        public static string CreateTransactionID(bool pForObu)
        {
            DateTime now = DateTime.Now;
            string TransID = string.Empty;
            //20090801080642
            //year:month:day:hour:minute:second:milisecond
            //string TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss:fff}", now);
            if (pForObu)
            {
                TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss:fff}", now);
            }
            else
            {
                TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss}", now);
            }

            TransID = TransID.Replace(":", "");
            return TransID;
        }

        /// <summary>
        /// Tao chuoi datestring cho moi luot soat ve
        /// </summary>
        /// <param name="pForObu">De biet transid nay co phai dung cho obu hay khong</param>
        /// <returns></returns>
        public static string CreateTransactionID(DateTime dt, bool pForObu)
        {
            // DateTime now = DateTime.Now;
            string TransID = string.Empty;
            //20090801080642
            //year:month:day:hour:minute:second:milisecond
            //string TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss:fff}", now);
            if (pForObu)
            {
                TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss:fff}", dt);
            }
            else
            {
                TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss}", dt);
            }
            // TransID = String.Format("{0:yyyy:MM:dd:HH:mm:ss}", dt);


            TransID = TransID.Replace(":", "");
            return TransID;
        }

        /// <summary>
        /// Dùng để move thư mục, du`ng cho cac thu muc cu`ng o dia
        /// </summary>
        /// <param name="pSourceFolder"></param>
        /// <param name="pDestinationFolder"></param>
        public static void MoveFolder(string pSourceFolder, string pDestinationFolder)
        {
            try
            {
                System.IO.Directory.Move(pSourceFolder, pDestinationFolder);
            }
            catch (Exception ex)
            {
                string function = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString()
                                  + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                TKUtils.WriteLogFile(function, ex.Message);
            }
        }

        public static void ShowError(string pMessage)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSource">Noi phat sinh loi</param>
        /// <param name="pMessage">Noi dung loi</param>
        public static void ShowError(string pSource, string pMessage)
        {
            if (pSource != null && pMessage != null)
            {
                MessageBox.Show(pMessage, pSource, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ShowError(Exception ex)
        {
            string msg = string.Format("{0}.{1}(): {2}.", ex.TargetSite.DeclaringType.FullName, ex.TargetSite.Name,
                ex.Message);
           
        }

        /// <summary>
        /// Chuan hoa servername
        /// Ex: NormalizeServerName(VODINHHUY\SQL2008)=>VODINHHUY
        /// </summary>
        /// <param name="pServerName"></param>
        /// <returns></returns>
        public static string NormalizeServerName(string pServerName)
        {
            try
            {
                string server = string.Empty;
                if (string.IsNullOrEmpty(pServerName))
                {
                    return string.Empty;
                }
                else
                {
                    string[] tempArr = pServerName.Split('\\');
                    if (tempArr.Length > 0) return tempArr[0];
                    else
                        return pServerName;
                }
            }
            catch (Exception ex)
            {
                string function = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString()
                                  + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                TKUtils.WriteLogFile(function, ex.Message);
            }
            return string.Empty;
        }

        

        public static void WriteBatFile(string strFuncName)
        {
            string filename = string.Format(BAT_FILE);
            System.IO.File.Delete(filename);
            System.IO.StreamWriter sw = System.IO.File.AppendText(filename);
            try
            {
                sw.Write("net time \\");
                sw.Write("\\" + strFuncName + " /set /y");
            }
            finally
            {
                sw.Close();
            }
        }

        public static string FilterVehiclePlate(string plate)
        {
            try
            {
                string temp = plate.Substring(2, 1);
                if (temp == "6")
                {
                    plate = plate.Remove(2, 1);
                    plate = plate.Insert(2, "C");
                }
                else if (temp == "8")
                {
                    plate = plate.Remove(2, 1);
                    plate = plate.Insert(2, "B");
                }
                else if (temp == "0")
                {
                    plate = plate.Remove(2, 1);
                    plate = plate.Insert(2, "D");
                }
                else if (temp == "4")
                {
                    plate = plate.Remove(2, 1);
                    plate = plate.Insert(2, "A");
                }
                if (plate.Contains("Z"))
                {
                    plate = plate.Replace("Z", "2");
                }
                return plate;
            }
            catch (Exception ex)
            {
                string function = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString()
                                  + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                TKUtils.WriteLogFile(function, ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Ma hoa so xe
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static string EncodePlateNumber(string strSource)
        {
            try
            {
                string M = string.Empty;
                int i, k;
                if (string.IsNullOrEmpty(strSource)) return string.Empty;
                strSource = strSource.ToUpper();
                strSource = strSource.Replace(" ", "");
                for (i = 0; i < strSource.Length; i++)
                {
                    k = strSource.Substring(i, 1)[0] + (2 * (i + 1));
                    M = M + System.Convert.ToChar(k);
                }
                return M;
            }
            catch (Exception ex)
            {
                string function = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString()
                                  + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                TKUtils.WriteLogFile(function, ex.Message);
            }
            return string.Empty;
        }


        /// <summary>
        /// Neu so xe nd giong so xe pRegisPlate 
        /// </summary>
        /// <param name="pRegisPlate"></param>
        /// <param name="pRecogPlate"></param>
        /// <param name="pNSame"></param>
        /// <returns></returns>
        public static string RefinePlate(string pRegisPlate, string pRecogPlate, int pNSame)
        {
            string res = pRecogPlate;
            try
            {
                if (string.IsNullOrEmpty(pRegisPlate) || string.IsNullOrEmpty(pRecogPlate))
                    return pRecogPlate;
                if (pRecogPlate.Length < 4) return pRecogPlate;
                /*
                 */
            }
            catch (Exception ex)
            {
                string function = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString()
                                  + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                TKUtils.WriteLogFile(function, ex.Message);
            }
            return res;
        }

        /// <summary>
        /// remove cac ki tu thuy dien, sech
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        

        public static string ComparePlateGiaLai2(string pRecogPlate, string pRegisPlate, int nSameChar = 2)
        {
            string OptimizedPlate = pRecogPlate;
            try
            {
                /*kiem tra neu so ki tu cua recogplate >4 va so ki tu giong >=nsameChar thi coi nhu trung
               */
                int MIN_NUM_SAM_CHAR = 2; //so ki tu giong nhau toi thieu=2
                if (nSameChar > 1 && nSameChar < 10)
                {
                    //1->4
                    MIN_NUM_SAM_CHAR = nSameChar;
                }
                if (string.IsNullOrEmpty(pRecogPlate) || string.IsNullOrEmpty(pRegisPlate) || pRecogPlate.Length < 4)
                {
                    //kiem tra neu so ki tu nhan dnag = empty hoac qua it => ko xu ly

                    return OptimizedPlate;
                }
                //so sanh so ki tu trung
                int len = pRecogPlate.Length;
                if (pRecogPlate.Length > pRegisPlate.Length)
                {
                    //tinh chieu dai de so sanh
                    len = pRegisPlate.Length;
                }
                int DuplicateCount = 0;
                // So sanh lech 1 chu so bat ky trong bien so (V/d: 30N3456 vs 30N3x56)
                for (int i = 0; i < pRecogPlate.Length; i++)
                {
                    char tems = pRecogPlate[i];
                    for (int j = 0; j < pRegisPlate.Length; j++)
                    {
                        if (tems == pRegisPlate[j])
                        {
                            //neu bang nhau-> tang bien dem, break
                            DuplicateCount++;
                            break; //
                        }
                    }
                }
                if (DuplicateCount >= MIN_NUM_SAM_CHAR)
                {
                    OptimizedPlate = pRegisPlate;
                }
              
            }
            catch (Exception ex)
            {
                string function = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString()
                                  + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
                TKUtils.WriteLogFile(function, ex.Message);
            }
            return OptimizedPlate;
        }

        /// <summary>
        /// So sanh bien so ND va bien so DK (lech 1 chu so)
        /// </summary>
        /// <returns>
        ///     1. Lech n chu so bat ky trong bien so
        ///     2. Thieu n chu so dau trong bien so va co the lech n so voi day so con lai
        ///     -1. Lech hoan toan
        /// </returns>
        public static string ComparePlateGiaLai(string pRecogPlate, string pRegisPlate, int nDiffChar = 2)
        {
            string OptimizedPlate = pRecogPlate;
            int retValue = -1;
            int NUM_DIFF_CHAR = 1;
            if (nDiffChar > 1 && nDiffChar < 5)
            {
                //1->4
                NUM_DIFF_CHAR = nDiffChar;
            }
            try
            {
                int numDiffChar = 0;
                // So sanh lech 1 chu so bat ky trong bien so (V/d: 30N3456 vs 30N3x56)
                if (pRecogPlate.Length == pRegisPlate.Length)
                {
                    retValue = 1;
                    for (int i = 0; i < pRegisPlate.Length; i++)
                    {
                        if (pRecogPlate[i] != pRegisPlate[i])
                            numDiffChar++; //tinh so ki tu sai khac

                        if (numDiffChar > NUM_DIFF_CHAR)
                        {
                            retValue = -1;
                            break;
                        }
                    }
                }
                // So sanh thieu n chu so
                if (retValue == -1 && pRecogPlate.Length >= pRegisPlate.Length - NUM_DIFF_CHAR)
                {
                    // Thieu n chu so bat ky trong bien so (V/d: 30N345_ vs 30N3456, 3_N3456 vs 30N3456)
                    for (int i = 0; i < pRegisPlate.Length; i++)
                    {
                        retValue = 2;
                        numDiffChar = 0;

                        string tempSoXe_DK = pRegisPlate.Remove(i, 1);
                        for (int j = 0; j < tempSoXe_DK.Length; j++)
                        {
                            if (pRecogPlate[j] != tempSoXe_DK[j])
                                numDiffChar++;

                            //   if (numDiffChar > 0)
                            if (numDiffChar > NUM_DIFF_CHAR)
                            //vdhuy update 26/08/2016 cho phep sai khac 1 so khi chieu dai bang nhau
                            {
                                retValue = -1;
                                break;
                            }
                        }

                        if (retValue == 2)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                retValue = -1;
                string msg = ex.Message;
                WriteLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString() + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
            }
            if (retValue == 1 || retValue == 2)
            {
                OptimizedPlate = pRegisPlate;
            }
            string ss =
                string.Format(
                    "ComparePlateGiaLai Regis {0} Recog {1} return value {2} NUM_DIFF_CHAR {3} OptimizedPlate {4}",
                    pRegisPlate, pRecogPlate, retValue, NUM_DIFF_CHAR, OptimizedPlate);
            return OptimizedPlate;
            // return retValue;
        }

        /// <summary>
        /// So sanh bien so ND va bien so DK (lech 1 chu so)
        /// </summary>
        /// <returns>
        ///     1. Lech n chu so bat ky trong bien so
        ///     2. Thieu n chu so dau trong bien so va co the lech n so voi day so con lai
        ///     -1. Lech hoan toan
        /// </returns>
        public static int ComparePlate(string pRecogPlate, string pRegisPlate)
        {
            int retValue = -1;
            int NUM_DIFF_CHAR = 1;
            try
            {
                if (string.IsNullOrEmpty(pRecogPlate) || string.IsNullOrEmpty(pRegisPlate))
                {
                    //20sep2016
                    return -1;
                }
                int numDiffChar = 0;
                // So sanh lech 1 chu so bat ky trong bien so (V/d: 30N3456 vs 30N3x56)
                if (pRecogPlate.Length == pRegisPlate.Length)
                {
                    retValue = 1;
                    for (int i = 0; i < pRegisPlate.Length; i++)
                    {
                        if (pRecogPlate[i] != pRegisPlate[i])
                            numDiffChar++; //tinh so ki tu sai khac

                        if (numDiffChar > NUM_DIFF_CHAR)
                        {
                            retValue = -1;
                            break;
                        }
                    }
                }
                // So sanh thieu n chu so
                if (retValue == -1 && pRecogPlate.Length == pRegisPlate.Length - NUM_DIFF_CHAR)
                {
                    // Thieu n chu so bat ky trong bien so (V/d: 30N345_ vs 30N3456, 3_N3456 vs 30N3456)
                    for (int i = 0; i < pRegisPlate.Length; i++)
                    {
                        retValue = 2;
                        numDiffChar = 0;

                        string tempSoXe_DK = pRegisPlate.Remove(i, 1);
                        for (int j = 0; j < tempSoXe_DK.Length; j++)
                        {
                            if (pRecogPlate[j] != tempSoXe_DK[j])
                                numDiffChar++;

                            //   if (numDiffChar > 0)
                            if (numDiffChar > NUM_DIFF_CHAR)
                            //vdhuy update 26/08/2016 cho phep sai khac 1 so khi chieu dai bang nhau
                            {
                                retValue = -1;
                                break;
                            }
                        }

                        if (retValue == 2)
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                retValue = -1;
                string msg = ex.Message;
                WriteLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString() + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
            }

            return retValue;
        }

        public static bool ComparePlate(string pRecogPlate, string pRegisPlate, int samechars = 2)
        {
            //int NUM_DIFF_CHAR = numDiff;
            try
            {
                if (string.IsNullOrEmpty(pRecogPlate) || string.IsNullOrEmpty(pRegisPlate))
                {
                    //20sep2016
                    return false;
                }

                int DuplicateCount = 0;
                // So sanh lech 1 chu so bat ky trong bien so (V/d: 30N3456 vs 30N3x56)
                for (int i = 0; i < pRecogPlate.Length; i++)
                {
                    char tems = pRecogPlate[i];
                    for (int j = 0; j < pRegisPlate.Length; j++)
                    {
                        if (tems == pRegisPlate[j])
                        {
                            //neu bang nhau-> tang bien dem, break
                            DuplicateCount++;
                            break; //
                        }
                    }
                }

                if (DuplicateCount >= samechars)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                WriteLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString() + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name, msg);
            }

            return false;
        }

        public static string RefinePlateNumber(string _RegisPlate)
        {
            var RefinedRegisPlate = _RegisPlate.Trim('\0'); //remove null
            //neu ky tu dau cua _regisplate la so va ky tu cuoi la T or X thi remove ky tu cuoi
            int len = RefinedRegisPlate.Length;
            if (len > 5)
            {
                int t = len - 1; //vi tri ki tu cuoi
                string first = RefinedRegisPlate[0].ToString();
                string end = RefinedRegisPlate[t].ToString().ToUpper(); //lay ki tu cuoi , upper
                if (int.TryParse(first, out t) && (end == "T" || end == "X"))
                //neu ki tu dau la so va ky tu cuoi la T or X
                {
                    //neu ky tu dau la so-> kiem tra ky tu cuoi la T or X
                    RefinedRegisPlate = RefinedRegisPlate.Remove(len - 1); //remove ky tu cuoi
                }
            }
            return RefinedRegisPlate;
        }

        public static string Add_T_RegisPlate(string pRegisPlate)
        {
            //Refine so xe dang ky
            var temp = pRegisPlate.Substring(pRegisPlate.Length - 5, 5); //Cat 5 ky tu cuoi
            //Get first char
            char firstChar = char.Parse(temp.Substring(0, 1));
            bool checkNum = Char.IsDigit(firstChar);
            if (checkNum)
                pRegisPlate = pRegisPlate + "T";
            return pRegisPlate;
        }

        #region New ComparePlate

        /// <summary>
        /// Correct recognation plate number
        /// </summary>
        /// <param name="regisPlate"></param>
        /// <param name="recogPlate"></param>
        /// <returns></returns>
       
        #endregion

        public static DateTime ConvertDateString(string pDateString)
        {
            //Lấy thời gian hiện tại làm thời gian soát
            //20090801080642
            DateTime now = DateTime.Now;
            try
            {
                if (pDateString.Length < 14) return now;
                int year = Convert.ToInt32(pDateString.Substring(0, 4));
                int month = Convert.ToInt32(pDateString.Substring(4, 2));
                int day = Convert.ToInt32(pDateString.Substring(6, 2));
                int hour = Convert.ToInt32(pDateString.Substring(8, 2));
                int minute = Convert.ToInt32(pDateString.Substring(10, 2));
                int second = Convert.ToInt32(pDateString.Substring(12, 2));
                now = new DateTime(year, month, day, hour, minute, second);
                //CheckTime = hour * 3600 + minute * 60 + second;
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString() + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return now;
        }

     

        internal static void ComparePlateSamPoistion(string pPlate, string p1, int p2)
        {

        }

        /// <summary>
        /// Hàm đọc file log cắt từng row đưa vào list để xử lý
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> ParseData_FromFile(string filePath)
        {
            List<string> lstResult = new List<string>();
            try
            {
                // Read a text file line by line.  
                lstResult = File.ReadAllLines(filePath).ToList();

            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString() + "." +
                   System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return lstResult;
        }

        /// <summary>
        /// Input là List<string> từ function ParseData_FromFile
        /// </summary>
        /// <returns></returns>
        public static List<LogFileModel> ParseToObject(List<string> lstString)
        {
            List<LogFileModel> lstResult = new List<LogFileModel>();
            try
            {
                
                foreach (var item in lstString)
                {
                    if (string.IsNullOrWhiteSpace(item))
                        continue;
                    LogFileModel logfile = new LogFileModel();
                    var time  = item.Substring(0, 23); 

                    logfile.TimeLog = DateTime.Parse(time);
                    var method = item.Remove(0, 27);
                    var methods = method.Replace("[", "").Replace("]", "").Split('-');
                    if (methods != null && methods.Count() > 1)
                    {
                        //xóa 1 khoảng trắng đầu tiên
                        logfile.MethodName = methods[0].Remove(1,1);
                        logfile.Content = methods[1].Remove(1, 1);
                    }

                    lstResult.Add(logfile);


                }
            }
            catch (Exception ex)
            {
                TKUtils.WriteLogFile(
                  System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString() + "." +
                  System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            return lstResult;
        }

    }
}
