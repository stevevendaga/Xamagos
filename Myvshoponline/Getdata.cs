using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Threading.Tasks;

namespace Myvshoponline
{
    public class Getdata
    {
        MyvshoponlineEntities db = new MyvshoponlineEntities();
        public void Insert_Email_NewsLetter(string email, DateTime date)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "Insert into NewsletterEmail(Email,DateCreated)Values(@email,@date)";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@email", email);
            comm.Parameters.AddWithValue("@date", date);
            comm.ExecuteScalar();
            conn.Close();
        }

        public string Get_RandomNumber()
        {
            string strval = "";
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            int size = 550;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                strval = Convert.ToString(builder.Append(ch));
            }
            return strval;
        }

        public int Generate_OTP_Numeric()
        {
            Random random = new Random();
            int num = random.Next(99, 999999);
            return num;
        }
        public string Generate_OTP_Char()
        {
            string strval = "";
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            int size = 6;
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                strval = Convert.ToString(builder.Append(ch));
            }
            return strval;
        }

        //this function Convert to Encord your Password
        //public static string EncodePasswordToBase64(string password)
        public string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
               return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        } //this function Convert to Decode your Password
    /// <summary>
    /// Decode pasword that has been encrypted. <br/>This method is generic and it 
    /// can be used dynamically.
    /// <br/>[Developed by Mvendaga Ukor]
    /// </summary>
    /// <param name="encodedData"></param>
    /// <returns></returns>
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
       

       public bool Is_SupperAdmin(string username,string userrole)
        {
            if (!string.IsNullOrEmpty(username) && userrole == "Super Admin" || userrole == "Site Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    public bool Is_Password_Verified(string hashPw, string password)
    {
      var verified = Crypto.VerifyHashedPassword(hashPw, password);
      if (verified)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

        public bool Is_ShopAdmin(string username, string userrole)
        {
            if (!string.IsNullOrEmpty(username) && userrole == "Shop Admin")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    /// <summary>
    /// Enable shop online order
    /// </summary>
    /// <param name="shopid"></param>

        public void EnableOnlineOrder(int shopid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update shop set enableonlineorder='Yes' where id=@shopid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@shopid", shopid);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void DisableOnlineOrder(int shopid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update shop set enableonlineorder='No' where id=@shopid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@shopid", shopid);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void EnableOnlinePayment(int shopid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update shop set enableonlinepayment='Yes' where id=@shopid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@shopid", shopid);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void DisableOnlinePayment(int shopid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update shop set enableonlinepayment='No' where id=@shopid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@shopid", shopid);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_ShopStatus_to_Inactive_on_Expiry(int userid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update shop set ShopStatus='Inactive' where userid=@userid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@userid", userid);
            comm.ExecuteScalar();
            conn.Close();
        }
         
        public void Update_ShopStatus_to_Active_on_Renewal(int userid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update shop set ShopStatus='Active' where userid=@userid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@userid", userid);
            comm.ExecuteScalar();
            conn.Close();
        }


        public void Loop_through_user_to_update_planexpiry(int UserID)
        {
            MyvshoponlineEntities db = new MyvshoponlineEntities();
            Getdata mydata = new Getdata();
            int lYear = (int)db.Users.Where(k => k.ID == UserID).Select(k => k.LaunchYear).FirstOrDefault();
            int lmonth = (int)db.Users.Where(k => k.ID == UserID).Select(k => k.LaunchMonth).FirstOrDefault();
            int lday = (int)db.Users.Where(k => k.ID == UserID).Select(k => k.LaunchDay).FirstOrDefault();

            int pYear = (int)db.Users.Where(k => k.ID == UserID).Select(k => k.PreviousYear).FirstOrDefault();
            int pmonth = (int)db.Users.Where(k => k.ID == UserID).Select(k => k.PreviousMonth).FirstOrDefault();
            int pday = (int)db.Users.Where(k => k.ID == UserID).Select(k => k.PreviousDay).FirstOrDefault();

            DateTime lauchd = new DateTime(lYear, lmonth, lday);
            DateTime prevd = new DateTime(pYear, pmonth, pday);
            TimeSpan diff = lauchd.Subtract(prevd);
            int diff1 = diff.Days;
            int FixedDays =(int) db.Users.Find(UserID).FixedDays;
            int Ndays =(int) db.Users.Find(UserID).NDays;

            if (diff1 >FixedDays)
            {
                //return to free plan
                var user = db.Set<User>().Find(UserID);
                user.PlanID = db.PricingPlans.Where(s => s.PlanName == "FREE PLAN").Select(s => s.ID).FirstOrDefault();
                user.BillingCycleID =db.BillingCycles.Where(s => s.Cycle == "Free").Select(s => s.ID).FirstOrDefault();
                db.SaveChanges();
                
                if (db.Shops.Where(s => s.UserID == UserID).Count() > 0)
                {
                    Update_ShopStatus_to_Inactive_on_Expiry(UserID);
                }
            }
        }


        public void ResizePicture(string efilepath)
        {
            Image image = Image.FromFile(efilepath);
            float aspectRatio = (float)image.Size.Width / (float)image.Size.Height;
            int newHeight = 250;
            int newWidth = Convert.ToInt32(aspectRatio * newHeight);
            Bitmap thumbBitmap = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(thumbBitmap);
            thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
            thumbGraph.DrawImage(image, imageRectangle);
            image.Dispose();
            thumbBitmap.Save(efilepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            thumbGraph.Dispose();
            thumbBitmap.Dispose();
            image.Dispose();
        }

        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public void SendMail(string to, string subject, string msg)
        {
            //string from = "support@xamagos.com";
            MailMessage mail = new MailMessage();
            //Mail Server IP 172.106.164.2 or mail.marketsquare247.com
            //SmtpClient SmtpServer = new SmtpClient("mail.xamagos.com");
            SmtpClient SmtpServer = new SmtpClient("mail5019.site4now.net");
            mail.From = new MailAddress("info@xamagos.com", "XAMAGOS");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = msg;
            mail.IsBodyHtml = true;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            // 8889
            //SmtpServer.Port = 8889;
            //for ssl
            SmtpServer.Port = 587;
            //for ssl
            //working one SmtpServer.Port = 587;
            //SmtpServer.Port = 587;
            SmtpServer.Credentials = new NetworkCredential("info@xamagos.com", "Xamagosinfo@2023");
            SmtpServer.Send(mail);

        }

    
    public void UpdateUserBonusStatus(int userid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update userbonus set status=0 where userid=@userid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@userid", userid);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Send_SMS(string strPhoneNumber, string strMessage, string strSender, DateTime datetime)
        {
            string url = "https://www.80kobosms.com/tools/geturl/Sms.php?username=ukormvendagas@yahoo.com&password=Pascal@1992&sender=" + strSender + " &message=" + strMessage + "&flash=1&sendtime=" + datetime + "&recipients=" + strPhoneNumber + "&forcednd=1";
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(string.Format(url));
            webReq.Method = "GET";
            HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();
            Stream answer = webResponse.GetResponseStream();
            StreamReader _recivedAnswer = new StreamReader(answer);
        }

        //public void Send_SMS_80Kobo(string strPhoneNumber, string strMessage, string strSender, DateTime datetime)
        //{
        //    // Initialize variables ( set your variables here )
        //    string username = "ukormvendagas@yahoo.com";
        //    string password = "Pascal@1992";
        //    // Separate multiple numbers by comma
        //    // Set your domain's API URL
        //    string url = "https://api.80kobosms.com/v2/app/sms";
        //    var request = (HttpWebRequest)WebRequest.Create(url);
        //    var postData = "username=" + Uri.EscapeDataString(username);
        //    postData += "&password=" + Uri.EscapeDataString(password);
        //    postData += "&message=" + Uri.EscapeDataString(strMessage);
        //    postData += "&sender=" + Uri.EscapeDataString(strSender);
        //    postData += "&recipients=" + Uri.EscapeDataString(strPhoneNumber);
        //    postData += "&flash=" + Uri.EscapeDataString("1");
        //    postData += "&forcednd=" + Uri.EscapeDataString("1");
        //    var data = Encoding.ASCII.GetBytes(postData);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = data.Length;

        //    using (var stream = request.GetRequestStream())
        //    {
        //        stream.Write(data, 0, data.Length);
        //    }

        //    var response = (HttpWebResponse)request.GetResponse();
        //}

        public void Send_SMS_KudiSMS(string strPhoneNumber, string strMessage, string strSender, DateTime datetime)
        {
            // Initialize variables ( set your variables here )
            string username = "ukormvendagas@gmail.com";
            string password = "Pascal@1992";
            // Separate multiple numbers by comma
            // Set your domain's API URL
            string url = "https://account.kudisms.net/api/";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var postData = "username=" + Uri.EscapeDataString(username);
            postData += "&password=" + Uri.EscapeDataString(password);
            // postData += "&message=" + Uri.EscapeDataString(strMessage);
            postData += "&message=" + Uri.EscapeDataString(strMessage);
            postData += "&sender=" + Uri.EscapeDataString(strSender);
            postData += "&mobiles=" + (strPhoneNumber);
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
        }

        public void Send_SMS_WebClient(string strPhoneNumber, string strMessage, string strSender, DateTime datetime)
        {
            var wb = new WebClient();
            var data = new NameValueCollection();
            string url = "https://account.kudisms.net/api/";
            data["username"] = "ukormvendagas@gmail.com";
            data["password"] = "Pascal@1992";
            data["message"] = strMessage;
            data["sender"] = strSender;
            data["mobiles"] = strPhoneNumber;
            var response = wb.UploadValues(url, "POST", data);
        }

        public void ResetNGMailStatustoZero()
{
var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
SqlConnection conn = new SqlConnection(constring);
conn.Open();
string oString = "update MailingListNG set status=0";
SqlCommand comm = new SqlCommand(oString, conn);
comm.ExecuteScalar();
conn.Close();
}

        public void ResetNonNGMailStatustoZero()
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update MailingListNonNG set status=0";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.ExecuteScalar();
            conn.Close();
        }


        public void Loop_through_popularstores_to_update_expiry(int ShopID)
        {
            MyvshoponlineEntities db = new MyvshoponlineEntities();
            Getdata mydata = new Getdata();
            int lYear = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status=="Active").Select(k => k.LaunchYear).FirstOrDefault();
            int lmonth = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.LaunchMonth).FirstOrDefault();
            int lday = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.LaunchDay).FirstOrDefault();

            int pYear = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.PreviousYear).FirstOrDefault();
            int pmonth = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.PreviousMonth).FirstOrDefault();
            int pday = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.PreviousDay).FirstOrDefault();

            DateTime lauchd = new DateTime(lYear, lmonth, lday);
            DateTime prevd = new DateTime(pYear, pmonth, pday);
            TimeSpan diff = lauchd.Subtract(prevd);
            int diff1 = diff.Days;
            int TotalDays = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.TotalDaysPaid).FirstOrDefault();
            int daysActive = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.DaysActive).FirstOrDefault();
            int PSID = (int)db.PopularStores.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active").Select(k => k.ID).FirstOrDefault();
            if (diff1 > TotalDays)
            {
                int StatusID = db.PopularStoreStatus.Where(k => k.Status == "Expired").Select(k => k.ID).FirstOrDefault();
                Update_Popularstores_to_Inactive_on_Expiry(PSID, StatusID);

                
                var r = db.Set<PopularStore>().Find(PSID);
                r.DaysActive = diff1;
                r.LaunchYear = DateTime.Now.Year;
                r.LaunchMonth = DateTime.Now.Month;
                r.LaunchDay = DateTime.Now.Day;
                db.SaveChanges();
            }
            else
            {
                var r = db.Set<PopularStore>().Find(PSID);
                r.DaysActive = diff1;
                r.LaunchYear = DateTime.Now.Year;
                r.LaunchMonth = DateTime.Now.Month;
                r.LaunchDay = DateTime.Now.Day;
                db.SaveChanges();
            }
        }

        public void Update_Popularstores_to_Inactive_on_Expiry(int PSID, int StatusID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update popularstore set PopularStoreStatusID=@StatusID where id=@PSID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@PSID", PSID);
            comm.Parameters.AddWithValue("@StatusID", StatusID);
            comm.ExecuteScalar();
            conn.Close();
        }



        public void Loop_through_searchoptimizations_to_update_expiry()
        {
            MyvshoponlineEntities db = new MyvshoponlineEntities();
            Getdata mydata = new Getdata();
            if (db.SearchOptimizations.Count() > 0)
            {
            foreach (var item in db.SearchOptimizations.ToList())
                {
                    Loop_through_eachShop(Convert.ToInt32(item.ShopID));
                }
            }
        }

        public void Loop_through_eachShop(int ShopID)
        {
            foreach (var item in db.SearchOptimizations.Where(s => s.ShopID == ShopID && s.PopularStoreStatu.Status == "Active").ToList())
            {
                Loop_update_SEO(ShopID, Convert.ToInt32(item.ProductID));
            }
        }

        public void Loop_update_SEO(int ShopID,int ProductID)
        {
            int lYear = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID==ProductID).Select(k => k.LaunchYear).FirstOrDefault();
            int lmonth = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.LaunchMonth).FirstOrDefault();
            int lday = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.LaunchDay).FirstOrDefault();

            int pYear = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.PreviousYear).FirstOrDefault();
            int pmonth = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.PreviousMonth).FirstOrDefault();
            int pday = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.PreviousDay).FirstOrDefault();

            DateTime lauchd = new DateTime(lYear, lmonth, lday);
            DateTime prevd = new DateTime(pYear, pmonth, pday);
            TimeSpan diff = lauchd.Subtract(prevd);
            int diff1 = diff.Days;
            int TotalDays = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.TotalDaysPaid).FirstOrDefault();
            int daysActive = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.DaysActive).FirstOrDefault();
            int PID = (int)db.SearchOptimizations.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.ID).FirstOrDefault();
            if (diff1 > TotalDays)
            {
                int StatusID = db.PopularStoreStatus.Where(k => k.Status == "Expired").Select(k => k.ID).FirstOrDefault();
                Update_SEO_to_Inactive_on_Expiry(PID, StatusID);

                var r = db.Set<SearchOptimization>().Find(PID);
                r.DaysActive = diff1;
                r.LaunchYear = DateTime.Now.Year;
                r.LaunchMonth = DateTime.Now.Month;
                r.LaunchDay = DateTime.Now.Day;
                db.SaveChanges();
            }
            else
            {
                var r = db.Set<SearchOptimization>().Find(PID);
                r.DaysActive = diff1;
                r.LaunchYear = DateTime.Now.Year;
                r.LaunchMonth = DateTime.Now.Month;
                r.LaunchDay = DateTime.Now.Day;
                db.SaveChanges();
            }
        }

        public void Update_SEO_to_Inactive_on_Expiry(int PSID, int StatusID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update SearchOptimization set PopularProductStatusID=@StatusID where id=@PSID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@PSID", PSID);
            comm.Parameters.AddWithValue("@StatusID", StatusID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_Payment_Promote_SEO_Sql(string refno, decimal amount, int ShopID, int noofdays,string enddate,int StatusID)
        {
            DateTime datepaid = DateTime.Now;
            int launchyear= DateTime.Now.Year;
           int launchmonth= DateTime.Now.Month;
           int launchday= DateTime.Now.Day;
            int previousyear= DateTime.Now.Year;
            int previousmonth=  DateTime.Now.Month;
            int previousday=  DateTime.Now.Day;
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update  SearchOptimization set  paymentstatus=1,RefNo=@refno,Amount=@amount,TotalDaysPaid=@noofdays,datepaid=@datepaid,PopularProductStatusID=@StatusID,launchyear=@launchyear,launchmonth=@launchmonth,launchday=@launchday,previousyear=@previousyear,previousmonth=@previousmonth,previousday=@previousday,enddate=@enddate,daysactive=0 where paymentstatus=0 AND shopid=@shopid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@refno", refno);
            comm.Parameters.AddWithValue("@amount", amount);
            comm.Parameters.AddWithValue("@shopid", ShopID);
            comm.Parameters.AddWithValue("@noofdays", noofdays);
            comm.Parameters.AddWithValue("@datepaid", datepaid);
            comm.Parameters.AddWithValue("@StatusID", StatusID);
            comm.Parameters.AddWithValue("@enddate", enddate);
            comm.Parameters.AddWithValue("@launchyear",launchyear);
            comm.Parameters.AddWithValue("@launchmonth", launchmonth);
            comm.Parameters.AddWithValue("@launchday", launchday);
            comm.Parameters.AddWithValue("@previousyear", previousyear);
            comm.Parameters.AddWithValue("@previousmonth", previousmonth);
            comm.Parameters.AddWithValue("@previousday", previousday);
            comm.ExecuteScalar();
            conn.Close();
        }


        public void Update_Payment_Promote_PopularProduct_Sql(string refno, decimal amount, int ShopID, int noofdays, string enddate, int StatusID)
        {
            DateTime datepaid = DateTime.Now;
            int launchyear = DateTime.Now.Year;
            int launchmonth = DateTime.Now.Month;
            int launchday = DateTime.Now.Day;
            int previousyear = DateTime.Now.Year;
            int previousmonth = DateTime.Now.Month;
            int previousday = DateTime.Now.Day;
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update  PopularProduct set  paymentstatus=1,RefNo=@refno,Amount=@amount,TotalDaysPaid=@noofdays,datepaid=@datepaid,PopularProductStatusID=@StatusID,launchyear=@launchyear,launchmonth=@launchmonth,launchday=@launchday,previousyear=@previousyear,previousmonth=@previousmonth,previousday=@previousday,enddate=@enddate,daysactive=0 where paymentstatus=0 AND shopid=@shopid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@refno", refno);
            comm.Parameters.AddWithValue("@amount", amount);
            comm.Parameters.AddWithValue("@shopid", ShopID);
            comm.Parameters.AddWithValue("@noofdays", noofdays);
            comm.Parameters.AddWithValue("@datepaid", datepaid);
            comm.Parameters.AddWithValue("@StatusID", StatusID);
            comm.Parameters.AddWithValue("@enddate", enddate);
            comm.Parameters.AddWithValue("@launchyear", launchyear);
            comm.Parameters.AddWithValue("@launchmonth", launchmonth);
            comm.Parameters.AddWithValue("@launchday", launchday);
            comm.Parameters.AddWithValue("@previousyear", previousyear);
            comm.Parameters.AddWithValue("@previousmonth", previousmonth);
            comm.Parameters.AddWithValue("@previousday", previousday);
            comm.ExecuteScalar();
            conn.Close();
        }


        public void Loop_through_PopularProducts_to_update_expiry()
        {
            MyvshoponlineEntities db = new MyvshoponlineEntities();
            Getdata mydata = new Getdata();
            if (db.SearchOptimizations.Count() > 0)
            {
                foreach (var item in db.SearchOptimizations.ToList())
                {
                    Loop_through_eachShop_PopularProducts(Convert.ToInt32(item.ShopID));
                }
            }
        }

        public void Loop_through_eachShop_PopularProducts(int ShopID)
        {
            foreach (var item in db.PopularProducts.Where(s => s.ShopID == ShopID && s.PopularStoreStatu.Status == "Active").ToList())
            {
                Loop_update_PopularProducts(ShopID, Convert.ToInt32(item.ProductID));
            }
        }

        public void Loop_update_PopularProducts(int ShopID, int ProductID)
        {
            int lYear = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.LaunchYear).FirstOrDefault();
            int lmonth = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.LaunchMonth).FirstOrDefault();
            int lday = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.LaunchDay).FirstOrDefault();

            int pYear = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.PreviousYear).FirstOrDefault();
            int pmonth = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.PreviousMonth).FirstOrDefault();
            int pday = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.PreviousDay).FirstOrDefault();

            DateTime lauchd = new DateTime(lYear, lmonth, lday);
            DateTime prevd = new DateTime(pYear, pmonth, pday);
            TimeSpan diff = lauchd.Subtract(prevd);
            int diff1 = diff.Days;
            int TotalDays = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.TotalDaysPaid).FirstOrDefault();
            int daysActive = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.DaysActive).FirstOrDefault();
            int PID = (int)db.PopularProducts.Where(k => k.ShopID == ShopID && k.PopularStoreStatu.Status == "Active" && k.ProductID == ProductID).Select(k => k.ID).FirstOrDefault();
            if (diff1 > TotalDays)
            {
                int StatusID = db.PopularStoreStatus.Where(k => k.Status == "Expired").Select(k => k.ID).FirstOrDefault();
                Update_PopularProducts_to_Inactive_on_Expiry(PID, StatusID);

                var r = db.Set<PopularProduct>().Find(PID);
                r.DaysActive = diff1;
                r.LaunchYear = DateTime.Now.Year;
                r.LaunchMonth = DateTime.Now.Month;
                r.LaunchDay = DateTime.Now.Day;
                db.SaveChanges();
            }
            else
            {
                var r = db.Set<PopularProduct>().Find(PID);
                r.DaysActive = diff1;
                r.LaunchYear = DateTime.Now.Year;
                r.LaunchMonth = DateTime.Now.Month;
                r.LaunchDay = DateTime.Now.Day;
                db.SaveChanges();
            }
        }

        public void Update_PopularProducts_to_Inactive_on_Expiry(int PSID, int StatusID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update PopularProduct set PopularProductStatusID=@StatusID where id=@PSID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@PSID", PSID);
            comm.Parameters.AddWithValue("@StatusID", StatusID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Publish_Products(int ShopID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update Product set ProductStatusID=1,Published=1 where ShopID=@ShopID and Ready_for_Publishing=1 and Published=0";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@ShopID", ShopID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_ready_for_publishing_back(int ID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update Product set Ready_for_Publishing=1 where ID=@ID and Published=0";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@ID",ID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_ready_for_publishing_After_ImageUpload(int ShopID,int ProductID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update Product set Ready_for_Publishing=1 where ShopID=@ShopID and ID=@ProductID and Published=0";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@ShopID", ShopID);
            comm.Parameters.AddWithValue("@ProductID", ProductID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Delete_Product_Visits(int productid)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "delete from ProductVisit where ProductID=@productid";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@productid", productid);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_Negotiable_Yes(int ProductID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update Product set Negotiable='Yes' where ID=@ProductID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@ProductID", ProductID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_Negotiable_No(int ProductID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update Product set Negotiable='No' where ID=@ProductID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@ProductID", ProductID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Update_Negotiation_to_No_On_Accept_Negotiation(int OrderID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update OrderNegotiation set CustomerAccept='No', SellerAccept='No' where OrderID=@OrderID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@OrderID", OrderID);
            comm.ExecuteScalar();
            conn.Close();
        }


        public void Update_Shipping_Information(int OrderID,string Address,string DeliveryNote,DateTime DateCreated)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update ShippingInformation set Address=@Address, DeliveryNotes=@DeliveryNote,DateCreated=@DateCreated where OrderID=@OrderID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@OrderID", OrderID);
            comm.Parameters.AddWithValue("@Address", Address);
            comm.Parameters.AddWithValue("@DeliveryNote", DeliveryNote);
            comm.Parameters.AddWithValue("@DateCreated", DateCreated);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Insert_Delevery_Cities(int StateID,int ShopID, DateTime date)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "Insert into DeliveryLocation(LocationID,ShopID,DateCreated)Values(@StateID,@ShopID,@date)";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@StateID", StateID);
            comm.Parameters.AddWithValue("@ShopID", ShopID);
            comm.Parameters.AddWithValue("@date", date);
            comm.ExecuteScalar();
            conn.Close();
        }
        public void Update_TempPayment_Status_to_zero(int CustomerID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "update TempPendingPayment set Status=0 where ShopCustomerID=@CustomerID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@CustomerID", CustomerID);
            comm.ExecuteScalar();
            conn.Close();
        }

        public void Delete_TempPayment_OrderGroupID(int OrderGroupID)
        {
            var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
            SqlConnection conn = new SqlConnection(constring);
            conn.Open();
            string oString = "delete from TempPendingPayment where OrderGroupID=@OrderGroupID";
            SqlCommand comm = new SqlCommand(oString, conn);
            comm.Parameters.AddWithValue("@OrderGroupID", OrderGroupID);
            comm.ExecuteScalar();
            conn.Close();
        }

    //public void Update_orderStatus_after_payment_OrderGroupID(int OrderGroupID, string refno, string PaymentChannel,DateTime DatePaid)
    //{
    //    var constring = ConfigurationManager.ConnectionStrings["MarketsquareConnection"].ToString();
    //    SqlConnection conn = new SqlConnection(constring);
    //    conn.Open();
    //    string oString = "update Order set PaymentRef=@refno, PaymentStatusID=1,PaymentChannel=@PaymentChannel,DatePaid=@DatePaid where OrderGroupID=@OrderGroupID";
    //    SqlCommand comm = new SqlCommand(oString, conn);
    //    comm.Parameters.AddWithValue("@OrderGroupID", OrderGroupID);
    //    comm.Parameters.AddWithValue("@refno", refno);
    //    comm.Parameters.AddWithValue("@PaymentChannel", PaymentChannel);
    //    comm.Parameters.AddWithValue("@DatePaid", DatePaid);
    //    comm.ExecuteScalar();
    //    conn.Close();
    //}


  }
}
