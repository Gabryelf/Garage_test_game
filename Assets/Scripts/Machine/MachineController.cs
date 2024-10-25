using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    public Transform machineTargetZone; // ����, ���� ������� ������ ���� ���������

    public List<int> requiredItems; // ������ ID ���������, ����������� ��� ������
    private List<int> placedItems = new List<int>(); // ������ ��� ������������ ���������� ���������

    public void PlaceItem(GameObject item)
    {
        PickupItem pickupItem = item.GetComponent<PickupItem>();
        if (pickupItem != null && requiredItems.Contains(pickupItem.itemId))
        {
            // ����������� �������� � ���� ������
            item.transform.position = machineTargetZone.position;
            item.transform.SetParent(machineTargetZone);

            // ��������� ID �������� � ������
            placedItems.Add(pickupItem.itemId);
            Debug.Log("Item placed with ID: " + pickupItem.itemId);

            // �������� �� ������
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
        // ���������, ��������� �� ������ ����������� ��������� � ������������
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


