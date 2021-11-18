#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <string.h>
#include <ctype.h>
#include <mysql.h>
#include <my_global.h>

#include <ctype.h>
#include <pthread.h>

MYSQL *conn;
pthread_mutex_t mutex=PTHREAD_MUTEX_INITIALIZER;

//comentario para el git

typedef struct{
	char nombre[20];
	int socket;
} JugadorConectado;
typedef struct{
	JugadorConectado Lista[100];
	int num;
}ListaJugadoresConectados;

ListaJugadoresConectados miLista;

typedef struct {
	int oc; //o indica que la entrada está libre y 1 que está ocupada
	char nombre [30];
	int puntos;
} TEntrada;

typedef TEntrada TablaPartidas [100];

void Inicializar (TablaPartidas tabla)
{
	int i;
	for (i=0; i<100; i++)
		tabla[i].oc=0;
}

void Login(char contrasena[100], char nombre[100],char respuesta[512]){
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	
	strcpy (consulta,"Select jugador.id FROM (jugador) WHERE jugador.nombre= '"); 
	strcat (consulta, nombre);
	strcat (consulta,"'");
	strcat (consulta," AND jugador.password='"  );
	strcat (consulta, contrasena);
	strcat (consulta,"'");
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL)
		sprintf(respuesta,"11/NO");
	else{
		sprintf(respuesta,"11/%s",row[0]);
	}
}
void Register(char contrasena[100], char nombre [100],char respuesta[512]){
	
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta[100];
	
	strcpy (consulta,"Select jugador.nombre,jugador.password FROM (jugador) WHERE jugador.nombre= '"); 
	strcat (consulta, nombre);
	strcat (consulta,"'");
	
	
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL){
		char select[100];
		strcpy(select,"Select MAX(jugador.id) FROM (jugador)");
		err=mysql_query (conn, select); 
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		//recogemos el resultado de la consulta 
		resultado = mysql_store_result (conn); 
		row = mysql_fetch_row (resultado);
		int idJugador=atoi(row[0])+1;
		char insert[100];
		strcpy (insert,"Insert INTO jugador(id,nombre,password) values(");
		sprintf(insert,"%s%d",insert,idJugador);
		strcat (insert,",'");
		strcat (insert, nombre);
		strcat (insert,"','");
		strcat (insert, contrasena);
		strcat (insert,"')");
		pthread_mutex_lock(&mutex);
		err=mysql_query (conn, insert); 
		if (err!=0) {
			printf ("Error al insertar datos de la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit (1);
			sprintf(respuesta,"21/NO");					}
		else
			sprintf(respuesta,"21/SI");
		pthread_mutex_unlock(&mutex);
	}
	else{
		sprintf(respuesta,"21/NO");
	}
	
}
void PartidasConsecutivas( char nombre [100],char respuesta[512]){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	
	char consulta [180];
	int max =0;
	strcpy(consulta,"SELECT partidas.ganador FROM(partidas,participacion,jugador) WHERE partidas.id=participacion.idP AND participacion.idJ=jugador.id AND jugador.nombre='");
	// construimos la consulta SQL
	strcat (consulta, nombre);
	strcat (consulta,"'");
	
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	
	if (row == NULL){
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta,"32/%s","-1");
	}
	else{
		int consecutivo=0;
		int x=0;
		while(row!=NULL){
			if (strcmp(row[0],nombre)==0){
				consecutivo=1;
				x++;
			}
			else{
				consecutivo=0;
				x=0;
			}
			if((consecutivo==1)&&(x>max)){
				max=x;
			}
			row = mysql_fetch_row (resultado);
		}
		sprintf(respuesta,"32/%d",max);
	}
}

void PuntosTotales(char nombre[100],char respuesta[512]){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	printf("%s\n",nombre);
	sprintf(consulta,"SELECT sum(participacion.puntos) FROM (participacion, jugador) WHERE jugador.nombre = '%s' AND jugador.id = participacion.idJ", nombre);
	printf("%s\n",nombre);
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf("%s\n",nombre);
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		sprintf(respuesta,"33/%s","-1");
	}
	else{
		sprintf(respuesta,"33/%s", row[0]);
	}
}

void PerdidoContra(char nombre[100],char respuesta[512]){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Respuesta -1 si el jugador no existe
	char consulta [280];
	// construimos la consulta SQL
	strcpy (consulta,"Select partidas.ganador FROM (jugador,participacion,partidas) WHERE jugador.nombre= '"); 
	strcat (consulta, nombre);
	strcat (consulta,"'");
	strcat (consulta," AND participacion.idJ=jugador.id AND participacion.idP=partidas.id AND NOT partidas.ganador= '"  );
	strcat (consulta, nombre);
	strcat (consulta,"'");
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	row = mysql_fetch_row (resultado);
	if (row == NULL)
		sprintf (respuesta,"34/%s","-1");
	else{
		int i=0;
		while(row !=NULL){
			// El resultado debe ser una matriz con una sola fila
			// y una columna que contiene el nombre
			sprintf (respuesta,"%s%s/", respuesta,row[0]);
			row = mysql_fetch_row (resultado);
		}
		char auxiliar[512];
		strcpy(auxiliar,respuesta);
		sprintf (respuesta,"35/%s",auxiliar);
	}
}

void VSJugador(char nombre[100],int idUsuario,char respuesta[512]){
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW rowS;
	MYSQL_ROW rowI;
	char consulta [200];
	int idPI[100];
	int idPS[100];
	// Ahora vamos a buscar el ID de las partidas que ha jugado el jugador contincante
	// construimos la consulta SQL
	sprintf (consulta,"SELECT participacion.idP FROM (participacion) WHERE participacion.idJ IN ( SELECT jugador.id FROM (jugador) WHERE jugador.nombre = '%s')",nombre);
	// hacemos la consulta 
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	rowS = mysql_fetch_row (resultado);
	int x=0;
	int y=0;
	if ((rowS == NULL)||(rowS[0]==NULL)){
		printf ("No se han obtenido datos en la consulta\n");
		sprintf(respuesta,"%s","-1");
	}
	else{
		while (rowS !=NULL) {
			// El resultado debe ser una matriz con 3 filas
			// y una columna que contiene el id de partida
			idPS[x]=atoi(rowS[0]);
			x++;
			rowS = mysql_fetch_row (resultado);
		}
		y=x;
		// Ahora vamos a buscar las IDs de las partidas que ha jugado el jugador solicitante
		// construimos la consulta SQL
		sprintf (consulta,"SELECT participacion.idP FROM participacion WHERE idJ = %d",idUsuario);
		// hacemos la consulta 
		err=mysql_query (conn, consulta); 
		if (err!=0) {
			printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
			sprintf (respuesta,"%s","-1");
			exit (1);
		}
		//recogemos el resultado de la consulta 
		resultado = mysql_store_result (conn); 
		rowI = mysql_fetch_row (resultado);
		int i=0;
		int j=0;
		if ((rowI == NULL)||(rowI[0]==NULL))
			sprintf (respuesta,"%s","-1");
		else{
			while (rowI !=NULL) {
				// El resultado debe ser una matriz con 3 filas
				// y una columna que contiene el id de partida
				idPI[i]=atoi(rowI[0]);
				i++;
				rowI = mysql_fetch_row (resultado);
			}
			j=i;
			int k=0;
			for (x=0;x<y;x++){
				for (i=0;i<j;i++){
					if(idPS[x]==idPI[i]){
						sprintf(respuesta,"%s%d,",respuesta,idPS[x]);
						k=1;
					}
				}
			}
			if (k==0){
				sprintf (respuesta,"%s","-1");
			}
			char auxiliar[512];
			strcpy(auxiliar,respuesta);
			sprintf (respuesta,"35/%s", auxiliar);
		}
	}
}
void AnadirJugadorListaConectados(char nombre[100],int socket){
	strcpy(miLista.Lista[miLista.num].nombre,nombre);
	miLista.Lista[miLista.num].socket=socket;
	miLista.num++;
}
void EliminarJugadorListaCon(char* nombre){
	int encontrado=0;
	int i=0;
	while((encontrado==0) && (i<miLista.num)){
		if(strcmp(nombre,miLista.Lista[i].nombre)==0){
			encontrado=1;
			int j=i;
			for(j;j<miLista.num;j++){
				strcpy(miLista.Lista[j].nombre,miLista.Lista[j+1].nombre);
				miLista.Lista[j].socket=miLista.Lista[j+1].socket;
			}
			miLista.num--;
		}
		i++;
	}
	
}
void CharJugCon(char respuesta[512]){ //Respuesta "NumeroJugadores,Nombre1,Nombre2,Nombre3" (char)
	pthread_mutex_lock(&mutex);
	sprintf(respuesta,"%d,",miLista.num);
	for(int i=0;i<miLista.num;i++){
		sprintf(respuesta,"%s%s,",respuesta,miLista.Lista[i].nombre);
	}
	respuesta[strlen(respuesta)-1]='\0';
	pthread_mutex_unlock(&mutex);
	
}

void *AtenderCliente (void *socket){
	//bucle de atencion al cliente
	int terminar=0;
	int sock_conn, ret;
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	int err;
	
	
	int *s;
	s=(int *) socket;
	sock_conn=*s;
	
	char peticion[512];
	char respuesta[512];
	char notificacion[512];
	int notificado=0;
	// Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	while (terminar ==0){	
		strcpy(respuesta,"");
		
		printf ("Escuchando\n");
		// Ahora recibimos la petici?n
		ret=read(sock_conn,peticion, sizeof(peticion));
		printf ("Recibido\n");
		
		// Tenemos que a?adirle la marca de fin de string 
		// para que no escriba lo que hay despues en el buffer
		peticion[ret]='\0';
		
		
		printf ("Peticion: %s\n",peticion);
		
		// vamos a ver que quieren
		char *p = strtok( peticion, "/");
		int codigo =  atoi (p);
		// Ya tenemos el c?digo de la petici?n
		char nombre[20];
		char contrasena[100];
		
		p = strtok( NULL, "/");
		
		if(p!=NULL){
			strcpy (nombre, p);
			// Ya tenemos el nombre
				printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
		}
		
		
		if (codigo ==0){//petici?n de desconexi?n
			if(nombre!=NULL){
				pthread_mutex_lock(&mutex);
				EliminarJugadorListaCon(nombre);
				pthread_mutex_unlock(&mutex);
				CharJugCon(notificacion);
				notificado=1;
			}
			terminar=1;
		}
		else if (codigo ==11){ //Aquí se hace el login
			p = strtok( NULL, "/");
			strcpy(contrasena,p);
			Login(contrasena,nombre,respuesta);
			if(strcmp(respuesta,"11/NO")!=0){
				pthread_mutex_lock(&mutex);
				AnadirJugadorListaConectados(nombre,*s);
				pthread_mutex_unlock(&mutex);
				CharJugCon(notificacion);
				notificado=1;
			}
			// cerrar la conexion con el servidor MYSQL 
			/*				mysql_close (conn);*/
		}
		else if (codigo ==21){
			char consulta[100];
			p = strtok( NULL, "/");
			strcpy(contrasena,p);
			Register(contrasena,nombre,respuesta);
			// cerrar la conexion con el servidor MYSQL 
			/*				mysql_close (conn);*/
		}
		else if (codigo ==32){ //Peticion partidas consecutivas ganadas 
			//por el Jugador cuyo nombre se recibe como "31/Xavier"
			//Respuesta "numeroPartidas" (enetro)
			PartidasConsecutivas(nombre,respuesta);
		}
		else if (codigo ==33){ //Peticion Suma de los puntos del jugador introducido
			//Peticion "33/Victoria"
			//Respuesta "Puntos" (enetro)
			//Respuesta -1 si el jugador no existe
			// construimos la consulta SQL
			PuntosTotales( nombre,respuesta);
			
		}
		else if (codigo ==34){ //Peticion Lista nombre jugadores que han ganado contra el jugador introducido
			//Peticion "34/Genis"
			//Respuesta "Nombre1/Nombre2/Nombre3/" (char)
			PerdidoContra(nombre,respuesta);
		}
		else if (codigo ==35){ //Peticion 35 Lista de IDs de partidas que el Usuario ha tenido con un jugador introducido
			//Peticion "35/Carme/idUsuario"
			//Respuesta "idPartida1/idPartida2/idPartida3/" (char)
			//Respuesta -1 si el jugador no existe
			p = strtok( NULL, "/");
			int idUsuario=atoi(p);
			VSJugador(nombre, idUsuario, respuesta);
			
		}
		
		
		printf ("Respuesta: %s\n", respuesta);
		printf("Notificación:%s\n",notificacion);
		// Enviamos respuesta
		write (sock_conn,respuesta, strlen(respuesta));
		if(notificado==1)
		{
			char cabecera[512]="36/";
			strcat(cabecera,notificacion);
			//sprintf(notificacion,"36/%s",notificacion);
			printf("Notificación:%s\n",cabecera);
			for(int j=0;j<miLista.num;j++){
				write (miLista.Lista[j].socket,cabecera, strlen(cabecera));
			}
			
		}
		notificado=0;
		
	}
	// Se acabo el servicio para este cliente
	close(sock_conn); 
}

int main(int argc, char *argv[])
{
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	char notificacion[512];
	
	miLista.num=0;
	// INICIALITZACIONS
	// Obrim el socket
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creant socket");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(50060);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion
	conn = mysql_real_connect (conn, "shiva2.upc.es","root", "mysql", "T4BBDD",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	int i=0;
	int *sockets;
	sockets= (int *) calloc(miLista.num+1,sizeof(int));
	pthread_t thread[100];
	// Bucle infinito
	for (;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexi?n\n");
		sockets[i]=sock_conn;
		
		//sock_conn es el socket que usaremos para este cliente
		pthread_create(&thread[i],NULL, AtenderCliente, &sockets[i]);
		i=i+1;
		
		
	}
}
