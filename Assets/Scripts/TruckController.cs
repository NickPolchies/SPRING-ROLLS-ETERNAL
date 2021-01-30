using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


//TODO break some of this out maybe?
public class TruckController : MonoBehaviour
{
    public float startingCash;
    public float startingTemperature;
    public int truckGeneratorPower;
    public float cash;
    public float temperature;
    public int power;
    public float highTemperature, maxTemperature, lowTemperature, minTemperature;
    public float lifetimeCash;
    private float soundSwitch = 0;
    public TextMeshProUGUI cashText;
    public GameObject powerText;

    public float insulationRating = 10;

    private Grid grid;
    public EquipmentType[] startingEquipment = new EquipmentType[5];
    public Equipment equipmentTemplate;
    private MouseUI mouseUI; //TODO really hate how this works, maybe redo this
    //private bool powerCheck;

    private void OnValidate()
    {
        if (startingEquipment.Length > 4)
        {
            Debug.LogError("ERR: Setting starting equipment to more than 5 will cause runtime errors");
        }
    }

    void Start()
    {
        grid = GetComponent<Grid>();
        mouseUI = FindObjectOfType<MouseUI>();

        power = 0;
        cash = 999999;

        power += truckGeneratorPower;

        for (int i = 0; i < startingEquipment.Length; i++)
        {
            BuyEquipment(i, 0, startingEquipment[i]);
        }

        cash = startingCash;
        temperature = startingTemperature;

        lifetimeCash = startingCash;
    }

    void Update()
    {
        List<Equipment> equipment = grid.GetAllEquipment();

        float time = Time.time;
        UpdatePower(equipment);

        foreach (Equipment e in equipment)
        {
            RunEquipment(e, time);
        }
    }

    public void BuyEquipment(Vector2Int point, EquipmentType equipType)
    {
        BuyEquipment(point.x, point.y, equipType);
    }

    public void BuyEquipment(int col, int row, EquipmentType equipType)
    {
        int width = equipType.Size.GridSize.x;
        int height = equipType.Size.GridSize.y;
        if (height == 2 && row == 1)
        {
            row = 0;
        }
        if (width + col > grid.width)
        {
            col = grid.width - width;
        }

        if (col < 0 || equipType.Roof && row < 2 || !equipType.Roof && row >= 2)
        {
            return;
        }

        List<Equipment> previousEquipment = GetEquipmentAtGrid(col, row, width, height);

        float powerBack = 0;

        foreach (Equipment e in previousEquipment)
        {
            powerBack -= e.type.Power;
        }

        if (cash - equipType.Cost >= 0 && power + powerBack + equipType.Power >= 0)
        {
            Equipment equipment = Instantiate(equipmentTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            equipment.type = equipType;
            equipment.enabled = true;

            grid.AddEquipment(col, row, equipment);
            if (soundSwitch == 2)
            {
                GetComponent<AudioSource>().Play();
            }
            else
            {
                soundSwitch += 1;
            }

            equipment.SetMouseUI(mouseUI);

            cash -= equipType.Cost;
        }
        else if (cash - equipType.Cost < 0)
        {
           StartCoroutine(FlashCashText());
        }
        else if (power + powerBack + equipType.Power < 0)
        {
            StartCoroutine(FlashPowerText());
        }
        return;
    }

    private List<Equipment> GetEquipmentAtGrid(int col, int row, int width, int height)
    {
        List<Equipment> previousEquipment = new List<Equipment>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Equipment e = grid.GetEquipmentAt(col + i, row + j);

                if (e != null && !previousEquipment.Contains(e))
                {
                    previousEquipment.Add(grid.GetEquipmentAt(col + i, row + j));
                }
            }
        }

        return previousEquipment;
    }

    public Vector2Int GetMouseGridPosition()
    {
        return grid.ScreenToGridCoords((Vector2)Input.mousePosition);
    }

    public void HeatTransfer(float outsideTemp)
    {
        temperature += (outsideTemp - temperature) / insulationRating * Time.deltaTime;
        List<Equipment> equipment = grid.GetAllEquipment();

        foreach (Equipment e in equipment)
        {
            if (e.type.Solar)
            {
                e.UpdateSolarPower(outsideTemp);
            }
        }
    }

    private void RunEquipment(Equipment e, float time)
    {
        Equipment.Stats stats = e.UpdateStats(power, time);

        if (!ValidateCashFlow(e, stats))
        {
            stats.power = 0;
            stats.cash = 0;
            stats.heat = 0;
        }

        if (!e.type.Roof)
        {
            temperature += stats.heat * Equipment.tickLength;
        }

        cash += stats.cash;// * Equipment.tickLength;
        lifetimeCash += stats.cash > 0 ? stats.cash : 0;
    }

    private bool ValidateCashFlow(Equipment equipment, Equipment.Stats stats)
    {
        if(cash + stats.cash < 0)
        {
            equipment.ShutDown();
            return false;
        }
        return true;
    }

    public Equipment GetEquipmentAtMouse()
    {
        Vector2Int mouseGridCoords = GetMouseGridPosition();

        if (mouseGridCoords.x > -1)
        {
            return grid.GetEquipmentAt(GetMouseGridPosition());
        }
        return null;
    }

    private void UpdatePower(List<Equipment> equipment)
    {
        power = 0;

        foreach (Equipment e in equipment)
        {
            if (e.powerStage > 0)
            {
                power += e.GetStats().power;
            }
        }

        power += truckGeneratorPower;
    }

    IEnumerator FlashCashText()
    {
        cashText.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        cashText.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        cashText.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        cashText.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.1f);
        cashText.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        cashText.color = new Color32(255, 255, 255, 255);
    }
    IEnumerator FlashPowerText()
    {
        powerText.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        powerText.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        powerText.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        powerText.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        powerText.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        powerText.SetActive(true);
        yield return new WaitForSeconds(0.1f);



    }

    public void EndGame()
    {
        this.enabled = false;
    }
}
