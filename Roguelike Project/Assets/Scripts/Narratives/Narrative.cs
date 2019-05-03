using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrative : MonoBehaviour {

    public Sprite[] sprites = new Sprite[4];
    public int narrativeID;
    public TextAsset theText;
    [Header("Starter Lines")]
    public int startLine;
    public int endLine;
    public int optionTriggerLine;
    public int optionReadLine;
    [Header("Variables")]
    public int timesTalked = 0;
    public int timesResponded = 0;
    private TextManager _clsTextManager;
    public bool positionForChat;
    public bool destroyAfterActive;
    public bool requireButtonPress;
    private GameObject _chatIndicator;
    private GameObject _player;
    [System.NonSerialized]
    public SpriteRenderer SprRender;
    [System.NonSerialized]
    public Sprite nonTalkingSprite;


    // Use this for initialization
    void Start ()
    {
        _clsTextManager = FindObjectOfType<TextManager>();
        _chatIndicator = gameObject.transform.GetChild(0).gameObject;
        _player = GameObject.FindGameObjectWithTag("Player");
        SprRender = gameObject.GetComponent<SpriteRenderer>();
        nonTalkingSprite = SprRender.sprite;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (positionForChat && Input.GetKeyDown(KeyCode.E) && !_clsTextManager.isActive)
        {
            _clsTextManager.clsNarrative = this;
            _clsTextManager.GetNarrativeValues();
            AdjustSprite();

            if (destroyAfterActive)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (requireButtonPress)
            {
                positionForChat = true;
                _chatIndicator.SetActive(true);
            }
            else
            {
                _clsTextManager.clsNarrative = this;
                _clsTextManager.GetNarrativeValues();
            }

            if (destroyAfterActive)
            {
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            positionForChat = false;
            _chatIndicator.SetActive(false);
        }
    }

    void AdjustSprite()
    {
        Vector2 direction = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int index = (int)((Mathf.Round(angle / 90f) + 4) % 4); //add a modulo over 4 to get a normalized index

        switch (index)
        {
            case 0:
                SprRender.sprite = sprites[0];
                break;
            case 1:
                SprRender.sprite = sprites[1];
                break;
            case 2:
                SprRender.sprite = sprites[2];
                break;
            case 3:
                SprRender.sprite = sprites[3];
                break;
        }
    }
}
