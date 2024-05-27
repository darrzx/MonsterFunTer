using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

public class WizardSkill : MonoBehaviour
{
    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public Animator _animator;
    public AudioSource shoot;
    public AudioSource jump, land;
    public AudioSource fire;
    public GameObject fireEffects;
    float lastFireTime = 0, lastFlyTime = 0;
    public GameObject skill1, skill1Cooldown, skill2, skill2Cooldown, particle, crossHair;
    public CinemachineVirtualCamera virtualAimCam;

    public Camera cam;
    private Vector3 dest;
    public GameObject bullet;
    public Transform firePos;

    private bool isSkill1Used = false;
    private bool isSkill2Used = false;
    private bool isCollidedFly = false;

    public CinemachineImpulseSource source;
    public GameObject effectBullet;
    public static bool isCheat = false;

    public Rig rig;
    Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // SET ANIMASI BASIC ATTACK
        if (Input.GetMouseButton(1))
        {
            rig.weight = 1;
            crossHair.SetActive(true);
            _animator.SetBool("isAiming", true);
            virtualAimCam.gameObject.SetActive(true);
            virtualAimCam.Priority = 20;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(MissionController.missionCount == 1 && MissionController.missionProgress[0] < 10)
                {
                    MissionController.missionProgress[0]++;
                }
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    dest = hit.point;
                    EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                        enemy.setCurrHealth(10f);
                    }
                    BossController boss = hit.transform.GetComponent<BossController>();
                    if (boss != null)
                    {
                        boss.setCurrHealth(10f);
                    }
                }
                else
                {
                    dest = ray.GetPoint(1000);
                }
                effectBullet.SetActive(true);
                var projectileObj = Instantiate(bullet, firePos.position, Quaternion.identity) as GameObject;
                projectileObj.SetActive(true);
                projectileObj.GetComponent<Rigidbody>().velocity = (dest - firePos.position).normalized * 50;
                source.GenerateImpulse(Camera.main.transform.forward);
                shoot.Play();
            }
            else
            {
                effectBullet.SetActive(false);
            }
        }
        else
        {
            rig.weight = 0;
            crossHair.SetActive(false);
            _animator.SetBool("isAiming", false);
            virtualAimCam.Priority = 5;
            virtualAimCam.gameObject.SetActive(false);
        }


        // SET ANIMASI FLYING
        if (Input.GetKeyDown(KeyCode.R) && !_animator.GetBool("isFlying") && Time.time - lastFlyTime > 10f)
        {
            if (MissionController.missionCount == 2 && isSkill1Used == false)
            {
                MissionController.missionProgress[1]++;
                isSkill1Used = true;
            }
            isCollidedFly = false;
            var pos = instance.playerActive.transform.position;
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                pos.y = 5f;
            }
            else
            {
                pos.y = 2f;
            }
            instance.playerActive.transform.position = pos;
            _animator.SetBool("isFlying", true);
            if (isCheat)
            {
                Invoke("FlyingFalse", 7f);
            }
            else
            {
                Invoke("FlyingFalse", 5f);
            }
            lastFlyTime = Time.time;
            skill1.SetActive(false);
            skill1Cooldown.SetActive(true);
            jump.Play();
        }
        else
        {
            if (Time.time - lastFlyTime > 10f)
            {
                skill1.SetActive(true);
                skill1Cooldown.SetActive(false);
            }
        }

        if (_animator.GetBool("isFlying"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -45f);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 45f);
            }
            else
            {
                rotation = transform.rotation;
            }

            var pos = instance.playerActive.transform.position;
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                pos.y = 5f;
            }
            else
            {
                pos.y = 2f;
            }
            instance.playerActive.transform.position = pos;
            if (isCheat)
            {
                transform.position += transform.forward * 0.2f;
            }
            else
            {
                transform.position += transform.forward * 0.15f;
            }
        }

        // SET ANIMASI FIRE
        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastFireTime > 5f)
        {
            if (MissionController.missionCount == 2 && isSkill2Used == false)
            {
                MissionController.missionProgress[1]++;
                isSkill2Used = true;
            }
            _animator.SetBool("isCasting", true);
            fireEffects.SetActive(false);
            StartCoroutine(Cast());
            skill2.SetActive(false);
            skill2Cooldown.SetActive(true);
            fire.Play();
        }
    }

    IEnumerator Cast()
    {
        yield return new WaitForSeconds(1.2f);
        fireEffects.SetActive(true);
        particle.SetActive(true);
        yield return new WaitForSeconds(3f);
        _animator.SetBool("isCasting", false);
        yield return new WaitForSeconds(0.3f);
        fireEffects.SetActive(false);
        particle.SetActive(false);
        lastFireTime = Time.time;
        yield return new WaitForSeconds(5f);
        skill2.SetActive(true);
        skill2Cooldown.SetActive(false);
    }

    public void FlyingFalse()
    {
        _animator.SetBool("isFlying", false);
        if (isCollidedFly == false)
        {
            lastFlyTime = Time.time;
            land.Play();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_animator.GetBool("isFlying"))
        {
            FlyingFalse();
            isCollidedFly = true;
        }
    }
}
