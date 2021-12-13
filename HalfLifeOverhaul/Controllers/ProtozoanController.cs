using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HalfLifeOverhaul.Controllers
{
    public class ProtozoanController : MonoBehaviour
    {
        const float ANGULAR_SPEED = 1f;

        private float _altitude;
        private float _longitude;
        private float _latitude;

        private float _moveTimer = 0f;
        private float _timer = 0f;
        private Vector2 _moveDirection;

        public void PlaceAt(Transform parent, float altitude, float longitude, float latitude)
        {
            this._altitude = altitude;
            this._longitude = longitude;
            this._latitude = latitude;
            transform.parent = parent;
            transform.localPosition = SphericalToCartesian(altitude + Mathf.Cos(_timer), longitude, latitude);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            _moveTimer -= Time.deltaTime;
            if(_moveTimer < 0)
            {
                // Pick a new direction
                _moveDirection = Random.insideUnitCircle.normalized;
                _moveTimer = Random.Range(4f, 20f);
            }

            _longitude += _moveDirection.x * ANGULAR_SPEED * Time.deltaTime / ((_altitude * _altitude) / (90000f));
            _latitude += _moveDirection.y * ANGULAR_SPEED * Time.deltaTime / ((_altitude * _altitude) / (90000f));

            var r = _altitude + Mathf.Cos(_timer);
            transform.localPosition = SphericalToCartesian(r, _longitude, _latitude);
            transform.LookAt(transform.parent.position);
        }

        private Vector3 SphericalToCartesian(float r, float theta, float phi)
        {
            var sinPhi = Mathf.Sin(Mathf.Deg2Rad * phi);
            var x = Mathf.Cos(Mathf.Deg2Rad * theta) * sinPhi;
            var y = Mathf.Sin(Mathf.Deg2Rad * theta) * sinPhi;
            var z = Mathf.Cos(Mathf.Deg2Rad * phi);
            return new Vector3(x, y, z) * r;
        }
    }
}
