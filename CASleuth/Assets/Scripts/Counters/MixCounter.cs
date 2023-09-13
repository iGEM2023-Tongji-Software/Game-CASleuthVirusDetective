using UnityEngine;
using System;
public class MixCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnObjectPlaced;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    [SerializeField] private String type;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;
    private void Start()
    {
        state = State.Idle;
        fryingRecipeSO = null;
        burningRecipeSO = null;
    }
    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        // 煎好了
                        fryingTimer = 0f;
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        Debug.Log("Object fried!");
                        state = State.Fried;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }
                    break;
                case State.Fried:
                    //burningTimer += Time.deltaTime;

                    //OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    //{
                    //    progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    //});

                    //if (burningTimer > burningRecipeSO.burningTimerMax)
                    //{
                    //    // 煎好了
                    //    fryingTimer = 0f;
                    //    GetKitchenObject().DestroySelf();
                    //    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                    //    Debug.Log("Object burned!");
                    //    state = State.Burned;

                    //    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    //    {
                    //        progressNormalized = 0f
                    //    });
                    //}
                    break;
                case State.Burned:
                    break;
            }
            Debug.Log(state);
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 柜子上没有物品
            if (player.HasKitchenObject())
            {
                // 角色有物品，且是等待合成的RNA或DNA或蛋白质，放置物品
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())|| player.GetKitchenObject().GetKitchenObjectSO().objectName==type)
                {       
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                // 角色没有物品
            }
        }
        else
        {
            // 柜子上有物品
            if (player.HasKitchenObject())
            {
                // 角色有物品 
                if(GetKitchenObject().GetKitchenObjectSO().objectName.Contains("gRNA"))
                {
                    if(player.GetKitchenObject().GetKitchenObjectSO().objectName==type)
                    {
                        player.GetKitchenObject().DestroySelf();
                        fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Frying;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        fryingTimer = 0f;

                        OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                    }
                    else
                    {
                        Debug.Log("Wrong Mix!!");
                    }
                }
                else if(GetKitchenObject().GetKitchenObjectSO().objectName == type)
                {
                    if(player.GetKitchenObject().GetKitchenObjectSO().objectName.Contains("gRNA"))
                    {
                        GetKitchenObject().DestroySelf();
                        player.GetKitchenObject().SetKitchenObjectParent(this);
                        fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        state = State.Frying;
                        fryingTimer = 0f;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });
                    }
                    else
                    {
                        Debug.Log("Wrong Mix!!");
                    }
                }

            }
            else
            {
                // 角色没有物品，拾取物品
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });

                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
