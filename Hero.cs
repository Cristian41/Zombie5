using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using zom = NPC.Muertoviviente;
using DEMON = NPC.EsPundead;
using civbody = NPC.Ally;

// se crea la clase que tendrá todo lo relacionado con el heroe y la UI del hero como la vida y cuantos enemigos y aliados quedan en el mapa


public sealed class Hero : MonoBehaviour
{
    public Creacion OL;
    float distanciaA;
    float distanciaB;
    public float tempus;
    public int salud;
    public TextMeshProUGUI TextZombie;
    public TextMeshProUGUI TextDemon;
    public TextMeshProUGUI Textcitizen;
    public TextMeshProUGUI Textvida;
    GameObject[] citizens, zombies, Demons;
    public GameObject weapon;
    Civitasinfo civinfo = new Civitasinfo();
    zomboidinfo zombieinfo = new zomboidinfo();
    //crando todo lo relacionado al heroe, declarando las caracteristicas del arma, la posicion, el tamaño y declarando cuanta salud tendra el heroe en el juego
    void Start()
    {
        //StartCoroutine(Buscarnpcs());
        print("weapon");
        weapon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        weapon.transform.position = new Vector3(this.gameObject.transform.position.x + .314f, this.gameObject.transform.position.y + .314f, this.gameObject.transform.position.z + .314f);
        weapon.transform.localScale = new Vector3(.3f, .3f, .3f);
        weapon.transform.Rotate(90, 0, 0);
        weapon.transform.SetParent(this.gameObject.transform);
        weapon.AddComponent<Weapon>();
        salud = 100;
        this.gameObject.tag = "Hero";
        this.gameObject.name = "Hero";
        // Declarando los textos publicos que se mostraran en pantalla y que el jugador vá a ver
        TextZombie = GameObject.FindGameObjectWithTag("TextZombie").GetComponent<TextMeshProUGUI>();
        TextDemon = GameObject.FindGameObjectWithTag("TextDemon").GetComponent<TextMeshProUGUI>();
        Textcitizen = GameObject.FindGameObjectWithTag("TextCitizen").GetComponent<TextMeshProUGUI>();
        Textvida = GameObject.FindGameObjectWithTag("Textvida").GetComponent<TextMeshProUGUI>();
    }
    //se inicia el contador con la variable flotante declarada en la clase
    // y un if por si la salud del heroe llega a 0 se perdera la partida y se reiniciara
    public void Update()
    {
        tempus += Time.fixedDeltaTime;
        Textvida.text = salud.ToString();

        if (salud <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
    // Cuando el jugador collisiona con un zombie muestra sus respectivos mensajes
     void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie")
        {
            OL = FindObjectOfType <Creacion>();

            OL.mensajezombie.text = "Waaaarrrr quiero comer ";
        }
        if (collision.gameObject.tag == "Demon")
        {
            OL = FindObjectOfType<Creacion>();
            
            OL.mensajezombie.text = "Waaaarrrr quiero comer " + zombieinfo.antojo;
        }
    }
    //si el juagador toca el gameonject con el tag de la medicina su vida aumentara 5 puntos limite maximo de vida 100, al llegar a 100 no se le sumara mas puntos.

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Medicine")
        {
            salud += 5;
            if(salud >= 100)
            {
                salud = 100;
                other.gameObject.SetActive(false);
            }
        }
    }
    //Controlador del heroe para que se mueva horizontal y vertical respectivamente con sus teclas con velocidad aleatoria
    public sealed class MoverH : MonoBehaviour
    {
        Velocidad velocidad;
        public readonly float MovX;
        private void Start()
        {
            velocidad = new Velocidad(Random.Range(0.25f, 2f));
        }
        private void Update()
        {
            float MovX = Input.GetAxisRaw("Horizontal");
            float MovY = Input.GetAxisRaw("Vertical");
            transform.Translate(0f, 0f, MovY * velocidad.velo);
            transform.Rotate(0f, MovX * 2f, 0);
        }
    }  
}
// Clase para el desplazamiento read only del heroe
public sealed class Velocidad
{
    public readonly float velo;
    public Velocidad(float vel)
    {
        velo = vel;
    }
}
