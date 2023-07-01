using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    internal class Grass
    {
        private uint energy = 100;
        private uint immunity = 100;
        private double photosynthesisAbility = 0.9;
        public uint Energy
        {
            get { return energy; }
            set { energy = value; }
        }
        public uint Immunity
        {
            get { return immunity; }
            set { immunity = value; }
        }

        public bool IsAlive()
        {
            if (energy > 0) return true;
            else return false;
        }

        private bool CanBirth(int newEnergy)
        {
            if(newEnergy == 0)
                return false;
            else if (newEnergy >= 20)
                return true;
            else
            {
                Random random = new Random();
                if (newEnergy - random.Next(0, newEnergy) < 3) return false;
                else return true;
            }
        }
        public bool Live(int sun)
        {
            var newEnergy = sun * photosynthesisAbility;
            if (CanBirth((int)newEnergy))
            {
                energy = 100;
                return true;
            }
            else
            {
                energy -= 20;
                return false;
            }
        }
    }
}
