using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 20f;
    public float horizontalSpeed;
    public float jumpForce = 5f; // Сила прыжка
    private bool isGrounded; // Флаг, указывающий на то, что игрок на земле

    private float horizontalInput;
    private float forwardInput;

    public Transform targetZone; // Зона, куда перемещается предмет после взятия
    private GameObject heldItem = null;

    private Rigidbody rb; // Компонент Rigidbody для управления физикой

    // Ссылка на объект UI
    public GameObject successMessage; // Объект, который активируется при успешной загрузке
    public Text itemCountText; // Текст для отображения количества загруженных предметов
    public GameObject winMessage; // Объект, который активируется при победе
    public GameObject message_item;

    private int totalItemsToLoad = 5; // Общее количество предметов для загрузки (измените по необходимости)
    private int itemsLoaded = 0; // Количество загруженных предметов

    void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();

        // Скрываем сообщения об успехе и победе при старте
        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }
        if (winMessage != null)
        {
            winMessage.SetActive(false);
        }

        // Обновляем текстовое сообщение о количестве загруженных предметов
        UpdateItemCountText();
    }

    void Update()
    {
        // Управление движением игрока
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Rotate(Vector3.up, Time.deltaTime * horizontalSpeed * horizontalInput);

        // Проверка нажатия клавиши E для попытки положить предмет
        if (Input.GetKeyDown(KeyCode.E) && heldItem != null)
        {
            TryPlaceItem();
        }

        // Проверка нажатия пробела для прыжка
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если у игрока нет предмета и он коснулся подбираемого предмета
        if (heldItem == null)
        {
            PickupItem item = other.GetComponent<PickupItem>();
            if (item != null)
            {
                // Устанавливаем объект как удерживаемый
                heldItem = item.gameObject;
                heldItem.transform.position = targetZone.position;
                heldItem.transform.SetParent(targetZone);
                message_item.SetActive(true);
                Debug.Log("Picked up item with ID: " + item.itemId);
            }
        }
    }

    private void TryPlaceItem()
    {
        // Проверка, находится ли игрок рядом с машиной
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        foreach (Collider hit in hits)
        {
            MachineController machine = hit.GetComponent<MachineController>();
            if (machine != null)
            {
                machine.PlaceItem(heldItem);
                itemsLoaded++; // Увеличиваем количество загруженных предметов
                successMessage.SetActive(true);
                UpdateItemCountText(); // Обновляем текстовое сообщение
                StartCoroutine(HideSuccessMessage(3));

                // Проверяем, не достигли ли мы цели по количеству предметов
                if (itemsLoaded >= totalItemsToLoad)
                {
                    WinGame();
                }

                heldItem = null; // Сбрасываем ссылку на удерживаемый предмет
                Debug.Log("Item placed in machine.");
                message_item.SetActive(false);
                break;
            }
        }
    }

    private void UpdateItemCountText()
    {
        if (itemCountText != null)
        {
            itemCountText.text = " " + itemsLoaded + " / " + totalItemsToLoad;
        }
    }

    private void WinGame()
    {
        if (winMessage != null)
        {
            winMessage.SetActive(true); // Активируем сообщение о победе
        }
        Debug.Log("You Win!");
    }

    private IEnumerator HideSuccessMessage(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (successMessage != null)
        {
            successMessage.SetActive(false);
        }
    }

    private void Jump()
    {
        // Применяем силу прыжка к Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Устанавливаем флаг, что игрок не на земле
        Debug.Log("Jumped!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, соприкасается ли игрок с землёй
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Устанавливаем флаг, что игрок на земле
        }
    }


    // Метод для перезапуска текущей сцены
    public void RestartScene()
    {
        // Получаем текущую сцену
        Scene currentScene = SceneManager.GetActiveScene();
        // Загружаем текущую сцену заново
        SceneManager.LoadScene(currentScene.name);
    }
}




