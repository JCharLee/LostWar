using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColor : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Material[] objectColor = new Material[2];
    [SerializeField] private Light _light;

    private void Awake()
    {
        mesh = transform.GetChild(1).GetComponent<MeshRenderer>();
        _light = mesh.GetComponentInChildren<Light>();
    }

    private void Update()
    {
        if (!transform.parent.GetComponent<Lv3_Door>().isOpen)
        {
            mesh.material = objectColor[0];
            _light.color = Color.red;
        }
        else
        {
            mesh.material = objectColor[1];
            _light.color = Color.green;
        }
    }
}