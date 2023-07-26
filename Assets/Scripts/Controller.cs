using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private PlayerCharacter _player;
    void Update()
    {
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");

        _player.SetInput(H, V);

        SendMove();
    }

    private void SendMove()
    {
        _player.GetMoveInfo(out Vector3 position);
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"x", position.x},
            {"y", position.z}
        };  
        MultiplayerManager.Instance.SendMessage("move", data);
    }
}
