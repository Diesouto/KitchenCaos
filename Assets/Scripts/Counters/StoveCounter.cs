using System;
using System.Collections;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
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
        Burnt
    }


    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    State state;
    float fryingTimer;
    FryingRecipeSO fryingRecipeSO;
    float burningTimer;
    BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
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
                    Frying();
                    break;
                case State.Fried:
                    Burning();
                    break;
                case State.Burnt:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter no tiene nada
            if (player.HasKitchenObject())
            {
                // Player tiene algo
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // El objeto que el jugador tiene se puede freir
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    // Cambiar el estado a Frying
                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
            }
            else
            {
                // Player no tiene nada
            }
        }
        else
        {
            // Counter tiene algo
            if (player.HasKitchenObject())
            {
                // Player tiene algo
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player tiene un plato
                    // Movemos ingrediente al plato
                    plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // Se ha añadido el ingrediente al plato
                        GetKitchenObject().DestroySelf();

                        // Cambiar el estado a Idle
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                }
            }
            else
            {
                // Player no tiene nada
                GetKitchenObject().SetKitchenObjectParent(player);

                // Cambiar el estado a Idle
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    private void Frying()
    {
        fryingTimer += Time.deltaTime;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)fryingTimer / fryingRecipeSO.fryingTimerMax
        });

        if (fryingTimer > fryingRecipeSO.fryingTimerMax)
        {
            // Fried
            fryingTimer = 0f;

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            // Cambiar el estado a Fried
            state = State.Fried;
            burningTimer = 0f;

            burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
            });
        }
    }

    private void Burning()
    {
        burningTimer += Time.deltaTime;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = (float)burningTimer / burningRecipeSO.burningTimerMax
        });

        if (burningTimer > burningRecipeSO.burningTimerMax)
        {
            // Burnt
            burningTimer = 0f;

            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

            // Cambiar el estado a Burnt
            state = State.Burnt;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
                state = state
            });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        if (fryingRecipeSO.input == inputKitchenObjectSO)
        {
            return fryingRecipeSO.output;
        }

        return null;
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
