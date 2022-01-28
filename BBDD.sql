DROP DATABASE IF EXISTS T4BBDD;
CREATE DATABASE T4BBDD;

USE T4BBDD;

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
