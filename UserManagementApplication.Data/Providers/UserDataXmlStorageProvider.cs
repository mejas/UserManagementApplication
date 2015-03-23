using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UserManagementApplication.Common.Enumerations;
using UserManagementApplication.Common.Exceptions;
using UserManagementApplication.Common.Security;
using UserManagementApplication.Data.DataEntities;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Providers
{
    public class UserDataXmlStorageProvider : IUserDataStorageProvider
    {
        #region Declarations
        protected List<User> UserCache = new List<User>();
        private int _newUserId = 0; 
        #endregion

        #region Properties
        public string XmlFile { get; private set; } 
        #endregion

        #region Constructors
        public UserDataXmlStorageProvider()
        {
            if (String.IsNullOrEmpty(XmlFile))
            {
                Configuration assemblyConfig = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);

                if (assemblyConfig.AppSettings.Settings.Count == 0)
                {
                    throw new CriticalException("Database configuration not found.");
                }

                XmlFile = assemblyConfig.AppSettings.Settings["XmlDbFile"].Value;
            }

            initializeCacheAndDb();
        }

        public UserDataXmlStorageProvider(string fileName)
        {
            XmlFile = fileName;
            initializeCacheAndDb();
        } 
        #endregion

        #region Methods
        public User GetUser(int id)
        {
            return UserCache.Find(user => user.UserId == id);
        }

        public IList<User> GetUsers(string firstName, string lastName)
        {
            firstName = firstName.ToLower();
            lastName = lastName.ToLower();

            return UserCache.FindAll(user =>
                {
                    var userFirstName = user.FirstName.ToLower();
                    var userLastName  = user.LastName.ToLower();

                    return  String.IsNullOrEmpty(firstName) && userLastName == lastName ||
                            String.IsNullOrEmpty(lastName) && userFirstName == firstName ||
                            userFirstName == firstName && userLastName == lastName;
                });
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

            userToUpdate.Username = user.Username;
            userToUpdate.Password = user.Password;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Birthdate = user.Birthdate;
            userToUpdate.RoleType = user.RoleType;
            userToUpdate.BadLogins = user.BadLogins;

            InvalidateCache();

            return user;
        }

        public User GetUser(string username)
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
        #endregion

        #region Functions
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

            var lastAddedUser = UserCache.OrderByDescending(item => item.UserId).FirstOrDefault();

            _newUserId = lastAddedUser != null ? lastAddedUser.UserId + 1 : 0;
        }

        private void initializeCacheAndDb()
        {
            FileInfo fileInfo = new FileInfo(XmlFile);

            if (!fileInfo.Exists)
            {
                User user = new User(this, new DefaultDataSecurityProvider());
                user.Create("administrator", new HashGenerator().GenerateHash("$ystemM@stER"),
                            "Administrator", "Administrator", DateTime.Now, RoleType.Admin);
            }
            else if (fileInfo.Length > 1)
            {
                loadCache();
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
        
        #endregion
    }
}
