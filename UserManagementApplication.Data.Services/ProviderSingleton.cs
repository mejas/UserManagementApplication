﻿using UserManagementApplication.Data.Providers;
using UserManagementApplication.Data.Providers.Interfaces;

namespace UserManagementApplication.Data.Services
{
    public class ProviderSingleton
    {
        #region Declarations
        private static ProviderSingleton _instance = null;
        private ISessionDataStorageProvider _sessionDataStorageProvider = null;
        private IUserDataStorageProvider _storageProvider = null;
        private IDataSecurityProvider _dataSecurityProvider = null; 
        #endregion

        #region Properties
        public ISessionDataStorageProvider SessionDataStorageProvider
        {
            get
            {
                return _sessionDataStorageProvider;
            }
        }

        public IUserDataStorageProvider UserDataStorageProvider
        {
            get
            {
                return _storageProvider;
            }
        }

        public IDataSecurityProvider DataSecurityProvider
        {
            get
            {
                return _dataSecurityProvider;
            }
        } 
        #endregion

        #region Constructors
        public static ProviderSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProviderSingleton();
                }

                return _instance;
            }
        }

        private ProviderSingleton()
        {
            _sessionDataStorageProvider = new SessionDataCacheStorageProvider();
            _storageProvider = new UserDataXmlStorageProvider();
            _dataSecurityProvider = new DefaultDataSecurityProvider();
        }
        #endregion
    }
}
