using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFollow : MonoBehaviour
{
    GameObject player;
    float playerHeight;
    Transform sliderPos;
    public float offsetZ;
    public void SetupSlider(GameObject _player,float _playerHeight,Transform _sliderPos)
    {
        player = _player;
        playerHeight = _playerHeight;
        sliderPos = _sliderPos;

    }

    public void UpdatePosition()
    {

        if (player!=null)
        {
            transform.position = new Vector3(player.transform.position.x, sliderPos.position.y, player.transform.position.z+offsetZ);
        }
    }

    private void LateUpdate()
    {
        UpdatePosition();
    }

    public void DestroySlider(int currentHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
