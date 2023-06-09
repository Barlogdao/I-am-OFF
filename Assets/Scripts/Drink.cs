using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Drink : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider;
    private LayerMask _mask;
    private Vector3 _localTransform;
    private DrinkSO _drinkData;
    private HumanPlayer _player;
    [SerializeField] SpriteRenderer _questionLabel;
    private bool _drinkIsTaken = false;
    private Tweener _tweener;


    public static event Func<DrinkSO> DrinkChanged;

    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider.enabled = false;
        _spriteRenderer.enabled = false;
        _questionLabel.enabled = false;
    }
    
    public void Init(Game game)
    {
        _mask = game.PlayerMask;
        _player = game.Player;
        _drinkData = DrinkChanged?.Invoke();
        _spriteRenderer.sprite = _drinkData.Image;
        _circleCollider.enabled = true;
        _spriteRenderer.enabled = true;
        HumanPlayer.PlayerMindChanged += OnPlayerOFF;
        Game.GameOvered += OnGameOver;
        Game.GameStarted += OnGameStartedl;
        HumanPlayer.SobrietyChanged += OnSobrietyChanged;
        _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        _questionLabel.enabled = false;
    }

    private void OnGameStartedl()
    {
        StartCoroutine(DrinkUpdate());
    }

    private void OnSobrietyChanged(SobrietyLevel sobrietyLevel)
    {
        DrinkVisibility(sobrietyLevel);
    }

    private void OnPlayerOFF(bool playerIsOFF)
    {
        

        _circleCollider.enabled = !playerIsOFF;
        _spriteRenderer.color = playerIsOFF ? Color.gray : Color.white;
        _questionLabel.color = playerIsOFF ? Color.gray : Color.white; 

    }

    private void OnMouseDown()
    {
        _localTransform = transform.localPosition;
        _drinkIsTaken = true;

    }
    private void OnMouseDrag()
    {
        transform.position = GetMousePos();
    }

    private void OnMouseUp()
    {
 
        if (_circleCollider.IsTouching(_player.PlayerCollider) && Game.Instance.State != GameState.GameOver && !_player.PlayerIsDrinking)
        {
            _player.Drink(_drinkData);
            StartCoroutine(DestroyDrink());
        }
        else
        {
            transform.localPosition = _localTransform;
        }
        _drinkIsTaken = false;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity; 
    }
    private void OnGameOver()
    {
        StopAllCoroutines();
        _circleCollider.enabled = false;
        _spriteRenderer.enabled = false;
        _questionLabel.enabled = false;
    }

    public static Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }


    private void OnDestroy()
    {
        Game.GameOvered -= OnGameOver;
        HumanPlayer.PlayerMindChanged -= OnPlayerOFF;
        HumanPlayer.SobrietyChanged -= OnSobrietyChanged;
        Game.GameStarted -= OnGameStartedl;
    }
    private IEnumerator DestroyDrink()
    {
        // ������ ���������� � ���������� ����������
        transform.localPosition = _localTransform;
        _circleCollider.enabled = false;
        _spriteRenderer.enabled = false;
        _questionLabel.enabled = false;
        // �������� �����
        yield return new WaitForSeconds(1f);
        // ����������� ��
        _drinkData = DrinkChanged?.Invoke();
        // ���������� ����� �����
        transform.localScale = Vector3.zero;
        transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
        _circleCollider.enabled = !_player.PlayerIsOFF;
        _spriteRenderer.enabled = true;
        DrinkVisibility(_player.Sobriety);
        _spriteRenderer.sprite = _drinkData.Image;
    }

    private IEnumerator DrinkUpdate()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 5f));
        while(Game.Instance.State != GameState.GameOver)
        {
            if (_drinkIsTaken == false)
            {
                _drinkData = DrinkChanged?.Invoke();
                _spriteRenderer.sprite = _drinkData.Image;
                transform.localScale = Vector3.zero;
                transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);

            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(3f,5f));
        }
    }

    private void DrinkVisibility(SobrietyLevel sobrietyLevel)
    {
        switch (sobrietyLevel)
        {
            case SobrietyLevel.Sober:
                _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                _questionLabel.enabled = false;
                break;
            case SobrietyLevel.Drunk:
                _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                _questionLabel.enabled = false;
                break;
            case SobrietyLevel.DrunkAsHell:
                _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                _questionLabel.enabled = true;
                _questionLabel.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        _tweener = transform.DOScale(1.15f, 0.2f).SetLoops(-1, LoopType.Yoyo);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _tweener.Kill();
        transform.localScale = Vector3.one;
    }
}
   
