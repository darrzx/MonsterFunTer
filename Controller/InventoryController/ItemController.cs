using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemController : MonoBehaviour
{

    public static int meatCount = 1;
    public static int potionCount = 2;
    public TextMeshProUGUI meatText, potionText;
    public GameObject meatItemFirst, potionItemFirst;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        meatText.text = meatCount.ToString();
        potionText.text = potionCount.ToString();

        // SET ANIMASI GANTI ITEM (T)
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (meatItemFirst.activeSelf)
            {
                meatItemFirst.SetActive(false);
                potionItemFirst.SetActive(true);
            }
            else
            {
                meatItemFirst.SetActive(true);
                potionItemFirst.SetActive(false);
            }

        }
    }
}
