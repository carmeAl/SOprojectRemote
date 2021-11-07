DROP DATABASE IF EXISTS BBDDv1;
CREATE DATABASE BBDDv1;

USE BBDDv1;

CREATE TABLE jugador(
	id INT not null,
	PRIMARY KEY(id),
	nombre VARCHAR(60),
	password VARCHAR(20)
)ENGINE=InnoDB;

CREATE TABLE partidas(
	id INT not null,
	PRIMARY KEY(id),
	fecha VARCHAR(19),
	duracion INT,
	ganador VARCHAR(20) 
)ENGINE=InnoDB;

CREATE TABLE participacion(
	idJ INT,
	idP INT,
	puntos INT,
	personaje VARCHAR(20),
	FOREIGN KEY (idP) REFERENCES partidas(id),
	FOREIGN KEY (idJ) REFERENCES jugador(id)
)ENGINE=InnoDB;


INSERT INTO jugador VALUES(1,"Xavier","123");
INSERT INTO jugador VALUES(2,"Victoria","456");
INSERT INTO jugador VALUES(3,"Genis","789");
INSERT INTO jugador VALUES(4,"Carme","1234");


INSERT INTO partidas VALUES(1,"29/09/2021 15:38",10,"Xavier");
INSERT INTO partidas VALUES(2,"29/09/2021 15:39",12,"Victoria");
INSERT INTO partidas VALUES(3,"29/09/2021 15:40",9,"Genis");
INSERT INTO partidas VALUES(4,"29/09/2021 15:41",5,"Carme");
INSERT INTO partidas VALUES(5,"29/09/2021 15:42",15,"Xavier");
INSERT INTO partidas VALUES(6,"29/09/2021 15:43",9,"Victoria");


INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(1,1,20,"Personaje 1");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(2,1,10,"Personaje 2");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(2,2,20,"Personaje 3");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(4,2,10,"Personaje 4");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(3,3,20,"Personaje 5");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(4,4,20,"Personaje 6");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(3,3,10,"Personaje 7");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(2,4,10,"Personaje 8");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(1,5,20,"Personaje 9");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(2,6,20,"Personaje 10");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(3,6,10,"Personaje 11");
INSERT INTO participacion(idJ,idP,puntos,personaje) VALUES(4,5,10,"Personaje 12");
