using Managers;
using Miniatures;
using Mirror;
using Tiles;
using UnityEngine;

namespace Helpers
{
    public class MiniatureMouseHelper : NetworkBehaviour
    {
        [SerializeField] private GameObject _miniature;
        private bool _isAttached = false;
        private bool _canSpawnMiniatureOnMap = false;

        public void Attachment(GameObject element)
        {
            if (element == null) return;

            _isAttached = true;
            _miniature = element;
        }

        private void AttachmentOnMouse()
        {
            if (_isAttached)
            {
                var position = GetPositionOnWorld();
                Color color;

                _miniature.transform.position = new Vector2(position.x, position.y);

                if (_miniature.GetComponent<Miniature>().CanAddOnBoard(position))
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

        private void AddOnBoard()
        {
            if (_miniature != null && _isAttached && _canSpawnMiniatureOnMap && Input.GetButtonDown("Fire1"))
            {
                int x = (int)_miniature.transform.position.x;
                int y = (int)_miniature.transform.position.y;
                _miniature.GetComponent<Miniature>().AddOnBoard((y, x));

                _miniature = null;
                _isAttached = false;
            }
        }

        private void Actions()
        {
            var miniature = GameManager.Instance.gamePlayManager.GetCurrentMiniature();

            if (Input.GetButtonDown("Fire1") && miniature != null) // left mouse button
            {
                var position = GetPositionOnWorld();

                miniature.Move(position);
                miniature.Attack(position);
            }
        }

        private void Update()
        {
            if(!GameManager.Instance.gamePlayManager.isStartGame && !isLocalPlayer) return;
            
            AttachmentOnMouse();
            AddOnBoard();
            Actions();
        }

        public static (int y, int x) GetPositionOnWorld()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var x = Mathf.FloorToInt(mousePos.x + .5f);
            var y = Mathf.FloorToInt(mousePos.y + .5f);
            (int y, int x) position = (y, x);

            return position;
        }

        public static bool HasTouchMe(Tile tile) => GetPositionOnWorld() == tile.position;
    }
}
