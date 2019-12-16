using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    class ItemNode
    {
        public ItemNode()
        {
            int ID = 0;
            int meleeDamageBonus = 0;
            int rangeDamageBonus = 0;
            int defenseBonus = 0;
            int range = 0;
            string weaponType = null;
            Boolean hinderance = false;
            MapNode mapLocation = null;
            Character character = null;
            int itemDistanceScore = 0;
        }

        public int itemDistanceScore { get; set; }
        public int ID { get; set; }
        public int range { get; set; }
        public Boolean hinderance { get; set; }
        public int rangeDamageBonus { get; set; }
        public int meleeDamageBonus { get; set; }
        public int defenseBonus { get; set; }
        public string weaponType { get; set; }
        public MapNode mapLocation { get; set; }
        public Character character { get; set; }

    }
}
