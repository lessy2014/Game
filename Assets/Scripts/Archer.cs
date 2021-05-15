using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;


namespace Assets.Scripts
{

    class Archer: Support
    {
        public override void Awake()
        {
            GetComponents();
            Instance = this;
        }
        public void Update()
        {
            var entity = FindObjectsOfType<Entity>();
            var tr = new List<Transform>();
            foreach (var i in entity)
                tr.Add(i.transform);
            var distanceToEnemy = (GetClosestEnemy(tr.ToArray()).position - transform.position);
            if (distanceToEnemy.magnitude < 5)
            {
                Attack(distanceToEnemy);
            }
        }
         public Transform GetClosestEnemy(Transform[] enemies)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (Transform t in enemies)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }
        public void Attack(Vector3 enemyDirection)
        {

        }
    }
}
