using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    //only main camera idiot
    private Camera playerCamera;
    //[SerializeField]
    //the layers the items to pickup
    //will be on duh
    //private LayerMask layerMaskItem;
    
    [SerializeField]
    //the layers the triggers will be on duh
    private LayerMask layerMaskTrigger;
    //how long it takes to pick up on item
    
    private float pickupTime;
    [SerializeField]
    //the root of the images (progess bar should be here too)
    private RectTransform pickupImageRoot;
    [SerializeField]
    //the ring around the image
    private Image pickupProgressImage;
    [SerializeField]

    //private DialogueTrigger trigger;
    private ItemSlotTest itemBeingPickedUp;
    private Dialogue dialogueInteract;
    private CutsceneStart cutsceneStart;
    private float currentPickupTimerElapsed;

    // Update is called once per frame
    void Update()
    {
        SelectItemBeingPickedUpFromRay();
        SelectDialogueBeingPickedByRay();
        SelectCutsceneBeingPickedByRay();

        if (HasItemTargetted())
        {

            pickupImageRoot.gameObject.SetActive(true);

            if(GameHandler.Instance != null)
            {
                if (GameHandler.Instance.interacting)
                {
                    IncrementPickupProgressAndTryCompleteItem();
                }
                else
                {
                    currentPickupTimerElapsed = 0f;
                }
                UpdatePickupProgressImage();
            }
            
        }

        if (HasDialogueTargetted())
        {
            pickupImageRoot.gameObject.SetActive(true);

            if(GameHandler.Instance != null)
            {
                if (GameHandler.Instance.interacting)
                {
                    IncrementPickupProgressAndTryDialogue();
                }
                else
                {
                    currentPickupTimerElapsed = 0f;
                }
                UpdatePickupProgressImage();
            }
        }

        if (HasCutsceneTargetted())
        {
            pickupImageRoot.gameObject.SetActive(true);

            if(GameHandler.Instance != null)
            {
                if (GameHandler.Instance.interacting)
                {
                    IncrementPickupProgressAndTryCutscene();
                }
                else
                {
                    currentPickupTimerElapsed = 0f;
                }
                UpdatePickupProgressImage();
            }
        }

        if (!HasItemTargetted() && !HasDialogueTargetted() && !HasCutsceneTargetted()) 
        {
            pickupImageRoot.gameObject.SetActive(false);
            currentPickupTimerElapsed = 0f;
        }

        if(itemBeingPickedUp != null && HasItemTargetted())
        {
            pickupTime = itemBeingPickedUp.item.pickUpTime;
        }

        if(dialogueInteract != null && HasDialogueTargetted())
        {
            pickupTime = 0.1f;
        }

        if(cutsceneStart != null && HasCutsceneTargetted()) 
        {
            pickupTime = 0.1f;
        }
    }

    private bool HasItemTargetted()
    {
        return itemBeingPickedUp != null;
    }

    private bool HasDialogueTargetted()
    {
        return dialogueInteract != null;
    }
    private bool HasCutsceneTargetted()
    {
        return cutsceneStart != null;
    }

    private void IncrementPickupProgressAndTryCompleteItem()
    {
        currentPickupTimerElapsed += Time.deltaTime;
        if(currentPickupTimerElapsed >= pickupTime)
        {
            //add item to inv e.g Inventory.addItem()
            //itemBeingPickedUp.();
        }
    }
    private void IncrementPickupProgressAndTryDialogue()
    {
        currentPickupTimerElapsed += Time.deltaTime;
        if(currentPickupTimerElapsed >= pickupTime && !dialogueInteract.alreadyTalking)
        {
            dialogueInteract.StartDialogue();
        }
    }

    private void IncrementPickupProgressAndTryCutscene()
    {
        currentPickupTimerElapsed += Time.deltaTime;
        if(currentPickupTimerElapsed >= pickupTime)
        {
            cutsceneStart.Activate();
        }
    }

    private void UpdatePickupProgressImage()
    {
        float pct = currentPickupTimerElapsed / pickupTime;
        pickupProgressImage.fillAmount = pct;
    }

    private void SelectItemBeingPickedUpFromRay()
    {
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one / 2f);
        Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 2f))
        {
            var hitItem = hitInfo.collider.GetComponent<ItemSlotTest>();

            if(hitItem == null)
            {
                itemBeingPickedUp = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                itemBeingPickedUp = hitItem;
            }
        }
        else
        {
            itemBeingPickedUp = null;
        }
    }

    private void SelectDialogueBeingPickedByRay()
    {
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one / 2f);
        //Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 2f))
        {
            var hitItem = hitInfo.collider.GetComponent<Dialogue>();

            if(hitItem == null)
            {
                dialogueInteract = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp && !hitItem.alreadyTalking)
            {
                dialogueInteract = hitItem;
            }
        }
        else
        {
            dialogueInteract = null;
        }
    }

    private void SelectCutsceneBeingPickedByRay()
    {
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one / 2f);
        //Debug.DrawRay(ray.origin, ray.direction * 2f, Color.red);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, 2f))
        {
            var hitItem = hitInfo.collider.GetComponent<CutsceneStart>();

            if(hitItem == null)
            {
                cutsceneStart = null;
            }
            else if (hitItem != null && hitItem != itemBeingPickedUp)
            {
                cutsceneStart = hitItem;
            }
        }
        else
        {
            dialogueInteract = null;
        }
    }
}
