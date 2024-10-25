using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    public Transform machineTargetZone; // Зона, куда предмет должен быть перемещён

    public List<int> requiredItems; // Список ID предметов, необходимых для победы
    private List<int> placedItems = new List<int>(); // Список для отслеживания помещённых предметов

    public void PlaceItem(GameObject item)
    {
        PickupItem pickupItem = item.GetComponent<PickupItem>();
        if (pickupItem != null && requiredItems.Contains(pickupItem.itemId))
        {
            // Перемещение предмета в зону машины
            item.transform.position = machineTargetZone.position;
            item.transform.SetParent(machineTargetZone);

            // Добавляем ID предмета в список
            placedItems.Add(pickupItem.itemId);
            Debug.Log("Item placed with ID: " + pickupItem.itemId);

            // Проверка на победу
            if (CheckWinCondition())
            {
                Debug.Log("All required items placed! You win!");
            }
        }
        else
        {
            Debug.Log("This item is not required by the machine.");
        }
    }

    private bool CheckWinCondition()
    {
        // Проверяем, совпадает ли список размещённых предметов с необходимыми
        foreach (int requiredId in requiredItems)
        {
            if (!placedItems.Contains(requiredId))
            {
                return false;
            }
        }
        return true;
    }
}


