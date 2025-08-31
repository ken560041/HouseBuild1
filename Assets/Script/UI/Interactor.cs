using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class Interactor : MonoBehaviour
{
    // Start is called before the first frame update

    InputAction interactorAction;
    public float interactRange = 2f;
    public LayerMask interactableLayer;
    private IInteractable currentInteractable;

    public GameObject interaction_Info_UI; // UI hiển thị thông tin
    public TMP_Text interaction_text;     // TextMeshPro component

    private List<InteractableObject> previousInteractables = new List<InteractableObject>();

    private int selectedIndex = 0; // vị trí lựa chọn hiện tại

    private List<IInteractable> interactables = new List<IInteractable>();

    void Start()
    {
        interactorAction = GetComponent<PlayerInput>().actions["interactorAction"];  


    }

    // Update is called once per frame
    void Update()
    {
        UpdateInteractables();
        
        HandleScrollInput();
        HandleInteraction();

    }

    void UpdateInteractables()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);

        /*var sortedInteractables = hitColliders.Select(hit => new InteractableObject
        {
            _collider = hit,
            Distance = Vector3.Distance(transform.position, hit.transform.position)
        })
        .OrderBy(obj => obj.Distance)
        .Take(4)
        .ToList();*/

        // sửa lại

        var sortedInteractables = hitColliders
        .GroupBy(hit => hit.transform) // Lọc theo transform của object
        .Select(group => new InteractableObject
        {
            _collider = group.First(), // Lấy collider đầu tiên của object
            Distance = Vector3.Distance(transform.position, group.Key.position)
         })
        .OrderBy(obj => obj.Distance)
        .Take(4)
        .ToList();


        // Kiểm tra nếu danh sách không thay đổi, không cần render lại
        if (!AreListsEqual(previousInteractables, sortedInteractables))
        {
            previousInteractables = sortedInteractables;
            selectedIndex = 0;
            SelectionManager.Instance.RenderButton(sortedInteractables);
        }

        // Cập nhật danh sách trước đó
        


        
       
    }




    void GetInteractable()
    {
        if (interactorAction.triggered)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange, interactableLayer);
            
            var sortedInteractables = hitColliders.Select(hit => new InteractableObject
            {
                _collider = hit,
                Distance = Vector3.Distance(transform.position, hit.transform.position)
            })
            .OrderBy(obj => obj.Distance)
            .Take(3)
            .ToList();


            foreach (Collider _collider in hitColliders)
            {
                if(_collider.TryGetComponent(out NPC _npc))
                {
                    _npc.Interact();
                }

                if(_collider.TryGetComponent(out ItemPickUp _item))
                {
                    _item.Interact();


                }
            }
        }
    }

    void HandleScrollInput()
    {
        if (previousInteractables.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + previousInteractables.Count) % previousInteractables.Count;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % previousInteractables.Count;
        }

        SelectionManager.Instance.UpdateSelectInteractor(previousInteractables, selectedIndex);
    }


    void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && previousInteractables.Count > 0 )
        {
            Collider _collider= previousInteractables[selectedIndex]._collider;

            if (_collider.TryGetComponent(out NPC _npc))
            {
                _npc.Interact();
            }

            if (_collider.TryGetComponent(out ItemPickUp _item))
            {
                _item.Interact();


            }
            if(_collider.TryGetComponent(out DoorObject doorObject))
            {
                doorObject.Interact();
            }
            if(_collider.TryGetComponent(out WindowObject windowObject))
            {
                windowObject.Interact();
            }
        }
    }


    bool AreListsEqual(List<InteractableObject> oldList, List<InteractableObject> newList)
    {
        if (oldList.Count != newList.Count) return false;

        for (int i = 0; i < oldList.Count; i++)
        {
            if (oldList[i]._collider != newList[i]._collider)
            {
                return false;
            }
        }

        return true;
    }
}
