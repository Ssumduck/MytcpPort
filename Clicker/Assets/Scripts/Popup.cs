using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField]
    Text context, confirm;
    [SerializeField]
    public Button btn;

    public void Init(string _context, string _confirm)
    {
        context.text = _context;
        confirm.text = _confirm;
        btn.onClick.AddListener(() => Destroy(gameObject));
    }
}
