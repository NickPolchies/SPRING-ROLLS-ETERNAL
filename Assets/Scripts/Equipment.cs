using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Equipment : MonoBehaviour, Clickable
{
    public struct Stats
    {
        public float cash;
        public float heat;
        public int power;
    }

    public EquipmentType type;
    
    public int powerStage;
    private bool turnedOn;

    public enum PowerCycling{
        none =  0,
        up   =  1,
        down = -1
    };
    public PowerCycling powerCycling;

    public static readonly float tickLength = 1; //This is implemented poorly
    private float tickTimer;
    private float lastUpdateTime = Mathf.Infinity;
    private Stats stats;

    public AudioSource PowerUp;
    public AudioSource PowerDown;

    private Animator animator;
    private SpriteRenderer sprite;
    private ParticleSystem particles;
    public UnityEngine.UI.Image progressBar;
    private MouseUI mouseUI;

    private FloatingText floatingText;
    public TMP_FontAsset floatingTextFont;
    [SerializeField] private BatteryIconManager batteryIconManager = null;

    private void Start()
    {
        GameObject graphics = Instantiate(type.Graphics, transform.position, Quaternion.identity);
        graphics.transform.SetParent(transform);
        graphics.transform.localScale = Vector3.one;

        BoxCollider2D collider = GetComponentInChildren<BoxCollider2D>();
        collider.offset = type.Size.ColliderOffset;
        collider.size = type.Size.ColliderSize;
        floatingText = new FloatingText(new Vector3(collider.offset.x, collider.offset.y, transform.position.z), transform, floatingTextFont);

        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        powerStage = 1;

        tickTimer = 0;
        lastUpdateTime = Time.time;

        stats.cash = 0;
        stats.heat = 0;
        stats.power = 0;
        turnedOn = true;
    }

    public void CyclePower(int powerIn)
    {
        //If equipment doesn't use power or isn't cycling or is on with no power scaling and cycling up
        if (type.Power == 0 || powerCycling == PowerCycling.none || (type.PowerScaling == 0 && powerStage >= 1 && powerCycling == PowerCycling.up))
        {
            return;
        }

        int deltaPower;

        if(powerCycling == PowerCycling.up)
        {
            switch (powerStage)
            {
                case 0:
                    deltaPower = type.Power;
                    break;
                default:
                    deltaPower = type.PowerScaling;
                    break;
            }
        }
        else
        {
            switch (powerStage)
            {
                case 0:
                    return;
                case 1:
                    deltaPower = -type.Power;
                    break;
                default:
                    deltaPower = -type.PowerScaling;
                    break;
            }
        }

        if(powerIn + deltaPower >= 0 || deltaPower > 0)
        {
            powerStage += (int)powerCycling;

            if (powerCycling == PowerCycling.up)
            {
                batteryIconManager.AddBattery();
                PowerUp.Play();
            }
            else
            {
                batteryIconManager.RemoveBattery();
                PowerDown.Play();
            }

            if (powerStage > 0)
            {
                Awaken();
            }
            else
            {
                Suspend();
            }
        }
    }

    public void Clicked(MouseButton button)
    {
        if (type.Solar)
        {
            return;
        }

        if(button == MouseButton.RightMouse)
        {
            powerCycling = PowerCycling.down;
        }
        if (button == MouseButton.LeftMouse)
        {
            powerCycling = PowerCycling.up;
        }
    }

    public Stats UpdateStats(int powerIn, float updateTime)
    {
        float deltaTime = updateTime - lastUpdateTime;

        if (deltaTime <= 0)
        {
            return stats;
        }

        //Move the floating text
        floatingText.UpdateText(deltaTime);

        CyclePower(powerIn);
        powerCycling = PowerCycling.none;

        if (powerStage > 0)
        {
            CheckPower(powerIn);
            CalculateStats(powerIn, deltaTime);
        }
        else
        {
            stats.power = 0;
            stats.heat = 0;
            stats.cash = 0;
        }

        lastUpdateTime = updateTime;

        return stats;
    }

    private void CheckPower(int powerIn)
    {
        int scaleFactor = Mathf.Max(powerStage - 1, 0);
        stats.power = type.Power + scaleFactor * type.PowerScaling;

        if (stats.power < 0)
        {
            if (powerIn < 0 && turnedOn)
            {
                Suspend();
            }
            if (powerIn >= 0 && !turnedOn)
            {
                Awaken();
            }
        }
    }

    private void CalculateStats(int powerIn, float deltaTime)
    {
        if (!turnedOn && stats.power < 0)
        {
            stats.heat = 0;
            stats.cash = 0;
            return;
        }

        int scaleFactor = Mathf.Max(powerStage - 1, 0);
        stats.heat = type.Heat * deltaTime + scaleFactor * type.HeatScaling * deltaTime;
        stats.cash = 0;

        tickTimer += deltaTime;
        if (tickTimer > tickLength)
        {
            tickTimer -= tickLength;

            stats.cash = type.CashFlow * tickLength + scaleFactor * type.CashFlowScaling * tickLength;

            if (type.CashFlow != 0)
            {
                floatingText.SpawnText("$" + stats.cash, tickLength / 2);
            }
        }
    }

    public void UpdateSolarPower(float temperature)
    {
        powerStage = (int)temperature / (int)type.HeatScaling + 1;
    }

    public Stats GetStats()
    {
        return stats;
    }

    public void SetMouseUI(MouseUI mouseUIIn)
    {
        mouseUI = mouseUIIn;
    }

    public void OnMouseEnter()
    {
        mouseUI.MouseEnter(type);
        mouseUI.MouseEnter(this);
    }

    public void OnMouseExit()
    {
        mouseUI.MouseExit();
    }

    public void EndGame()
    {
        floatingText.DespawnText();
        this.enabled = false;
    }

    private void Awaken()
    {
        turnedOn = true;
        sprite.color = Color.white;
        animator.SetBool("Powered", true);

        if (particles)
        {
            particles.Play();
        }
    }

    private void Suspend()
    {
        turnedOn = false;
        sprite.color = Color.grey;
        animator.SetBool("Powered", false);

        if (particles)
        {
            particles.Stop();
        }
    }

    public void ShutDown()
    {
        powerStage = 1;
        powerCycling = Equipment.PowerCycling.down;
        CyclePower(int.MaxValue);
        batteryIconManager.RemoveAll();
    }
}
