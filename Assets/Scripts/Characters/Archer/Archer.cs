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

    class Archer : Support
    {
        public bool isAtacking;
        private Vector3 distanceToEnemy;
        public Transform arrowPos;
        public GameObject arrow;
        private LayerMask rayTo = (1 << 8) | (1 << 9) | (1 << 14);

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
                        if (wallInfo.collider != null && wallInfo.collider.gameObject.layer != 8)
                        {
                            if (wallInfo.collider.gameObject.layer == 14 && Ghost_king_boss.Instance.solidSnake || wallInfo.collider.gameObject.layer != 14)
                                Attack(distanceToEnemy);
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
                Instantiate(arrow, arrowPos.position,  Quaternion.Euler(0, 0,0 ));
            else
                Instantiate(arrow, arrowPos.position,  Quaternion.Euler(0, 180,0 ));
        }
        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(1f);
            isAtacking = false;
        }
        
    }
}
