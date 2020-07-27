using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Equipment : MonoBehaviour, Clickable
{
    public struct Stats
    {
        public float cash;
        public float heat;
        public float power;
    }

    public float thermalRating;
    public int purchaseCost;
    public float cashFlow;
    public float power;
    public bool powered;
    public bool powerCycling;

    public int width, height;
    public bool roof;

    public static readonly float tickLength = 1;
    private float tickTimer;
    private float lastUpdateTime;
    private Stats stats;

    private Animator animator;
    private SpriteRenderer sprite;
    public UnityEngine.UI.Image progressBar;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        powered = true;

        tickTimer = 0;
        lastUpdateTime = 0;

        stats.cash = 0;
        stats.heat = 0;
        stats.power = 0;
    }

    public void CyclePower()
    {
        if (power >= 0 && cashFlow > 0)
        {
            return;
        }

        powered = !powered;

        if (powered)
        {
            sprite.color = Color.white;
            animator.SetBool("Powered", true);
        }
        else
        {
            sprite.color = Color.grey;
            animator.SetBool("Powered", false);
            tickTimer = 0;
        }
    }

    public void Clicked(MouseButton button)
    {
        if(button == MouseButton.RightMouse)
        {
            powerCycling = true;
        }
    }

    public Stats UpdateStats(float powerIn, float updateTime)
    {
        stats.heat = 0;
        stats.cash = 0;
        stats.power = 0;

        if (updateTime == lastUpdateTime)
        {
            return stats;
        }

        if (powered)
        {
            stats.heat = thermalRating * Time.deltaTime;

            tickTimer += Time.deltaTime;
            if (tickTimer > tickLength)
            {
                tickTimer -= tickLength;

                stats.cash = cashFlow * tickLength;
            }

            UpdateProgressBar();
        }

        if (powerCycling)
        {
            float requestedPower = powered ? -power : power;
            if(requestedPower + powerIn >= 0)
            {
                CyclePower();
                stats.power = requestedPower;
            }

            powerCycling = false;
        }

        lastUpdateTime = updateTime;

        return stats;
    }

    public Stats GetStats()
    {
        return stats;
    }

    //TODO add a progressbar and have this update it
    private void UpdateProgressBar()
    {
        progressBar.fillAmount = tickTimer / tickLength;
    }
}
