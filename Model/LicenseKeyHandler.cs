using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Newtonsoft.Json.Linq;

namespace News2025
{
    internal class LicenseKeyHandler
    {
        private const string filePath = "cg_license.txt";
        private static string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string targetPath = Path.Combine(appDataPath, filePath);

        public static string readLicenseLocalFile()
        {
            if (File.Exists(targetPath))
            {
                using (StreamReader reader = new StreamReader(targetPath))
                {
                    return reader.ReadToEnd();
                }
            }

            return String.Empty;
        }

        public static void writeLicenseLocalFile(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                MessageBox.Show("Không thể lưu key rỗng!");
            }

            using (StreamWriter writer = new StreamWriter(targetPath))
            {
                writer.WriteLine(content);
            }
        }

        public static string onGetProductId()
        {
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem WHERE OSArchitecture = '64-bit'").Get().Cast<ManagementObject>().First();
            return searcher["SerialNumber"].ToString().Replace("-", "");
        }

        private static string DecryptStringAES(string encryptedValue)
        {
            var keybytes = Encoding.UTF8.GetBytes("VzggMVwlSAGverwr");
            var iv = Encoding.UTF8.GetBytes("VzggMVwlSAGverwr");

            //DECRYPT FROM CRIPTOJS
            var encrypted = Convert.FromBase64String(encryptedValue);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);

            return decriptedFromJavascript;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new AesManaged())
            {
                //Settings
                //rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        private static bool isLicenseExpiredDate(string expirationDate)
        {
            DateTimeOffset expiredDate = onGetExpirationDate(expirationDate);
            DateTimeOffset now = DateTimeOffset.Now;

            if (expiredDate < now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTimeOffset onGetExpirationDate(String expirationDate)
        {
            string timeZoneId = "SE Asia Standard Time";

            DateTime dateTime;
            if (!DateTime.TryParse(expirationDate, out dateTime))
            {
                throw new Exception("Định dạng chuỗi thời gian không hợp lệ.");
            }

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTimeOffset expiredDate = new DateTimeOffset(dateTime, timeZone.GetUtcOffset(dateTime));
            return expiredDate;
        }

        public static string onGetValueOfLicenseByKey(string licenseKey, string nameOfKey)
        {
            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                return String.Empty;
            }

            var key = DecryptStringAES(licenseKey);
            string result = (string)JObject.Parse(key)[nameOfKey];
            return result;
        }

        public static bool onCheckLicenseKeyIsValid(string licenseKey, bool isShowMessageBox)
        {
            if (string.IsNullOrWhiteSpace(licenseKey))
            {
                if (isShowMessageBox)
                {
                    MessageBox.Show("Key bản quyền không tồn tại. Xin hãy nhập key để tiếp tục!");
                }
                return false;
            }

            string productIdInKey = onGetValueOfLicenseByKey(licenseKey, "productId");
            string expirationDate = onGetValueOfLicenseByKey(licenseKey, "expirationDate");

            bool isProductIdValid = productIdInKey == onGetProductId();

            if (!isProductIdValid)
            {
                MessageBox.Show("Thông tin máy không trùng khớp!");
                return false;
            }

            if (isLicenseExpiredDate(expirationDate))
            {
                MessageBox.Show("Key bản quyền đã hết hạn, vui lòng liên hệ để được hỗ trợ!");
                return false;
            }
            // Alarm if license is about to expire (less than 10 days)
            if (isLicenseAboutToExpire(expirationDate, 10))
            {
                var expiredDate = onGetExpirationDate(expirationDate);
                MessageBox.Show($"Key bản quyền của bạn sẽ hết hạn vào ngày {expiredDate:dd/MM/yyyy}. Vui lòng gia hạn để tránh gián đoạn sử dụng!");
            }

            return true;
        }
        private static bool isLicenseAboutToExpire(string expirationDate, int daysThreshold = 10)
        {
            DateTimeOffset expiredDate = onGetExpirationDate(expirationDate);
            DateTimeOffset now = DateTimeOffset.Now;
            double daysLeft = (expiredDate - now).TotalDays;
            return daysLeft > 0 && daysLeft <= daysThreshold;
        }
    }
}
