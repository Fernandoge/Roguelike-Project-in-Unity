﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonController : MonoBehaviour
{
    [Header("Config")]
    public int boardRows;
    public int boardColumns;
    public int minRoomSize, maxRoomSize;
    [Header("Assets")]
    public RandomTools.WeightedObject[] dungeonRoomFloors; 
    public GameObject dungeonCorridorFloor;
    public GameObject dungeonGateway;
    [SerializeField]
    private Walls dungeonWalls = default;
    [Header("Elements")]
    public GameObject player;
    public DungeonEnemy[] enemies;
    [Header("Values")]
    public float corridorSpeed;
    [Header("Gameplay Info")]
    public int currentRoom;
    public int roomsCompleted;

    private GameObject roomHolder;
    private GameObject corridorHolder;
    public List<DungeonRoom> dungeonRooms = new List<DungeonRoom>();
    public List<GameObject> dungeonCorridors = new List<GameObject>();
    public GameObject[,] dungeonFloorsPosition;
    public GameObject[,] dungeonWallsPosition;

    private static int _totalCorridorIdCount = 0;
    private readonly RandomTools _clsRandomTools = new RandomTools();

    [System.Serializable]
    public struct Walls
    {
        public RandomTools.WeightedObject[] top;
        public GameObject bottom;
        public GameObject left;
        public GameObject right;
        public GameObject topLeftCorner;
        public GameObject topRightCorner;
        public GameObject bottomLeftCorner;
        public GameObject bottomRightCorner;
    }

    public class DungeonCorridor
    {
        public Rect rect;
        public int totalCorridorId;

        public DungeonCorridor(Rect rect, int totalCorridorId)
        {
            this.rect = rect;
            this.totalCorridorId = totalCorridorId;
        }
    }


    public class SubDungeon
    {
        public SubDungeon leftDungeon, rightDungeon;
        public Rect rectangle;
        public Rect room = new Rect(-1, -1, 0, 0);
        public List<DungeonCorridor> corridors = new List<DungeonCorridor>();

        public SubDungeon(Rect mrect)
        {
            rectangle = mrect;
        }

        public bool IAmLeaf()
        {
            return leftDungeon == null && rightDungeon == null;
        }

        public bool Split(int minRoomSize, int maxRoomSize)
        {
            if (!IAmLeaf())
            {
                return false;
            }

            bool splitHorizontal;
            if (rectangle.width / rectangle.height >= 1.25)
            {
                splitHorizontal = false;
            }
            else if (rectangle.height / rectangle.width >= 1.25)
            {
                splitHorizontal = true;
            }
            else
            {
                splitHorizontal = Random.Range(0.0f, 1.0f) > 0.5;
            }

            if (Mathf.Min(rectangle.height, rectangle.width) / 2 < minRoomSize)
            {
                return false;
            }

            if (splitHorizontal)
            {
                // split so that the resulting sub-dungeons widths are not too small
                // (since we are splitting horizontally) 
                int split = Random.Range(minRoomSize, (int)(rectangle.width - minRoomSize));

                leftDungeon = new SubDungeon(new Rect(rectangle.x, rectangle.y, rectangle.width, split));
                rightDungeon = new SubDungeon(new Rect(rectangle.x, rectangle.y + split, rectangle.width, rectangle.height - split));
            }
            else
            {
                int split = Random.Range(minRoomSize, (int)(rectangle.height - minRoomSize));

                leftDungeon = new SubDungeon(new Rect(rectangle.x, rectangle.y, split, rectangle.height));
                rightDungeon = new SubDungeon(new Rect(rectangle.x + split, rectangle.y, rectangle.width - split, rectangle.height));
            }

            return true;
        }

        public void CreateRoom()
        {
            if (leftDungeon != null)
            {
                leftDungeon.CreateRoom();
            }
            if (rightDungeon != null)
            {
                rightDungeon.CreateRoom();
            }
            if (leftDungeon != null && rightDungeon != null)
            {
                CreateCorridorBetween(leftDungeon, rightDungeon);
            }
            if (IAmLeaf())
            {
                int roomWidth = (int)Random.Range(rectangle.width / 2, rectangle.width - 2);
                int roomHeight = (int)Random.Range(rectangle.height / 2, rectangle.height - 2);
                int roomX = (int)Random.Range(1, rectangle.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rectangle.height - roomHeight - 1);

                // room position will be absolute in the board, not relative to the sub-dungeon
                room = new Rect(rectangle.x + roomX, rectangle.y + roomY, roomWidth, roomHeight);
            }
        }


        public void CreateCorridorBetween(SubDungeon left, SubDungeon right)
        {
            _totalCorridorIdCount++;
            Rect lroom = left.GetRoom();
            Rect rroom = right.GetRoom();

            // attach the corridor to a random point in each room
            Vector2 lpoint = new Vector2((int)Random.Range(lroom.x + 1, lroom.xMax - 1), (int)Random.Range(lroom.y + 1, lroom.yMax - 1));
            Vector2 rpoint = new Vector2((int)Random.Range(rroom.x + 1, rroom.xMax - 1), (int)Random.Range(rroom.y + 1, rroom.yMax - 1));

            // Be sure that left point is on the left to simplify the code
            if (lpoint.x > rpoint.x)
            {
                Vector2 temp = lpoint;
                lpoint = rpoint;
                rpoint = temp;
            }

            int w = (int)(lpoint.x - rpoint.x);
            int h = (int)(lpoint.y - rpoint.y);

            // if the points are not aligned horizontally
            if (w != 0)
            {
                // choose at random to go horizontal then vertical or the opposite
                if (Random.Range(0, 1) > 2)
                {
                    // add a corridor to the right
                    corridors.Add(new DungeonCorridor(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1), _totalCorridorIdCount));
                    // if left point is below right point go up
                    // otherwise go down
                    if (h < 0)
                    {
                        corridors.Add(new DungeonCorridor(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)), _totalCorridorIdCount));
                    }
                    else
                    {
                        corridors.Add(new DungeonCorridor(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)), _totalCorridorIdCount));
                    }
                }
                else
                {
                    // go up or down
                    if (h < 0)
                    {
                        corridors.Add(new DungeonCorridor(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)), _totalCorridorIdCount));
                    }
                    else
                    {
                        corridors.Add(new DungeonCorridor(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)), _totalCorridorIdCount));
                    }

                    // then go right
                    corridors.Add(new DungeonCorridor(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 1), _totalCorridorIdCount));
                }
            }
            else
            {
                // if the points are aligned horizontally
                // go up or down depending on the positions
                if (h < 0)
                {
                    corridors.Add(new DungeonCorridor(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)), _totalCorridorIdCount));
                }
                else
                {
                    corridors.Add(new DungeonCorridor(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)), _totalCorridorIdCount));
                }
            }
        }

        public Rect GetRoom()
        {
            if (IAmLeaf())
            {
                return room;
            }
            if (leftDungeon != null)
            {
                Rect lroom = leftDungeon.GetRoom();
                if (lroom.x != -1)
                {
                    return lroom;
                }
            }
            if (rightDungeon != null)
            {
                Rect rroom = rightDungeon.GetRoom();
                if (rroom.x != -1)
                {
                    return rroom;
                }
            }

            // workaround non nullable structs
            return new Rect(-1, -1, 0, 0);
        }
    }

    public void SpacePartition(SubDungeon subDungeon)
    {
        if (subDungeon.IAmLeaf())
        {
            //split subDungeon if it's large
            if (subDungeon.rectangle.width > maxRoomSize || subDungeon.rectangle.height > maxRoomSize || Random.Range(0.0f, 1.0f) > 0.25)
            {
                if (subDungeon.Split(minRoomSize, maxRoomSize))
                {
                    SpacePartition(subDungeon.leftDungeon);
                    SpacePartition(subDungeon.rightDungeon);
                }
            }
        }
    }

    public void DrawRooms(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        if (subDungeon.IAmLeaf())
        {
            //first we create holders for the rooms 
            //room prefab
            GameObject roomParent = Instantiate(roomHolder, transform.GetChild(0));
            DungeonRoom thisRoom = new DungeonRoom(roomParent, transform.GetChild(0).childCount, subDungeon.room);
            dungeonRooms.Add(thisRoom);
            //floor holder of room prefab
            Transform roomFloorHolder = roomParent.transform.GetChild(1);
            List<Transform> dungeonFloors = new List<Transform>();
            Vector3 newRoomPosition = new Vector3(0f, 0f, 0f);
            Vector3 floorPosition = new Vector3(0f, 0f, 0f);
            GameObject instance = null;

            //we loop the tiles to instantiate floors
            for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
            {
                for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                {
                    floorPosition = new Vector3(i, j, 0f);
                    instance = Instantiate(_clsRandomTools.PickOne(dungeonRoomFloors), floorPosition, Quaternion.identity, roomFloorHolder);
                    dungeonFloorsPosition[i, j] = instance;
                    dungeonFloors.Add(instance.transform);
                    newRoomPosition += floorPosition;
                }
            }


            //adjust roomHolder position to the center of the floors calculating the average position
            newRoomPosition /= roomFloorHolder.childCount;
            roomParent.transform.position = newRoomPosition;

            //we revert the floor positions since we moved their parent
            foreach (Transform floor in dungeonFloors)
            {
                floor.position -= newRoomPosition;
            }

            //adjust room box collider size
            BoxCollider2D roomCollider = roomParent.GetComponent<BoxCollider2D>();
            Vector2 newColliderSize = new Vector2(0, 0)
            {
                x = subDungeon.room.xMax - subDungeon.room.xMin - 2,
                y = subDungeon.room.yMax - subDungeon.room.yMin - 4
            };
            Vector2 newColliderOffset = new Vector2(0, 0)
            {
                x = 0f,
                y = -1f
            };
            roomCollider.size = newColliderSize;
            roomCollider.offset += newColliderOffset;

            //add walls to the room
            Transform roomWallHolder = roomParent.transform.GetChild(2);
            //room top walls
            for (int i = (int)subDungeon.room.x + 1; i < subDungeon.room.xMax - 1; i++)
            {
                instance = Instantiate(_clsRandomTools.PickOne(dungeonWalls.top), new Vector3(i, (int)subDungeon.room.yMax - 1, 0f), Quaternion.identity, roomWallHolder);
                dungeonWallsPosition[i, (int)subDungeon.room.yMax - 1] = instance;
                dungeonWallsPosition[i, (int)subDungeon.room.yMax - 2] = instance;
                dungeonWallsPosition[i, (int)subDungeon.room.yMax - 3] = instance;
            }
            //room bottom walls
            for (int i = (int)subDungeon.room.x + 1; i < subDungeon.room.xMax - 1; i++)
            {
                DrawWall(dungeonWalls.bottom, i, (int)subDungeon.room.y, roomWallHolder);
            }
            //room left walls
            for (int i = (int)subDungeon.room.y + 1; i < subDungeon.room.yMax - 1; i++)
            {
                DrawWall(dungeonWalls.left, (int)subDungeon.room.x, i, roomWallHolder);
            }
            //room right walls
            for (int i = (int)subDungeon.room.y + 1; i < subDungeon.room.yMax - 1; i++)
            {
                DrawWall(dungeonWalls.right, (int)subDungeon.room.xMax - 1, i, roomWallHolder);
            }

            //room corner walls
            DrawWall(dungeonWalls.topLeftCorner, (int)subDungeon.room.x, (int)subDungeon.room.yMax - 1, roomWallHolder);
            DrawWall(dungeonWalls.topRightCorner, (int)subDungeon.room.xMax - 1, (int)subDungeon.room.yMax - 1, roomWallHolder);
            DrawWall(dungeonWalls.bottomLeftCorner, (int)subDungeon.room.x, (int)subDungeon.room.y, roomWallHolder);
            DrawWall(dungeonWalls.bottomRightCorner, (int)subDungeon.room.xMax - 1, (int)subDungeon.room.y, roomWallHolder);

        }
        else
        {
            DrawRooms(subDungeon.leftDungeon);
            DrawRooms(subDungeon.rightDungeon);
        }
    }


    public void DrawWall(GameObject wall, int x, int y, Transform wallParent)
    {
        GameObject instance = Instantiate(wall, new Vector3(x, y, 0f), Quaternion.identity, wallParent);
        dungeonWallsPosition[x, y] = instance;
    }

    public void DrawCorridors(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        DrawCorridors(subDungeon.leftDungeon);
        DrawCorridors(subDungeon.rightDungeon);

        GameObject corridorParent = null;
        Transform corridorFloorHolder = null;
        Transform corridorFloorHolderAux = null;
        DungeonCorridor corridorAux = new DungeonCorridor(new Rect(-1, -1, 0, 0), 0);
        Vector3 corridorFloorPosition = new Vector3(0f, 0f, 0f);

        foreach (DungeonCorridor corridor in subDungeon.corridors)
        {
            //create new holder if it's a different corridors
            if (corridor.totalCorridorId != corridorAux.totalCorridorId)
            {
                corridorParent = Instantiate(corridorHolder, transform.GetChild(1));
                corridorFloorHolder = corridorParent.transform.GetChild(0);
                corridorFloorHolderAux = corridorFloorHolder;
            }
            corridorAux = corridor;

            //instantiate corridor tiles
            for (int i = (int)corridor.rect.x; i < corridor.rect.xMax; i++)
            {
                for (int j = (int)corridor.rect.y; j < corridor.rect.yMax; j++)
                {
                    //check if there isn't a room tile
                    if (dungeonFloorsPosition[i, j] == null)
                    {
                        corridorFloorPosition = new Vector3(i, j, 0f);
                        //instantiate corridor tile          
                        GameObject instance = Instantiate(dungeonCorridorFloor, corridorFloorPosition, Quaternion.identity, corridorFloorHolder);
                        instance.tag = CorridorValidator(i, j);
                        dungeonFloorsPosition[i, j] = instance;

                        //check if the corridor tile collides with a corner or top wall, in this case we add extra tiles so when we generate the portals the player don't have the path to it blocked
                        //corridor collision with top walls from left and right
                        int k = j;
                        while (dungeonWallsPosition[i + 2, k] != null && dungeonWallsPosition[i + 2, k].layer == LayerMask.NameToLayer("TopWall") ||
                            dungeonWallsPosition[i - 2, k] != null && dungeonWallsPosition[i - 2, k].layer == LayerMask.NameToLayer("TopWall"))
                        {
                            k--;
                            if (dungeonFloorsPosition[i, k] == null)
                            {
                                instance = Instantiate(dungeonCorridorFloor, new Vector3(i, k, 0f), Quaternion.identity, corridorFloorHolder);
                                instance.tag = CorridorValidator(i, k);
                                dungeonFloorsPosition[i, k] = instance;
                            }
                        }

                        //corridor collision with bottom corners from left and right
                        if (((dungeonWallsPosition[i + 1, j] != null && dungeonWallsPosition[i + 1, j].layer == LayerMask.NameToLayer("BottomLeftCorner")) ||
                            (dungeonWallsPosition[i - 1, j] != null && dungeonWallsPosition[i - 1, j].layer == LayerMask.NameToLayer("BottomRightCorner"))) && dungeonFloorsPosition[i, j + 1] == null)
                        {
                            instance = Instantiate(dungeonCorridorFloor, new Vector3(i, j + 1, 0f), Quaternion.identity, corridorFloorHolder);
                            instance.tag = CorridorValidator(i, j);
                            dungeonFloorsPosition[i, j + 1] = instance;
                        }

                        //corridor collision with left corners from top and bottom
                        if (((dungeonWallsPosition[i, j + 1] != null && dungeonWallsPosition[i, j + 1].layer == LayerMask.NameToLayer("BottomLeftCorner")) ||
                            (dungeonWallsPosition[i, j - 1] != null && dungeonWallsPosition[i, j - 1].layer == LayerMask.NameToLayer("TopLeftCorner"))) && dungeonFloorsPosition[i + 1, j] == null)
                        {

                            instance = Instantiate(dungeonCorridorFloor, new Vector3(i + 1, j, 0f), Quaternion.identity, corridorFloorHolder);
                            instance.tag = CorridorValidator(i, j);
                            dungeonFloorsPosition[i + 1, j] = instance;
                        }

                        //corridor collision with right corners from top and bottom
                        if (((dungeonWallsPosition[i, j + 1] != null && dungeonWallsPosition[i, j + 1].layer == LayerMask.NameToLayer("BottomRightCorner")) ||
                            (dungeonWallsPosition[i, j - 1] != null && dungeonWallsPosition[i, j - 1].layer == LayerMask.NameToLayer("TopRightCorner"))) && dungeonFloorsPosition[i - 1, j] == null)
                        {
                            instance = Instantiate(dungeonCorridorFloor, new Vector3(i - 1, j, 0f), Quaternion.identity, corridorFloorHolder);
                            instance.tag = CorridorValidator(i, j);
                            dungeonFloorsPosition[i - 1, j] = instance;
                        }

                    }
                }
            }
            if (!dungeonCorridors.Contains(corridorParent))
                dungeonCorridors.Add(corridorParent);
        }
    }

    public string CorridorValidator(int x, int y)
    {
        if ((dungeonWallsPosition[x + 2, y] != null && dungeonWallsPosition[x + 2, y].layer == LayerMask.NameToLayer("TopWall")) ||
            (dungeonWallsPosition[x - 2, y] != null && dungeonWallsPosition[x - 2, y].layer == LayerMask.NameToLayer("TopWall")))
            return "Untagged";
        else
            return "ValidCorridor";
    }


    public void GenerateGateways()
    {
        foreach (GameObject corridor in dungeonCorridors)
        {
            Transform corridorFloorHolder = corridor.transform.GetChild(0);
            foreach (Transform child in corridorFloorHolder)
            {
                if (child.tag != "ValidCorridor")
                    continue;

                int posX = (int)child.position.x;
                int posY = (int)child.position.y;

                int neighboursCount = 0;

                if (dungeonFloorsPosition[posX + 1, posY] != null)
                {
                    if (dungeonWallsPosition[posX + 1, posY] == null && dungeonFloorsPosition[posX + 1, posY].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX + 1, posY].tag == "ValidCorridor")
                        neighboursCount++;
                    else if (!dungeonFloorsPosition[posX + 1, posY].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX + 2, posY] != null && ((dungeonFloorsPosition[posX + 2, posY].tag != "ValidCorridor" &&
                        dungeonFloorsPosition[posX + 2, posY].transform.parent.parent.tag != "DungeonRoom") || dungeonFloorsPosition[posX + 2, posY].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }
                if (dungeonFloorsPosition[posX - 1, posY] != null)
                {
                    if (dungeonWallsPosition[posX - 1, posY] == null && dungeonFloorsPosition[posX - 1, posY].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX - 1, posY].tag == "ValidCorridor")
                        neighboursCount++;
                    else if (!dungeonFloorsPosition[posX - 1, posY].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX - 2, posY] != null && ((dungeonFloorsPosition[posX - 2, posY].tag != "ValidCorridor" &&
                        dungeonFloorsPosition[posX - 2, posY].transform.parent.parent.tag != "DungeonRoom") || dungeonFloorsPosition[posX - 2, posY].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }

                if (dungeonFloorsPosition[posX, posY - 1] != null)
                {
                    if (dungeonWallsPosition[posX, posY - 1] == null && dungeonFloorsPosition[posX, posY - 1].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX, posY - 1].tag == "ValidCorridor")
                        neighboursCount++;
                    else if (!dungeonFloorsPosition[posX, posY - 1].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX, posY - 2] != null && ((dungeonFloorsPosition[posX, posY - 2].tag != "ValidCorridor" &&
                        dungeonFloorsPosition[posX, posY - 2].transform.parent.parent.tag != "DungeonRoom") || dungeonFloorsPosition[posX, posY - 2].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }
                    
                if (dungeonFloorsPosition[posX, posY + 1] != null)
                {
                    if (dungeonWallsPosition[posX, posY + 1] == null && dungeonFloorsPosition[posX, posY + 1].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX, posY + 1].tag == "ValidCorridor")
                        neighboursCount++;
                    else if (dungeonFloorsPosition[posX, posY + 1].tag != "ValidCorridor" && dungeonFloorsPosition[posX, posY + 4] != null && dungeonFloorsPosition[posX, posY + 4].transform.IsChildOf(corridorFloorHolder))
                        neighboursCount++;
                    else if (!dungeonFloorsPosition[posX, posY + 1].transform.IsChildOf(corridorFloorHolder) && dungeonFloorsPosition[posX, posY + 2] != null && ((dungeonFloorsPosition[posX, posY + 2].tag != "ValidCorridor" &&
                        dungeonFloorsPosition[posX, posY + 2].transform.parent.parent.tag != "DungeonRoom") || dungeonFloorsPosition[posX, posY + 2].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }

                if (neighboursCount < 2)
                {
                    GameObject gateway;
                    if (dungeonWallsPosition[posX, posY + 1] != null)
                    {

                        gateway = Instantiate(dungeonGateway, new Vector3(posX, posY + 1, 0f), Quaternion.identity, dungeonFloorsPosition[posX, posY + 1].transform.parent.parent.GetChild(0));
                        gateway.GetComponent<GatewayPortal>().firstDirection = 0;
                        dungeonFloorsPosition[posX, posY + 1].tag = "Gateway";
                        if (dungeonWallsPosition[posX, posY + 1].layer == LayerMask.NameToLayer("Default") || dungeonWallsPosition[posX, posY + 1].layer == LayerMask.NameToLayer("TopWall"))
                            Destroy(dungeonWallsPosition[posX, posY + 1]);
                    }
                    if (dungeonWallsPosition[posX, posY - 1] != null)
                    {
                        gateway = Instantiate(dungeonGateway, new Vector3(posX, posY - 1, 0f), Quaternion.identity, dungeonFloorsPosition[posX, posY - 1].transform.parent.parent.GetChild(0));
                        gateway.GetComponent<GatewayPortal>().firstDirection = 2;
                        dungeonFloorsPosition[posX, posY - 1].tag = "Gateway";
                        if (dungeonWallsPosition[posX, posY - 1].layer == LayerMask.NameToLayer("Default") || dungeonWallsPosition[posX, posY - 1].layer == LayerMask.NameToLayer("TopWall"))
                            Destroy(dungeonWallsPosition[posX, posY - 1]);
                    }
                    if (dungeonWallsPosition[posX + 1, posY] != null)
                    {
                        gateway = Instantiate(dungeonGateway, new Vector3(posX + 1, posY, 0f), Quaternion.identity, dungeonFloorsPosition[posX + 1, posY].transform.parent.parent.GetChild(0));
                        gateway.GetComponent<GatewayPortal>().firstDirection = 3;
                        dungeonFloorsPosition[posX + 1, posY].tag = "Gateway";
                        if (dungeonWallsPosition[posX + 1, posY].layer == LayerMask.NameToLayer("Default") || dungeonWallsPosition[posX + 1, posY].layer == LayerMask.NameToLayer("TopWall"))
                            Destroy(dungeonWallsPosition[posX + 1, posY]);
                    }
                    if (dungeonWallsPosition[posX - 1, posY] != null)
                    {
                        gateway = Instantiate(dungeonGateway, new Vector3(posX - 1, posY, 0f), Quaternion.identity, dungeonFloorsPosition[posX - 1, posY].transform.parent.parent.GetChild(0));
                        gateway.GetComponent<GatewayPortal>().firstDirection = 1;
                        dungeonFloorsPosition[posX - 1, posY].tag = "Gateway";
                        if (dungeonWallsPosition[posX - 1, posY].layer == LayerMask.NameToLayer("Default") || dungeonWallsPosition[posX - 1, posY].layer == LayerMask.NameToLayer("TopWall"))
                            Destroy(dungeonWallsPosition[posX - 1, posY]);
                    }
                }
            }
        }
    }

    public void DefineRooms()
    {
        RoomController roomComponent = null;
        foreach (DungeonRoom dungeonRoom in dungeonRooms)
        {
            int roomChance = Random.Range(1, 4);

            switch (roomChance)
            {
                //enemy room
                case 1:
                    roomComponent = dungeonRoom.room.AddComponent(typeof(EnemiesRoom)) as EnemiesRoom;
                    break;
                //treasure room
                case 2:
                    roomComponent = dungeonRoom.room.AddComponent(typeof(TreasureRoom)) as TreasureRoom;
                    break;
                //boss room
                case 3:
                    roomComponent = dungeonRoom.room.AddComponent(typeof(BossRoom)) as BossRoom;
                    break;
            }

            roomComponent.id = dungeonRoom.id;
            roomComponent.roomRectangle = dungeonRoom.roomRectangle;
            roomComponent.DrawRoomInteriors();
        }
    }

    public void SpawnPlayer()
    {
        //Spawn at the first room with the room already completed
        GameObject room = transform.GetChild(0).GetChild(0).gameObject;
        room.GetComponent<RoomController>().isCompleted = true;
        Instantiate(player, room.transform.position, Quaternion.identity);
    }

    void Start()
    {
        roomHolder = Resources.Load<GameObject>("Room");
        corridorHolder = Resources.Load<GameObject>("Corridor");
        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, boardRows, boardColumns));
        dungeonFloorsPosition = new GameObject[boardRows, boardColumns];
        dungeonWallsPosition = new GameObject[boardRows, boardColumns];
        dungeonRoomFloors = _clsRandomTools.CreateWeightedObjectsArray(dungeonRoomFloors);
        dungeonWalls.top = _clsRandomTools.CreateWeightedObjectsArray(dungeonWalls.top);

        SpacePartition(rootSubDungeon);
        rootSubDungeon.CreateRoom();
        DrawRooms(rootSubDungeon);
        DefineRooms();
        SpawnPlayer();
        DrawCorridors(rootSubDungeon);
        GenerateGateways();
    }
}