using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private Vector3 centorPosition = new Vector3(0, 1.5f, 7.5f);
    private Vector3 rightPosition = new Vector3(12f, 2f, 10f);
    private Vector3 leftPosition = new Vector3(-12f, 2f, 10f);

    [SerializeField] private GameObject panelPrefab;

    // Start is called before the first frame update
    void Start()
    {
       initializePanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initializePanel()
    {
        Instantiate(panelPrefab, centorPosition, Quaternion.identity);
        Instantiate(panelPrefab, rightPosition, Quaternion.identity);
        Instantiate(panelPrefab, leftPosition, Quaternion.identity);
    }
}
