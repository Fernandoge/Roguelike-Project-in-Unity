﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonController : MonoBehaviour {
    [Header("Config")]
    public int boardRows;
    public int boardColumns;
    public int minRoomSize,maxRoomSize;
    [Header("Assets")]
    public GameObject dungeonRoomFloor;
    public GameObject dungeonWalls;
    public GameObject dungeonCorridorFloor;
    public GameObject dungeonGateway;
    [Header ("Holders")]
    public GameObject roomHolder;
    public GameObject corridorHolder;
    [Header("Gameplay Info")]
    public int currentRoom;
    public int roomsCompleted;

    public GameObject[,] dungeonFloorsPosition;
    public GameObject[,] dungeonWallsPosition;

	public class SubDungeon {
		public SubDungeon leftDungeon, rightDungeon;
		public Rect rectangle;
		public Rect room = new Rect(-1,-1, 0, 0);
		public List<Rect> corridors = new List<Rect>();


		public SubDungeon(Rect mrect) {
			rectangle = mrect;
		}

		public bool IAmLeaf() {
			return leftDungeon == null && rightDungeon == null;
		}

		public bool Split(int minRoomSize, int maxRoomSize) {
			if (!IAmLeaf()) {
				return false;
			}

			bool splitHorizontal;
			if (rectangle.width / rectangle.height >= 1.25) {
				splitHorizontal = false;
			} else if (rectangle.height / rectangle.width >= 1.25) {
				splitHorizontal = true;
			} else {
				splitHorizontal = Random.Range (0.0f, 1.0f) > 0.5;
			}

			if (Mathf.Min(rectangle.height, rectangle.width) / 2 < minRoomSize) {
				return false;
			}

			if (splitHorizontal) {
				// split so that the resulting sub-dungeons widths are not too small
				// (since we are splitting horizontally) 
				int split = Random.Range (minRoomSize, (int)(rectangle.width - minRoomSize));

				leftDungeon = new SubDungeon (new Rect (rectangle.x, rectangle.y, rectangle.width, split));
				rightDungeon = new SubDungeon (new Rect (rectangle.x, rectangle.y + split, rectangle.width, rectangle.height - split));
			}
			else {
				int split = Random.Range (minRoomSize, (int)(rectangle.height - minRoomSize));

				leftDungeon = new SubDungeon (new Rect (rectangle.x, rectangle.y, split, rectangle.height));
				rightDungeon = new SubDungeon (new Rect (rectangle.x + split, rectangle.y, rectangle.width - split, rectangle.height));
			}

			return true;
		}

		public void CreateRoom() {
			if (leftDungeon != null) {
				leftDungeon.CreateRoom ();
			}
			if (rightDungeon != null) {
				rightDungeon.CreateRoom ();
			}
			if (leftDungeon != null && rightDungeon != null) {
				CreateCorridorBetween(leftDungeon, rightDungeon);
			}
			if (IAmLeaf()) {
				int roomWidth = (int)Random.Range (rectangle.width / 2, rectangle.width - 2);
				int roomHeight = (int)Random.Range (rectangle.height / 2, rectangle.height - 2);
				int roomX = (int)Random.Range (1, rectangle.width - roomWidth - 1);
				int roomY = (int)Random.Range (1, rectangle.height - roomHeight - 1);

				// room position will be absolute in the board, not relative to the sub-dungeon
				room = new Rect (rectangle.x + roomX, rectangle.y + roomY, roomWidth, roomHeight);
			}
		}


		public void CreateCorridorBetween(SubDungeon left, SubDungeon right) {
			Rect lroom = left.GetRoom ();
			Rect rroom = right.GetRoom ();

			// attach the corridor to a random point in each room
			Vector2 lpoint = new Vector2 ((int)Random.Range (lroom.x, lroom.xMax), (int)Random.Range (lroom.y, lroom.yMax));
			Vector2 rpoint = new Vector2 ((int)Random.Range (rroom.x, rroom.xMax), (int)Random.Range (rroom.y, rroom.yMax));

			// Be sure that left point is on the left to simplify the code
			if (lpoint.x > rpoint.x) {
				Vector2 temp = lpoint;
				lpoint = rpoint;
				rpoint = temp;
			}
				
			int w = (int)(lpoint.x - rpoint.x);
			int h = (int)(lpoint.y - rpoint.y);

			// if the points are not aligned horizontally
			if (w != 0) { 
				// choose at random to go horizontal then vertical or the opposite
				if (Random.Range (0, 1) > 2) {
					// add a corridor to the right
					corridors.Add (new Rect (lpoint.x, lpoint.y, Mathf.Abs (w) + 1, 1));

					// if left point is below right point go up
					// otherwise go down
					if (h < 0) { 
						corridors.Add (new Rect (rpoint.x, lpoint.y, 1, Mathf.Abs (h)));
					} else {
						corridors.Add (new Rect (rpoint.x, lpoint.y, 1, -Mathf.Abs (h)));
					}
				} else {
					// go up or down
					if (h < 0) {
						corridors.Add (new Rect (lpoint.x, lpoint.y, 1, Mathf.Abs (h)));
					} else {
						corridors.Add (new Rect (lpoint.x, rpoint.y, 1, Mathf.Abs (h)));
					}

					// then go right
					corridors.Add (new Rect (lpoint.x, rpoint.y, Mathf.Abs (w) + 1, 1));
				}
			} else {
				// if the points are aligned horizontally
				// go up or down depending on the positions
				if (h < 0) {
					corridors.Add (new Rect ((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs (h)));
				} else {
					corridors.Add (new Rect ((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs (h)));
				}
			} 
		}

		public Rect GetRoom() {
			if (IAmLeaf()) {
				return room;
			}
			if (leftDungeon != null) {
				Rect lroom = leftDungeon.GetRoom ();
				if (lroom.x != -1) {
					return lroom;
				}
			}
			if (rightDungeon != null) {
				Rect rroom = rightDungeon.GetRoom ();
				if (rroom.x != -1) {
					return rroom;
				}	
			}

			// workaround non nullable structs
			return new Rect (-1, -1, 0, 0);
		}
	}

	public void SpacePartition(SubDungeon subDungeon) {
		if (subDungeon.IAmLeaf()) {
			//split subDungeon if it's large
			if (subDungeon.rectangle.width > maxRoomSize || subDungeon.rectangle.height > maxRoomSize || Random.Range(0.0f, 1.0f) > 0.25)
            {
				if (subDungeon.Split (minRoomSize, maxRoomSize)) {
					SpacePartition(subDungeon.leftDungeon);
					SpacePartition(subDungeon.rightDungeon);
				}
			}
		}
	}

	public void DrawRooms(SubDungeon subDungeon) {
		if (subDungeon == null) {
			return;
		}

        if (subDungeon.IAmLeaf())
        {
            //first we create holders for the rooms 
            //room prefab
            GameObject roomParent = Instantiate(roomHolder, transform.GetChild(0));
            //Room controller
            RoomController clsRoomController = roomParent.GetComponent<RoomController>();
            clsRoomController.roomRectangle = subDungeon.room;
            clsRoomController.id = transform.GetChild(0).childCount;
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
                    instance = Instantiate(dungeonRoomFloor, floorPosition, Quaternion.identity, roomFloorHolder);
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
                x = subDungeon.room.xMax - subDungeon.room.xMin,
                y = subDungeon.room.yMax - subDungeon.room.yMin
            };
            roomCollider.size = newColliderSize;

            //add walls to the room
            Transform roomWallHolder = roomParent.transform.GetChild(2);
            //room bottom walls
            for (int i = (int)subDungeon.room.x - 1; i < subDungeon.room.xMax + 1; i++)
            {
                floorPosition = new Vector3(i, (int)subDungeon.room.y - 1, 0f);
                instance = Instantiate(dungeonWalls, floorPosition, Quaternion.identity, roomWallHolder);
                dungeonWallsPosition[i, (int)subDungeon.room.y - 1] = instance;
                dungeonFloors.Add(instance.transform);
            }         
            //room upper walls
            for (int i = (int)subDungeon.room.x - 1; i < subDungeon.room.xMax + 1; i++)
            {
                floorPosition = new Vector3(i, (int)subDungeon.room.yMax, 0f);
                instance = Instantiate(dungeonWalls, floorPosition, Quaternion.identity, roomWallHolder);
                dungeonWallsPosition[i, (int)subDungeon.room.yMax] = instance;
                dungeonFloors.Add(instance.transform);
            }
            //room left walls, two iterations less because upper and lower tile have already been instantiated
            for (int i = (int)subDungeon.room.y; i < subDungeon.room.yMax; i++)
            {
                floorPosition = new Vector3((int)subDungeon.room.x - 1, i, 0f);
                instance = Instantiate(dungeonWalls, floorPosition, Quaternion.identity, roomWallHolder);
                dungeonWallsPosition[(int)subDungeon.room.x - 1, i] = instance;
                dungeonFloors.Add(instance.transform);
            }
            //room right walls, two iterations less because upper and lower tile have already been instantiated
            for (int i = (int)subDungeon.room.y; i < subDungeon.room.yMax; i++)
            {
                floorPosition = new Vector3((int)subDungeon.room.xMax, i, 0f);
                instance = Instantiate(dungeonWalls, floorPosition, Quaternion.identity, roomWallHolder);
                dungeonWallsPosition[(int)subDungeon.room.xMax, i] = instance;
                dungeonFloors.Add(instance.transform);
            }
            

		}
        else
        {
			DrawRooms (subDungeon.leftDungeon);
			DrawRooms (subDungeon.rightDungeon);
		}
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
        Transform corridorWallHolder = null;

        foreach (Rect corridor in subDungeon.corridors)
        {

            corridorParent = Instantiate(corridorHolder, transform.GetChild(1));
            corridorFloorHolder = corridorParent.transform.GetChild(0);
            corridorWallHolder = corridorParent.transform.GetChild(1);
            List<Transform> corridorFloors = new List<Transform>();
            Vector3 newCorridorPosition = new Vector3(0f, 0f, 0f);
            Vector3 corridorFloorPosition = new Vector3(0f, 0f, 0f);

            //instantiate corridor tiles
            for (int i = (int)corridor.x; i < corridor.xMax; i++)
            {
                for (int j = (int)corridor.y; j < corridor.yMax; j++)
                {
                    //check if there isn't a room tile
                    if (dungeonFloorsPosition[i, j] == null)
                    {
                        corridorFloorPosition = new Vector3(i, j, 0f);
                        //in case the corridor tile spawns on top of a wall tile, destroy the wall
                        if (dungeonWallsPosition[i, j] != null)
                        {
                            //destroying a room wall instantiates a gateway
                            if (dungeonWallsPosition[i, j].transform.parent.parent.tag == "DungeonRoom")
                            {
                                Transform roomGatewayHolder = dungeonWallsPosition[i, j].transform.parent.parent.GetChild(0);
                                Instantiate(dungeonGateway, corridorFloorPosition, Quaternion.identity, roomGatewayHolder);
                            }
                            Destroy(dungeonWallsPosition[i, j]);
                        }
                        //instantiate corridor tile
                        GameObject instance = Instantiate(dungeonCorridorFloor, corridorFloorPosition, Quaternion.identity, corridorFloorHolder);
                        dungeonFloorsPosition[i, j] = instance;
                        corridorFloors.Add(instance.transform);
                        newCorridorPosition += corridorFloorPosition;

                    }
                }
            }

            if (corridorFloorHolder.childCount == 0)
                Destroy(corridorParent);
            else
            {
                //revert the floor positions since we moved their parent
                newCorridorPosition /= corridorFloorHolder.childCount;
                corridorParent.transform.position = newCorridorPosition;
                foreach (Transform floor in corridorFloors)
                {
                    floor.position -= newCorridorPosition;
                    //instantiate corridor walls
                    for (int i = (int)floor.position.x - 1; i < (int)floor.position.x + 2; i++)
                    {
                        for (int j = (int)floor.position.y - 1; j < (int)floor.position.y + 2; j++)
                        {
                            if (dungeonFloorsPosition[i, j] == null && dungeonWallsPosition[i, j] == null)
                            {
                                GameObject instance = Instantiate(dungeonWalls, new Vector3(i, j, 0f), Quaternion.identity, corridorWallHolder);
                                dungeonWallsPosition[i, j] = instance;
                            }
                        }
                    }
                }
            }
        }
    }

    void Start() {

		SubDungeon rootSubDungeon = new SubDungeon (new Rect (0, 0, boardRows, boardColumns));
		SpacePartition (rootSubDungeon);
		rootSubDungeon.CreateRoom ();
		dungeonFloorsPosition = new GameObject[boardRows, boardColumns];
		dungeonWallsPosition = new GameObject[boardRows + 1, boardColumns + 1];
		DrawRooms (rootSubDungeon);
		DrawCorridors (rootSubDungeon);

	}
}
