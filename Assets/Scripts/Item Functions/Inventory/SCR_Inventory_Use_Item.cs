using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.VisualScripting;

public class SCR_Inventory_Use_Item : MonoBehaviour
{
    enum playmode { Singleplayer, Multiplayer }
    [SerializeField] playmode playMode;
    SCR_Inventory_System_Singleplayer inventory;
    SCR_Inventory_Visual visualInventory;
    SCR_Inventory_Visual_Multiplayer visualInventoryMultiplayer;
    [Header("Related Equipment")]
    [SerializeField] SCR_Flashlight_Non_VR flashlight;
    [SerializeField] List<SCR_FuseBox> fuseBoxes;
    [SerializeField] List<SCR_Key_Card_Reader> keyReaders;
    [SerializeField] SCR_Squeaky_Toy_Functionality[] squeakyToys;
    [SerializeField] SCR_Inventory_Item_Data mrWhiskarsData;
    [SerializeField] SCR_Inventory_Item_Data msBunnyData;
    [Header("Item IDs")]
    [SerializeField] string[] itemID;
    [Header("Item Amount Text")]
    [SerializeField] TextMeshProUGUI[] amountIndicators;
    [Header("Item Amount Text")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] List<AudioClip> audioClips;
    [Header("Icons")]
    [SerializeField] Image[] itemIcons;
    // Start is called before the first frame update
    void Start()
    {
        inventory = SCR_Inventory_System_Singleplayer.current;

        visualInventoryMultiplayer = GetComponent<SCR_Inventory_Visual_Multiplayer>();

        visualInventory = GetComponent<SCR_Inventory_Visual>();

        foreach (GameObject fuseBox in GameObject.FindGameObjectsWithTag("FuseBox"))
            fuseBoxes.Add(fuseBox.GetComponent<SCR_FuseBox>());

        foreach (GameObject keyReader in GameObject.FindGameObjectsWithTag("KeycardReader"))
            keyReaders.Add(keyReader.GetComponent<SCR_Key_Card_Reader>());
    }

    // Update is called once per frame
    void Update()
    {
        ShowItemAmountLeft();
        //ShowItemIcons();
    }

    void ShowItemAmountLeft()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[0] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[0].text = item.stackSize.ToString();
            }

            if (itemID[1] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[1].text = item.stackSize.ToString();
            }

            if (itemID[2] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[2].text = item.stackSize.ToString();
            }

            if (itemID[3] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[3].text = item.stackSize.ToString();
            }

            if (itemID[4] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[4].text = item.stackSize.ToString();
            }

            if (itemID[5] == item.itemData.itemID && item.stackSize >= 0)
            {
                amountIndicators[5].text = item.stackSize.ToString();
            }
        }
    }

    void ShowItemIcons()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        Color alphaColor;

        foreach (Inventory_Item item in inventoryCopy)
        {
            
        }
    }

    public void UseBatteries()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[0] == item.itemData.itemID && item.stackSize > 0)
            {
                if (playMode == playmode.Singleplayer)
                {
                    flashlight.RefillBatteries();
                    inventory.SubtractItem(item.itemData);
                    amountIndicators[0].text = item.stackSize.ToString();
                }
                else if (playMode == playmode.Multiplayer)
                {
                    inventory.SubtractItem(item.itemData);
                    amountIndicators[0].text = item.stackSize.ToString();
                }
            }
        }
    }

    public void UseFuzes()
    {
        Debug.Log("usefuse function was called");
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        Debug.Log("inventory copy size " + inventoryCopy.Count + "");

        foreach (Inventory_Item item in inventoryCopy)
        {
            foreach (SCR_FuseBox fuseBox in fuseBoxes)
            {
                if (fuseBox.canInsertFuse)
                {
                    if (itemID[1] == item.itemData.itemID && item.stackSize > 0 && !fuseBox.isActivated)
                    {
                        Debug.Log("Player successfully attempted to use fuse");
                        fuseBox.FillFusebox();
                        inventory.SubtractItem(item.itemData);
                        amountIndicators[1].text = item.stackSize.ToString();

                        float randomPitch = UnityEngine.Random.Range(0.8f, 1.2f);

                        audioSource.pitch = randomPitch;

                        audioSource.PlayOneShot(audioClips[0]);
                    }
                    else
                    {
                        Debug.Log("I was allowed to insert fuse but still failed. \n i tried with item id: " + item.itemData.itemID + "\nwhich has a stacksize of " + item.stackSize + "\nand is the fusebox activated? " + fuseBox.isActivated);
                    }
                }
                else
                {
                    Debug.Log("I cannot insert fuse.");
                }
            }
        }
    }

    public void UseLevel1Keycard()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            foreach (SCR_Key_Card_Reader locks in keyReaders)
            {
                if (locks.canReadCard && locks.canActivate)
                {
                    if (itemID[2] == item.itemData.itemID && item.stackSize > 0 && locks.canReadCard == true && itemID[2] == locks.keycardItemID)
                    {
                        locks.ReadCard();
                        audioSource.PlayOneShot(audioClips[1]);
                    }
                }
            }
        }
    }

    public void UseLevel2Keycard()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            foreach (SCR_Key_Card_Reader locks in keyReaders)
            {
                if (locks.canReadCard && locks.canActivate)
                {
                    if (itemID[4] == item.itemData.itemID && item.stackSize > 0 && locks.canReadCard == true && itemID[4] == locks.keycardItemID)
                    {
                        locks.ReadCard();
                        audioSource.PlayOneShot(audioClips[1]);
                    }
                }
            }
        }
    }

    public void UseMrWhiskars()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[3] == item.itemData.itemID && item.stackSize > 0 && !squeakyToys[0].isHolding)
            {
                squeakyToys[0].BringUpToy();
                inventory.SubtractItem(item.itemData);
                amountIndicators[3].text = item.stackSize.ToString();
                if (playMode == playmode.Singleplayer)
                {
                    visualInventory.ChangeInventoryState();
                }
                else if (playMode == playmode.Multiplayer)
                {
                    visualInventoryMultiplayer.ChangeInventoryState();
                }

                if (squeakyToys[1].isHolding)
                {
                    squeakyToys[1].BringDownToy();
                    inventory.AddItem(msBunnyData);
                }
            }
        }
    }

    public void UseMsBunny()
    {
        List<Inventory_Item> inventoryCopy = new List<Inventory_Item>(inventory.inventory);

        foreach (Inventory_Item item in inventoryCopy)
        {
            if (itemID[5] == item.itemData.itemID && item.stackSize > 0 && !squeakyToys[1].isHolding)
            {
                squeakyToys[1].BringUpToy();
                inventory.SubtractItem(item.itemData);
                amountIndicators[5].text = item.stackSize.ToString();
                if (playMode == playmode.Singleplayer)
                {
                    visualInventory.ChangeInventoryState();
                }
                else if (playMode == playmode.Multiplayer)
                {
                    visualInventoryMultiplayer.ChangeInventoryState();
                }

                if (squeakyToys[0].isHolding)
                {
                    squeakyToys[0].BringDownToy();
                    inventory.AddItem(mrWhiskarsData);
                }
            }
        }
    }
}
