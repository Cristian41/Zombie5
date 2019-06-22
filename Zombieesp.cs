using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using civbody = NPC.Ally;
namespace NPC
{
    namespace EsPundead
    {
        //Este es script es igual al del zombie solamente que cambia el color pero basicamente es una copia del anterio script pero con diferentes declaraciones
        public class Zombieesp : MonoBehaviour
        {
            public Hero dañorecibido;
            public zomboidinfo statusdelzombie;
            bool cursed = false;
            public string Partes;
            public int Accion = 0;
            public float edad = 0;
            public float tiempo = 0;
            public bool mirar = false;
            public float followSpeed;
            public State Zombiestate;
            public Vector3 direccion;
            float PuntoA;
            float PuntoB;
            public bool persigue = false;
            GameObject Target, heroe;
            GameObject[] Aldea;
           
            IEnumerator buscadorpremium()
            {
                heroe = GameObject.FindGameObjectWithTag("Hero");
                Aldea = GameObject.FindGameObjectsWithTag("Citizen");
                foreach (GameObject person in Aldea)
                {
                    yield return new WaitForEndOfFrame();
                    civbody.Ciudadanos referenciaciv = person.GetComponent<civbody.Ciudadanos>();
                    if (referenciaciv != null)
                    {
                        PuntoB = Mathf.Sqrt(Mathf.Pow((heroe.transform.position.x - transform.position.x), 2) + Mathf.Pow((heroe.transform.position.y - transform.position.y), 2) + Mathf.Pow((heroe.transform.position.z - transform.position.z), 2));
                        PuntoA = Mathf.Sqrt(Mathf.Pow((person.transform.position.x - transform.position.x), 2) + Mathf.Pow((person.transform.position.y - transform.position.y), 2) + Mathf.Pow((person.transform.position.z - transform.position.z), 2));
                        if (!persigue)
                        {

                            if(PuntoA < 5f)
                            {
                                Zombiestate = State.Pursuing;
                                Target = person;
                                persigue = true;
                            }
                            else if (PuntoB < 5f)
                            {
                                Zombiestate = State.Pursuing;
                                Target = heroe;
                                persigue = true;
                            }
                        }
                        if (PuntoA < 5f && PuntoB < 5f)
                        {
                            Target = person;
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
                StartCoroutine(buscadorpremium());
            }
            public enum BodyParts
            {
                Cerebros, Higados, Riñones, Brazos, Piernas
            }
            public enum State
            {
                Moving, Idle, Rotating, Pursuing
            }
            void Start()
            {
                if (!cursed)
                {
                    edad = (int)Random.Range(15, 101);
                    statusdelzombie = new zomboidinfo();
                    Rigidbody Zom;
                    Zom = this.gameObject.AddComponent<Rigidbody>();
                    Zom.constraints = RigidbodyConstraints.FreezeAll;
                    Zom.useGravity = false;
                    this.gameObject.name = "Demon";
                }
                else
                {
                    edad = statusdelzombie.edad;
                    this.gameObject.name = statusdelzombie.nombre;
                }
                
                StartCoroutine(buscadorpremium());
                dañorecibido = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
                followSpeed = 10 / edad;
                this.gameObject.tag = "Demon";
                BodyParts bodyparts;
                bodyparts = (BodyParts)Random.Range(0, 5);
                Partes = bodyparts.ToString();
                statusdelzombie.antojo = Partes;
                this.gameObject.GetComponent<Renderer>().material.color = Color.cyan;  
            }
            void Update()
            {
                tiempo += Time.deltaTime;
                if (!persigue)
                {
                    if (tiempo >= 3)
                    {
                        Accion = Random.Range(0, 3);
                        mirar = true;
                        tiempo = 0;
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
                        this.gameObject.transform.Rotate(0, Random.Range(1, 50), 0);
                        break;

                    case State.Pursuing:
                        direccion = Vector3.Normalize(Target.transform.position - transform.position);
                        transform.position += direccion * followSpeed;
                        break;
                }
            }
            private void OnCollisionEnter(Collision collision)
            {
                if (collision.gameObject.tag == "Hero")
                {
                    dañorecibido.salud -= 20;
                }
                if (collision.gameObject.tag == "Citizen")
                {
                    Destroy(collision.gameObject.GetComponent<NPC.Ally.Ciudadanos>());
                    collision.gameObject.AddComponent<Zombieesp>().statusdelzombie = collision.gameObject.GetComponent<NPC.Ally.Ciudadanos>().citizenstats;
                    collision.gameObject.GetComponent<Zombieesp>().cursed = true;
                   
                }
                if(collision.gameObject.tag == "Bullet")
                {
                    this.gameObject.tag = "Muerto";
                    this.gameObject.SetActive(false);
                    collision.gameObject.SetActive(false);
                } 
            }  
        }
    }
}
