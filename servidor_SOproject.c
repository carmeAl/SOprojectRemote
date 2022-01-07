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

int *sockets;


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
	int oc; //0 indica que la entrada está libre y 1 que está ocupada
	char nombre1 [30];
	char nombre2 [30];
	int socket1;
	int socket2;
} TEntrada;

TEntrada TablaPartidasActivas [100];

void Inicializar (TEntrada tabla[100])
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
		sprintf(respuesta,"11/0/NO");
	else{
		sprintf(respuesta,"11/0/%s",row[0]);
	}
}
void RandomizeV(int n,int v[12],char listafrase[512]){
	int i,aux,k;
	time_t t;
	n = 12;
	srand((unsigned) time(&t));
	for( i = 0 ; i < n ; i++ ) 
	{
		v[i]=30;
	}
	for( i = 0 ; i < n ; i++ ) 
	{
		k=0;
		aux=rand() % 24;
		while( k < n){
			
			if(v[k]==aux)
			{
				aux=rand() % 24;
				k=0;
			}
			else{
				k=k+1;
			}
			
		}
		v[i]=aux;
	}
	sprintf(listafrase,"%d,",n);
	for( i = 0 ; i < n ; i++ ) 
	{
		sprintf(listafrase,"%s%d,",listafrase,v[i]);
	}
	listafrase[strlen(listafrase)-1]='\0';
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
			sprintf(respuesta,"21/0/NO");					}
		else
			sprintf(respuesta,"21/0/SI");
		pthread_mutex_unlock(&mutex);
	}
	else{
		sprintf(respuesta,"21/0/NO");
	}
	
}
void BuscarIdUsurio(char *nombre,char *idJ){//Busca el id del jugador con el nombre introducido
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT jugador.id FROM (jugador) WHERE jugador.nombre = '%s'", nombre);
	
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("BuscarIdUsuario Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		sprintf(idJ,"-1");
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		sprintf(idJ,"-1");
	}
	else{
		strcpy(idJ, row[0]);
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
		sprintf(respuesta,"32/0/%s","-1");
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
		sprintf(respuesta,"32/0/%d",max);
	}
}

void PuntosTotales(char nombre[100],char respuesta[512]){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT sum(participacion.puntos) FROM (participacion, jugador) WHERE jugador.nombre = '%s' AND jugador.id = participacion.idJ", nombre);
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("PuntosTotales Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		strcpy(respuesta,"0");
	}
	else{
		strcpy(respuesta, row[0]);
	}
}

int MaxPuntos(char nombre[100]){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT (participacion.puntos) FROM (participacion, jugador) WHERE jugador.nombre = '%s' AND jugador.id = participacion.idJ", nombre);
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("MaxPuntos Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		return 0;
	}
	else{
		int valor;
		int suma=0;
		while(row !=NULL){
			// El resultado debe ser una matriz con una sola fila
			// y una columna que contiene el nombre
			valor=atoi(row[0]);
			if (suma+valor>suma){
				suma=suma+valor;
			}
			row = mysql_fetch_row (resultado);
		}
		return suma;
	}
}
int PartidasGanadas(char *idJ,char *nombre){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT COUNT(*) FROM (participacion, partidas) WHERE participacion.idJ = %s AND participacion.idP = partidas.id AND partidas.ganador='%s'", idJ,nombre);
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("PartdiasGanadas Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		return 0;
	}
	else{
		return (atoi(row[0]));
	}
}
int PartidasPerdidas(char *idJ,char *nombre){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT COUNT(*) FROM (participacion, partidas) WHERE participacion.idJ = %s AND participacion.idP = partidas.id AND partidas.ganador!='%s'", idJ,nombre);

	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("PartidasPerdidas Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		return 0;
	}
	else{
		return (atoi(row[0]));
	}
}
int PartidasJugadas(char *idJ){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT COUNT(participacion.idP) FROM (participacion) WHERE participacion.idJ = %s", idJ);
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("PartidasJugadas Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		return -1;
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		return 0;
	}
	else{
		return (atoi(row[0]));
	}
}
void CartaMasUsada(char *idJ, char *carta){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT (participacion.personaje) FROM (participacion) WHERE participacion.idJ = %s ORDER BY participacion.personaje DESC LIMIT 1", idJ);

	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("CartaMasUsada Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		sprintf(carta,"-1");
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		strcpy(carta,"0");
	}
	else{
		strcpy(carta,row[0]);
	}
}
void Ranking(char *resp){
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	char consulta [200];
	sprintf(consulta,"SELECT (jugador.nombre) FROM (jugador)");
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("Ranking Error al consultar datos de la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		sprintf(resp,"-1");
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn);
	row = mysql_fetch_row (resultado);
	if ((row == NULL)||(row[0]==NULL)){
		sprintf(resp,"-1");
	}
	else{
		char puntos[512];
		while(row !=NULL){
			// El resultado debe ser una matriz con una sola fila
			// y una columna que contiene el nombre
			PuntosTotales(row[0],puntos);
			sprintf(resp,"%s%s,%s,",resp,row[0],puntos);
			row = mysql_fetch_row (resultado);
		}
		resp[strlen(resp)-1]='\0';
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
		sprintf (respuesta,"34/0/%s","-1");
	else{
		int i=0;
		sprintf (respuesta,"34/0/");
		while(row !=NULL){
			// El resultado debe ser una matriz con una sola fila
			// y una columna que contiene el nombre
			sprintf (respuesta,"%s%s/", respuesta,row[0]);
			row = mysql_fetch_row (resultado);
		}
	}
}

void VSJugador(char nombre[100],int idUsuario,char * res){
	int err;
	MYSQL_RES *resultado;
	MYSQL_ROW rowS;
	MYSQL_ROW rowI;
	char consulta [200];
	int idPI[100];
	int idPS[100];
	char idJVs[20];
	BuscarIdUsurio(nombre,idJVs);
	// Ahora vamos a buscar el ID de las partidas que tienen en comun los dos jugadores
	// construimos la consulta SQL
	sprintf(consulta,"SELECT participacion.idP FROM participacion WHERE idJ = %d AND participacion.idP IN (SELECT participacion.idP FROM participacion WHERE idJ = %d)",atoi(idJVs),idUsuario);
	err=mysql_query (conn, consulta); 
	if (err!=0) {
		printf ("VSJugador Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//recogemos el resultado de la consulta 
	resultado = mysql_store_result (conn); 
	rowS = mysql_fetch_row (resultado);
	
	int x=0;
	int y=0;
	if ((rowS == NULL)||(rowS[0]==NULL)){
		printf ("No se han obtenido datos en la consulta\n");
		strcpy(res,"0");
	}
	else{
		while (rowS !=NULL) {
			sprintf(res,"%s%s,",res,rowS[0]);
			printf("partida:%d\n",atoi(rowS[0]));
			rowS = mysql_fetch_row (resultado);
		}
		
		res[strlen(res)-1]='\0';
		printf("Dentro codgigo: %s\n",res);
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
				sockets[j]=sockets[j+1];
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

int SocketNomJug(char* nombre){ //Devuelve el socket del nombre del jugador que se introduce como parametro, 
	//devuelve -1 si no lo ha encontrado
	int encontrado=0;
	int i=0;
	while ((encontrado==0)&(i<miLista.num)){
		if (strcmp(nombre,miLista.Lista[i].nombre)==0){
			encontrado=1;
		}
		else{
			i++;
		}
	}
	if (encontrado==1){
		return (miLista.Lista[i].socket);
	}
	else{
		return(-1);
	}
}
void EnviarListaJugadoresConectados(char* notificacion,int ListaSockets[100],char c[3]){
	printf("Notificación:%s\n",notificacion);
	char cabecera[512]="36/0/";
	
	strcat(cabecera,notificacion);
	//sprintf(notificacion,"36/%s",notificacion);
	printf("Notificación:%s\n",cabecera);
	sprintf(notificacion,"%s",cabecera);
	int contador=0;
	for(int j=0;j<miLista.num;j++){
		ListaSockets[j]=miLista.Lista[j].socket;
		contador++;
	}
	sprintf(c,"%d",contador);
}
void EnviarInvitacion(char *p,char *notificacion,char *nombre,int ListaSockets[100],char * c){
	char *NomInv=strtok(p,",");
/*	strcpy(notificacion,"41/0/");*/
/*	strcat(notificacion,nombre);*/
	int contador=0;
	while(NomInv!=NULL){
		int sock=SocketNomJug(NomInv);
		if (sock!=-1){
			ListaSockets[0]=sock;
			contador++;
			printf("Invitacion enviada a %s\n",NomInv);
		}
		else{
			printf("41/0/ Jugador %s no encontrado\n",NomInv);
		}
		NomInv=strtok(NULL,",");
	}
	sprintf(c,"%d",contador);
}
void EnviarIDPartida(char *notificacion,char *nombreCreador,char *nombreInvitado,int IDpartida,int ListaSockets[100],char *c,char *NumForm){
/*	strcpy(notificacion,"43/");*/
/*	strcat(notificacion,NumForm);*/
/*	strcat(notificacion,"/");*/
/*	strcat(notificacion,nombreCreador);*/
/*	strcat(notificacion,"/");*/
/*	strcat(notificacion,nombreInvitado);*/
/*	strcat(notificacion,"/");*/
	sprintf(notificacion,"%s/%d",notificacion,IDpartida);
	
	int v[12];
	int n=12;
	char fr[512];//cambiar a 12
	RandomizeV(n,v,fr);
	sprintf(notificacion,"%s/%s",notificacion,fr);
	ListaSockets[0]=SocketNomJug(nombreCreador);
	ListaSockets[1]=SocketNomJug(nombreInvitado);
	sprintf(c,"%d",2);
}

int PonerPartidaATablaPartidasActivas(char *nombreCreador,char *nombreInvitado){
	int encontrado=0;
	int i=0;
	int IDPartida=-1;
	while((encontrado==0)&&(i<100)){
		if (TablaPartidasActivas[i].oc==0){
			encontrado=1;
			TablaPartidasActivas[i].oc=1;
			strcpy(TablaPartidasActivas[i].nombre1,nombreCreador);
			strcpy(TablaPartidasActivas[i].nombre2,nombreInvitado);
			TablaPartidasActivas[i].socket1=SocketNomJug(nombreCreador);
			TablaPartidasActivas[i].socket2=SocketNomJug(nombreInvitado);
			IDPartida=i;
		}
		else{
			i++;
		}
	}
	if (encontrado==0){
		printf("NO se ha podido poner la partida en la tabla de partidas activas\n");
	}
	else if(encontrado==1){
		printf("Partida introducida en la tabla de partidas activas SARISFACTORIAMENTE\n");
	}
	return IDPartida;
	
	
}
void EliminarPartdiaDeTablaPartidasActivas(int IDPartida){
	TablaPartidasActivas[IDPartida].oc=0;
}
void EnviarMSN(char * notificacion,int idP,char *nombre,char *msn,int ListaSockets[100],char *c){
	int contador=0;
	sprintf(notificacion,"44/%d/%s/%s",idP,nombre,msn);
	
	
	
	if(strcmp(nombre,TablaPartidasActivas[idP].nombre1)==0){
		ListaSockets[0]=SocketNomJug(TablaPartidasActivas[idP].nombre2);
		contador++;
		printf("44/Se ha enviado a %s el msn: %s\n",TablaPartidasActivas[idP].nombre2,notificacion);
	}
	else if(strcmp(nombre,TablaPartidasActivas[idP].nombre2)==0){
		ListaSockets[0]=SocketNomJug(TablaPartidasActivas[ idP].nombre1);
		contador++;
		printf("44/Se ha enviado a %s el msn: %s\n",TablaPartidasActivas[ idP].nombre1,notificacion);
	}
	else{
		printf("44/No se ha podido enviar el MSN\n");
	}
	sprintf(c,"%d",contador);
}
void EnviarCancelacion(char * notificacion,int IDPartida,char *nombre,int ListaSockets[100],char * c){
	strcpy(notificacion,"45/0/");
	int contador=0;
	if(strcmp(nombre,TablaPartidasActivas[IDPartida].nombre1)==0){
		sprintf(notificacion,"%s%d",notificacion,IDPartida);
		ListaSockets[0]=SocketNomJug(TablaPartidasActivas[IDPartida].nombre2);
		contador++;
		printf("Se ha enviado a %s la cancelacion\n",TablaPartidasActivas[IDPartida].nombre2);
	}
	else if(strcmp(nombre,TablaPartidasActivas[IDPartida].nombre2)==0){
		sprintf(notificacion,"%s%d",notificacion,IDPartida);
		ListaSockets[0]=SocketNomJug(TablaPartidasActivas[IDPartida].nombre1);
		contador++;
		printf("Se ha enviado a %s la cancelacion\n",TablaPartidasActivas[IDPartida].nombre1);
	}
	else{
		printf("No se ha podido enviar la cancelacion\n");
	}
	sprintf(c,"%d",contador);
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
	
	// Entramos en un bucle para atender todas las peticiones de este cliente
	//hasta que se desconecte
	while (terminar ==0){	
		
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
		
		char respuesta[512]="H";
		char notificacion[512]="H";
		char nombre[20];
		char contrasena[100];
		
		int ListaSockets[100];
		char contador[3];
		
		
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
				EnviarListaJugadoresConectados(notificacion,ListaSockets,contador);
			}
			terminar=1;
		}
		else if (codigo ==11){ //Aquí se hace el login
			p = strtok( NULL, "/");
			strcpy(contrasena,p);
			Login(contrasena,nombre,respuesta);
			if(strcmp(respuesta,"11/0/NO")!=0){
				pthread_mutex_lock(&mutex);
				AnadirJugadorListaConectados(nombre,*s);
				pthread_mutex_unlock(&mutex);
				CharJugCon(notificacion);
				EnviarListaJugadoresConectados(notificacion,ListaSockets,contador);
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
			char respuesta1[512];
			PuntosTotales( nombre,respuesta1);
			sprintf(respuesta,"33/0/%s",respuesta1);
			
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
		else if (codigo ==37){ //Cliente envia "37/nombre/idJ"
			//Servidor envia "37/PuntosTotales/PuntosMax..."
			char respuesta1[20];
			char respuesta2[20];
			char *idJ = strtok( NULL, "/");
			PuntosTotales(nombre,respuesta1);
			int MaxPunt=MaxPuntos(nombre);
			int PartGanadas=PartidasGanadas(idJ,nombre);
			int PartPerdidas=PartidasPerdidas(idJ,nombre);
			int PartJugadas=PartidasJugadas(idJ);
			CartaMasUsada(idJ,respuesta2);
			sprintf(respuesta,"37/0/%s/%d/%d/%d/%d/%s",respuesta1,MaxPunt,PartGanadas,PartPerdidas,PartJugadas,respuesta2);
		}
		else if (codigo ==38){
			char respuesta1[512]="";
			Ranking(respuesta1);
			sprintf(respuesta,"38/0/%s",respuesta1);
		}
		else if (codigo ==39){//Recibe "39/nomreUsuario/idUsuario/NombreVs"
			char respuesta1[20]="";
			char respuesta2[500]="";
			char respuesta3[20]="";
			char *idJ = strtok( NULL, "/");
			char *nombreVs = strtok( NULL, "/");
			char idJVs[20];
			BuscarIdUsurio(nombreVs,idJVs);
			PuntosTotales(nombreVs,respuesta1);
			int MaxPunt=MaxPuntos(nombreVs);
			int PartGanadas=PartidasGanadas(idJVs,nombreVs);
			int PartPerdidas=PartidasPerdidas(idJVs,nombreVs);
			int PartJugadas=PartidasJugadas(idJVs);
			int PartGanadasVs=PartidasGanadas(idJVs,nombre);
			int PartPerdidasVs=PartidasGanadas(idJ,nombreVs);
			printf("Antes codgio: %s\n",respuesta2);
			VSJugador(nombre, atoi(idJVs), respuesta2);
			printf("Fuera codgio: %s\n",respuesta2);
			int PartJugadasVs=0;
			char *p = strtok( respuesta2, ",");
			if((strcmp(p,"0")!=0)&&(strcmp(p,"-1")!=0))
			{
				while (p!=NULL){
					printf("39 codP %s\n",p);
					PartJugadasVs++;
					p = strtok( NULL, ",");
				}
			}
			CartaMasUsada(idJVs,respuesta3);
			sprintf(respuesta,"39/0/%s/%s/%s/%d/%d/%d/%d/%d/%d/%d/%s",nombreVs,idJVs,respuesta1,MaxPunt,PartGanadas,PartPerdidas,PartJugadas,PartGanadasVs,PartPerdidasVs,PartJugadasVs,respuesta3);
		}
		
		else if (codigo ==41){ //Cliente envia "41/NumForm/NombreJugadorQueHaCreadoPartida/JugadorInvitado1,JI2,JI3/Parametros,Partida"
			//Servidor envia "41/NumForm/NombreJugadorQueHaCreadoPartida/Parametros,Partida"
			//Procedimiento de invitacion
			char notificacion1[512];
			char *nombre1 = strtok( NULL, "/");
			sprintf(notificacion1,"41/%s/%s",nombre,nombre1);
			p=strtok( NULL, "/");
			char *parametros = strtok( NULL, "/");
			EnviarInvitacion(p,notificacion1,nombre1,ListaSockets,contador);
			sprintf(notificacion,"%s/%s",notificacion1,parametros);
		}
		
		else if (codigo ==42){ //Cliente envia "42/NumForm/NombreJugadorQueHaCreadoPartida/JugadorQueHaAceptadoORechazado/SIoNO"
			//Servidor envia "42/NumForm/nombreCreador/nombreInvitado/SIoNO/IDpartida/lista,Random"
			strcpy(notificacion,"42/");
			strcat(notificacion,nombre);
			strcat(notificacion,"/");
			char *nombreCreador=strtok( NULL, "/");
			strcat(notificacion,nombreCreador);
			strcat(notificacion,"/");
			char *nombreInvitado=strtok( NULL, "/");
			char *SIoNO=strtok( NULL, "/");
			strcat(notificacion,nombreInvitado);
			strcat(notificacion,"/");
			strcat(notificacion,SIoNO);
			
			if (strcmp(SIoNO,"Si")==0){
				int IDPartida=PonerPartidaATablaPartidasActivas(nombreCreador,nombreInvitado);
				EnviarIDPartida(notificacion,nombreCreador,nombreInvitado,IDPartida,ListaSockets,contador,nombre);	
			}
			else{
				int sock=SocketNomJug(nombreCreador);
				write (sock,notificacion, strlen(notificacion));
				sprintf(notificacion,"H");
			}
			
		}
		else if (codigo ==44){ //Cliente envia "44/Nform/IDPartida/NombreQuienEnviaMsn/Msn"
			//Servidor envia "44/IDPartida/NombreQuienEnviaMsn/Msn
			
			p = strtok( NULL, "/");
			int IDPartida=atoi(p);
			p=strtok(NULL,"/");
			char NombreQuienEnviaMsn[20];
			strcpy(NombreQuienEnviaMsn,p);
			char Msn[500];
			p=strtok(NULL,"/");
			strcpy(Msn,p);
			EnviarMSN(notificacion,IDPartida,NombreQuienEnviaMsn,Msn,ListaSockets,contador);
		}
		else if (codigo ==45){ //Cliente envia "45/IDPartida/NombreQuienEnviaCancelacion"
			//Servidor envia "45/IDPartida"
			int IDPartida=atoi(nombre);
			char *NombreQuienEnviaCancelacion=strtok(NULL,"/");
			EnviarCancelacion(notificacion,IDPartida,NombreQuienEnviaCancelacion,ListaSockets,contador);
		}
		else if (codigo ==46){ //Cliente envia "46/IDPartida/NombreQuienGira/NumeroCarta" el que gira la carta
			//Servidor envia "46/IDPartida/NumeroCarta" al jugador que no ha girado la carta
			
			int IDPartida=atoi(p);
			p=strtok(NULL,"/");
			char NombreQuienEnviaMsn[20];
			strcpy(NombreQuienEnviaMsn,p);
			char Msn[500];
			p=strtok(NULL,"/");
			strcpy(Msn,p);
			EnviarMSN(notificacion,IDPartida,NombreQuienEnviaMsn,Msn,ListaSockets,contador);
			sprintf(notificacion,"46/%d/%s",IDPartida,Msn);
		}
		
		printf ("Respuesta: %s\n", respuesta);
		printf ("notificacion: %s\n", notificacion);
		
		// Enviamos respuesta
		
		if(strcmp(respuesta,"H")!=0){
			write (sock_conn,respuesta, strlen(respuesta));
			printf ("respuesta enviada\n");
		}
		if(strcmp(notificacion,"H")!=0){
			int indice=0;
			while(indice< atoi(contador)){
				write (ListaSockets[indice],notificacion, strlen(notificacion));
				indice++;
			}
			printf ("notificacion enviada\n");
		}
		
		
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
		printf("Error creant socket\n");
	// Fem el bind al port
	
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;
	
	// asocia el socket a cualquiera de las IP de la m?quina. 
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// establecemos el puerto de escucha
	serv_adr.sin_port = htons(50060);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind\n");
	
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen\n");
	
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
	
	
	pthread_t *thread;
	thread= (pthread_t *) calloc(miLista.num+1,sizeof(pthread_t));
	
	sockets= (int *) calloc(miLista.num+1,sizeof(int));
	
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
