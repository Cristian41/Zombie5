using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zom = NPC.Muertoviviente;
using Deemon = NPC.EsPundead;
//Declaracion del namespace NPC 
namespace NPC
{
    //Declaracion del nameSpace Ally que esta dentro del namespace mayor NPC 
    namespace Ally
    {
        // Declaraciones necesarias para ciudadano como la informacion del script Datos a utilizar el estado si ya se volvio un zombi o no y la distacia
        // a recorrer del ciudadno y al informacion basica del ciudadno
        //ademas se crea un array de gameobject para el zombie y demonios.
        public sealed class Ciudadanos : MonoBehaviour
        {
            public Civitasinfo citizenstats = new Civitasinfo();
            public Estado estadovillager;
            float tempus;
            public float distancia;
            public float velhuida;
            public float edad;
            int Accion;
            public bool Huida = false;
            bool mirar = false;
            public Vector3 direccion;
            GameObject Objetivo;
            GameObject[] Zombie,Demon;
            //Declarar un enum de nombre para los ciudadanos
            public enum nombres
            {
                pablo, hilario, sergio, ramon, socorro, auxilio, jose, maria, luisa, nicanor, neptalí, eulalia, jacinta, ufano, candido, teodosia,
                castulo, gervasia, eufemio, higinio
            }
            // estados variados del ciudadano a utilizar            
            public enum Estado
            {
                Idle, Moving, Rotating, Running
            }
            // Corrotina que hace correr a los aldeanos al estar en menos de 5 unidades de un zombie y lo hace en sentido contrario al zombie
            IEnumerator buscaZombies()
            {
                Demon = GameObject.FindGameObjectsWithTag("Demon");
                Zombie = GameObject.FindGameObjectsWithTag("Zombie");
                foreach (GameObject Elemento in Zombie)
                {
                    zom.Zombie referentezombie = Elemento.GetComponent<zom.Zombie>();
                    if (referentezombie != null)
                    {
                        distancia = Mathf.Sqrt(Mathf.Pow((Elemento.transform.position.x - transform.position.x), 2) + Mathf.Pow((Elemento.transform.position.y - transform.position.y), 2) + Mathf.Pow((Elemento.transform.position.z - transform.position.z), 2));
                        if (!Huida)
                        {
                            if (distancia < 5f)
                            {
                                estadovillager = Estado.Running;
                                Objetivo = Elemento;
                                Huida = true;
                            }
                        }
                    }
                }
                foreach (GameObject Elemento2 in Demon)
                {
                    Deemon.Zombieesp referentezombieesp = Elemento2.GetComponent<Deemon.Zombieesp>();
                    if (referentezombieesp != null)
                    {
                       distancia = Mathf.Sqrt(Mathf.Pow((Elemento2.transform.position.x - transform.position.x), 2) + Mathf.Pow((Elemento2.transform.position.y - transform.position.y), 2) + Mathf.Pow((Elemento2.transform.position.z - transform.position.z), 2));
                        if (!Huida)
                        {
                            if(distancia < 5f)
                            {
                                estadovillager = Estado.Running;
                                Objetivo = Elemento2;
                                Huida = true;
                            }
                        }
                    }    
                }
                // si el ciudadano esta a mas de 5 unidades del zombie vuelve otra vez a estos aleatorios
                if (Huida)
                {
                    if (distancia > 5f)
                    {
                        Huida = false;
                    }
                }
                yield return new WaitForSeconds(0.2f);
                StartCoroutine(buscaZombies());
            }
            //Se le agrega toda la informacion necesaria al ciudadano
            void Start()
            {
                Rigidbody civbody;
                this.gameObject.tag = "Citizen";
                civbody = this.gameObject.AddComponent<Rigidbody>();
                civbody.constraints = RigidbodyConstraints.FreezeAll;
                civbody.useGravity = false;
                nombres nombre;
                civbody.GetComponent<Renderer>().material.color = Color.yellow;
                nombre = (nombres)Random.Range(0, 20);
                citizenstats.nombre = nombre.ToString();
                edad = (int)Random.Range(15, 101);
                citizenstats.edad = (int)edad;
                velhuida = 10 / edad;
                this.gameObject.name = nombre.ToString();
                StartCoroutine(buscaZombies());
            }
            // se iplementa
            // se usa la variable tiempo declarada en el constructor para implementar un contador
            // se asigna una rango aleatorio a la variable accion que usada en en los condionales para determinar el comportamiento del zombie seugn el nuemero asignado
            void Update()
            {
                tempus += Time.deltaTime;
                if (!Huida)
                {
                    if (tempus >= 3)
                    {
                        Accion = Random.Range(0, 3);
                        mirar = true;
                        tempus = 0;
                        if (Accion == 0)
                        {
                            estadovillager = Estado.Idle;
                        }
                        else if (Accion == 1)
                        {
                            estadovillager = Estado.Moving;
                        }
                        else if (Accion == 2)
                        {
                            estadovillager = Estado.Rotating;

                        }
                    }
                }
                // declarando que hacer en cada caso de los estados del ciudadno si un estado es verdadero hacer las acciones requeridas
                switch (estadovillager)
                {
                    case Estado.Idle:
                        break;
                    case Estado.Moving:
                        if (mirar)
                        {
                            this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        this.gameObject.transform.Translate(0, 0, 0.05f);
                        mirar = false;
                        break;
                    case Estado.Rotating:
                        this.gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;
                    case Estado.Running:
                        direccion = Vector3.Normalize(Objetivo.transform.position - transform.position);
                        transform.position -= direccion * velhuida;
                        break;
                }
            }  
        }
    }
}

