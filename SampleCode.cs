//librerias usadas en el proyecto-----------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using UnityEditor;


//clase de SampleCode------------------------------------------------------------------------------------------

public class SampleCode : MonoBehaviour {



    public Transform goal1;
    public Transform goal2;
    public Transform goal3;
    public Transform goal4;

	public float menor_distancia;
	public int salida_peligrosa;

	//Camera cam1;
	//Camera cam2; 
	//Camera cam3; 
	//Camera cam4; 

	public Vector3 puerta1;
	public Vector3 puerta2;
	public Vector3 puerta3;
	public Vector3 puerta4;

    public NavMeshAgent agent;

    BoxCollider limite_fuego;

    float minfx, maxfx, minfz, maxfz;

    public NavMeshAgent[] a = new NavMeshAgent[200];//arreglo de agentes

    public float[,] datos = new float[200, 4];//matriz de información de cada agente

    public string[] caracteres = new string[9]; // caracteres por default



    float minx;//variable para saber los limites del barco soen
    float maxx;//variable para saber los limites del barco soen
    float minz;//variable para saber los limites del barco soen
    float maxz;//variable para saber los limites del barco soen

    public GameObject fuego;//variable que tendrá un puntero al objeto fuego
    public GameObject soen;//variable que tendrá un puntero al barco soen

    public System.Random srand = new System.Random((int)DateTime.Now.Ticks);//semilla para que no sea siempre el mismo patrón de valores aleatorios

    public GameObject obj; //servirá para saber a cuál salida acudir

    public NavMeshObstacle fire;

    public GameObject llamaO;
    public GameObject humoO;

    public ParticleSystem llamaC;
    public ParticleSystem humoC;

    public string objeto_quemado;

    public NavMeshAgent agtt;


    void Start()
    {
        //variables para el modelo socio-cultural
        float e, hh, w, cal, inc, sum, c;

        float religiosidad, educacion, nivelSocial, nivelSociable, Region, idiomas, nivelSolidario, totallll;

        float fa;


			
        goal1= GameObject.Find("salida1").transform;
        goal2= GameObject.Find("salida2").transform;
        goal3= GameObject.Find("salida3").transform;
        goal4= GameObject.Find("salida4").transform;




		/*GameObject camara;


			
		camara= GameObject.Find ("Main Camera");
		cam1 = camara.GetComponent<Camera> ();
		camara = GameObject.Find ("Camera"); 
		cam2 = camara.GetComponent<Camera> ();
		camara = GameObject.Find ("Camera (1)"); 
		cam3 = camara.GetComponent<Camera> ();
		camara = GameObject.Find ("Camera (2)"); 
		cam4 = camara.GetComponent<Camera> ();

		if (Display.displays.Length > 1)
			Display.displays[1].Activate();
		if (Display.displays.Length > 2)
			Display.displays[2].Activate();
		if (Display.displays.Length > 3)
			Display.displays[3].Activate();
		if (Display.displays.Length >4)
			Display.displays[4].Activate();

		cam1.enabled = true;
		cam2.enabled = false;
		cam3.enabled = false;
		cam4.enabled = false;*/

        BuildSlope90(); // sirve para que detecte superficies como navmesh inferiores a 90 grados

        //soen = GameObject.Find("soenbybarco"); //a la variable de tipo objeto soen, se le asigna el puntero al objeto soen del proyecto

        //asignacion de los nombre de los caracteres
       
		caracteres[0] = "Abuelita_Unity";
		caracteres[1] = "Abuelo_Unity";
		caracteres[2] = "Besher_Unity";
		caracteres[3] = "fatty_Unity";
		caracteres[4] = "Granny_Unity";
		caracteres[5] = "Gustavo_Unity"; 
		caracteres[6] = "shanny_Unity";
		caracteres[7] = "Sup_Unity";
		caracteres[8] = "juan_Unity";
        /* caracteres[9] = c4;
         caracteres[10] = c5;
         caracteres[11] = c6;
         caracteres[12] = c1;
         caracteres[13] = c2;
         caracteres[14] = c3;
         caracteres[15] = c4;
         caracteres[16] = c5;
         caracteres[17] = c6;
         caracteres[18] = c1;
         caracteres[19] = c2;

       */ 


        int i = 0; //contador que servirá para almacenar los agentes en el arreglo de agentes
        float x, y, z;//variables para un vector

        Vector3 d; //vector que asignado sus valores, se podrá definir una posición de un objeto


        //int p = 0;       // contador para recorrer el arreglo de los nombres de los caracteres
        int j = 0; //contador que servirá para ir guardando en una matriz la información de cada agente
        float control; //variable que servirá para determinar en cuál piso se le asigna posición a los agentes

        GameObject coli = GameObject.Find("Cube");


        BoxCollider col = coli.GetComponent<BoxCollider>();

        

        minx = col.bounds.min.x;
        maxx = col.bounds.max.x;
        minz = col.bounds.min.z;
        maxz = col.bounds.max.z;




        fuego = GameObject.Find("fuego"); //ubicando el objeto fuego del proyecto

        float xx = srand.Next((int)minx, (int)maxx); //procedimiento para caluclar un x dentro del barco
        float yy = 9.4f; //altura del primer piso por default
        float zz = srand.Next((int)minz,(int)maxz);//procedimiento para caluclar un z dentro del barco

        Vector3 dd = new Vector3(xx, yy, zz);//se crea el vector en base a los ejes anteriormente calculados
        fuego.transform.position = dd; //a la posición inicial del fuego, se le asigna el vector inicializado en la línea anterior.


		puerta1 = GameObject.Find("puerta1").transform.position;
		puerta2 = GameObject.Find("puerta2").transform.position;
		puerta3 = GameObject.Find("puerta3").transform.position;
		puerta4 = GameObject.Find("puerta4").transform.position;

		//verificar la puerta que se encuentra más cercana al fuego
	

		menor_distancia= Vector3.Distance(puerta1,fuego.transform.position);
		salida_peligrosa = 1;


		if (Vector3.Distance (puerta2, fuego.transform.position) < menor_distancia) 
		{
			menor_distancia = Vector3.Distance (puerta2, fuego.transform.position);
			salida_peligrosa = 2;
		} 
		else if (Vector3.Distance (puerta3, fuego.transform.position) < menor_distancia) 
		{
			menor_distancia=Vector3.Distance (puerta3, fuego.transform.position);
			salida_peligrosa = 3;
		}
		else if (Vector3.Distance (puerta4, fuego.transform.position)< menor_distancia) 
		{
			menor_distancia=Vector3.Distance (puerta4, fuego.transform.position);
			salida_peligrosa = 4;
		}
			
        while (i < 200)//ciclo para crear a todos los agentes en la escena (debe recibir como entrada la cantidad de personas a bordo)
        {


            control = srand.Next(0, 2) + 1;//random para saber si lo coloca arriba o abajo


            x = srand.Next((int)col.bounds.min.x, (int)col.bounds.max.x); //para que quede dentro del barco
            z = srand.Next((int)col.bounds.min.z, (int)col.bounds.max.z);  //para que quede dentro del barco


            if (control == 1) // es abajo (y= 10.15)
            {
              
                y = 9.406f; //altera del primer piso
           
            }
            else // es arriba (y=21.354)
            {
                y = 21.012f; //altura del segundo piso

            }

            d = new Vector3(x, y, z); //se crea el vector donde se va a posicionar el agente



            //-------------------------------------------------------------------------------- 
            // PARTE IMPORTANTE PARA QUE APAREZCAN DISTINTOS CARACTERES EN EL CAMPO
            
            if (p < 9)
            {
                GameObject TheObject = GameObject.Find(caracteres[p]);
                GameObject InstObject = (GameObject)Instantiate(TheObject);
                InstObject.transform.position = d;
                InstObject.transform.localScale = new Vector3(2f, 2f, 2f);

				CapsuleCollider f = InstObject.AddComponent<CapsuleCollider>();
				f.isTrigger = true;

				Rigidbody cuerpo = InstObject.AddComponent<Rigidbody>();
           		cuerpo.useGravity = false;
            	cuerpo.isKinematic = true;

                 agent = InstObject.AddComponent<NavMeshAgent>();
                 agent.stoppingDistance = 2f;
                 agent.speed = 2f;
              

                p++;
            }
            else
                p = 0;
            

            //  ----------------------------------------------------------------------------------------------


            /* GameObject TheObject = GameObject.Find("juan_Unity");
             GameObject InstObject = (GameObject)Instantiate(TheObject);
             InstObject.transform.position = d; //se le da su posicion con el vector previamente calculado
             InstObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); //se le da una escala(tamaño)

             Rigidbody cuerpo = InstObject.AddComponent<Rigidbody>();
             cuerpo.useGravity = false;
             cuerpo.isKinematic = true;

             agent = InstObject.AddComponent<NavMeshAgent>();  //al objeto se le agrega el componente o habilidad de agente
             agent.stoppingDistance = 2f; //distancia para detenerse del goal
             agent.speed = 2f; //la velocidad inicial del agente
             */

             /*

            if (i % 50 == 0)
            {
                GameObject TheObject = GameObject.Find("Abuelo_Unity");
                GameObject InstObject = (GameObject)Instantiate(TheObject);
                InstObject.transform.position = d;
                InstObject.transform.localScale = new Vector3(2f, 2f, 2f);

                CapsuleCollider ff = InstObject.AddComponent<CapsuleCollider>();
                ff.isTrigger = true;

                Rigidbody cuerpof = InstObject.AddComponent<Rigidbody>();
                cuerpof.useGravity = false;
                cuerpof.isKinematic = true;

                agent = InstObject.AddComponent<NavMeshAgent>();
                agent.stoppingDistance = 2f;
                agent.speed = 2f;

            }
*/
            

            //INSTANCIA DE UN TIPO PRIMITIVO CILINDRO

           /* GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder); // se crea el objeto (agente)
            cylinder.transform.position = d; //se le da su posicion con el vector previamente calculado
            cylinder.transform.localScale = new Vector3(0.6f,0.6f,0.6f); //se le da una escala(tamaño)

            CapsuleCollider f = cylinder.AddComponent<CapsuleCollider>();

            f.isTrigger = true;

            Rigidbody cuerpo = cylinder.AddComponent<Rigidbody>();
            cuerpo.useGravity = false;
            cuerpo.isKinematic = true;




            agent = cylinder.AddComponent<NavMeshAgent>();  //al objeto se le agrega el componente o habilidad de agente
            agent.stoppingDistance = 2f; //distancia para detenerse del goal
            agent.speed = 2f; //la velocidad inicial del agente
*/

            //Cáclulo de la salida de evacuación más segura------------
            

           // exit = srand.Next(0, 4);

			switch (salida_peligrosa)
            {
			case 1:

				menor_distancia = Vector3.Distance(d, puerta2); //se asume que la puerta más ercana es la 2


				if (Vector3.Distance(d, puerta3) < menor_distancia)
				{
					agent.destination = goal3.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
				} 
				else if (Vector3.Distance(d, puerta4) < menor_distancia) 
				{
					agent.destination = goal4.position;
				} 
				else
					agent.destination = goal2.position;
				    
                    //obj = GameObject.Find("salida1");//el objeto a encontrar, es considerado un objeto que será un goal
                    //agent.destination = goal1.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
                    break;
                case 2:

				menor_distancia = Vector3.Distance(d, puerta1); //se asume que la puerta más ercana es la 1


				if (Vector3.Distance(d, puerta3) < menor_distancia)
				{
					agent.destination = goal3.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
				} 
				else if (Vector3.Distance(d, puerta4) < menor_distancia) 
				{
					agent.destination = goal4.position;
				} 
				else
					agent.destination = goal1.position;
                   // agent.destination = goal2.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
                    //obj = GameObject.Find("salida2");//el objeto a encontrar, es considerado un objeto que será un goal
                    break;
                case 3:

				menor_distancia = Vector3.Distance(d, puerta1); //se asume que la puerta más ercana es la 1


				if (Vector3.Distance(d, puerta2) < menor_distancia)
				{
					agent.destination = goal2.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
				} 
				else if (Vector3.Distance(d, puerta4) < menor_distancia) 
				{
					agent.destination = goal4.position;
				} 
				else
					agent.destination = goal1.position;
                    //agent.destination = goal3.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
                    //obj = GameObject.Find("salida3");//el objeto a encontrar, es considerado un objeto que será un goal
                    break;
                case 4:

				menor_distancia = Vector3.Distance(d, puerta1); //se asume que la puerta más ercana es la 1


				if (Vector3.Distance(d, puerta2) < menor_distancia)
				{
					agent.destination = goal2.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
				} 
				else if (Vector3.Distance(d, puerta3) < menor_distancia) 
				{
					agent.destination = goal3.position;
				} 
				else
					agent.destination = goal1.position;
                    //agent.destination = goal4.position; //se le asigna un goal al agente (previamente asignado el valor del goal a la variable goal)
                    //obj = GameObject.Find("salida4");//el objeto a encontrar, es considerado un objeto que será un goal
                    break;
            }
            
           // goal = obj.transform; //se le asigna el transform del objeto a donde se va a definir el goal


            






            //modelo socio-cultural-------------------------------------------------------------

            //variables empleadas aleatoriamente para el modelo socio-cultural
            religiosidad = (float)(srand.Next(0, 10) + 1) / 10;
            educacion = (float)(srand.Next(0, 10) + 1) / 10;
            nivelSocial = (float)(srand.Next(0, 10) + 1) / 10;
            nivelSociable = (float)(srand.Next(0, 10) + 1) / 10;
            Region = (float)(srand.Next(0, 10) + 1) / 10;
            idiomas = (float)(srand.Next(0, 10) + 1) / 10;
            nivelSolidario = (float)(srand.Next(0, 10) + 1) / 10;

            if (idiomas > 0.5)
                idiomas = 1;
            else
                idiomas = (float)0.5;

            totallll = (float)Math.Pow(religiosidad * educacion * nivelSocial * nivelSociable * Region * idiomas, nivelSolidario);

            fa = totallll;

            if (fa > 0.01 && fa < 0.1)
            {
                fa = fa * 10;
            }
            else if (fa > 0.001 && fa <= 0.009)//0.001
            {
                fa = fa * 100;
            }
            else if (fa < 0.001)
                fa = (float)0.1;


            //parte de las variables del modelo de velocidad pedestre

            c = (float)srand.Next(0, 10) + 1;
            e = (float)(srand.Next(0, 10) + 1) / 10;
            hh = (float)(srand.Next(0, 10) + 1) / 10;
            w = (float)(srand.Next(0, 10) + 1) / 10;
            cal = (float)(srand.Next(0, 10) + 1) / 10;
            inc = (float)(srand.Next(0, 10) + 1) / 10;

           // gameObjectsRigidBody.mass = w*10; // Set the GO's mass to 5 via the Rigidbody.

            sum = (hh + w + cal) / 3;


            //asignación de datos importantes de cada agente en la matriz (n° acompañantes,edad,grado de incapacidad,sumatoria de atributos)
            datos[j, 0] = c;
            datos[j, 1] = e;
            datos[j, 2] = inc;
            datos[j, 3] = sum;

 

            //f= m*a (segunda Ley de Newton)   -->    a= f/m
            agent.acceleration = fa / w;

            if (agent.acceleration < 0.09 && agent.acceleration > 0.01)
                agent.acceleration = agent.acceleration * 10;
            else if (agent.acceleration > 0 && agent.acceleration < 1)
                agent.acceleration = agent.acceleration * 10;

            a[j] = agent; //al arreglo de agentes, se le va asignando cada agente creado


            i++; //se incrementa el contador
            j++; //se incrementa el contador

        }






   }


    void OnCollisionEnter(Collision hit)//se muere el agente
    {
        objeto_quemado = hit.gameObject.name;
        if (objeto_quemado == "Cylinder" && gameObject.name == "fuego")
        {
            Destroy(hit.gameObject);
        }

    }




    // Update is called once per frame
  void Update ()
  {

        limite_fuego = fuego.GetComponent<BoxCollider>();

        limite_fuego.size = fuego.transform.localScale;

        minfx = limite_fuego.bounds.min.x;
        maxfx = limite_fuego.bounds.max.x;
        minfz = limite_fuego.bounds.min.z;
        maxfz = limite_fuego.bounds.max.z;


        //verifica que el fuego esté dentro del barco
        if (((minfx >= minx) && (maxfx <= maxx)) || ((minfz >= minz) && (maxfz <= maxz)))
        {
            if ((((minfx >= minx) && (maxfx <= maxx)) && ((minfz < minz) || (maxfz > maxz))))
            {
                //incrementa sólo en x
                fuego.transform.localScale += new Vector3(Time.deltaTime * 0.1f, 0f, 0f) * 4f;

            }
            else if ((((minfz >= minz) && (maxfz <= maxz)) && ((minfx < minx) || (maxfx > maxx))))
            {
                //incrementa solo en z
                fuego.transform.localScale += new Vector3(0f, 0f, Time.deltaTime * 0.1f) * 4f;
            }
            else if (((minfx >= minx) && (maxfx <= maxx)) && ((minfz >= minz) && (maxfz <= maxz)))
            {
                //incrementa en ambas coordenadas
                fuego.transform.localScale += new Vector3(Time.deltaTime * 0.1f, 0f, Time.deltaTime * 0.1f) * 4f;

            }





            //fire = fuego.GetComponent<NavMeshObstacle>();
            //fire.size = fuego.transform.localScale; //incrementa el navmesh obstacle para abarcar más espacio en el barco



            llamaO = GameObject.Find("WallOfFireFlame");
            llamaO.transform.localScale = fuego.transform.localScale / 100;

            llamaC = llamaO.GetComponent<ParticleSystem>();
            llamaC.startSize = fuego.transform.localScale.magnitude;
            llamaC.maxParticles += (int)(Time.deltaTime);



            humoO = GameObject.Find("WallOfFireSmoke");
            humoO.transform.localScale += new Vector3(Time.deltaTime * 0.2f, 1f, Time.deltaTime * 0.2f);

            humoC = humoO.GetComponent<ParticleSystem>();
            humoC.startSize += Time.deltaTime * 0.2f;


        }





        float totalll,kk;//variables relacionadas con el modelo de velocidad pedestre




                 for (int i = 0; i < 200; i++)//ciclo para recorrer cada agente durante tiempo de ejecucion
                 {
                    if (a[i] != null)
                    {


                //ecuacion de velocidad----------------
                agtt = a[i];
                agtt.destination = agtt.destination;
             

                //modelo de velocidad pedestre-----------------------------------------------------


                float sec = Time.realtimeSinceStartup;

                        kk = 1f / sec;


                        if (kk > 0 && kk < 0.1)
                        {
                            kk = (float)kk * 10f;
                        }


                        totalll = ((datos[i, 1] * kk * datos[i, 3] * datos[i, 2]) / datos[i, 0]);

                        if (totalll > 0.01f && totalll < 0.1f)
                        {
                            totalll = totalll * 10f;
                        }
                        else if (totalll > 0.001f && totalll <= 0.009f)//0.001
                        {
                            totalll = totalll * 100f;
                        }
                        else if (totalll < 0.001f)
                            totalll = 0.1f;




                        agtt.speed = totalll*10f; //se modifica la velocidad del agente a partir del resultado del modelo de velocidad pedestre
                        
                    }


            




        }


        



    }

    // funcion que sirve para que detecte superficies inferiores a 90 grados en el navmesh-----------------------------------------------------------

    [MenuItem("NavMesh/Build With Slope 90")]
    public static void BuildSlope90()
    {
        SerializedObject obj = new SerializedObject(NavMeshBuilder.navMeshSettingsObject);
        SerializedProperty prop = obj.FindProperty("m_BuildSettings.agentSlope");
        prop.floatValue = 90.0f;
        obj.ApplyModifiedProperties();
        NavMeshBuilder.BuildNavMesh();
    }




    

}


