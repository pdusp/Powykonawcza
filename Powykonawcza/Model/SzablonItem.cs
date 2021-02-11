﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Powykonawcza.Model.Szablon
{
    [Serializable]
    public class SzablonItem : INotifyPropertyChanged
    {
        public SzablonItem(string nazwa, bool import, string inne)
        {
            _nazwa  = nazwa;
            _import = import;
            _inne   = inne;
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


        public static SzablonItem SampleInstance { get; } = new SzablonItem("Blabla", true, "inne");

        public string nazwa
        {
            get => _nazwa;
            set => SetAndNotify(ref _nazwa, value);
        }

        public bool import
        {
            get => _import;
            set => SetAndNotify(ref _import, value);
        }

        public string inne
        {
            get => _inne;
            set => SetAndNotify(ref _inne, value);
        }

        private string _nazwa;
        private bool _import;
        private string _inne;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}