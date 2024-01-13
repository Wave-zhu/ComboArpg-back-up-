using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGG.Tool;
using GGG.Tool.Singleton;
using MyGame.Combat;
using MyGame.Movement;
using Unity.VisualScripting;

public class EnemyManager : GGG.Tool.Singleton.Singleton<EnemyManager>
{
    private Transform _mainPlayer;

    [SerializeField] private List<GameObject> _allEnemies=new List<GameObject>();
    [SerializeField] private List<GameObject> _activeEnemies=new List<GameObject>();
    [SerializeField] private WaitForSeconds _waitTime;
    private bool _closeAttackCommandCoroutine;

    protected override void Awake()
    {
        base.Awake();
        _mainPlayer = GameObject.FindWithTag("Player").transform;
        _waitTime = new WaitForSeconds(6f);
    }
    private void Start()
    {
        foreach (var enemy in _allEnemies)
        {
            if (enemy.activeSelf)
            {
                if(enemy.TryGetComponent(out EnemyMovementControl _control))
                {
                    _control.EnableCharacterController(true);
                }                 
                _activeEnemies.Add(enemy);
            }
        }
        //here!
        if (_activeEnemies.Count > 0)
            _closeAttackCommandCoroutine = false;
        StartCoroutine(EnableEnemyAttackCommand());

    }


    IEnumerator EnableEnemyAttackCommand()
    {
        //stop coroutine
        if (_activeEnemies == null||_activeEnemies.Count==0) yield break;

        while (_activeEnemies.Count > 0)
        {
            if (_closeAttackCommandCoroutine) yield break;
            var index = Random.Range(0, _activeEnemies.Count);
            if (index < _activeEnemies.Count)
            {
                GameObject temp = _activeEnemies[index];
                if (temp.TryGetComponent(out EnemyCombatControl enemyCombatControl))
                {
                    enemyCombatControl.SetAttackCommand();
                }
            }
            yield return _waitTime;
        }
        yield break;

    }
    public void CloseAttackCommandCoroutine()
    {
        _closeAttackCommandCoroutine=true;
        StopCoroutine(EnableEnemyAttackCommand());
    }
    public void StopAllActiveEnemy()
    {
        foreach (var enemy in _activeEnemies)
        { 
            if(enemy.TryGetComponent(out EnemyCombatControl enemyCombatControl))
            {
                enemyCombatControl.StopAllAction();
            }
        }
    }
    public void AddEnemy(GameObject enemy)
    {
        if (!_allEnemies.Contains(enemy))
        {
            _allEnemies.Add(enemy);
        }
    }
    public void AddActiveEnemy(GameObject enemy)
    {
        if (!_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Add(enemy);
            if (enemy.TryGetComponent(out EnemyMovementControl _control))
            {
                _control.EnableCharacterController(true);
            }
        }
    }
    public void RemoveFromActiveEnemy(GameObject enemy)
    {
        if (_activeEnemies.Contains(enemy))
        {
            _activeEnemies.Remove(enemy);
        }
    }

    public Transform GetMainPlayer() => _mainPlayer;
}
