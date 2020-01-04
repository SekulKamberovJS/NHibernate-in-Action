using System;
using System.ComponentModel;

namespace NHibernateInAction.CaveatEmptor.Model
{
    public class FieldChangedEventArgs : EventArgs
    {
        public FieldChangedEventArgs(string propertyName)
        {
        }
    }

    public delegate void EntityChangedEventHandler(
        object sender, FieldChangedEventArgs e);

    public class User
    {
        public bool IsAdmin = true;
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnEntityChanged("Name");
            }
        }

        public event EntityChangedEventHandler EntityChanged;

        protected virtual void OnEntityChanged(string propertyName)
        {
            if (EntityChanged != null)
                EntityChanged(this, new FieldChangedEventArgs(propertyName));
        }
    }

    public class User2 : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged("Name");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public delegate void NameChangedEventHandler(
        object sender, EventArgs e);

    public class User3
    {
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;
                name = value;
                OnNameChanged();
            }
        }

        public event NameChangedEventHandler NameChanged;

        protected virtual void OnNameChanged()
        {
            if (NameChanged != null)
                NameChanged(this, EventArgs.Empty);
        }
    }
}