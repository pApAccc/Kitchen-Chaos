using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// 
/// </summary>
namespace ns
{
    public class Player : NetworkBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyPlayerSpawned;
        public static event EventHandler OnAnyPlayerPickupSomething;
        public static Player LocalInstance { get; private set; }

        public event EventHandler OnPickSomething;
        public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
        public class OnSelectedCounterChangedEventArgs : EventArgs
        {
            public BaseCounter selectedCounter;
        }

        private KitchenObject kitchenObject;
        private float moveSpeed = 10;
        private bool isWalking;
        private Vector3 lastMoveDir;
        private BaseCounter selectedCounter;

        [SerializeField] private PlayerAnimationController animatorController;
        [SerializeField] private Transform pickupPoint;
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private List<Vector3> spawnPosition;


        private void Start()
        {
            GameInput.Instance.OnInteract += GameInput_OnInteract;
            GameInput.Instance.OnCutting += GameInput_OnCutting;
        }

        private void GameInput_OnCutting(object sender, EventArgs e)
        {
            if (selectedCounter != null && GameManager.Instance.IsGamePlaying())
            {
                selectedCounter.InteractAlternate(this);
            }
        }

        private void GameInput_OnInteract(object sender, EventArgs e)
        {
            //处理当玩家按下按钮时的counter逻辑
            if (selectedCounter != null && GameManager.Instance.IsGamePlaying())
            {
                selectedCounter.Interact(this);
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                LocalInstance = this;
            }
            transform.position = spawnPosition[(int)OwnerClientId];
            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

            if (IsServer)
                NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }


        private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
        {
            if (clientID == OwnerClientId && HasKitchenObject())
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
            }
        }

        private void Update()
        {
            if (!IsOwner) return;

            HandleMOvement();
            HandleInteract();
        }

        private void HandleMOvement()
        {
            Vector3 moveDir = new Vector3(GameInput.Instance.GetMoveDirInput().x, 0, GameInput.Instance.GetMoveDirInput().y);
            isWalking = moveDir != Vector3.zero;

            //float playerHeight = 2f;
            float playerRadius = .7f;
            float moveDistance = moveSpeed * Time.deltaTime;
            bool canMove = !Physics.BoxCast(
                transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionMask
                );

            //解决斜对角移动时，不能在x和z轴上移动
            if (!canMove)
            {
                //尝试在x轴上移动
                Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
                canMove = (moveDir.x < -.5f || moveDir.x > .5f) && !Physics.BoxCast(
                transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, collisionMask
                    );

                if (canMove)
                {
                    moveDir = moveDirX;
                }
                else
                {
                    //尝试在z轴上移动
                    Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                    canMove = (moveDir.z < -.5f || moveDir.z > .5f) && !Physics.BoxCast(
                transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, collisionMask
                        );

                    if (canMove)
                    {
                        moveDir = moveDirZ;
                    }
                }
            }

            if (canMove)
            {
                transform.position += moveDir.normalized * Time.deltaTime * moveSpeed;
            }

            if (isWalking)
                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * moveSpeed);

            animatorController.MoveAnimationControl(isWalking);
        }

        private void HandleInteract()
        {
            Vector3 moveDir = new Vector3(GameInput.Instance.GetMoveDirInput().x, 0, GameInput.Instance.GetMoveDirInput().y);
            if (moveDir != Vector3.zero)
            {
                lastMoveDir = moveDir;
            }
            float interactDiatance = 2f;
            if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit hit, interactDiatance))
            {
                if (hit.transform.TryGetComponent(out BaseCounter clearCounter))
                {
                    //处理当玩家射线选中的counter的视觉逻辑
                    if (selectedCounter != clearCounter)
                    {
                        SetSelectedCounter(clearCounter);
                    }
                }
                else
                {
                    SetSelectedCounter(null);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        private void SetSelectedCounter(BaseCounter clearCounter)
        {
            selectedCounter = clearCounter;
            OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
            {
                selectedCounter = selectedCounter
            });
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;

            if (kitchenObject != null)
            {
                OnPickSomething?.Invoke(this, EventArgs.Empty);
                OnAnyPlayerPickupSomething?.Invoke(this, EventArgs.Empty);
            }
        }

        public Transform GetHoldPointTransform()
        {
            return pickupPoint;
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return kitchenObject != null;
        }

        public bool IsWalking() => isWalking;

        public static void ResetStaticData()
        {
            OnAnyPlayerSpawned = null;
        }

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    }
}
