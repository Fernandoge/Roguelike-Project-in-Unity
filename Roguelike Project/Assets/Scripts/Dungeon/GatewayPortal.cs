using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayPortal : MonoBehaviour
{
    private Transform player;
    private PlayerMovement clsPlayerMovement;
    private PlayerSpriteManager clsPlayerSpriteManager;
    private DungeonController clsDungeonController;
    private float speed;
    private float distanceBetweenNextFloor;
    private GameObject targetFloor;
    [System.NonSerialized]
    public int firstDirection;
    private int currentDirection;
    private int currentTotalNeighbours;
    private bool destinyReached;
    private bool choosingDirection;
    private bool _directionSelectorChanged;
    private GameObject[] availableNeighbours;
    private GameObject[,] floors;
    private GameObject[,] walls;

    private void SetTargetFloor(GameObject targetFloor)
    {
        this.targetFloor = targetFloor;
    }

    void Awake()
    {
        clsDungeonController = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<DungeonController>();
        floors = clsDungeonController.dungeonFloorsPosition;
        walls = clsDungeonController.dungeonWallsPosition;
        speed = clsDungeonController.corridorSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        clsPlayerSpriteManager = player.parent.GetChild(1).GetComponent<PlayerSpriteManager>();
        clsPlayerMovement = player.gameObject.GetComponent<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        if (!choosingDirection)
        {
            distanceBetweenNextFloor = Vector2.Distance(player.position, targetFloor.transform.position); 
            player.position = Vector2.MoveTowards(player.position, targetFloor.transform.position, speed * Time.deltaTime);
            if (distanceBetweenNextFloor == 0f)
            {
                if (destinyReached)
                {
                    enabled = false;
                    ManagePlayerStatus(true);
                }
                if (targetFloor.tag == "Gateway")
                {
                    destinyReached = true;
                }
                CheckNeighbours((int)targetFloor.transform.position.x, (int)targetFloor.transform.position.y, currentDirection);
            }
        }
    }

    private void Update()
    {
        if (choosingDirection)
        {
            if (_directionSelectorChanged)
            {
                //if (Input.GetAxisRaw("Vertical") < 0.5f && availableNeighbours[0] != null)
                if (Input.GetKeyDown(KeyCode.S) && availableNeighbours[0] != null)
                    SelectDirection(0);
                if (Input.GetKeyDown(KeyCode.D) && availableNeighbours[1] != null)
                    SelectDirection(1);
                if (Input.GetKeyDown(KeyCode.W) && availableNeighbours[2] != null)
                    SelectDirection(2);
                if (Input.GetKeyDown(KeyCode.A) && availableNeighbours[3] != null)
                    SelectDirection(3);
            }
            //this occurs when the directionSelector combination is repeated, so the player don't have to insert inputs per each tile that are adjacents
            else
            {
                choosingDirection = false;
                clsPlayerSpriteManager.directionSelector.SetActive(false);
                SetTargetFloor(availableNeighbours[currentDirection]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && clsPlayerMovement.canMove)
        {
            currentDirection = firstDirection;
            SetTargetFloor(gameObject);
            destinyReached = false;
            choosingDirection = false;
            ResetDirectionSelector();
            enabled = true;
            ManagePlayerStatus(false);
        }
    }

    private void CheckNeighbours(int x, int y, int direction)
    {
        currentTotalNeighbours = 0;
        availableNeighbours = new GameObject[4];

        if (floors[x, y - 1] != null && walls[x, y - 1] == null && direction != 2)
            AddNeighbour(0, x, y - 1);
        if (floors[x + 1, y] != null && walls[x + 1, y] == null && direction != 3)
            AddNeighbour(1, x + 1, y);
        if (floors[x, y + 1] != null && walls[x, y + 1] == null && direction != 0)
            AddNeighbour(2, x, y + 1);
        if (floors[x - 1, y] != null && walls[x - 1, y] == null && direction != 1)
            AddNeighbour(3, x - 1, y);

        //in case the path is only one
        if (currentTotalNeighbours == 1)
        {
            LoopSides(true);
            _directionSelectorChanged = true;
        }    
        //in case you have more than one path but the other paths are gateways
        else if (currentTotalNeighbours > 1 && targetFloor == gameObject)
            LoopSides(false);
        //in case you have more than one path
        else
        {
            if (!destinyReached)
                DirectionSelector();
            else
                //for the extra step to the room when you reach a gateway
                LoopSides(false);
        }  
    }

    private void DirectionSelector()
    {
        clsPlayerSpriteManager.directionSelector.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (availableNeighbours[i] != null && !clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.activeSelf)
            {
                clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(true);
                _directionSelectorChanged = true;
            }
            else if(availableNeighbours[i] == null && clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.activeSelf)
            {
                clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(false);
                _directionSelectorChanged = true;
            }
                
        }
        choosingDirection = true;
    }

    private void ResetDirectionSelector()
    {
        for (int i = 0; i < 4; i++)
        {
            clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SelectDirection(int direction)
    {
        currentDirection = direction;
        SetTargetFloor(availableNeighbours[direction]);
        choosingDirection = false;
        clsPlayerSpriteManager.directionSelector.SetActive(false);
        _directionSelectorChanged = false;
    }

    private void AddNeighbour(int direction, int x, int y)
    {
        currentTotalNeighbours++;
        availableNeighbours[direction] = floors[x, y];
    }

    private void LoopSides(bool ignoreGatewayCheck)
    {
        for (int i = 0; i < 4; i++)
        {
            if (availableNeighbours[i] != null && (ignoreGatewayCheck || availableNeighbours[i].tag != "Gateway"))
            {
                currentDirection = i;
                SetTargetFloor(availableNeighbours[i]);
            }
        }
    }

    /// <summary>
    /// Set player movement and sprite manager variables on and off when it enters a corridor.
    /// Set state to false if the player enters a corridor and true if the corridor phase finish.
    /// </summary>
    private void ManagePlayerStatus(bool state)
    {
        clsPlayerMovement.canMove = state;
        clsPlayerMovement.myRigidbody.velocity = Vector2.zero;
        clsPlayerSpriteManager.corridorParticles.SetActive(!state);
        if (state == false)
        {
            clsPlayerSpriteManager.animator.enabled = state;
            clsPlayerSpriteManager.sprRender.sprite = clsDungeonController.playerSpriteInCorridor;
        }    
    }

}