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
        private Vector3 distanceToEnemy;
        public Transform arrowLeftPos;
        public Transform arrowRightPos;
        public GameObject arrow;
        private LayerMask rayIgnore = (1 << 12) | (1 << 11) | (1 << 10) | (1 << 0);
        private LayerMask rayTo = (1 << 8) | (1 << 9);
        public float closestEnemy;
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
                var closesetEnemyTransform = GetClosestEnemy(tr.ToArray());
                if (closesetEnemyTransform != null)
                {
                    distanceToEnemy = closesetEnemyTransform.position - transform.position;
                    closestEnemy = distanceToEnemy.magnitude;
                    if (distanceToEnemy.magnitude < 20 && !isAtacking)
                    {
                        var wallInfo = Physics2D.Raycast(transform.position, distanceToEnemy,
                            distanceToEnemy.magnitude, rayTo);
                        if (wallInfo.collider != null)
                        {
                            // print(wallInfo.collider.gameObject.layer);
                            if (wallInfo.collider.gameObject.layer == 9)
                            {
                                Attack(distanceToEnemy);
                            }
                        }
                    }
                }
            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(transform.position, distanceToEnemy);
        }
        public Transform GetClosestEnemy(Transform[] enemies)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (Transform t in enemies)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist && isRight == (t.position - currentPos).x > 0)  
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
