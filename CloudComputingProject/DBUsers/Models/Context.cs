using System;
using System.Collections.Generic;
using System.IO;
namespace DBUsers.Models
{
    public class Context
    {
        public List<User> Users { get; set; }

        private string path;
        public Context()
        {
            path = Directory.GetCurrentDirectory() + "/DataContext/UserDB.csv";
            Users = new List<User>();
            using (var fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader rd = new StreamReader(fs))
                {
                    string line;
                    while ((line = rd.ReadLine()) != null)
                    {
                        User user = new User();
                        user.UserName=line;
                        Users.Add(user);
                    }
                }
            }
        }
        public bool AddUser(User user){
            try
            {
                 File.AppendAllText(path,user.UserName+Environment.NewLine);
                 return true;
            }
            catch (System.Exception)
            {   
               return false;
            }
            
        }
    }
}