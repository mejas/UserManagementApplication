using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
