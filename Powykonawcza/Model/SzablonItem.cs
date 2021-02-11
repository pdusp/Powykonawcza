using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Powykonawcza.Model.Szablon
{
    [Serializable]
    public class SzablonItem : INotifyPropertyChanged
    {
        private string _nazwa;
        private bool _import;
        private string _inne;
        public SzablonItem(string nazwa, bool import, string inne)
        {
            this._nazwa = nazwa;
            this._import = import;
            this._inne = inne;
        }
        public string nazwa
        {
            get
            {
                return _nazwa;
            }
            set
            {
                _nazwa = value;
                OnPropertyRaised();
            }
        }
        public bool import
        {
            get
            {
                return _import;
            }
            set
            {
                _import = value;
                OnPropertyRaised();
            }
        }
        public string inne
        {
            get
            {
                return _inne;
            }

            set
            {
                _inne = value;
                OnPropertyRaised();
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised([CallerMemberName] string propertyname=null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }


        public static SzablonItem SampleInstance { get; } = new SzablonItem("Blabla", true, "inne");

    }






}
