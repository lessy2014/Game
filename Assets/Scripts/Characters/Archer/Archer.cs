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
        private float attackRange = 10;
        public bool isAtacking;
        private Vector3 distanceToEnemy;
        public Transform arrowPos;
        public GameObject arrow;
        private LayerMask rayTo = (1 << 8) | (1 << 9) | (1 << 14);
        public Archer Instance;

    public float closestEnemy;
        public void Awake()
        {
            GetComponents();
            Instance = this;
        }
        public void Update()
        {
            var rayInfo = Physics2D.Raycast(transform.position, transform.right, attackRange, rayTo);
            if (rayInfo.collider != null && rayInfo.collider.gameObject.layer != 8)
                if (rayInfo.collider.gameObject.layer == 14 && Ghost_king_boss.Instance.solidSnake ||
                    rayInfo.collider.gameObject.layer != 14)
                    Attack(distanceToEnemy);
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawRay(transform.position, distanceToEnemy);
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
