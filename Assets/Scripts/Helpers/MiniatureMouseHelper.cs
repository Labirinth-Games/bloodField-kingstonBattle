using Managers;
using Render;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Helpers
{
    public class MiniatureMouseHelper : MonoBehaviour
    {
        [SerializeField] private GameObject miniaturePrefab;

        [SerializeField] private GameObject _miniature;
        private bool _isAttached = false;
        private bool _canSpawnMiniatureOnMap = false;

        public void CreateMiniature(CardSO card)
        {
            if (card == null) return;

            _isAttached = true;
            _miniature = MiniatureRender.Render(card, miniaturePrefab);
        }

        private void MoveMiniaturePosition()
        {
            if (_isAttached)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Color color;
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
            if (_miniature != null && _isAttached && _canSpawnMiniatureOnMap && Input.GetButtonDown("Fire1"))
            {
                int x = (int)_miniature.transform.position.x;
                int y = (int)_miniature.transform.position.y;
                var miniature = _miniature.GetComponent<Miniature>();
                miniature.self.MoveTo((y, x));
                miniature.SetReady();

                _miniature = null;
                _isAttached = false;
            }
        }

        private void Update()
        {
            MoveMiniaturePosition();
            AddMiniatureOnMap();
        }

        public static (int y, int x) GetPositionOnWorld()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var x = Mathf.FloorToInt(mousePos.x + .5f);
            var y = Mathf.FloorToInt(mousePos.y + .5f);
            (int y, int x) position = (y, x);

            return position;
        }
    }
}
