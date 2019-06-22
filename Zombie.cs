using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using zombie = NPC.Ally;
//Declaracion del namespace NPC 
namespace NPC
{
    //Declaracion del nameSpace Muertoviviente que esta dentro del namespace mayor NPC 
    namespace Muertoviviente
    {
        // declaraciones para el zombie como la informacion el color gusto y las demas cosas necesarias para el zombie
        public class Zombie : MonoBehaviour
        {
            public zomboidinfo zombiestats;
            public Hero Daño;
            bool cursed = false;
            int asigna;
            public string Partes;
            public int Accion = 0;
            public float edad = 0;
            public float tempus = 0;
            public bool mirar = false;
            public float followSpeed;
            public State Zombiestate;
            public Vector3 direccion;
            float PuntoA;
            float PuntoB;
            public bool persigue = false;
            GameObject objetivo, heroe;
            GameObject[] aldeanos;
            // Creacion de una corrutina que pone al zombie a buscar un objetivo y si esta dentro de 5 unidades del zombie lo persigue priorizando a los aldeanos

            IEnumerator buscaAldeanos()
            {
                heroe = GameObject.FindGameObjectWithTag("Hero");
                aldeanos = GameObject.FindGameObjectsWithTag("Citizen");
                foreach (GameObject person in aldeanos)
                {
                    yield return new WaitForEndOfFrame();
                    zombie.Ciudadanos referenciaciv = person.GetComponent<zombie.Ciudadanos>();
                    if (referenciaciv != null)
                    {
                        PuntoB = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        PuntoA = Mathf.Sqrt(Mathf.Pow((person.transform.position.x - transform.position.x), 2) + Mathf.Pow((person.transform.position.y - transform.position.y), 2) + Mathf.Pow((person.transform.position.z - transform.position.z), 2));
                        if (!persigue)
                        {

                            if(PuntoA < 5f)
                            {
                                Zombiestate = State.Pursuing;
                                objetivo = person;
                                persigue = true;
                            }
                            else if (PuntoB < 5f)
                            {
                                Zombiestate = State.Pursuing;
                                objetivo = heroe;
                                persigue = true;
                            }
                        }
                        if (PuntoA < 5f && PuntoB < 5f)
                        {
                            objetivo = person;
                        }
                    }
                }

                if (persigue)
                {
                    if (PuntoA > 5f && PuntoB > 5f)
                    {
                        persigue = false;
                    }
                }
                
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(buscaAldeanos());
            }

            
            // se crea un enum cuyos items corresponden a las partes del cuerpo que quiere comer el zombie
            
            public enum BodyParts
            {
                Cerebros, Higados, Riñones, Brazos, Piernas
            }

            
            // se crea un enum cuyos items corresponden a los estados del zombie
            
            public enum State
            {
                Moving, Idle, Rotating, Pursuing
            }


            // Se le otorga toda la información necesaria al zombie

            void Start()
            {
                if (!cursed)
                {
                    edad = (int)Random.Range(15, 101);
                    zombiestats = new zomboidinfo();
                    asigna = Random.Range(0, 3);
                    Rigidbody Zom;
                    Zom = this.gameObject.AddComponent<Rigidbody>();
                    Zom.constraints = RigidbodyConstraints.FreezeAll;
                    Zom.useGravity = false;
                    this.gameObject.name = "Zombie";
                }
                else
                {
                    edad = zombiestats.edad;
                    this.gameObject.name = zombiestats.nombre;
                }
                
                StartCoroutine(buscaAldeanos());
                Daño = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
                followSpeed = 10 / edad;
                this.gameObject.tag = "Zombie";
                BodyParts bodyparts;
                bodyparts = (BodyParts)Random.Range(0, 5);
                Partes = bodyparts.ToString();
                zombiestats.antojo = Partes;
                this.gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                
            }

            //Se le aplica una random entre estados para que no este siempre en un mismo estado

            void Update()
            {
                tempus += Time.deltaTime;
                if (!persigue)
                {
                    if (tempus >= 3)
                    {
                        Accion = Random.Range(0, 3);
                        mirar = true;
                        tempus = 0;
                        if (Accion == 0)
                        {
                            Zombiestate = State.Idle;
                        }
                        else if (Accion == 1)
                        {
                            Zombiestate = State.Moving;
                        }
                        else if (Accion == 2)
                        {
                           Zombiestate = State.Rotating;
                        }
                    }
                }


                //declarando que hacer en cada caso de los estados del ciudadno si un estado es verdadero hacer las acciones requeridas
                switch (Zombiestate)
                {
                    case State.Idle:
                        break;

                    case State.Moving:
                        if (mirar)
                        {
                            this.gameObject.transform.Rotate(0, Random.Range(0, 361), 0);
                        }
                        this.gameObject.transform.Translate(0, 0, 0.05f);
                        mirar = false;
                        break;

                    case State.Rotating:
                        this.gameObject.transform.Rotate(0, Random.Range(0.5f,2f), 0);
                        break;

                    case State.Pursuing:
                        direccion = Vector3.Normalize(objetivo.transform.position - transform.position);
                        transform.position += direccion * followSpeed;
                        break;
                }
            }


            // Una colisión para cada vez que toca a un aldeano se convierta en un zombie y le agrega los script del zombie para se comporte como tal
            // y si colisiona con el heroe se acaba el juego

            void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Citizen")
                {
                    Destroy(collision.gameObject.GetComponent<NPC.Ally.Ciudadanos>());
                    collision.gameObject.AddComponent<Zombie>().zombiestats = collision.gameObject.GetComponent<NPC.Ally.Ciudadanos>().citizenstats;
                    collision.gameObject.GetComponent<Zombie>().cursed = true;
                   
                }

                if (collision.gameObject.tag == "Hero")
                {
                    Daño.salud -= 10;
                }
                if (collision.gameObject.tag == "Bullet")
                {
                    this.gameObject.tag = "Muerto";
                    this.gameObject.SetActive(false);
                    collision.gameObject.SetActive(false);
                }
            }

           
        }
    }
}
