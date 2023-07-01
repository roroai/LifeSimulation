using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    internal class MultiGrass
    {
        private uint energy = 100;
        private uint immunity = 100;
        private uint organic = 100;
        private double photosynthesisAbility = 0.45;
        private double foodsynthesisAbility = 0.45;
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
        public uint Organic
        {
            get { return organic; }
            set { organic = value; }
        }
        public bool IsAlive()
        {
            if (energy > 0) return true;
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
        public bool Live(int sun, int organic)
        {
            var newEnergy = sun * photosynthesisAbility + organic * foodsynthesisAbility;
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
