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
    
    public int powered;
    private int powerCycling;

    public static readonly float tickLength = 1;
    private float tickTimer;
    private float lastUpdateTime = Mathf.Infinity;
    private Stats stats;

    private Animator animator;
    private SpriteRenderer sprite;
    private ParticleSystem particles;
    public UnityEngine.UI.Image progressBar;
    private MouseUI mouseUI;

    private FloatingText floatingText;
    public TMP_FontAsset floatingTextFont;
    [SerializeField] BatteryIconManager batteryIconManager;

    //TODO clean this up. Fewer complex calls. Caching?
    private void Start()
    {
        GameObject graphics = Instantiate(type.Graphics, transform.position, Quaternion.identity);
        graphics.transform.SetParent(transform);
        graphics.transform.localScale = Vector3.one;

        BoxCollider2D collider = GetComponentInChildren<BoxCollider2D>();
        collider.offset = type.Size.ColliderOffset;
        collider.size = type.Size.ColliderSize;

        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
        powered = 1;

        tickTimer = 0;
        lastUpdateTime = Time.time;

        stats.cash = 0;
        stats.heat = 0;
        stats.power = 0;

        BoxCollider2D rect = gameObject.GetComponent<BoxCollider2D>();
        floatingText = new FloatingText(new Vector3(rect.offset.x, rect.offset.y, transform.position.z), transform, floatingTextFont);

        Transform progressBarContainer = progressBar.transform.parent;
        //progressBarContainer.gameObject.SetActive(type.CashFlow != 0);
        progressBarContainer.GetComponent<RectTransform>().localPosition += new Vector3(type.Size.ColliderOffset.x, 0, 0);
    }

    public void CyclePower(int powerIn)
    {
        if (type.Power == 0 || (type.PowerScaling == 0 && powered + powerCycling >= 2))
        {
            return;
        }

        int deltaPower;

        if(powerCycling > 0)
        {
            switch (powered)
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
            switch (powered)
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

        if(powerIn + deltaPower >= 0)
        {

            if (powerCycling > 0)
            {
                batteryIconManager.AddBattery();
            }
            else
            {
                batteryIconManager.RemoveBattery();
            }

            powered += powerCycling;

            if (powered > 0)
            {
                sprite.color = Color.white;
                animator.SetBool("Powered", true);

                if (particles)
                {
                    particles.Play();
                }
            }
            else
            {
                sprite.color = Color.grey;
                animator.SetBool("Powered", false);
                tickTimer = 0;

                if (particles)
                {
                    particles.Stop();
                }
            }
        }
    }

    public void Clicked(MouseButton button)
    {
        if(button == MouseButton.RightMouse)
        {
            powerCycling = -1;
        }
        if (button == MouseButton.LeftMouse)
        {
            powerCycling = 1;
        }
    }

    public Stats UpdateStats(int powerIn, float updateTime)
    {
        stats.heat = 0;
        stats.cash = 0;
        stats.power = 0;
        float deltaTime = updateTime - lastUpdateTime;

        if (deltaTime <= 0)
        {
            return stats;
        }

        floatingText.UpdateText(deltaTime);

        if (powerCycling != 0)
        {
            CyclePower(powerIn);
            powerCycling = 0;
        }

        if (powered > 0)
        {
            int scaleFactor = Mathf.Max(powered - 1, 0);
            stats.power = type.Power + scaleFactor * type.PowerScaling;
            stats.heat = type.Heat * deltaTime + scaleFactor * type.HeatScaling * deltaTime;

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

            //UpdateProgressBar();
        }

        lastUpdateTime = updateTime;

        return stats;
    }

    public Stats GetStats()
    {
        return stats;
    }

    private void UpdateProgressBar()
    {
        progressBar.fillAmount = tickTimer / tickLength;
    }

    public void SetMouseUI(MouseUI mouseUIIn)
    {
        mouseUI = mouseUIIn;
    }

    public void OnMouseEnter()
    {
        mouseUI.MouseEnter(type);
    }

    public void OnMouseExit()
    {
        mouseUI.MouseExit();
    }
}
