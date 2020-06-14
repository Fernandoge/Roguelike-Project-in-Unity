using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerController : MovingObject
{
    public GameObject spellShooter;
    public GameObject spell;
    public LineRenderer spellLine;

    private Camera _mainCamera;
    
    protected override void Start()
    {
        GameManager.Instance.player = this;
        _mainCamera = Camera.main;
        base.Start();
    }

    private void Update()
    {
        if (canMove)
            AttemptMove();

#if UNITY_EDITOR
        PlayerInputDebug();
#else
        PlayerInput();
#endif
    }

    private void PlayerInputDebug()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     spellLine.enabled = true;
        // }
        //
        // if (Input.GetMouseButton(0))
        // {
        //     DrawSpellLine(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
        // }
        
        if (Input.GetMouseButtonUp(0))
        {
            spellLine.enabled = false;
            Shoot(Input.mousePosition);
        }
        
        if (Input.GetKey(KeyCode.G))
            print(LayerMask.LayerToName(GameManager.Instance.tilesLayers[(int)transform.position.x, (int)transform.position.y]));
    }

    private void PlayerInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began: 
                    spellLine.enabled = true;
                    break;
                case TouchPhase.Stationary: 
                case TouchPhase.Moved:
                    DrawSpellLine(_mainCamera.ScreenToWorldPoint(touch.position));
                    break;
                case TouchPhase.Ended:
                    spellLine.enabled = false;
                    Shoot(Input.mousePosition);
                    break;
            }
        }
    }

    private void DrawSpellLine(Vector3 tapPosition)
    {
        RaycastHit2D spellLineHit = Physics2D.Linecast(transform.position, tapPosition);
        Vector3 lineEndPosition = spellLineHit.collider == null ? tapPosition : new Vector3(spellLineHit.point.x, spellLineHit.point.y, -1f);
        spellLine.SetPosition(0, transform.position);
        spellLine.SetPosition(1, lineEndPosition);
    }
    
    protected override void Movement()
    {
        int horizontal = 0;
        int vertical = 0;

        if (SimpleInput.GetAxisRaw("Horizontal") > 0.5f)
            horizontal = 1;
        else if (SimpleInput.GetAxisRaw("Horizontal") < -0.5f)
            horizontal = -1;
        
        if (SimpleInput.GetAxisRaw("Vertical") > 0.5f)
            vertical = 1;
        else if (SimpleInput.GetAxisRaw("Vertical") < -0.5f)
            vertical = -1;

        if (horizontal != 0 || vertical != 0)
            Move(horizontal, vertical);
    }

    private void Shoot(Vector3 shootPosition)
    {
        Vector2 lookDirection = _mainCamera.ScreenToWorldPoint(shootPosition) - spellShooter.transform.position;
        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        spellShooter.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        int index = (int) ((Mathf.Round(lookAngle / 90f) + 4) % 4);
        
        _clsSpriteManager.UpdateSpriteToDirection(index);
        // GameObject shootedSpell = Instantiate(spell, spellShooter.transform.position, spellShooter.transform.rotation);
        // Vector3 cameraShootPosition = _mainCamera.ScreenToWorldPoint(shootPosition);
        // shootedSpell.GetComponent<BulletConfig>().targetPosition = new Vector3(cameraShootPosition.x, cameraShootPosition.y);
    }
}