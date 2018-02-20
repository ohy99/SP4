using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

//public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

//    [HideInInspector]
//    public Transform parentToReturnTo = null;
//    [HideInInspector]
//    public Transform container = null;

//    private int indexToChange = 0;
//    private GameObject placeHolder = null;

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        placeHolder = new GameObject();
//        placeHolder.transform.SetParent( this.transform.parent );

//        indexToChange = 0;

//        // keep the scale of the card constant even when a card is remove from container
//        //LayoutElement le = placeHolder.AddComponent<LayoutElement>();
//        //le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
//        //le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
//        //le.flexibleWidth = 0;
//        //le.flexibleHeight = 0;

//        //placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex() );

//        //set parent to orignal (the container)
//        parentToReturnTo = this.transform.parent;
//        //placeHolderParent = parentToReturnTo;

//        //this.transform.SetParent(this.transform.parent.parent);
//        container = this.transform.parent;

//        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
//    }

//    public void OnDrag(PointerEventData eventData)
//    {
//        this.transform.position = eventData.position;

//        for(int i = 0; i < container.childCount; ++i)
//        {
//            if (this.transform.position.y <= container.GetChild(i).position.y + container.GetChild(i).localScale.y
//                && this.transform.position.y >= container.GetChild(i).position.y - container.GetChild(i).localScale.y
//               && this.transform.position.x <= container.GetChild(i).position.x + container.GetChild(i).localScale.x
//               && this.transform.position.x >= container.GetChild(i).position.x - container.GetChild(i).localScale.x)
//            {
//                indexToChange = i;
//                break;
//            }
//        }


//        //if (placeHolder.transform.parent != placeHolderParent)
//        //{
//        //    placeHolder.transform.SetParent(placeHolderParent);
//        //}

//        //int newSiblingIndex = placeHolderParent.childCount;

//        //Debug.Log("numChild" + placeHolderParent.childCount);
//        //for (int i = 0; i < placeHolderParent.childCount; i++)
//        //{
//        //    if (this.transform.position.y >= placeHolderParent.GetChild(i).position.y
//        //        && this.transform.position.x <= placeHolderParent.GetChild(i).position.x)
//        //    {
//        //        newSiblingIndex = i;

//        //        Debug.Log("nsIndex" + newSiblingIndex);

//        //        if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
//        //            newSiblingIndex--;


//        //        break;
//        //    }
//        //}

//        //placeHolder.transform.SetSiblingIndex(newSiblingIndex);
//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        //this.transform.SetParent(parentToReturnTo);
//        //this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
//        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
//        Destroy(placeHolder);
//    }
//}


public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [HideInInspector]
    public Transform parentToReturnTo = null;
    [HideInInspector]
    public Transform placeHolderParent = null;

    private GameObject placeHolder = null;

    private int startIndex = 0;
    private int endIndex = 0;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);

        // keep the scale of the card constant even when a card is remove from container
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        //Debug.Log("OnStartDrag: " + this.transform.GetSiblingIndex());
        startIndex = this.transform.GetSiblingIndex();
        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //set parent to orignal (the container)
        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;

        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("onDrag " + eventData.position);
        this.transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }

        int newSiblingIndex = placeHolderParent.childCount;
        // Debug.Log("numChild" + placeHolderParent.childCount);
        for (int i = 0; i < placeHolderParent.childCount; i++)
        {
            Debug.Log(placeHolderParent.GetChild(0).position);
            if (this.transform.position.y >= placeHolderParent.GetChild(i).position.y
                && this.transform.position.x <= placeHolderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

               // Debug.Log("nsIndex" + newSiblingIndex);

                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;


                break;
            }

           // Debug.Log("nothing happen");
        }

        placeHolder.transform.SetSiblingIndex(newSiblingIndex);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());

        endIndex = placeHolder.transform.GetSiblingIndex() - 1;
        InventoryManager.Instance.GetInventory("player").SwapSlots(startIndex, endIndex);

        //Debug.Log(InventoryManager.Instance.GetInventory("player").slot[endIndex]);
        //Debug.Log("sibilingIndex: " + placeHolder.transform.GetSiblingIndex()); //minus 1 for actual index

        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeHolder);
    }
}