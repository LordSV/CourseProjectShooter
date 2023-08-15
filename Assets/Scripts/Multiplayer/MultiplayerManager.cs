using Colyseus;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [field: SerializeField] public LossCounter LossCounter { get; private set;}
    [field: SerializeField] public SpawnPoints _spawnPoints { get; private set; }
    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private EnemyController _enemy;
    private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();
    private ColyseusRoom<State> _room;
    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room.Leave();
    }

    private async void Connect()
    {
        _spawnPoints.GetPoint(Random.Range(0, _spawnPoints.Length), out Vector3 spawnPosition, out Vector3 spawnRotation);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"points", _spawnPoints.Length },
            {"speed", _player.speed},
            {"hp", _player.maxHealth},
            {"pX", spawnPosition.x},
            {"pY", spawnPosition.y},
            {"pZ", spawnPosition.z},
            {"rY", spawnRotation.y},
        };
        _room = await Instance.client.JoinOrCreate<State>("state_handler", data);
        _room.OnStateChange += OnChange;
        _room.OnMessage<string>("Shoot", ApplyShoot);
    }

    private void ApplyShoot(string jsonShootInfo)
    {
        ShootInfo shootInfo = JsonUtility.FromJson<ShootInfo>(jsonShootInfo);
        if(_enemies.ContainsKey(shootInfo.key) == false)
        {
            Debug.LogError("Enemy not found");
            return;
        }
        _enemies[shootInfo.key].Shoot(shootInfo);
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == false) return;

        state.players.ForEach((key, player) =>
        {
            if (key == _room.SessionId) CreatePlayer(player);
            else CreateEnemy(key, player);
        });
        _room.State.players.OnAdd += CreateEnemy;
        _room.State.players.OnRemove += RemoveEnemy;
    }

    private void CreatePlayer(Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);
        Quaternion rotation = Quaternion.Euler(0, player.rY, 0);
        var playerCharacter = Instantiate(_player, position, rotation);
        player.OnChange += playerCharacter.OnChange;

        _room.OnMessage<int>("Restart", playerCharacter.GetComponent<Controller>().Restart);
    }

    private void CreateEnemy(string key, Player player)
    {
        var position = new Vector3(player.pX, player.pY, player.pZ);

        var enemy = Instantiate(_enemy, position, Quaternion.identity);
        enemy.Init(key, player);

        _enemies.Add(key, enemy);
    }
    private void RemoveEnemy(string key, Player value)
    {
        if (_enemies.ContainsKey(key) == false) return;

        var enemy = _enemies[key];
        enemy.Destroy();

        _enemies.Remove(key);
    }

    private void ForEachEnemy(string key, Player player)
    {
        if (key == _room.SessionId) return;

        var position = new Vector3(player.pX, player.pY, player.pZ);
        Instantiate(_enemy, position, Quaternion.identity);
    }

    public void SendMessage(string key, Dictionary<string, object> data)
    {
        _room.Send(key, data);
    }

    public void SendMessage(string key, string data)
    {
        _room.Send(key, data);
    }

    public string GetSessionID() => _room.SessionId;
}
