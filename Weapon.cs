using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase encargada de gestionar todo lo relacionado con el arma un array de gameobject de balas, y cuanta municion le queda.

public sealed class Weapon : MonoBehaviour
{
    public Hero Posicion;
    public GameObject[] balas;
    public GameObject bala;
    public int cuenta = 0;
    public bool ammo = true;
    //todo lo basico de la bala que se lanzara como el rigibody, la gravedad en false
    void Start()
    {
        Posicion = FindObjectOfType<Hero>();
        balas = new GameObject[100];
        for(int i = 0; i < balas.Length; i++)
        {
            bala = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bala.AddComponent<Rigidbody>();
            bala.GetComponent<Rigidbody>().useGravity = false;
            bala.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            
            bala.GetComponent<Collider>().isTrigger = false;
            bala.transform.position = new Vector3(10000, 10000, 10000);
            bala.transform.localScale = new Vector3(.3f, .3f, .3f);
            bala.tag = "Bullet";
            bala.SetActive(false);
            balas[i] = bala;
        }
    }
   // un condicional por si el booleano sea verdadero se pueda dispara las balas con la tecla space y si las balas llegan 100 se acabaran las balas y no se podra disparar mas
    void Update()
    {
        if (ammo)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                balas[cuenta].transform.position = new Vector3(Posicion.weapon.transform.position.x, Posicion.weapon.transform.position.y, Posicion.weapon.transform.position.z);
                balas[cuenta].transform.rotation = Posicion.weapon.transform.rotation;
                balas[cuenta].SetActive(true);
                balas[cuenta].GetComponent<Rigidbody>().AddForce(transform.up * 500f);
                cuenta += 1;
            }
        }
        if (cuenta == 100)
            ammo = false;
    }
}
