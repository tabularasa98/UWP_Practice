using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicButtons.Models
{
    public class InventoryItem
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        public virtual string Display { get; set; }

        public Guid Id { get; private set; }

        public InventoryItem()
        {
            Id = System.Guid.NewGuid();
        }
    }
    public class ProductByQuantity : InventoryItem
    {
        public  override string Display
        {
            get
            {
                return String.Format("{0,-20}\t{1,-20}\tPrice per Unit: {2,-20:C}", Name, Description, Price);
            }
        }

    }
    public class ProductByWeight : InventoryItem
    {
        public override string Display
        {
            get
            {
                return String.Format("{0,-20}\t{1,-20}\tPrice per Ounce: {2,-20:C}", Name, Description, Price);
            }
        }

    }
}
