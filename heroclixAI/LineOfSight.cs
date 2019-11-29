using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    class LineOfSight
    {
        //Upon creation set the initial values.
        public LineOfSight()
        {
            Boolean lineOfSight = false;
            Boolean isHinderance = false;
            Boolean isOccupied = false;
            Boolean isBlocked = false;
        }

        public Boolean lineOfSight { get; set; }
        public Boolean isHinderance { get; set; }
        public Boolean isOccupied { get; set; }
        public Boolean IsBlocked { get; set; }
    }
}
