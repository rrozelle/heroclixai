using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace heroclixAI
{
    [Serializable]
    class MapNode
    {
        //Upon creation sets all booleans to nothing hindering and can freely move to any nearby nodes
        public MapNode()
        {
            int x = 0;
            int y = 0;
            int g = 0;
            int h = 0;
            int f = 0;
            MapNode ParentNode = null;
            IsOccupied = false;
            IsBlocked = false;
            IsHinderance = false;
            IsConnectedToNorthNode = true;
            IsConnectedToNorthEastNode = true;
            IsConnectedToEastNode = true;
            IsConnectedToSouthEastNode = true;
            IsConnectedToSouthNode = true;
            IsConnectedToSouthWestNode = true;
            isConnectedToWestNode = true;
            IsConnectedToNorthWestNode = true;
        }

        public int h { get; set; }
        public int g { get; set; }
        public int y { get; set; }
        public int x { get; set; }
        public int f { get; set; }
        public MapNode ParentNode { get; set; }
        public Boolean IsOccupied { get; set; }
        public Boolean IsBlocked { get; set; }
        public Boolean IsHinderance { get; set; }

        public Boolean IsConnectedToNorthNode { get; set; }

        public Boolean IsConnectedToNorthEastNode { get; set; }

        public Boolean IsConnectedToEastNode { get; set; }

        public Boolean IsConnectedToSouthEastNode { get; set; }

        public Boolean IsConnectedToSouthNode { get; set; }

        public Boolean IsConnectedToSouthWestNode { get; set; }

        public Boolean isConnectedToWestNode { get; set; }

        public Boolean IsConnectedToNorthWestNode { get; set; }
    }
}
