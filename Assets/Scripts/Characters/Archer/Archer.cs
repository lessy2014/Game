using System;
using System.Collections;
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
        public bool isAtacking;
        public Transform arrowLeftPos;
        public Transform arrowRightPos;
        public GameObject arrow;
        public override void Awake()
        {
            GetComponents();
            Instance = this;
        }
        public void Update()
        {
            var entity = FindObjectsOfType<Entity>();
            
            var tr = new List<Transform>();
            if (entity.Length != 0)
            {
                foreach (var i in entity)
                    tr.Add(i.transform);
                var distanceToEnemy = (GetClosestEnemy(tr.ToArray()).position - transform.position);
                if (distanceToEnemy.magnitude < 20 && !isAtacking && distanceToEnemy.x > 0 == isRight)
                {
                    Attack(distanceToEnemy);
                }
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
            animator.Play("Attack_archer");
            isAtacking = true;
            StartCoroutine(AttackCooldown());
        }

        public void Shoot()
        {
            if (isRight)
                Instantiate(arrow, arrowRightPos.position,  Quaternion.Euler(0, 0,0 ));
            else
                Instantiate(arrow, arrowLeftPos.position,  Quaternion.Euler(0, 180,0 ));
        }
        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(1f);
            isAtacking = false;
        }
        
    }
}
