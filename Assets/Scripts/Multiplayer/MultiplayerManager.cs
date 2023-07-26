using Colyseus;
using System;
using UnityEngine;

public class MultiplayerManager : ColyseusManager<MultiplayerManager>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;
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
        _room = await Instance.client.JoinOrCreate<State>("state_handler");
        _room.OnStateChange += OnChange;
    }

    private void OnChange(State state, bool isFirstState)
    {
        if (isFirstState == true)
        {
            var player = state.players[_room.SessionId];
            var position = new Vector3(player.x - 200, 0, player.y - 200) / 8;
            Instantiate(_player, position, Quaternion.identity);
            state.players.ForEach(ForEachEnemy);
        }
    }

    private void ForEachEnemy(string key, Player player)
    {
        if (key == _room.SessionId) return;
        var position = new Vector3(player.x - 200, 0, player.y - 200) / 8;
        Instantiate(_enemy, position, Quaternion.identity);
    }
}
