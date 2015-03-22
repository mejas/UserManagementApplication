using System.Collections.Generic;

namespace UserManagementApplication.Client.Data
{
    public class ValidationResults
    {
        private Dictionary<string, string> _errorDictionary = new Dictionary<string, string>();

        public bool HasErrors
        {
            get
            {
                return ErrorDictionary.Count > 0;
            }
        }

        public Dictionary<string, string> ErrorDictionary
        {
            get
            {
                return _errorDictionary;
            }
        }

        public void AddEntry(string fieldName, string errorMessage)
        {
            ErrorDictionary[fieldName] = errorMessage;
        }

    }
}
