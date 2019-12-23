using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayPortal : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Sprite disabledSprite;
    public Animator animator;

    private GameObject _targetFloor;
    private Transform _player;
    private PlayerMovement _clsPlayerMovement;
    private PlayerSpriteManager _clsPlayerSpriteManager;
    private DungeonController _clsDungeonController;
    private GameObject _firstBossFloor;
    private GameObject _secondBossFloor;
    private GameObject gatewayReached;
    private GameObject[] _availableNeighbours;
    private GameObject[,] _tiles;
    private GameObject _corridorParticlesInstance;
    private float _speed;
    private float _distanceBetweenNextFloor;
    private bool _isBossGateway;
    private int _firstDirection;
    private int _currentDirection;
    private int _currentTotalNeighbours;
    private bool _destinyReached;
    private bool _choosingDirection;
    private bool _directionSelectorChanged;
    

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

    public void SetBossRoom(GameObject firstBossFloor, GameObject secondBossFloor)
    {
        _firstBossFloor = firstBossFloor;
        _secondBossFloor = secondBossFloor;
        _isBossGateway = true;
    }

    private void SetTargetFloor(GameObject targetFloor)
    {
        _targetFloor = targetFloor;
    }

    private void FixedUpdate()
    {
        if (!_choosingDirection)
        {
            _distanceBetweenNextFloor = Vector2.Distance(_player.position, _targetFloor.transform.position);
            Vector2 newPosition = Vector2.MoveTowards(_player.position, _targetFloor.transform.position, _speed * Time.deltaTime);
            _clsPlayerMovement.objRigidbody.MovePosition(newPosition);
            if (_distanceBetweenNextFloor == 0f)
            {
                if (_destinyReached)
                {
                    enabled = false;
                    ManagePlayerStatus(true);
                    RoomController RoomComponent;
                    if (!_isBossGateway)
                    {
                        RoomComponent = gatewayReached.transform.parent.parent.GetComponent<RoomController>();
                    }
                    else 
                    {
                        RoomComponent = gameObject.GetComponent<BossRoom>();
                    }
                    RoomComponent.ActivateRoom();
                }
                if (_targetFloor.tag == "Gateway" && (_targetFloor != gameObject || _currentDirection != _firstDirection))
                {
                    _destinyReached = true;
                    gatewayReached = _targetFloor;
                }
                if (!_isBossGateway)
                {
                    CheckNeighbours((int)_targetFloor.transform.position.x, (int)_targetFloor.transform.position.y, _currentDirection);
                }
                else
                {
                    if (_targetFloor == gameObject)
                        SetTargetFloor(_firstBossFloor);
                    else
                        SetTargetFloor(_secondBossFloor);
                }
            }
        }
    }

    private void Update()
    {
        if (_choosingDirection)
        {
            if (_directionSelectorChanged)
            {
                if (SimpleInput.GetAxisRaw("Horizontal") > 0.5f && _availableNeighbours[0] != null)
                    SelectDirection(0);
                if (SimpleInput.GetAxisRaw("Vertical") > 0.5f && _availableNeighbours[1] != null)
                    SelectDirection(1);
                if (SimpleInput.GetAxisRaw("Horizontal") < -0.5f && _availableNeighbours[2] != null)
                    SelectDirection(2);
                if (SimpleInput.GetAxisRaw("Vertical") < -0.5f && _availableNeighbours[3] != null)
                    SelectDirection(3);
            }
            //this occurs when the directionSelector combination is repeated, so the player don't have to insert inputs per each tile that are adjacents
            else
            {
                _choosingDirection = false;
                _clsPlayerSpriteManager.directionSelector.SetActive(false);
                SetTargetFloor(_availableNeighbours[_currentDirection]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && _clsPlayerMovement.canMove)
        { 
            SetTargetFloor(gameObject);
            _currentDirection = _firstDirection;
            _destinyReached = false;
            _choosingDirection = false;
            ResetDirectionSelector();
            enabled = true;
            ManagePlayerStatus(false);
        }
    }

    private void CheckNeighbours(int x, int y, int direction)
    {
        _currentTotalNeighbours = 0;
        _availableNeighbours = new GameObject[4];

        if (_tiles[x - 1, y] != null && _tiles[x - 1, y].layer != LayerMask.NameToLayer("Obstacle") && direction != 0)
            AddNeighbour(2, x - 1, y);
        if (_tiles[x, y - 1] != null && _tiles[x, y - 1].layer != LayerMask.NameToLayer("Obstacle") && direction != 1)
            AddNeighbour(3, x, y - 1);
        if (_tiles[x + 1, y] != null && _tiles[x + 1, y].layer != LayerMask.NameToLayer("Obstacle") && direction != 2)
            AddNeighbour(0, x + 1, y);
        if (_tiles[x, y + 1] != null && _tiles[x, y + 1].layer != LayerMask.NameToLayer("Obstacle") && direction != 3)
            AddNeighbour(1, x, y + 1);

        //in case the path is only one
        if (_currentTotalNeighbours < 2)
        {
            LoopSides(true);
            _directionSelectorChanged = true;
        }    
        //in case you have more than one path but the other paths are gateways
        else if (_currentTotalNeighbours >= 2 && _targetFloor == gameObject)
            LoopSides(false);
        //in case you have more than one path
        else
        {
            if (!_destinyReached)
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
            if (_availableNeighbours[i] != null && !_clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.activeSelf)
            {
                _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(true);
                _directionSelectorChanged = true;
            }
            else if(_availableNeighbours[i] == null && _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.activeSelf)
            {
                _clsPlayerSpriteManager.directionSelector.transform.GetChild(i).gameObject.SetActive(false);
                _directionSelectorChanged = true;
            }
                
        }
        _choosingDirection = true;
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
        _currentDirection = direction;
        SetTargetFloor(_availableNeighbours[direction]);
        _choosingDirection = false;
        _clsPlayerSpriteManager.directionSelector.SetActive(false);
        _directionSelectorChanged = false;
    }

    private void AddNeighbour(int direction, int x, int y)
    {
        _currentTotalNeighbours++;
        _availableNeighbours[direction] = _tiles[x, y];
    }

    private void LoopSides(bool ignoreGatewayCheck)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_availableNeighbours[i] != null && (ignoreGatewayCheck || _availableNeighbours[i].CompareTag("Gateway") == false))
            {
                if (!_destinyReached)
                {
                    _currentDirection = i;
                }
                SetTargetFloor(_availableNeighbours[i]);
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
        _clsPlayerSpriteManager.sprRender.enabled = state;

        if (state == false)
        {           
            _corridorParticlesInstance = Instantiate(_clsDungeonController.playerCorridorParticles, _player);
            _clsPlayerSpriteManager.animator.enabled = false;
        }  
        else
        {
            Destroy(_corridorParticlesInstance);
            _clsPlayerSpriteManager.UpdateSpriteDirection(_currentDirection);
        }
    }
}
