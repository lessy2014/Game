using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class Archer: Support
    {
        public override void Awake()
        {
            GetComponents();
            Instance = this;
        }
    }
}
