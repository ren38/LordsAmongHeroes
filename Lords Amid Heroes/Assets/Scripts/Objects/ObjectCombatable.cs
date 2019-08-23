using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCombatable : ObjectInteractable
{
    /*
     * Combatable objects cannot suffer from normal effects,
     * only burning and certain circumstantial effects.
     */
    static int objectType = 2;
    [SerializeField]
    protected bool dead = false;
    [SerializeField]
    private Color deadColor;

    void Awake()
    {
        setupCombatable();
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
    }
    */

    protected void setupCombatable()
    {
        deathObservers = new List<GameObjectObserver>();
        healingObservers = new List<GameObjectObserver>();
        rawHitObservers = new List<GameObjectObserver>();
        rawDamageObservers = new List<FloatAdjuster>();//still overseer, TODO switch to interpreter
        piercingHitObservers = new List<GameObjectObserver>();
        piercingDamageObservers = new List<FloatAdjuster>();//still overseer, TODO switch to interpreter
        deadColor = Color.Lerp(baseColor, Color.green, 0.4f);
    }

    [SerializeField]
    protected float maxHealth = 50;
    [SerializeField]
    protected float currentHealth = 50;

    public void takeDamageNoObs(float delta, ObjectInteractable source)//This should be for typeless damages; very rare. deltas should be positive.
    {
        if (!dead)
        {
            this.currentHealth -= delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public void takeRawHealing(float delta, ObjectInteractable source)//healing adds. deltas should be positive.
    {
        if (!dead)
        {
            this.currentHealth += delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            foreach (var observer in healingObservers)
            {
                observer.trigger(source.gameObject);
            }
        }
    }

    public void takeHealingNoObs(float delta, ObjectInteractable source)//healing over time adds. deltas should be positive.
    {
        if (!dead)
        {
            this.currentHealth += delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    public bool getDeathState()
    {
        return dead;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getPercentHealth()
    {
        return currentHealth / maxHealth;
    }

    public void changeCurrentHealthPercent(float percentChange)
    {
        if (!dead)
        {
            this.currentHealth += this.currentHealth * percentChange;
        }
    }

    public void changeMaxHealth(float delta)
    {
        if (!dead)
        {
            this.maxHealth += delta;
        }
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void changeMaxHealthPercent(float percentChange)
    {
        if (!dead)
        {
            this.maxHealth += this.maxHealth * percentChange;
        }
    }


    public override void beingHighlighted(bool value)
    {
        if (!dead)
        {
            if (value)
            {
                rend.material.color = highlighted;
            }
            else
            {
                rend.material.color = baseColor;
            }
        }
    }

    public override void beingSelected(bool value)
    {
        if (!dead)
        {
            if (value)
            {
                rend.material.color = selected;
            }
            else
            {
                rend.material.color = baseColor;
            }
        }
    }






    #region Death Handling
    /*
     * death subscription. Returns game object that caused death.
     */
    private List<GameObjectObserver> deathObservers;

    public void deathSubscribe(GameObjectObserver observer)
    {
        if(deathObservers == null)
        { Debug.Log("WTF"); }


        if (!deathObservers.Contains(observer))
            deathObservers.Add(observer);
        observer.connect(deathObservers);
    }

    private void notifyDeathSubscribers(GameObject source, bool nowDead)
    {
        foreach (var observer in deathObservers)
        {
            observer.trigger(source);
        }
    }
    #endregion

    #region Raw Damage Handling
    public float takeRawDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in rawHitObservers)
                {                    observer.trigger(source.gameObject);
                }
                foreach (var observer in rawDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            this.currentHealth -= delta;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                dead = true;
                notifyDeathSubscribers(source.gameObject, true);
                rend.material.color = deadColor;
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            return delta;
        }
        return 0.0f;
    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> rawHitObservers;

    public void rawHitSubscribe(GameObjectObserver observer)
    {
        if (!rawHitObservers.Contains(observer))
            rawHitObservers.Add(observer);
        observer.connect(rawHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> rawDamageObservers;

    public void rawDamageSubscribe(FloatAdjuster observer)
    {
        if (!rawDamageObservers.Contains(observer))
            rawDamageObservers.Add(observer);
        observer.connect(rawDamageObservers);
    }
    #endregion



    #region Piercing Damage Handling
    public float takePiercingDamage(float delta, ObjectInteractable source)//Damage subtracts. deltas should be positive.
    {
        if (!dead)
        {
            if (delta >= 0.5f)
            {
                foreach (var observer in piercingHitObservers)
                {
                    observer.trigger(source.gameObject);
                }
                foreach (var observer in piercingDamageObservers)
                {
                    delta = observer.trigger(delta);
                }
            }
            return takeRawDamage(delta, source);
        }
        else{ return 0; }
        
    }

    /*
     * hit subscription. Returns game object of the source.
     */
    private List<GameObjectObserver> piercingHitObservers;

    public void piercingHitSubscribe(GameObjectObserver observer)
    {
        if (!piercingHitObservers.Contains(observer))
            piercingHitObservers.Add(observer);
        observer.connect(piercingHitObservers);
    }

    /*
     * damage subscription. Returns the amount of damage taken.
     */
    private List<FloatAdjuster> piercingDamageObservers;

    public void piercingDamageSubscribe(FloatAdjuster observer)
    {
        if (!piercingDamageObservers.Contains(observer))
            piercingDamageObservers.Add(observer);
        observer.connect(piercingDamageObservers);
    }
    #endregion







        /*
         * healing subscription
         */

    private List<GameObjectObserver> healingObservers;

    public void healingSubscribe(GameObjectObserver observer)
    {
        if (!healingObservers.Contains(observer))
            healingObservers.Add(observer);
        observer.connect(healingObservers);
    }

    /* Damage Types:
     * Piercing
     * Bludgeoning
     * Slashing
     * Fire
     * Cold
     * Electric
     * Occult
     */ 

    /* Healing types:
     * radiant: creatures can only recieve this type of healing once every 30 seconds, else the healing = 0, etherial type creatures can only recieve this and raw healing
     * biological: only works on biological creatures
     * mechanical: only works on mechanical creatures
     * raw: all creatures can recieve this type, and there is no restriction to it.
     */
}
