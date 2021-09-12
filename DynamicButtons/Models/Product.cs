using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DynamicButtons.Models
{
    public class Product : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual double Price { get; set; }

        public virtual int Units { get; set; }
        public virtual String Display { get; set; }

        public virtual double Ounces { get; set; }

        public Guid Id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UnitProduct : Product
    {
        public override string Display
        {
            get
            {
                return String.Format("{0,-30}\t{1,-20}\tUnits: {2,-20}\tPrice: {3,-10:C}", Name, Description, Units, Price);
            }
        }

        public double UnitPrice { get; set; }

        private int units;
        public override int Units
        {
            get
            {
                return units;
            }

            set
            {
                units = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Display");
            }
        }
        public override double Price => units* UnitPrice;
    }
    public class OunceProduct : Product
    {
        public override string Display
        {
            get
            {
                return String.Format("{0,-30}\t{1,-20}\tOunces: {2,-20}\tPrice: {3,-10:C}", Name, Description, Ounces, Price);
            }
        }

        public double OuncePrice { get; set; }

        private double ounces;
        public override double Ounces
        {
            get
            {
                return ounces;
            }

            set
            {
                ounces = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Display");
            }
        }
        public override double Price => ounces * OuncePrice;
    }
}
