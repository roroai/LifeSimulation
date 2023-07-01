using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    internal class Area
    {
        private uint energy = 125;
        private uint organic = 125;

        public uint Organic
        {
            get { return organic; }
            set { organic = value; }
        }
        public uint Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        private bool IsSun()
        {
            if (energy > 0) return true;
            else return false;
        }

        private bool IsEarth()
        {
            if (organic > 0) return true;
            else return false;
        }

        public void Normalization(uint normOrganic, uint normEnergy)
        {
            energy = normEnergy;
            organic = normOrganic;
        }
        public void Disaster()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            if (IsSun())
            {
                energy = (uint)random.Next(0, 50);
            }
            else
            {
                energy -= (uint)random.Next(150, 200);
            }
        }

        public void VolcanicWinter()
        {
            if (!IsSun())
                energy = 0;
        }

        public void Hurricane()
        {
            if(!IsEarth())
                organic = 0;
        }

        public bool Burnt()
        {
            if (energy == 200) return true;
            else return false;
        }
    }
}
