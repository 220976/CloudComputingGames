using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using DBUsers.Models;

namespace DBUsers.Controllers
{
    public class UserController : Controller
    {
        private byte[] Key = Encoding.Unicode.GetBytes("⤑垺齤翥䊬鍶㸚皳䆚伧窭ꦋ焘");
        private byte[] IV = Encoding.Unicode.GetBytes("ꅘ躲䡆ﵟᖺ辢福꤈");

        Context _context;

        public UserController()
        {
            _context = new Context();
        }

        [HttpGet]
        [Route("/User/SignIn/{UserName}")]
        public IActionResult SignIn(string UserName)
        {
            if (_context.Users.Find(u => u.UserName == UserName) != null)
            {
                return Ok("This UserName Already Exist Please Try With Another UserName");
            }
            User user = new User
            {
                UserName = UserName
            };
            if (_context.AddUser(user))
            {
                return Ok(EncryptStringToBytes_Aes(user.UserName, Key, IV));
            }
            return Ok("An Error Has Been Occure");
        }
        [HttpGet]
        [Route("/User/IsSignIn")]
        public IActionResult IsSignIn()
        {
            string UserName = Request.Headers["UserName"];
            string Token = Request.Headers["Token"];
            if (!String.IsNullOrEmpty(UserName)&&!String.IsNullOrEmpty(Token))
            {
                if (_context.Users.Find(u => u.UserName == UserName) != null)
                {
                    string tkn=Convert.ToBase64String(EncryptStringToBytes_Aes(UserName, Key, IV));
                    if (tkn == Token)
                    {
                        return Ok(true);
                    }
                }
                return Ok(false);
            }

            return BadRequest();
        }
        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

    }
}
