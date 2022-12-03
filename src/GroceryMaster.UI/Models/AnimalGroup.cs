using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryMaster.UI.Models
{
    public class AnimalGroup : List<Animal>
    {
        public AnimalGroup(string name, List<Animal> animals) : base(animals)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}