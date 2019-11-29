using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayPortal : MonoBehaviour
{
    private Transform _player;
    private PlayerMovement _clsPlayerMovement;
    private PlayerSpriteManager _clsPlayerSpriteManager;
    private DungeonController _clsDungeonController;
    private float _speed;
    private float distanceBetweenNextFloor;
    private GameObject targetFloor;
    [System.NonSerialized]
    private int _firstDirection;
    private int currentDirection;
    public int currentTotalNeighbours;
    private bool destinyReached;
    private bool choosingDirection;
    private bool _directionSelectorChanged;
    public GameObject[] availableNeighbours;
    private GameObject[,] _tiles;

    public void Initialize(DungeonController dungeonController, int firstDirection, float speed, GameObject[,] tiles, Transform player, PlayerMovement playerMovement, PlayerSpriteManager playerSpriteManager)
    {
        _clsDungeonController = dungeonController;
        _clsPlayerMovement = playerMovement;
        _clsPlayerSpriteManager = playerSpriteManager;
        _tiles = tiles;
        _firstDirection = firstDirection;
        _speed = speed;
        _player = player;
    }

    private void SetTargetFloor(GameObject targetFloor)
    {
        this.targetFloor = targetFloor;
    }

    private void FixedUpdate()
    {
        if (!choosingDirection)
        {
            distanceBetweenNextFloor = Vector2.Distance(_player.position, targetFloor.transform.position);
            Vector2 newPosition = Vector2.MoveTowards(_player.position, targetFloor.transform.position, _speed * Time.deltaTime);
            _clsPlayerMovement.objRigidbody.MovePosition(newPosition);
            if (distanceBetweenNextFloor == 0f)
            {
                if (destinyReached)
                {
                    enabled = false;
                    ManagePlayerStatus(true);
                }
                if (targetFloor.tag == "Gateway" && (targetFloor != gameObject || currentDirection != _firstDirection))
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
                if (SimpleInput.GetAxisRaw("Vertical") < -0.5f && availableNeighbours[0] != null)
                    SelectDirection(0);
                if (SimpleInput.GetAxisRaw("Horizontal") > 0.5f && availableNeighbours[1] != null)
                    SelectDirection(1);
                if (SimpleInput.GetAxisRaw("Vertical") > 0.5f && availableNeighbours[2] != null)
                    SelectDirection(2);
                if (SimpleInput.GetAxisRaw("Horizontal") < -0.5f && availableNeighbours[3] != null)
                    SelectDirection(3);
            }
            //this occurs when the directionSelector combination is repeated, so the player don't have to insert inputs per each tile that are adjacents
            else
            {
                choosingDirection = false;
                _clsPlayerSpriteManager.directionSelector.SetActive(false);
                SetTargetFloor(availableNeighbours[currentDirection]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && _clsPlayerMovement.canMove)
        {
            currentDirection = _firstDirection;
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

        if (_tiles[x, y - 1] != null && _tiles[x, y - 1].layer != LayerMask.NameToLayer("Obstacle") && direction != 2)
            AddNeighbour(0, x, y - 1);
        if (_tiles[x + 1, y] != null && _tiles[x + 1, y].layer != LayerMask.NameToLayer("Obstacle") && direction != 3)
            AddNeighbour(1, x + 1, y);
        if (_tiles[x, y + 1] != null && _tiles[x, y + 1].layer != LayerMask.NameToLayer("Obstacle") && direction != 0)
            AddNeighbour(2, x, y + 1);
        if (_tiles[x - 1, y] != null && _tiles[x - 1, y].layer != LayerMask.NameToLayer("Obstacle") && direction != 1)
            AddNeighbour(3, x - 1, y);

        //in case the path is only one
        if (currentTotalNeighbours < 2)
        {
            LoopSides(true);
            _directionSelectorChanged = true;
        }    
        //in case you have more than one path but the other paths are gateways
        else if (currentTotalNeighbours >= 2 && targetFloor == gameObject)
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
        _clsPlayerSpriteManager.directionSelector.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            if (availableNeighbours[i] != null && !_clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.activeSelf)
            {
                _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(true);
                _directionSelectorChanged = true;
            }
            else if(availableNeighbours[i] == null && _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.activeSelf)
            {
                _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(false);
                _directionSelectorChanged = true;
            }
                
        }
        choosingDirection = true;
    }

    private void ResetDirectionSelector()
    {
        for (int i = 0; i < 4; i++)
        {
            _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SelectDirection(int direction)
    {
        currentDirection = direction;
        SetTargetFloor(availableNeighbours[direction]);
        choosingDirection = false;
        _clsPlayerSpriteManager.directionSelector.SetActive(false);
        _directionSelectorChanged = false;
    }

    private void AddNeighbour(int direction, int x, int y)
    {
        currentTotalNeighbours++;
        availableNeighbours[direction] = _tiles[x, y];
    }

    private void LoopSides(bool ignoreGatewayCheck)
    {
        for (int i = 0; i < 4; i++)
        {
            if (availableNeighbours[i] != null && (ignoreGatewayCheck || availableNeighbours[i].CompareTag("Gateway") == false))
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
        _clsPlayerMovement.canMove = state;
        _clsPlayerMovement.objRigidbody.velocity = Vector2.zero;
        _clsPlayerSpriteManager.corridorParticles.SetActive(!state);
        if (state == false)
        {
            _clsPlayerSpriteManager.animator.enabled = false;
            _clsPlayerSpriteManager.sprRender.sprite = _clsDungeonController.playerSpriteInCorridor;
        }
        else
            _clsPlayerSpriteManager.UpdateSpriteDirection(currentDirection);
    }

}