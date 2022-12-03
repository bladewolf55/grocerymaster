using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryMaster.UI.Models
{
    public class Animal
    {
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}