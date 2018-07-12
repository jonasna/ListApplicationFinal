using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListApplicationFinal.DataServices
{
    public class ApplicationUserService : IApplicationUserService
    {
        public ApplicationUserService()
        {
            _displayName = DisplayName;
            _name = Name;
        }
        
        private string _displayName;
        public string DisplayName
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("DisplayName"))
                {
                    return (string)Application.Current.Properties["DisplayName"];
                }
                return null;
            }
            set => _displayName = value;
        }

        private string _name;
        public string Name
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("Name"))
                {
                    return (string) Application.Current.Properties["Name"];
                }

                return null;
            }
            set => _name = value;
        }

        bool IApplicationUserService.IsValid => IsValid;

        public async Task SaveUserDataAsync()
        {
            if(string.IsNullOrWhiteSpace(_displayName))
                throw new ArgumentNullException(nameof(_displayName));

            if(string.IsNullOrWhiteSpace(_name))
                throw new ArgumentNullException(nameof(_name));

            Application.Current.Properties["Name"] = _name;
            Application.Current.Properties["DisplayName"] = _displayName;

            await Application.Current.SavePropertiesAsync();
        }

        public async Task ClearAsync()
        {
            Application.Current.Properties.Clear();
            await Application.Current.SavePropertiesAsync();
        }

        public static bool IsValid => Application.Current.Properties.ContainsKey("DisplayName") &&
                                      Application.Current.Properties.ContainsKey("Name");
      
    }
}