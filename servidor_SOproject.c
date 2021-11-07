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


int main(int argc, char *argv[])
{
	
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
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
	serv_adr.sin_port = htons(9121);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error al bind");
	
	if (listen(sock_listen, 3) < 0)
		printf("Error en el Listen");
	
	MYSQL *conn;
	int err;
	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexi??n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexion
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "BBDDv1",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexi??n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	int i;
	// Bucle infinito
	for (;;){
		printf ("Escuchando\n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("He recibido conexion\n");
		//sock_conn es el socket que usaremos para este cliente
		
		int terminar =0;
		// Entramos en un bucle para atender todas las peticiones de este cliente
		//hasta que se desconecte
		while (terminar ==0)
		{	strcpy(respuesta,"");
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
		if (codigo !=0) // Conseguir el nombre
		{
			p = strtok( NULL, "/");
			
			strcpy (nombre, p);
			// Ya tenemos el nombre
			printf ("Codigo: %d, Nombre: %s\n", codigo, nombre);
		}
		
		if (codigo ==0) //petici?n de desconexi?n
			terminar=1;
		else if (codigo ==11){ //Aquí se hace el login
			
			char consulta[100];
			p = strtok( NULL, "/");
			strcpy(contrasena,p);
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
				sprintf(respuesta,"NO");
			else{
				sprintf(respuesta,"%s",row[0]);
			}
			// cerrar la conexion con el servidor MYSQL 
			/*				mysql_close (conn);*/
		}
		else if (codigo ==21){
			char consulta[100];
			p = strtok( NULL, "/");
			strcpy(contrasena,p);
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
				err=mysql_query (conn, insert); 
				if (err!=0) {
					printf ("Error al insertar datos de la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
					sprintf(respuesta,"NO");					}
				else
					sprintf(respuesta,"SI");
			}
			else{
				sprintf(respuesta,"NO");
			}
			// cerrar la conexion con el servidor MYSQL 
			/*				mysql_close (conn);*/
		}
		else if (codigo ==32){ //Peticion partidas consecutivas ganadas 
			//por el Jugador cuyo nombre se recibe como "31/Xavier"
			//Respuesta "numeroPartidas" (enetro)
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
				sprintf(respuesta,"%s","-1");
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
				sprintf(respuesta,"%d",max);
			}
		}
		else if (codigo ==33){ //Peticion Suma de los puntos del jugador introducido
			//Peticion "33/Victoria"
			//Respuesta "Puntos" (enetro)
			//Respuesta -1 si el jugador no existe
			
			// construimos la consulta SQL
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
				sprintf(respuesta,"%s","-1");
			}
			else{
				sprintf(respuesta,"%s", row[0]);
			}
		}
		else if (codigo ==34){ //Peticion Lista nombre jugadores que han ganado contra el jugador introducido
			//Peticion "34/Genis"
			//Respuesta "Nombre1/Nombre2/Nombre3/" (char)
			//Respuesta -1 si el jugador no existe
			char consulta [80];
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
				sprintf (respuesta,"%s","-1");
			else{
				int i=0;
				while(row !=NULL){
					// El resultado debe ser una matriz con una sola fila
					// y una columna que contiene el nombre
					sprintf (respuesta,"%s%s/", respuesta,row[0]);
					row = mysql_fetch_row (resultado);
				}
			}
		}
		else if (codigo ==35){ //Peticion 35 Lista de IDs de partidas que el Usuario ha tenido con un jugador introducido
			//Peticion "35/Carme/idUsuario"
			//Respuesta "idPartida1/idPartida2/idPartida3/" (char)
			//Respuesta -1 si el jugador no existe
			p = strtok( NULL, "/");
			int idUsuario=atoi(p);
			
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
					
				}
			}
		}
		if (codigo !=0)
		{
			printf ("Respuesta: %s\n", respuesta);
			// Enviamos respuesta
			write (sock_conn,respuesta, strlen(respuesta));
		}
		}
		// Se acabo el servicio para este cliente
		close(sock_conn); 
	}
}
