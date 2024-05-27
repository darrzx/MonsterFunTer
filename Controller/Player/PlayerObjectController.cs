using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController
{
    private static PlayerObjectController instance = null;
    private int index, meat, potion;
    private float maxHealth, currHealth;
    private float maxStamina, currStamina;
    private float movementSpeed;
    public GameObject playerActive;

    private PlayerObjectController()
    {
        maxStamina = 100f;
        maxHealth = 100f;
        currHealth = maxHealth;
        currStamina = maxStamina;
        meat = 1;
        potion = 2;
        movementSpeed = 0.01f;
    }

    public static PlayerObjectController getInstance()
    {
        if (instance == null)
        {
            instance = new PlayerObjectController();
        }
        return instance;
    }

    public void setIndex(int number)
    {
        index = number;
    }

    public int getIndex()
    {
        return index;
    }

    public void setMeat(int meatCount)
    {
        meat = meatCount;
    }

    public int getMeat()
    {
        return meat;
    }

    public void setPotion(int potionCount)
    {
        potion = potionCount;
    }

    public int getPotion()
    {
        return potion;
    }

    public void setMaxHealth(float MaxHealthCount)
    {
        maxHealth = MaxHealthCount;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }

    public void setCurrHealth(float CurrHealthCount)
    {
        currHealth = CurrHealthCount;
    }

    public float getCurrHealth()
    {
        return currHealth;
    }

    public void setMaxStamina(float MaxStaminaCount)
    {
        maxStamina = MaxStaminaCount;
    }

    public float getMaxStamina()
    {
        return maxStamina;
    }

    public void setCurrStamina(float CurrStaminaCount)
    {
        currStamina = CurrStaminaCount;
    }

    public float getCurrStamina()
    {
        return currStamina;
    }

    public void setMovementSpeed(float MovementSpeedCount)
    {
        movementSpeed = MovementSpeedCount;
    }

    public float getMovementSpeed()
    {
        return movementSpeed;
    }
}
