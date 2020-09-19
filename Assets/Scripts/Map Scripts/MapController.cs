using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapController : MonoBehaviour
{
    public GameObject mapNode;
    public int mapWidth, mapHeight;
    private List<List<GameObject>> mapNodes;
    private GameController GameController;

    // Start is called before the first frame update
    void Start(){
        mapNodes = new List<List<GameObject>>(mapWidth);
        GameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        makeMap();
    }

    //gets node with coordinates, returns null if trying to get node outside the map
    public GameObject getNode(int nodeX, int nodeY){
        if(nodeX >= mapWidth-1 || nodeX < 0 || nodeY >= mapHeight-1 || nodeY < 0){
            return null;
        }else{
            return mapNodes[nodeX][nodeY];
        }
    }

    public Character getOccupant(int nodeX, int nodeY){
        if(nodeX >= mapWidth-1 || nodeX < 0 || nodeY >= mapHeight-1 || nodeY < 0){
            return null;
        }else{
            return mapNodes[nodeX][nodeY].GetComponent<NodeController>().occupant;
        }
    }

    //resets node variables for path finding
    public void resetNodesPathVariables(){
        foreach (Transform child in transform){
            child.GetComponent<NodeController>().resetPathVariables();
        }
    }

    //resets node variables for path finding
    public void resetNodeColors(){
        foreach (Transform child in transform){
            child.GetComponent<NodeController>().resetColor();
        }
    }

    //colors the grid based on is the node reachable with given move amount and source node
    public void showMoveRange(int range, GameObject sourceNode,bool checkDirect = false){
        if(sourceNode == null){return;}
        int sourceX = sourceNode.GetComponent<NodeController>().x;
        int sourceY = sourceNode.GetComponent<NodeController>().y;
        foreach (Transform child in transform){
            NodeController testedNode = child.GetComponent<NodeController>();
            if(testedNode.passable){
                if (testedNode.x <= sourceX+range && testedNode.x >= sourceX-range && testedNode.y <= sourceY+range && testedNode.y >= sourceY-range){
                    if(checkDirect){testedNode.showRange(range+1,sourceX,sourceY,true);}
                    else{testedNode.showRange(range+1,sourceX,sourceY);}
                    
                }else{
                    testedNode.showOutOfRange();
                }
            }else{
                testedNode.showOutOfRange();
            }
        }
    }

    //checks if path between two nodes is clear for shooting
    public bool checLine(GameObject start, GameObject end){
        bool passableLine = true;
        NodeController endNode = end.GetComponent<NodeController>();
        List<GameObject> path = getPath(start, endNode.x, endNode.y, false);
        foreach(GameObject node in path){
            if(!node.GetComponent<NodeController>().passable){passableLine = false;}
        }
        return passableLine;
    }

    //a* pathfinding. Adapted from https://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
    public List<GameObject> getPath(GameObject startNode, int targetX, int targetY, bool checkPassable = true){
        GameObject current = null;
        GameObject start = startNode;
        GameObject target = mapNodes[targetX][targetY];
        //check for if the target node is valid if not return list with only startnode
        NodeController targetNode = target.GetComponent<NodeController>();
        if(!targetNode.passable){
            return new List<GameObject>();
        }
        List<GameObject> openList = new List<GameObject>();
        List<GameObject> closedList = new List<GameObject>();
        int g = 0;
        openList.Add(start);

        while (openList.Count > 0){
            // get the square with the lowest F score
            var lowest = openList.Where(node => node != null).Min(l => l.GetComponent<NodeController>().F);
            current = openList.First(l => l.GetComponent<NodeController>().F == lowest);

            // add the current square to the closed list and remove from openList
            closedList.Add(current);
            openList.Remove(current);

            if (closedList.FirstOrDefault(l => l.GetComponent<NodeController>().x == targetX && l.GetComponent<NodeController>().y == targetY) != null)
                break;

            NodeController currentNode = current.GetComponent<NodeController>();
            var adjacentSquares = GetWalkableAdjacentSquares(currentNode.x, currentNode.y, mapNodes, checkPassable);
            g++;

            foreach(var adjacentSquare in adjacentSquares){
                // if this adjacent square is already in the closed list, ignore it
                if (closedList.FirstOrDefault(l => l == adjacentSquare) != null)
                    continue;

                // if it's not in the open list...
                if (openList.FirstOrDefault(l => l == adjacentSquare) == null){
                    // compute its score, set the parent
                    NodeController nodeScript = adjacentSquare.GetComponent<NodeController>();
                    nodeScript.G = g;
                    nodeScript.H = ComputeHScore(nodeScript.x, nodeScript.y, targetX, targetY);
                    nodeScript.F = nodeScript.G + nodeScript.H;
                    nodeScript.Parent = current;
            
                    // and add it to the open list
                    openList.Insert(0, adjacentSquare);
                }else{
                    NodeController nodeScript = adjacentSquare.GetComponent<NodeController>();
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (g + nodeScript.H < nodeScript.F) {
                        nodeScript.G = g;
                        nodeScript.F = nodeScript.G + nodeScript.H;
                        nodeScript.Parent = current;
                    }
                }
            }
        }
        //assemble final list
        List<GameObject> finalList = new List<GameObject>();
        while (current != null) {
            finalList.Add(current);
            current = current.GetComponent<NodeController>().Parent;
        }
        resetNodesPathVariables();
        return finalList;
    }

    //Adapted from https://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
    private int ComputeHScore(int x, int y, int targetX, int targetY){
        return Mathf.Abs(targetX - x) + Mathf.Abs(targetY - y);
    }

    //Adapted from https://gigi.nullneuron.net/gigilabs/a-pathfinding-example-in-c/
    private List<GameObject> GetWalkableAdjacentSquares(int x, int y, List<List<GameObject>> map, bool checkPassable){
        var proposedLocations = new List<GameObject>();
        if(x < mapWidth-1){proposedLocations.Add(map[x+1][y]);}
        if(x > 0){proposedLocations.Add(map[x-1][y]);}
        if(y < mapHeight-1){proposedLocations.Add(map[x][y+1]);}
        if(y > 0){proposedLocations.Add(map[x][y-1]);}
        //corners
        if(x > 0 && y > 0){proposedLocations.Add(map[x-1][y-1]);}
        if(x < mapWidth-1 && y > 0){proposedLocations.Add(map[x+1][y-1]);}
        if(x < mapWidth-1 && y < mapHeight-1){proposedLocations.Add(map[x+1][y+1]);}
        if(x > 0 && y < mapHeight-1){proposedLocations.Add(map[x-1][y+1]);}
        if(checkPassable){
            return proposedLocations.Where(node => node.GetComponent<NodeController>().passable == true).ToList();
        }else{
            return proposedLocations;
        }
    }

    public void getNeighbourSquares(GameObject centerNode){

    }

    //creates map nodes and puts them in 2d list for easy access by coordinates
    private void makeMap(){
        for(int i = 0; i < mapWidth; i++){
            List<GameObject> column = new List<GameObject>(mapHeight); 
            for(int j = 0; j < mapHeight; j++){
                GameObject newNode = Instantiate(mapNode, new Vector3(j,0,i), transform.rotation); 
                newNode.transform.SetParent(transform);
                newNode.GetComponent<NodeController>().SetCoordinates(i,j);
                column.Add(newNode);
            }
            mapNodes.Add(column);
        }
        GameController.mapGenerated();
    }
}
