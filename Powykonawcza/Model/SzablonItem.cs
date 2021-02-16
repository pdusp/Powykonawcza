using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Powykonawcza.Model.Szablon
{
    [Serializable]
    public class SzablonItem : INotifyPropertyChanged
    {
        public SzablonItem(string name, bool import, string other, string type)
        {
            _name  = name;
            _import = import;
            _other   = other;
            _type   = type;
        }

        protected void SetAndNotify<T>(ref T backField,
            T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(backField, value))
                return;
            backField = value;
            var handler = PropertyChanged;
            if (handler == null) return;
            var propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
            handler(this, propertyChangedEventArgs);
        }

        private void OnPropertyRaised([CallerMemberName] string propertyname = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }


        public static SzablonItem SampleInstance { get; } = new SzablonItem("Blabla", true, "inne", "string");

        public string Name
        {
            get => _name;
            set => SetAndNotify(ref _name, value);
        }

        public bool Import
        {
            get => _import;
            set => SetAndNotify(ref _import, value);
        }

        public string Other
        {
            get => _other;
            set => SetAndNotify(ref _other, value);
        }

        public string Type
        {
            get => _type;
            set => SetAndNotify(ref _type, value);
        }

        private string _name;
        private bool _import;
        private string _other;
        private string _type;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}