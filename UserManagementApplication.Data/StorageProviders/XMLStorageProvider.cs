using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers;

namespace UserManagementApplication.Data.StorageProviders
{
    public class XMLStorageProvider : IStorageProvider
    {
        protected List<User> UserCache = new List<User>();
        private int _newUserId = 0;

        public string XmlFile { get; private set; }

        public XMLStorageProvider()
            : this("userData.xml") { }

        public XMLStorageProvider(string fileName)
        {
            XmlFile = fileName;

            initializeCacheAndDb();
        }

        private void initializeCacheAndDb()
        {
            FileInfo fileInfo = new FileInfo(XmlFile);
            
            if (!fileInfo.Exists)
            {
                flushUserCacheToDisk();
            }
            else if(fileInfo.Length > 1)
            {
                loadCache();
            }
        }

        public IList<User> GetUsers()
        {
            return UserCache;
        }

        public User AddUser(User user)
        {
            user.UserId = _newUserId++;

            UserCache.Add(user);

            InvalidateCache();

            return user;
        }

        public User UpdateUser(User user)
        {
            var userToUpdate = UserCache.Find(item => item.UserId == user.UserId);

            userToUpdate.Username  = user.Username;
            userToUpdate.Password  = user.Password;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName  = user.LastName;
            userToUpdate.Birthdate = user.Birthdate;
            userToUpdate.RoleType  = user.RoleType;

            InvalidateCache();

            return user;
        }

        public User GetUserByUsername(string username)
        {
            return UserCache.Find(item => item.Username == username);
        }

        public void DeleteUser(User user)
        {
            var userToRemove = UserCache.Find(item => item.UserId == user.UserId);

            UserCache.Remove(userToRemove);

            InvalidateCache();
        }

        protected void InvalidateCache()
        {
            flushUserCacheToDisk();
            loadCache();
        }

        private void loadCache()
        {
            UserCache.Clear();

            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            using (StreamReader streamReader = new StreamReader(XmlFile))
            {
                using (XmlReader reader = XmlReader.Create(streamReader))
                {
                    UserCache = serializer.Deserialize(reader) as List<User>;
                }
            }
        }

        //TODO: optimize this so we don't do a big write
        private void flushUserCacheToDisk()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
            
            using (StreamWriter streamWriter = new StreamWriter(XmlFile))
            {
                using (XmlWriter writer = XmlWriter.Create(streamWriter))
                {
                    serializer.Serialize(writer, UserCache);
                }
            }
        }

        public IList<User> GetUsers(string firstName, string lastName)
        {
            return  UserCache.FindAll(user =>
                    String.IsNullOrEmpty(firstName) && user.LastName == lastName ||
                    String.IsNullOrEmpty(lastName) && user.FirstName == firstName ||
                    user.FirstName == firstName && user.LastName == lastName);
        }
    }
}
