using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Cinemachine;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Vector3 dir;
        [SerializeField] private Animator animator;
        [SerializeField] private float speed = 4;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform muzzle;
        [SerializeField] private Transform aimPoint;
        [SerializeField] private Transform viewPoint;
        
        private Animator _animator;
        private Rigidbody _rigidbody;
        private bool _isGround;
        private float _mouseSensitivity = 4f;
        private float _mouseY;
        private Transform _playerSpine;
        private bool _isRunning = false;

        public PhotonView pv;
        public GameObject vcamObject;
        public CinemachineVirtualCamera vcam;
        public Cinemachine3rdPersonFollow personFollow;
        public PlayerInfo playerInfo;
        private void Awake()
        {
            vcamObject.SetActive(true);
            _animator = GetComponent<Animator>();
            //playerInfo = GetComponent<PlayerInfo>();
            personFollow = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            _playerSpine = animator.GetBoneTransform(HumanBodyBones.Spine);
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        void Start()
        {
            CursorOff();
            if (!photonView.IsMine)
            {
                vcam.Priority = -1;
            }
            
        }

        void FixedUpdate()
        {
            if (!photonView.IsMine) return;
            if(dir != Vector3.zero)
            {
                transform.Translate(dir * speed * Time.deltaTime);
            }
        }
        
        private void LateUpdate()
        {
            if (!photonView.IsMine) return;
            Look();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            Vector2 input = context.ReadValue<Vector2>();
            if(input != null)
            {
                dir = new Vector3(input.x, 0f, input.y);
                animator.SetFloat("horizontal", input.x);
                animator.SetFloat("vertical", input.y);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            if (context.performed & _isGround) // Action type이 "Button"일 경우 키가 눌렸는지 확인함
            {
                animator.SetTrigger("isJump");
                _rigidbody.AddForce(Vector3.up * speed, ForceMode.Impulse);
            }
        }

        public void OnAcceleration(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            if (context.performed)
            {
                animator.SetBool("isRun", true);
                _isRunning = true;
                speed = 6f;
            }
            else
            {
                animator.SetBool("isRun", false);
                _isRunning = false;
                speed = 4f;
            }
        }
        
        private void CursorOn()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        private void CursorOff()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGround = true;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isGround = false;
            }
        }
        public void OnShot(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            if (_isRunning) return;
            if (context.performed) // Action type이 "Button"일 경우 키가 눌렸는지 확인함
            {
                _animator.SetBool("isFire", true);
                pv.RPC("Fire", RpcTarget.All);
            }
            else
            {
                _animator.SetBool("isFire", false);
            }
        }

        private void Look()
        {
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

            _mouseY += Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;
            _mouseY = Mathf.Clamp(_mouseY, -20f, 60f);

            viewPoint.transform.localEulerAngles = Vector3.left * _mouseY;
            personFollow.ShoulderOffset = new Vector3(0, -0.1f * _mouseY, 0.3f);
            
            _playerSpine.localRotation = Quaternion.Euler(0, 0,_mouseY * 0.8f);
        }
        
        [PunRPC]
        private void Fire()
        {
            if (playerInfo.curAmmo > 0)
            {
                playerInfo.curAmmo--;
                GameObject spawnedBullet = Instantiate(bullet, muzzle.position, muzzle.rotation);
                spawnedBullet.GetComponent<Rigidbody>().AddForce(aimPoint.forward * 2500f);
            }
            //PhotonNetwork.Instantiate(bullet.name, muzzle.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(muzzle.forward * 2500f); 
        }
        
        public void OnReload(InputAction.CallbackContext context)
        {
            if (!photonView.IsMine) return;
            if (_isRunning) return;
            pv.RPC("Reload", RpcTarget.All);
        }

        [PunRPC]
        public void Reload()
        {
            if (playerInfo.remainedAmmo > 0)
            {
                if (playerInfo.curAmmo >= playerInfo.MaxAmmo) return;
                
                int insertAmmo = playerInfo.MaxAmmo - playerInfo.curAmmo;
                if (playerInfo.remainedAmmo <= insertAmmo)
                {
                    playerInfo.curAmmo += playerInfo.remainedAmmo;
                    playerInfo.remainedAmmo = 0;
                }
                else
                {
                    playerInfo.curAmmo += insertAmmo;
                    playerInfo.remainedAmmo -= insertAmmo;
                }
                _animator.SetTrigger("isReload");
            }
        }
    }


