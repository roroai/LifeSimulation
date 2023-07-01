using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    internal class Mushroom
    {
        private uint immunity = 100;
        private uint organic = 100;
        private double foodsynthesisAbility = 0.9;
        public uint Immunity
        {
            get { return immunity; }
            set { immunity = value; }
        }
        
        public uint Organic
        {
            get { return organic; }
            set { organic = value;}
        }

        public bool IsAlive()
        {
            if (organic > 0) return true;
            else return false;
        }

        private bool CanBirth(int newEnergy)
        {
            if (newEnergy == 0)
                return false;
            else if (newEnergy >= 20)
                return true;
            else
            {
                Random random = new Random(DateTime.Now.Millisecond);
                if (newEnergy - random.Next(0, newEnergy) < 3) return false;
                else return true;
            }
        }
        public bool Live(int food)
        {
            var newEnergy = food * foodsynthesisAbility;
            if (CanBirth((int)newEnergy))
            {
                organic = 100;
                return true;
            }
            else
            {
                organic -= 20;
                return false;
            }
        }
    }
}
