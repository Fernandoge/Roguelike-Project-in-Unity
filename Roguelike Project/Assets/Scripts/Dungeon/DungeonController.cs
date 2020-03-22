﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonController : MonoBehaviour
{
    [Header("Dungeon Size")]
    public int boardRows;
    public int boardColumns;
    public int averageMinRoomTiles;
    public RoomSizeRandomness roomSizeRandomness;
    public int minRoomWidth, minRoomHeight;
    public int maxRoomWidth, maxRoomHeight;
    [Header("Dungeon Values")]
    public float corridorSpeed;
    public int treasureRoomQuantity;
    [Header("Dungeon Assets")]
    public RandomTools.WeightedObject[] dungeonRoomFloors; 
    public Walls dungeonWalls;
    public List<RandomTools.SizeWeightedObject> dungeonRoomInteriors;
    public RandomTools.WeightedObject[] dungeonWallDecos;
    public RandomTools.WeightedObject[] dungeonCorridorFloors;
    public GameObject playerCorridorParticles;
    public GameObject dungeonGateway;
    [Header("Elements")]
    public GameObject player;
    public List<DungeonEnemyPack> enemyPacks;
    public GameObject treasure;
    public GameObject bossRoom;
    public GameObject initialCorridor;
    [Header ("Controller References")]
    public Transform dungeonRoomsParent;
    public Transform dungeonCorridorsParent;
    public Camera dungeonCamera;
    [Header("Gameplay Info")]
    public RoomController currentRoom;
    public RoomController previousRoom;
    public int roomsCompleted;

    private int _roomSideSize;
    private GameObject _roomHolder;
    private GameObject _corridorHolder;
    private PlayerMovement _clsPlayerMovement;
    private PlayerSpriteManager _clsPlayerSpriteManager;
    private List<DungeonRoom> _dungeonRooms = new List<DungeonRoom>();
    private List<RoomController> _dungeonRoomsComponent = new List<RoomController>();
    private List<GameObject> _dungeonCorridors = new List<GameObject>();
    private List<GameObject> _incorrectLayerWalls = new List<GameObject>();
    public GameObject[,] tilesPosition;
    [System.NonSerialized] public BossRoom bossRoomInstance;

    private static int _totalCorridorIdCount = 0;

    #region Dungeon Components

    public enum RoomSizeRandomness
    {
        FullRandom = 0,
        BigRooms = 1,
        SmallRooms = 2
    }

    public struct DungeonRoom
    {
        public GameObject room;
        public int id;
        public Rect roomRectangle;

        public DungeonRoom(GameObject room, int id, Rect roomRectangle)
        {
            this.room = room;
            this.id = id;
            this.roomRectangle = roomRectangle;
        }
    }

    [System.Serializable]
    public struct Walls
    {
        public GameObject top;
        public GameObject bottom;
        public GameObject left;
        public GameObject right;
        public GameObject topLeftCorner;
        public GameObject topRightCorner;
        public GameObject bottomLeftCorner;
        public GameObject bottomRightCorner;
    }

    [System.Serializable]
    public struct DungeonEnemyPack
    {
        public GameObject[] enemies;
        public int stock;
        public int minRoomFloorsWidth, minRoomFloorsHeight, maxRoomFloorsWidth, maxRoomFloorsHeight;

        public DungeonEnemyPack(GameObject[] enemies, int stock, int minRoomFloorsWidth, int minRoomFloorsHeight, int maxRoomFloorsWidth, int maxRoomFloorsHeight)
        {
            this.enemies = enemies;
            this.stock = stock;
            this.minRoomFloorsWidth = minRoomFloorsWidth;
            this.minRoomFloorsHeight = minRoomFloorsHeight;
            this.maxRoomFloorsWidth = maxRoomFloorsWidth;
            this.maxRoomFloorsHeight = maxRoomFloorsHeight;
        }
    }

    public struct DungeonCorridor
    {
        public Rect rect;
        public int totalCorridorId;

        public DungeonCorridor(Rect rect, int totalCorridorId)
        {
            this.rect = rect;
            this.totalCorridorId = totalCorridorId;
        }
    }

    #endregion Dungeon Components

    #region Dungeon BSP
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

        public bool Split(int roomSideSize)
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
            
            if (Mathf.Min(rectangle.height, rectangle.width) / 2 < roomSideSize)
            {
                return false;
            }

            
            if (splitHorizontal)
            {
                // split so that the resulting sub-dungeons widths are not too small
                // (since we are splitting horizontally) 
                int split = Random.Range(roomSideSize, (int)(rectangle.width - roomSideSize));

                leftDungeon = new SubDungeon(new Rect(rectangle.x, rectangle.y, rectangle.width, split));
                rightDungeon = new SubDungeon(new Rect(rectangle.x, rectangle.y + split, rectangle.width, rectangle.height - split));
            }
            else
            {
                int split = Random.Range(roomSideSize, (int)(rectangle.height - roomSideSize));

                leftDungeon = new SubDungeon(new Rect(rectangle.x, rectangle.y, split, rectangle.height));
                rightDungeon = new SubDungeon(new Rect(rectangle.x + split, rectangle.y, rectangle.width - split, rectangle.height));
            }

            return true;
        }

        public void CreateRoom(int minRoomWidth, int minRoomHeight, int maxRoomWidth, int maxRoomHeight, RoomSizeRandomness randomnessLevel)
        {
            if (GameManager.Instance.generationFailed)
                return;

            if (leftDungeon != null)
            {
                leftDungeon.CreateRoom(minRoomWidth, minRoomHeight, maxRoomWidth, maxRoomHeight, randomnessLevel);
            }
            if (rightDungeon != null)
            {
                rightDungeon.CreateRoom(minRoomWidth, minRoomHeight, maxRoomWidth, maxRoomHeight, randomnessLevel);
            }
            if (leftDungeon != null && rightDungeon != null)
            {
                CreateCorridorBetween(leftDungeon, rightDungeon);
            }
            if (IAmLeaf())
            {
                int roomWidth = 0, roomHeight = 0;
                if (randomnessLevel == RoomSizeRandomness.FullRandom)
                {
                    roomWidth = (int)Random.Range(rectangle.width / 2, rectangle.width - 2);
                    roomHeight = (int)Random.Range(rectangle.height / 2, rectangle.height - 2);
                }
                else if (randomnessLevel == RoomSizeRandomness.BigRooms)
                {
                    roomWidth = (int)rectangle.width - 2;
                    roomHeight = (int)rectangle.height - 2;
                }
                else if (randomnessLevel == RoomSizeRandomness.SmallRooms)
                {
                    roomWidth = (int)rectangle.width / 2;
                    roomHeight = (int)rectangle.height / 2;
                }
                int roomX = (int)Random.Range(1, rectangle.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rectangle.height - roomHeight - 1);

                if (roomWidth < minRoomWidth || roomWidth > maxRoomWidth || roomHeight < minRoomHeight || roomHeight > maxRoomHeight)
                {
                    GameManager.Instance.generationFailed = true;
                }

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
            if (subDungeon.rectangle.width > _roomSideSize || subDungeon.rectangle.height > _roomSideSize)
            {
                if (subDungeon.Split(_roomSideSize))
                {
                    SpacePartition(subDungeon.leftDungeon);
                    SpacePartition(subDungeon.rightDungeon);
                }
            }
        }
    }
    #endregion Dungeon BSP

    #region Drawers
    public void DrawRoomWalls(SubDungeon subDungeon)
    {
        if (subDungeon == null)
        {
            return;
        }

        if (subDungeon.IAmLeaf())
        {
            //first we create holders for the rooms 
            //room prefab
            GameObject roomParent = Instantiate(_roomHolder, dungeonRoomsParent);
            DungeonRoom thisRoom = new DungeonRoom(roomParent, dungeonRoomsParent.childCount, subDungeon.room);
            _dungeonRooms.Add(thisRoom);
            //wall holder of room prefab
            Transform roomWallHolder = roomParent.transform.GetChild(2);

            //room top walls
            for (int i = (int)subDungeon.room.x + 1; i < subDungeon.room.xMax - 1; i++)
            {
                DrawWall(dungeonWalls.top, i, (int)subDungeon.room.yMax - 1, roomWallHolder, true);
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
            DrawWall(dungeonWalls.topLeftCorner, (int)subDungeon.room.x, (int)subDungeon.room.yMax - 1, roomWallHolder, true);
            DrawWall(dungeonWalls.topRightCorner, (int)subDungeon.room.xMax - 1, (int)subDungeon.room.yMax - 1, roomWallHolder, true);
            DrawWall(dungeonWalls.bottomLeftCorner, (int)subDungeon.room.x, (int)subDungeon.room.y, roomWallHolder, true);
            DrawWall(dungeonWalls.bottomRightCorner, (int)subDungeon.room.xMax - 1, (int)subDungeon.room.y, roomWallHolder, true);
        }
        else
        {
            DrawRoomWalls(subDungeon.leftDungeon);
            DrawRoomWalls(subDungeon.rightDungeon);
        }
    }


    public void DrawWall(GameObject wall, int x, int y, Transform wallParent, bool incorrectLayer = false)
    {
        GameObject instance = Instantiate(wall, new Vector3(x, y, 0f), Quaternion.identity, wallParent);
        tilesPosition[x, y] = instance;
        foreach (Transform child in tilesPosition[x, y].transform)
        {
            tilesPosition[(int)child.transform.position.x, (int)child.transform.position.y] = instance;
            _incorrectLayerWalls.Add(child.gameObject);
        }
        if (incorrectLayer)
        {
            _incorrectLayerWalls.Add(instance);
        }  
    }

    public void DefineRooms()
    {
        HashSet<int> treasureRooms = new HashSet<int>();
        //Define what room ids are going to be treasure rooms
        if (treasureRoomQuantity <= _dungeonRooms.Count)
        {
            while (treasureRoomQuantity > 0)
            {
                int randomRoomID = Random.Range(2, _dungeonRooms.Count + 1);
                if (!treasureRooms.Contains(randomRoomID))
                {
                    treasureRooms.Add(randomRoomID);
                    treasureRoomQuantity--;
                }
            }
        }
        int bottomStartRoom = GetRandomSideRoom(3);
        int topEndRoom = GetRandomSideRoom(1);

        RoomController roomComponent = null;
        foreach (DungeonRoom dungeonRoom in _dungeonRooms)
        {
            if (treasureRooms.Contains(dungeonRoom.id))
                roomComponent = dungeonRoom.room.AddComponent(typeof(TreasureRoom)) as TreasureRoom;
            else
                roomComponent = dungeonRoom.room.AddComponent(typeof(EnemiesRoom)) as EnemiesRoom;

            _dungeonRoomsComponent.Add(roomComponent);
            Rect roomFloorsRectangle = new Rect(dungeonRoom.roomRectangle.position, new Vector2(dungeonRoom.roomRectangle.width - 2f, dungeonRoom.roomRectangle.height - 4f));
            roomComponent.Initialize(this, tilesPosition, dungeonRoom.id, dungeonRoom.roomRectangle, roomFloorsRectangle);
            roomComponent.DrawRoomInteriors();
            //Set player initial position at a random floor tile of the first room
            if (dungeonRoom.id == bottomStartRoom)
            {
                GameObject initialCorridorInstance = Instantiate(initialCorridor, roomComponent.DestroyRandomSideWall(3), Quaternion.identity, roomComponent.transform.GetChild(0));
                GatewayPortal initialCorridorGateway = initialCorridorInstance.GetComponent<GatewayPortal>();
                GameObject firstRoomTile = tilesPosition[(int)initialCorridorInstance.transform.position.x, (int)initialCorridorInstance.transform.position.y + 1];

                roomComponent.isFirstRoom = true;
                firstRoomTile.tag = "Gateway";
                initialCorridorGateway.Initialize(this, 1, corridorSpeed, tilesPosition, _clsPlayerMovement.transform, _clsPlayerMovement, _clsPlayerSpriteManager);
                initialCorridorGateway.SetSimpleGateway(firstRoomTile);
                _clsPlayerMovement.gameObject.transform.position = new Vector3(initialCorridorInstance.transform.position.x, initialCorridorInstance.transform.position.y - 40);
            }
            //Set boss room in the last dungeon room
            if (dungeonRoom.id == topEndRoom)
            {
                GameObject bossRoomInstance = Instantiate(bossRoom, roomComponent.DestroyRandomSideWall(1), Quaternion.identity);
                BossRoom bossRoomComponent = bossRoomInstance.GetComponent<BossRoom>();
                GatewayPortal bossRoomGateway = bossRoomInstance.GetComponent<GatewayPortal>();

                bossRoomGateway.Initialize(this, 1, corridorSpeed, tilesPosition, _clsPlayerMovement.transform, _clsPlayerMovement, _clsPlayerSpriteManager);
                bossRoomGateway.SetSimpleGateway(bossRoomComponent.firstPortalStop);
                bossRoomComponent.Initialize(this, tilesPosition, dungeonRoom.id + 1);
                roomComponent.roomGateways.Add(bossRoomGateway);
            }
        }
    }
    
    private int GetRandomSideRoom(int side)
    {
        List<int> topRoomsList = new List<int>();
        List<int> bottomRoomsList = new List<int>();
        foreach (DungeonRoom dungeonRoom in _dungeonRooms)
        {
            bool noRoomTop = false, noRoomBot = false;

            if (Physics2D.BoxCast(new Vector2(dungeonRoom.roomRectangle.center.x, dungeonRoom.roomRectangle.yMax), new Vector2(dungeonRoom.roomRectangle.width, 1), 0f, Vector2.up).collider == null)
                noRoomTop = true;

            if (Physics2D.BoxCast(new Vector2(dungeonRoom.roomRectangle.center.x, dungeonRoom.roomRectangle.yMin), new Vector2(dungeonRoom.roomRectangle.width, 1), 0f, Vector2.down).collider == null)
                noRoomBot = true;

            if (noRoomTop == true && noRoomBot == false)
                topRoomsList.Add(dungeonRoom.id);

            if (noRoomTop == false && noRoomBot == true)
                bottomRoomsList.Add(dungeonRoom.id);
        }

        if (topRoomsList.Count == 0 || bottomRoomsList.Count == 0)
            return 1;
        if (side == 1)
            return topRoomsList[Random.Range(0, topRoomsList.Count)];
        if (side == 3)
            return bottomRoomsList[Random.Range(0, bottomRoomsList.Count)];

        return 0;
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
        DungeonCorridor corridorAux = new DungeonCorridor(new Rect(-1, -1, 0, 0), 0);

        foreach (DungeonCorridor corridor in subDungeon.corridors)
        {
            //create new holder if it's a different corridor
            if (corridor.totalCorridorId != corridorAux.totalCorridorId)
            {
                corridorParent = Instantiate(_corridorHolder, dungeonCorridorsParent);
                corridorFloorHolder = corridorParent.transform.GetChild(0);
            }
            corridorAux = corridor;

            //instantiate corridor tiles
            for (int i = (int)corridor.rect.x; i < corridor.rect.xMax; i++)
            {
                for (int j = (int)corridor.rect.y; j < corridor.rect.yMax; j++)
                {
                    //check if there isn't a room tile
                    if (tilesPosition[i, j] == null)
                    {
                        InstantiateCorridorTile(i, j, corridorFloorHolder);
                        //check if the corridor tile collides with a corner or top wall, in this case we add extra tiles so when we generate the portals the player don't have the path to it blocked

                        //corridor collision with top walls from left and right
                        int k = j;
                        while (tilesPosition[i + 2, k]?.layer == LayerMask.NameToLayer("TopWall") || tilesPosition[i - 2, k]?.layer == LayerMask.NameToLayer("TopWall"))
                        {
                            k--;
                            if (tilesPosition[i, k] == null)
                                InstantiateCorridorTile(i, k, corridorFloorHolder);
                        }
                        //corridor collision with bottom corners from left and right
                        if (tilesPosition[i + 1, j]?.layer == LayerMask.NameToLayer("BottomLeftCorner") || tilesPosition[i - 1, j]?.layer == LayerMask.NameToLayer("BottomRightCorner"))
                        {
                            if (tilesPosition[i, j + 1] == null)
                                InstantiateCorridorTile(i, j + 1, corridorFloorHolder);
                        }
                        //corridor collision with left corners from top and bottom
                        if (tilesPosition[i, j + 1]?.layer == LayerMask.NameToLayer("BottomLeftCorner") || tilesPosition[i, j - 1]?.layer == LayerMask.NameToLayer("TopLeftCorner"))
                        {
                            if (tilesPosition[i + 1, j] == null)
                                InstantiateCorridorTile(i + 1, j, corridorFloorHolder);
                        }
                        //corridor collision with right corners from top and bottom
                        if (tilesPosition[i, j + 1]?.layer == LayerMask.NameToLayer("BottomRightCorner") || tilesPosition[i, j - 1]?.layer == LayerMask.NameToLayer("TopRightCorner"))
                        {
                            if (tilesPosition[i - 1, j] == null)
                                InstantiateCorridorTile(i - 1, j, corridorFloorHolder);
                        }
                    }
                }
            }
            if (!_dungeonCorridors.Contains(corridorParent))
                _dungeonCorridors.Add(corridorParent);
        }         
    }

    public void InstantiateCorridorTile(int x, int y, Transform corridorFloorHolder)
    {
        GameObject instance = Instantiate(RandomTools.Instance.PickOne(dungeonCorridorFloors), new Vector3(x, y, 0f), Quaternion.identity, corridorFloorHolder);
        instance.tag = CorridorValidator(x, y);
        tilesPosition[x, y] = instance;
    }

    public string CorridorValidator(int x, int y)
    {
        if (tilesPosition[x + 2, y]?.layer == LayerMask.NameToLayer("TopWall") || tilesPosition[x - 2, y]?.layer == LayerMask.NameToLayer("TopWall"))
            return "Untagged";
        else
            return "ValidCorridor";
    }

    #endregion Drawers

    #region Gateways
    public void GenerateGateways()
    {
        foreach (GameObject corridor in _dungeonCorridors)
        {
            Transform corridorFloorHolder = corridor.transform.GetChild(0);
            foreach (Transform child in corridorFloorHolder)
            {
                if (child.CompareTag("ValidCorridor") == false)
                    continue;

                int x = (int)child.position.x;
                int y = (int)child.position.y;

                int neighboursCount = 0;

                if (tilesPosition[x + 1, y] != null)
                {
                    if (tilesPosition[x + 1, y].layer == 0 && tilesPosition[x + 1, y].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x + 1, y].CompareTag("ValidCorridor"))
                        neighboursCount++;
                    else if (!tilesPosition[x + 1, y].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x + 2, y] != null && ((!tilesPosition[x + 2, y].CompareTag("ValidCorridor") &&
                        tilesPosition[x + 2, y].transform.parent.parent.CompareTag("CorridorHolder")) || tilesPosition[x + 2, y].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }
                if (tilesPosition[x - 1, y] != null)
                {
                    if (tilesPosition[x - 1, y].layer == 0 && tilesPosition[x - 1, y].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x - 1, y].CompareTag("ValidCorridor"))
                        neighboursCount++;
                    else if (!tilesPosition[x - 1, y].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x - 2, y] != null && ((!tilesPosition[x - 2, y].CompareTag("ValidCorridor") &&
                        tilesPosition[x - 2, y].transform.parent.parent.CompareTag("CorridorHolder")) || tilesPosition[x - 2, y].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }

                if (tilesPosition[x, y - 1] != null)
                {
                    if (tilesPosition[x, y - 1].layer == 0 && tilesPosition[x, y - 1].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x, y - 1].CompareTag("ValidCorridor"))
                        neighboursCount++;
                    else if (!tilesPosition[x, y - 1].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x, y - 2] != null && ((!tilesPosition[x, y - 2].CompareTag("ValidCorridor") &&
                        tilesPosition[x, y - 2].transform.parent.parent.CompareTag("CorridorHolder")) || tilesPosition[x, y - 2].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }

                if (tilesPosition[x, y + 1] != null)
                {
                    if (tilesPosition[x, y + 1].layer == 0 && tilesPosition[x, y + 1].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x, y + 1].CompareTag("ValidCorridor"))
                        neighboursCount++;
                    else if (!tilesPosition[x, y + 1].CompareTag("ValidCorridor") && tilesPosition[x, y + 4] != null && tilesPosition[x, y + 4].transform.IsChildOf(corridorFloorHolder))
                        neighboursCount++;
                    else if (!tilesPosition[x, y + 1].transform.IsChildOf(corridorFloorHolder) && tilesPosition[x, y + 2] != null && ((!tilesPosition[x, y + 2].CompareTag("ValidCorridor") &&
                        tilesPosition[x, y + 2].transform.parent.parent.CompareTag("CorridorHolder")) || tilesPosition[x, y + 2].transform.IsChildOf(corridorFloorHolder)))
                        neighboursCount++;
                }

                //If the neighbours are less than two it means this is the corridor tile that generates the gateway since it's the last one
                if (neighboursCount < 2)
                {
                    //Check the four sides to see if it's a room wall
                    InstantiateGateway(x - 1, y, 0);
                    InstantiateGateway(x, y - 1, 1);
                    InstantiateGateway(x + 1, y, 2);
                    InstantiateGateway(x, y + 1, 3);
                }
            }
        }
        //After all corridors and gateways are instantiated all walls will have the same layer "Obstacle" since we don't need the previous layers anymore
        foreach (GameObject wall in _incorrectLayerWalls)
            if (wall.layer != LayerMask.NameToLayer("Default"))
                wall.layer = LayerMask.NameToLayer("Obstacle");

        //Get each room gateways
        foreach (RoomController room in _dungeonRoomsComponent)
            room.GetGateways();
    }

    private void InstantiateGateway(int x, int y, int firstDirection)
    {
        if (tilesPosition[x, y] != null && (tilesPosition[x, y].layer == LayerMask.NameToLayer("Obstacle") || tilesPosition[x, y].layer == LayerMask.NameToLayer("TopWall")))
        {
            //Instantiate floors in child positions since a top wall may be destroyed
            foreach (Transform child in tilesPosition[x, y].transform)
            {
                int posX = (int)child.transform.position.x;
                int posY = (int)child.transform.position.y;
                tilesPosition[posX, posY] =
                    Instantiate(RandomTools.Instance.PickOne(dungeonRoomFloors), new Vector3(posX, posY, 0f), Quaternion.identity, tilesPosition[posX, posY].transform.parent.parent.GetChild(1));
            }
            Destroy(tilesPosition[x, y]);
            GameObject gateway = Instantiate(dungeonGateway, new Vector3(x, y, 0f), Quaternion.identity, tilesPosition[x, y].transform.parent.parent.GetChild(0));
            tilesPosition[x, y] = gateway;
            gateway.GetComponent<GatewayPortal>().Initialize(this, firstDirection, corridorSpeed, tilesPosition, _clsPlayerMovement.transform, _clsPlayerMovement, _clsPlayerSpriteManager);
        }
    }
    #endregion Gateways

    #region Post-BSP Methods

    public void DecorateBigWalls()
    {
        GameObject objectToInstantiate;
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("WallDeco"))
        {
            objectToInstantiate = RandomTools.Instance.PickOne(dungeonWallDecos);
            if (objectToInstantiate != null)
            {
                Instantiate(objectToInstantiate, wall.transform);
            }
        }
    }

    public void SpawnPlayer()
    {
        GameObject playerInstance = Instantiate(player);
        dungeonCamera.GetComponent<CameraFollow>().player = playerInstance;
        _clsPlayerSpriteManager = playerInstance.GetComponent<PlayerSpriteManager>();
        _clsPlayerMovement = playerInstance.GetComponent<PlayerMovement>();
    }

    #endregion Post-BSP Methods

    void Start()
    {
        _roomSideSize = Mathf.CeilToInt(Mathf.Sqrt(averageMinRoomTiles));
        _roomHolder = Resources.Load<GameObject>("Room");
        _corridorHolder = Resources.Load<GameObject>("Corridor");
        dungeonRoomFloors = RandomTools.Instance.CreateWeightedObjectsArray(dungeonRoomFloors);
        dungeonWallDecos = RandomTools.Instance.CreateWeightedObjectsArray(dungeonWallDecos);
        dungeonCorridorFloors = RandomTools.Instance.CreateWeightedObjectsArray(dungeonCorridorFloors);

        do
        {
            GameManager.Instance.generationFailed = false;
            SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, boardRows, boardColumns));
            tilesPosition = new GameObject[boardRows, boardColumns];
            SpacePartition(rootSubDungeon);
            rootSubDungeon.CreateRoom(minRoomWidth, minRoomHeight, maxRoomWidth, maxRoomHeight, roomSizeRandomness);
            if (GameManager.Instance.generationFailed == false)
            {
                DrawRoomWalls(rootSubDungeon);
                SpawnPlayer();
                DefineRooms();
                DrawCorridors(rootSubDungeon);
                GenerateGateways();
                DecorateBigWalls();
                GameManager.Instance.InitializeDungeon(this);
                GameManager.Instance.ManageLoadingScreen(false);
            }
            
            else
            {
                Debug.Log("Generation Failed");
            }
            

        } while (GameManager.Instance.generationFailed);
    }
}