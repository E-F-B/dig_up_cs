using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TGJ2021.InGame.Background
{
    public class AutoScroller : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private float _speed;
        [SerializeField] private bool _back;

        private readonly List<Transform> _scrollObjects = new List<Transform>();
        private float totalSpeed = 0f;
        
        private float MoveSpeed { get; set; }
        private Vector3 Forward { get; set; }
        
        private void Awake()
        {
            foreach (Transform o in transform)
            {
                _scrollObjects.Add(o);
            }

            MoveSpeed = _speed;// _back ? _speed * -1 : _speed;
            Forward = _back ? Vector3.back : Vector3.forward;
        }

        private void Update()
        {
            var velocity = Forward * MoveSpeed * Time.deltaTime;
            totalSpeed += velocity.z;
            for (var i = 0; i < _scrollObjects.Count; i++)
            {
                var t = _scrollObjects[i];
                t.position += velocity;
            }

            if (Mathf.Abs(totalSpeed) > _distance)
            {
                totalSpeed = 0;
                var sortedList = _back
                    ? _scrollObjects.OrderByDescending(t => t.transform.position.z).ToList()
                    : _scrollObjects.OrderBy(t => t.transform.position.z).ToList();
                
                var last = sortedList.LastOrDefault();
                var first = sortedList.FirstOrDefault();
                last.transform.position = first.transform.position - Forward * _distance;
            }
        }
    }
}
