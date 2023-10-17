using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers
{
    public class MiniatureMouseHelper : MonoBehaviour
    {
        private GameObject _miniature;
        private bool _isAttached = false;
        private bool _canSpawnMiniatureOnMap = false;

        public void SelectedCard(Card card)
        {
            if(card == null) return;

            _isAttached = true;
            _miniature = MapRender.MiniatureRender(card);
        }

        private void MoveMiniaturePosition()
        {
            if (_isAttached)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Color color = new Color();
                var x = Mathf.FloorToInt(mousePos.x + .5f);
                var y = Mathf.FloorToInt(mousePos.y + .5f);

                _miniature.transform.position = new Vector2(x, y);

                if (GameManager.Instance.mapManager.CanSpawnMiniatures((y, x)))
                {
                    color = new Color(1, 1, 1, 1f);
                    _canSpawnMiniatureOnMap = true;
                }
                else
                {
                    _canSpawnMiniatureOnMap = false;
                    color = new Color(1, 0, 0, .5f);
                }

                _miniature.GetComponent<SpriteRenderer>().color = color;
            }
        }

        private void AddMiniatureOnMap()
        {
            if (_miniature != null && _canSpawnMiniatureOnMap && Input.GetButtonDown("Fire1"))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x = (int)_miniature.transform.position.x;
                int y = (int)_miniature.transform.position.y;
                var miniature = _miniature.GetComponent<Miniature>();
                miniature.self.MoveTo((y, x));

                //var tile = new Tile(card.stats.type, _miniature);

                //GameManager.Instance.mapManager.Register(tile, (y, x));

                _miniature = null;
                _isAttached = false;
            }
        }

        private void Update()
        {
            MoveMiniaturePosition();
            AddMiniatureOnMap();
        }

        private void Start()
        {
            GameManager.Instance.mouseHelper.OnCardSelected.AddListener(SelectedCard);
        }
    }
}
