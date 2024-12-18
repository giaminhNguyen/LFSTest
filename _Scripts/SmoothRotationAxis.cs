using System;
using UnityEngine;
using VInspector;

namespace _GameAssets._Scripts
{
    public class SmoothRotationAxis : MonoBehaviour
    {
        public float speed = 0.1f;
        public  Vector3[] angleRotate;
        private int       _index;
        private bool      _isResetRotate;
        
        [Space(3)]
        public Transform target;
        public SmoothRotationAxisData[] datas;
        private bool _canRotate;

        private void OnEnable()
        {
            EventManager.onDrag += RotateAxis;
        }

        private void OnDisable()
        {
            EventManager.onDrag -= RotateAxis;
        }

        private void RotateAxis(Vector2 startPosition, Vector2 previousPosition, Vector2 currentPosition)
        {
            if(!_canRotate) return;
            var delta = currentPosition - previousPosition;
            foreach(var data in datas)
            {
                RotateAxis(data, delta,target);
            }
        }

        private void RotateAxis(SmoothRotationAxisData data, Vector3 delta, Transform targetRotate)
        {
            float angle = 0;
            switch(data.valueDirection)
            {
                case Axis.X:
                    angle = delta.x * data.speed;
                    break;
                case Axis.Y:
                    angle = delta.y * data.speed;
                    break;
                case Axis.Z:
                    angle = delta.z * data.speed;
                    break;
            }
            targetRotate.RotateAround(data.axis, angle);
        }
        
        public void CanRotate(bool value)
        {
            _canRotate = value;
        }
        
        [Button]
        public void ResetRotate()
        {
            _isResetRotate = true;
            _index    = 0;
        }

        private void CheckAndRotate()
        {
            if(!_isResetRotate) return;

            if (target.transform.localRotation == Quaternion.identity || _index >= angleRotate.Length)
            {
                _isResetRotate = false;
                return;
            }

            var vt = angleRotate[_index];
            target.transform.localRotation = Quaternion.RotateTowards(target.transform.localRotation, Quaternion.Euler(vt), speed * 10 * Time.deltaTime);
            if(target.transform.localRotation == Quaternion.Euler(vt))
            {
                _index++;
            }
        }

        private void Update()
        {
            CheckAndRotate();
        }
    }
    
    [Serializable]
    public class SmoothRotationAxisData
    {
        public float   speed = 1f;
        public Vector3 axis  = Vector3.right;
        public Axis valueDirection = Axis.X;
    }
}