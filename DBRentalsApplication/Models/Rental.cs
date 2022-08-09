using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DBRentalsApplication.Models
{
    public partial class Rental : INotifyPropertyChanged
    {
        public int Id
        {
            get;
            set;
        }

        public int CarId
        {
            get;
            set;
        }

        public int DriverId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string? Comments { get; set; }

        public virtual Car Car { get; set; } = null!;
        public virtual Driver Driver { get; set; } = null!;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
