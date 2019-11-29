using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace heroclixAI
{
    class Pathfinder
    {
        //Returns null if no path was found. You must handle this possible event
        //Takes two MapNodes, one for the starting node and the second for the destination
        //Needs the recent gameMap data passed to it as well
        //You can optionally select it to Avoid Hinderance squares (true) or not (default false)
        //You can optionally pass a LinkedList filled with MapNodes to avoid for example you don't want the AI to pass through a dangerzone (default null)
        public LinkedList<MapNode> FindShortestPath(MapNode startLocation, MapNode endLocation, MapNode[,] gameMap, Boolean AvoidHinderance = false, LinkedList<MapNode> AvoidMapNodes = null)
        {
            LinkedList<MapNode> OpenList = new LinkedList<MapNode>();
            LinkedList<MapNode> ClosedList = new LinkedList<MapNode>();
            LinkedList<MapNode> ShortestPath = new LinkedList<MapNode>();
            MapNode current = null;
            int g = 0;

            OpenList.AddLast(startLocation);

            while (OpenList.Count > 0)
            {
                //Retrieve the lowest f score
                var lowest = OpenList.Min(l => l.f);
                current = OpenList.First(l => l.f == lowest);

                //Add and remove MapNode from lists
                ClosedList.AddLast(current);
                OpenList.Remove(current);

                //Check for if final destination is reached
                if (ClosedList.FirstOrDefault(l => l.x == endLocation.x && l.y == endLocation.y) != null)
                    break;

                //Move on through adjacent cells
                var adjacentMapNodes = CalculateAdjacentMapNodes(current.x, current.y, gameMap, AvoidHinderance);
                g++;

                //Runs through filtering, adding to OpenList, calculating g, h, f, and setting the ParentNode
                foreach(var adjacentMapNode in adjacentMapNodes)
                {
                    //Exclude if in AvoidMapNodes
                    if (AvoidMapNodes != null)
                    {
                        if (AvoidMapNodes.FirstOrDefault(l => l.x == adjacentMapNode.x && l.y == adjacentMapNode.y) != null)
                            continue;
                    }

                    //Exclude if in ClosedList
                    if (ClosedList.FirstOrDefault(l => l.x == adjacentMapNode.x && l.y == adjacentMapNode.y) != null)
                        continue;

                    //Check to see if it is not in the OpenList
                    if (OpenList.FirstOrDefault(l => l.x == adjacentMapNode.x && l.y == adjacentMapNode.y) == null)
                    {
                        //Calculate and set the g, h, f, and ParentNode
                        adjacentMapNode.g = g;
                        adjacentMapNode.h = CalculateHScore(adjacentMapNode.x, adjacentMapNode.y, endLocation.x, endLocation.y);
                        adjacentMapNode.f = adjacentMapNode.g + adjacentMapNode.h;
                        adjacentMapNode.ParentNode = current;

                        //Add it to the Open List
                        OpenList.AddFirst(adjacentMapNode);
                    }
                    else
                    {
                        //Check to see if there is a better path and update the parent
                        if (g + adjacentMapNode.h < adjacentMapNode.f)
                        {
                            adjacentMapNode.g = g;
                            adjacentMapNode.f = adjacentMapNode.g + adjacentMapNode.h;
                            adjacentMapNode.ParentNode = current;
                        }
                    }
                }
            }
            //Construct a LinkedList that is ordered with first MapNode moved to is placed in the first index of the LinkedList
            current = ClosedList.Last();

            while (current.ParentNode != null)
                {
                    ShortestPath.AddFirst(current);
                    current = current.ParentNode;
                }

            //If the destination was never reached return null
            if (ShortestPath.FirstOrDefault(l => l.x == endLocation.x && l.y == endLocation.y) == null)
            {
                ShortestPath = null;
            }

            return ShortestPath;
        }

        //Returns a list containing all adjacent MapNodes. Can toggle to exclude MapNodes with Hinderance.
        public List<MapNode> CalculateAdjacentMapNodes(int x, int y, MapNode[,] gameMap, Boolean AvoidHinderance)
        {
            var possibleMapNodes = new List<MapNode>();
            
            if (gameMap[x,y].IsConnectedToNorthNode == true)
            {
                possibleMapNodes.Add(gameMap[x, y - 1]);
            }
            if (gameMap[x,y].IsConnectedToNorthWestNode == true)
            {
                possibleMapNodes.Add(gameMap[x - 1, y - 1]);
            }
            if (gameMap[x,y].isConnectedToWestNode == true)
            {
                possibleMapNodes.Add(gameMap[x - 1, y]);
            }
            if (gameMap[x,y].IsConnectedToSouthWestNode == true)
            {
                possibleMapNodes.Add(gameMap[x - 1, y + 1]);
            }
            if (gameMap[x,y].IsConnectedToSouthNode == true)
            {
                possibleMapNodes.Add(gameMap[x, y + 1]);
            }
            if (gameMap[x,y].IsConnectedToSouthEastNode == true)
            {
                possibleMapNodes.Add(gameMap[x + 1, y + 1]);
            }
            if (gameMap[x,y].IsConnectedToEastNode == true)
            {
                possibleMapNodes.Add(gameMap[x + 1, y]);
            }
            if (gameMap[x,y].IsConnectedToNorthEastNode == true)
            {
                possibleMapNodes.Add(gameMap[x + 1, y - 1]);
            }

            if (AvoidHinderance == true)
            {
                return possibleMapNodes.Where(l => l.IsHinderance == false && l.IsBlocked == false).ToList();
            }

            return possibleMapNodes.Where(l => l.IsBlocked == false).ToList();
        }
        public int CalculateHScore(int startx, int starty, int endx, int endy)
        {
            return Math.Abs(endx - startx) + Math.Abs(endy - starty);
        }
    }
}
