using Managers;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Controls
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private int maxLimit = 12;
        [SerializeField] private int minLimit = 5;

        [SerializeField] private bool isFreezing = false;

        private int _scroll = 5;
        private bool _isDrag = false;
        [SerializeField] private float velocity = 20f;

        #region Actions
        public void Zoom()
        {
            Camera.main.orthographicSize = _scroll;
        }

        public void Drag()
        {
            if (Input.GetButtonUp("Fire1"))
                _isDrag = false;
            else if (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))
                _isDrag = true;

            if (!_isDrag) return;

            var x = Input.GetAxis("Mouse X") * Time.deltaTime * -1 * velocity;
            var y = Input.GetAxis("Mouse Y") * Time.deltaTime * -1 * velocity;

            Camera.main.transform.position += new Vector3(x, y, 0);
        }

        public void Center()
        {
            var size = GameManager.Instance.mapManager.Size();
            Camera.main.transform.Translate(15, 7, 0);
        }
        #endregion

        private void Scroll()
        {
            if (isFreezing) return;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && _scroll < maxLimit && Input.GetKey(KeyCode.LeftShift)) // forward
            {
                _scroll += 5;
                Zoom();
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f && _scroll > minLimit && Input.GetKey(KeyCode.LeftShift)) // backwards
            {
                _scroll -= 5;
                Zoom();
            }
        }

        private void Update()
        {
            Scroll();
            Drag();
        }
    }
}
