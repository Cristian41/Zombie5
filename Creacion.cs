using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using zomboid = NPC.Muertoviviente;
using civitas = NPC.Ally;
using Demon = NPC.EsPundead;

public sealed class Creacion : MonoBehaviour
{
    //declaracion de los textos en pantalla como la cantidad de zombies ciudadanos y de demonios dentro del juego,
    public TextMeshProUGUI cantidadZombies;
    public TextMeshProUGUI cantidadDemomns;
    public TextMeshProUGUI cantidadcitizens;
    public TextMeshProUGUI mensajezombie;
    public int cantidadZombi;
    public int cantidadciv;
    public int cantidaddemons;
    public GameObject[] Zombies,citizens,Demons;
    public bool nomorezombies = false;
    public bool nomoreDemons = false;

    void Start()
    {
        new Generarinstancias();

    }
    // Utilizamos el canvas en escena para iplementar un hub y mostrar cuantas instancias se han generado en el momento
    // convertimos los valores enteros a texto y estos textos van a ser usados por los textos de interfaz
     void Update()
    {
        Zombies = GameObject.FindGameObjectsWithTag("Zombie");
        citizens = GameObject.FindGameObjectsWithTag("Citizen");
        Demons = GameObject.FindGameObjectsWithTag("Demon");
        foreach (GameObject Elementoz in Zombies)
        {
            cantidadZombi = Zombies.Length;
        }
        foreach (GameObject ElementoD in Demons)
        {
            cantidaddemons = Demons.Length;
        }
        foreach (GameObject Elementociv in citizens)
        {
            cantidadciv = citizens.Length;
        }
        if(Zombies.Length == 0)
        {
            cantidadZombies.text = 0.ToString();
            nomorezombies = true;
        }
        else
        {
            cantidadZombies.text = cantidadZombi.ToString();
        }
        if(Demons.Length == 0)
        {
            cantidadDemomns.text = 0.ToString();
            nomoreDemons = true;
        }
        else
        {
            cantidadDemomns.text = cantidaddemons.ToString();
        }
        if (citizens.Length == 0)
        {
            cantidadcitizens.text = 0.ToString();
        }
        else
        {
            cantidadcitizens.text = cantidadciv.ToString();
        }
        cantidadZombies.text = cantidadZombi.ToString();
        cantidadDemomns.text = cantidaddemons.ToString();

        if(nomorezombies && nomoreDemons == true)
        {
            SceneManager.LoadScene(0);
        }
    }
}
// se generan al azar las instancias y se les asigna un componente que va determinado por un rango aleatorio
// todo esto se aplica sobre las variables locales tipo gameobject  
 class Generarinstancias 
{
    public GameObject recipiente;
    public GameObject Bandage;
    public readonly int minInstancias = Random.Range(5, 16);
    int asignador = 0;
    public int typeofenemy;
    const int MAX = 26;
    public Generarinstancias()
    {
        for(int i = 0; i < 3; i++)
        {
            Bandage = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Bandage.tag = "Medicine";
            Bandage.AddComponent<Rigidbody>().useGravity = false;
            Bandage.GetComponent<Collider>().isTrigger = true;
            Bandage.transform.Rotate(0, 0, 0);
            Bandage.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            Bandage.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            Bandage.GetComponent<Renderer>().material.color = Color.clear; ;

        }
        int rnd = Random.Range(minInstancias, MAX);
        recipiente = GameObject.CreatePrimitive(PrimitiveType.Cube);
        recipiente.AddComponent<Camera>();
        recipiente.AddComponent<Hero>();

        recipiente.AddComponent<Hero.MoverH>();
        recipiente.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));

        for (int i = 0; i < rnd; i++)
        {   
            int selecionar = Random.Range(0, 2);
            if (selecionar == 0)
            {
                recipiente = GameObject.CreatePrimitive(PrimitiveType.Cube);
                recipiente.AddComponent<civitas.Ciudadanos>();
                recipiente.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
            if (selecionar == 1)
            {
                recipiente = GameObject.CreatePrimitive(PrimitiveType.Cube);
                typeofenemy = Random.Range(0, 2);
                if (typeofenemy == 0)
                    recipiente.AddComponent<zomboid.Zombie>();
                else
                    recipiente.AddComponent<Demon.Zombieesp>();
                recipiente.transform.position = new Vector3(Random.Range(-20, 21), 0, Random.Range(-20, 21));
            }
        }
    }
}