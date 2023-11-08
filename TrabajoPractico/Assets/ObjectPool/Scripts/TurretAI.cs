﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour {

    public enum TurretType
    {
        Single = 1,
        Dual = 2,
        Catapult = 3,
    }
    public GameObject currentTarget;
    public Transform turreyHead;

    public float attackDist = 10.0f;
    public float attackDamage;
    public float shootCoolDown;
    private float timer;
    public float loockSpeed;
    public ObjectPoolCatapult ObjectpoolCatapul;
    public ObjectPoolDual ObjectpoolDual;
    public ObjectPoolSingle ObjectpoolSingle;
    private Vector3 randomRot;

    [Header("[Turret Type]")]
    public TurretType turretType = TurretType.Single;

    public Transform muzzleMain;
    public Transform muzzleSub;
    public GameObject muzzleEff;
    public GameObject bullet;
    private bool shootLeft = true;

    private Transform lockOnPos;


    void Start()
    {
        InvokeRepeating("ChackForTarget", 0, 0.5f);

        if (transform.GetChild(0).GetComponent<Animator>())
        {

        }

        randomRot = new Vector3(0, Random.Range(0, 359), 0);
    }

    void Update()
    {
        if (currentTarget != null)
        {
            FollowTarget();

            float currentTargetDist = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (currentTargetDist > attackDist)
            {
                currentTarget = null;
            }
        }
        else
        {
            IdleRitate();
        }

        timer += Time.deltaTime;
        if (timer >= shootCoolDown)
        {
            if (currentTarget != null)
            {
                timer = 0;

                Shoot(currentTarget);

            }
        }
    }

    private void ChackForTarget()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, attackDist);
        float distAway = Mathf.Infinity;

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Player")
            {
                float dist = Vector3.Distance(transform.position, colls[i].transform.position);
                if (dist < distAway)
                {
                    currentTarget = colls[i].gameObject;
                    distAway = dist;
                }
            }
        }
    }

    private void FollowTarget()
    {
        Vector3 targetDir = currentTarget.transform.position - turreyHead.position;
        targetDir.y = 0;

        if (turretType == TurretType.Single)
        {
            turreyHead.forward = targetDir;
        }
        else
        {
            turreyHead.transform.rotation = Quaternion.RotateTowards(turreyHead.rotation, Quaternion.LookRotation(targetDir), loockSpeed * Time.deltaTime);
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origen, float time)
    {
        Vector3 distance = target - origen;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDist);
    }

    public void IdleRitate()
    {
        bool refreshRandom = false;

        if (turreyHead.rotation != Quaternion.Euler(randomRot))
        {
            turreyHead.rotation = Quaternion.RotateTowards(turreyHead.transform.rotation, Quaternion.Euler(randomRot), loockSpeed * Time.deltaTime * 0.2f);
        }
        else
        {
            refreshRandom = true;

            if (refreshRandom)
            {

                int randomAngle = Random.Range(0, 359);
                randomRot = new Vector3(0, randomAngle, 0);
                refreshRandom = false;
            }
        }
    }

    public void Shoot(GameObject go)
    {
        GameObject missleGo;
        if (turretType == TurretType.Catapult)
        {
            lockOnPos = go.transform;
            Instantiate(muzzleEff, muzzleMain.transform.position, muzzleMain.rotation);
            missleGo = ObjectpoolCatapul.RequestBullet();
            Projectile projectile = missleGo.GetComponent<Projectile>();
            missleGo.transform.position = muzzleMain.transform.position;
            missleGo.transform.rotation = muzzleMain.transform.rotation;
            missleGo.SetActive(true);
            projectile.target = lockOnPos;
        }
        else if (turretType == TurretType.Dual)
        {
            if (shootLeft)
            {
                Instantiate(muzzleEff, muzzleMain.transform.position, muzzleMain.rotation);
                missleGo = ObjectpoolDual.RequestBullet();
                Projectile projectile = missleGo.GetComponent<Projectile>();
                projectile.target = transform.GetComponent<TurretAI>().currentTarget.transform;
                missleGo.transform.position = muzzleMain.transform.position;
                missleGo.transform.rotation = muzzleMain.transform.rotation;
                missleGo.SetActive(true);
            }
            else
            {
                Instantiate(muzzleEff, muzzleSub.transform.position, muzzleSub.rotation);
                missleGo = ObjectpoolDual.RequestBullet();
                Projectile projectile = missleGo.GetComponent<Projectile>();
                projectile.target = transform.GetComponent<TurretAI>().currentTarget.transform;
                missleGo.transform.position = muzzleSub.transform.position;
                missleGo.transform.rotation = muzzleSub.transform.rotation;
                missleGo.SetActive(true);
            }

            shootLeft = !shootLeft;
        }
        else
        {

            Instantiate(muzzleEff, muzzleMain.transform.position, muzzleMain.rotation);
            missleGo = ObjectpoolSingle.RequestBullet();
            Projectile projectile = missleGo.GetComponent<Projectile>();
            projectile.target = currentTarget.transform;
            missleGo.transform.position = muzzleMain.transform.position;
            missleGo.transform.rotation = muzzleMain.transform.rotation;
            missleGo.SetActive(true);
        }
    }

}