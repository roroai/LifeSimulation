using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    internal class Predator
    {
        private uint energy = 100;
        private uint immunity = 100;
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

        public void Live(int count)
        {
            if (IsAlive() && count < 100) energy--;
        }

        public bool CanEating(uint ability)
        {
            if(ability < 175) return true;
            else if(ability > 175 && ability < 200)
            {
                Random random = new Random();
                if (ability > random.Next(150, 200)) return false;
                else return true;
            }
            else return false;
        }
    }
}