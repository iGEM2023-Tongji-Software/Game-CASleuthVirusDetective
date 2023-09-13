using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour,IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }


    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        //��tab�Զ�����+=�����
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;        // ���ٿ��Խ��н���

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }

    }
    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;        // ���ٿ��Խ��н���

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }

    }

    //���ΪupdateСд��ĸ��ͷUnity�������
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // ���߻��е�����ӵ��ClearCounter.cs�ű�
                // �����ǰ�����Ĺ�̨������һ��ѡ�еĹ�̨���Ͱ�ѡ�е�clearCounter����Ϊ��ǰ�Ĺ�̨
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                // ���߻��е�����û��ClearCounter.cs�ű�
                SetSelectedCounter(null);
            }
        }
        else
        {
            // û��������ײ���κζ���
            SetSelectedCounter(null);
        }

        //Debug.Log(selectedCounter);
    
}
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        //����inputVector�����µ���ά��������ֱ����Ϊ0��
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.785f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        //if (moveDir != Vector3.zero)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
        //    transform.rotation = targetRotation;
        //}
        if (!canMove)
        {
            // ��������moveDir�����ƶ�ʱ

            // ������x���ƶ�
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized; // ��һ�����ٶȺ�ֱ�������ƶ���ͬ
            canMove = (moveDir.x < .5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // ������x���ƶ�
                moveDir = moveDirX;
            }
            else
            {
                /*// ������x�᷽���ƶ���������z�᷽���ƶ�
                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized; // ��һ�����ٶȺ�ֱ�������ƶ���ͬ
                canMove = (moveDir.z < .5f || moveDir.z > +.5f) &&!Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // ������z�᷽���ƶ�
                    moveDir = moveDirZ;
                }
                else
                {
                    // ���ܳ��κη����ƶ�
                }*/
                Vector3 moveDirZ = new Vector3(0f, 0f, 0f).normalized; // ��һ�����ٶȺ�ֱ�������ƶ���ͬ

            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = (inputVector != Vector2.zero);
        float rotateSpeed = 8f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }



    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
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
}
