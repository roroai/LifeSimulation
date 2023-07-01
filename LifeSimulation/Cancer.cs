using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeSimulation
{
    internal class Cancer
    {
        private uint energy = 100;
        private uint cancerAbility = 70;

        public uint Energy {
            get { return energy; }
            set { energy = value; }
        }

        public uint CancerAbility
        {
            get { return cancerAbility; }
            set { cancerAbility = value; }
        }

        public bool CanCancered(uint immunity)
        {
            if (cancerAbility >= immunity)
                return true;
            else
            {
                Random random = new Random(DateTime.Now.Millisecond);
                if (immunity >= random.Next(0, 100)) return false;
                else return true;
            }
        }

        public bool IsAlive()
        {
            if (energy > 0) return true;
            else return false;
        }

        public void Live(int count)
        {
            if (IsAlive() && count < 25) energy -= 10;
        }
    }
}