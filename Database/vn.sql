-- MySQL dump 10.13  Distrib 8.4.7, for Win64 (x86_64)
--
-- Host: localhost    Database: vn
-- ------------------------------------------------------
-- Server version	8.4.7

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `vn`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `vn` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_bin */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `vn`;

--
-- Table structure for table `alue`
--

DROP TABLE IF EXISTS `alue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `alue` (
  `alue_id` int unsigned NOT NULL AUTO_INCREMENT,
  `nimi` varchar(40) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`alue_id`),
  KEY `alue_nimi_index` (`nimi`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alue`
--

LOCK TABLES `alue` WRITE;
/*!40000 ALTER TABLE `alue` DISABLE KEYS */;
INSERT INTO `alue` VALUES (3,'Keski-Suomi'),(1,'Kuopio'),(2,'Lappi'),(5,'Pirkanmaa'),(4,'Uusimaa');
/*!40000 ALTER TABLE `alue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `asiakas`
--

DROP TABLE IF EXISTS `asiakas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `asiakas` (
  `asiakas_id` int unsigned NOT NULL AUTO_INCREMENT,
  `postinro` char(5) COLLATE utf8mb4_bin NOT NULL,
  `etunimi` varchar(20) COLLATE utf8mb4_bin DEFAULT NULL,
  `sukunimi` varchar(40) COLLATE utf8mb4_bin DEFAULT NULL,
  `lahiosoite` varchar(40) COLLATE utf8mb4_bin DEFAULT NULL,
  `email` varchar(50) COLLATE utf8mb4_bin DEFAULT NULL,
  `puhelinnro` varchar(15) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`asiakas_id`),
  KEY `fk_as_posti1_idx` (`postinro`),
  KEY `asiakas_snimi_idx` (`sukunimi`),
  KEY `asiakas_enimi_idx` (`etunimi`),
  CONSTRAINT `fk_asiakas_posti` FOREIGN KEY (`postinro`) REFERENCES `posti` (`postinro`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `asiakas`
--

LOCK TABLES `asiakas` WRITE;
/*!40000 ALTER TABLE `asiakas` DISABLE KEYS */;
INSERT INTO `asiakas` VALUES (2,'80000','Aleksi','Valavuori','vuorentie 88','aleksi.valavuori@savonia.fi','044 123 4567'),(3,'50500','Pasi','Kuikka','Silkkiuikuntie 40','pasikuikka82@gmail.com','0449876541');
/*!40000 ALTER TABLE `asiakas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lasku`
--

DROP TABLE IF EXISTS `lasku`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lasku` (
  `lasku_id` int NOT NULL,
  `varaus_id` int unsigned NOT NULL,
  `summa` double(8,2) NOT NULL,
  `alv` double(8,2) NOT NULL,
  `maksettu` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`lasku_id`),
  KEY `lasku_ibfk_1` (`varaus_id`),
  CONSTRAINT `lasku_ibfk_1` FOREIGN KEY (`varaus_id`) REFERENCES `varaus` (`varaus_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lasku`
--

LOCK TABLES `lasku` WRITE;
/*!40000 ALTER TABLE `lasku` DISABLE KEYS */;
/*!40000 ALTER TABLE `lasku` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mokki`
--

DROP TABLE IF EXISTS `mokki`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mokki` (
  `mokki_id` int unsigned NOT NULL AUTO_INCREMENT,
  `alue_id` int unsigned NOT NULL,
  `postinro` char(5) COLLATE utf8mb4_bin NOT NULL,
  `mokkinimi` varchar(45) COLLATE utf8mb4_bin DEFAULT NULL,
  `katuosoite` varchar(45) COLLATE utf8mb4_bin DEFAULT NULL,
  `hinta` double(8,2) NOT NULL,
  `kuvaus` varchar(150) COLLATE utf8mb4_bin DEFAULT NULL,
  `henkilomaara` int DEFAULT NULL,
  `varustelu` varchar(100) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`mokki_id`),
  KEY `fk_mokki_alue_idx` (`alue_id`),
  KEY `fk_mokki_posti_idx` (`postinro`),
  CONSTRAINT `fk_mokki_alue` FOREIGN KEY (`alue_id`) REFERENCES `alue` (`alue_id`),
  CONSTRAINT `fk_mokki_posti` FOREIGN KEY (`postinro`) REFERENCES `posti` (`postinro`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mokki`
--

LOCK TABLES `mokki` WRITE;
/*!40000 ALTER TABLE `mokki` DISABLE KEYS */;
INSERT INTO `mokki` VALUES (1,1,'70200','Eliaksenmökki','jokitalontie 20',180.00,'Kaunis lukaali Kallaveden välittömässä läheisyydessä.',6,'Kolme WC, ulkosauna, keittiö kaikkine hilppeineen, 4 makuuhuonetta, parvi'),(2,2,'60800','Otto-Pekan Mörskä','Ojalantie 67',60.00,'Erämökki ilman ylimääräisiä mukavuuksia',4,'Tupakeittiö, kaksi kerrossänkyä, ulkohuussi');
/*!40000 ALTER TABLE `mokki` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `palvelu`
--

DROP TABLE IF EXISTS `palvelu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `palvelu` (
  `palvelu_id` int unsigned NOT NULL,
  `alue_id` int unsigned NOT NULL,
  `nimi` varchar(40) COLLATE utf8mb4_bin DEFAULT NULL,
  `kuvaus` varchar(255) COLLATE utf8mb4_bin DEFAULT NULL,
  `hinta` double(8,2) NOT NULL,
  `alv` double(8,2) NOT NULL,
  PRIMARY KEY (`palvelu_id`),
  KEY `Palvelu_nimi_index` (`nimi`),
  KEY `palv_toimip_id_ind` (`alue_id`),
  CONSTRAINT `palvelu_ibfk_1` FOREIGN KEY (`alue_id`) REFERENCES `alue` (`alue_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `palvelu`
--

LOCK TABLES `palvelu` WRITE;
/*!40000 ALTER TABLE `palvelu` DISABLE KEYS */;
INSERT INTO `palvelu` VALUES (1,2,'Joulupukin tapaaminen','Koe Lapin aito joulu, ja käy tervehtimässä oikeaa Joulupukkia!',45.99,25.50),(2,1,'Puijontorni','Näe Kuopio yläilmoista. Hintaan kuuluu kyyditys, lippu torniin ja opastus',30.00,25.50);
/*!40000 ALTER TABLE `palvelu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `posti`
--

DROP TABLE IF EXISTS `posti`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posti` (
  `postinro` char(5) COLLATE utf8mb4_bin NOT NULL,
  `toimipaikka` varchar(45) COLLATE utf8mb4_bin DEFAULT NULL,
  PRIMARY KEY (`postinro`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `posti`
--

LOCK TABLES `posti` WRITE;
/*!40000 ALTER TABLE `posti` DISABLE KEYS */;
INSERT INTO `posti` VALUES ('50500','Tuntematon'),('60800','Tuntematon'),('70200','Tuntematon'),('80000','Tuntematon');
/*!40000 ALTER TABLE `posti` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `varauksen_palvelut`
--

DROP TABLE IF EXISTS `varauksen_palvelut`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `varauksen_palvelut` (
  `varaus_id` int unsigned NOT NULL,
  `palvelu_id` int unsigned NOT NULL,
  `lkm` int NOT NULL,
  PRIMARY KEY (`palvelu_id`,`varaus_id`),
  KEY `vp_varaus_id_index` (`varaus_id`),
  KEY `vp_palvelu_id_index` (`palvelu_id`),
  CONSTRAINT `fk_palvelu` FOREIGN KEY (`palvelu_id`) REFERENCES `palvelu` (`palvelu_id`),
  CONSTRAINT `fk_varaus` FOREIGN KEY (`varaus_id`) REFERENCES `varaus` (`varaus_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `varauksen_palvelut`
--

LOCK TABLES `varauksen_palvelut` WRITE;
/*!40000 ALTER TABLE `varauksen_palvelut` DISABLE KEYS */;
/*!40000 ALTER TABLE `varauksen_palvelut` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `varaus`
--

DROP TABLE IF EXISTS `varaus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `varaus` (
  `varaus_id` int unsigned NOT NULL AUTO_INCREMENT,
  `asiakas_id` int unsigned NOT NULL,
  `mokki_id` int unsigned NOT NULL,
  `varattu_pvm` datetime DEFAULT NULL,
  `vahvistus_pvm` datetime DEFAULT NULL,
  `varattu_alkupvm` datetime DEFAULT NULL,
  `varattu_loppupvm` datetime DEFAULT NULL,
  PRIMARY KEY (`varaus_id`),
  KEY `varaus_as_id_index` (`asiakas_id`),
  KEY `fk_var_mok_idx` (`mokki_id`),
  CONSTRAINT `fk_varaus_mokki` FOREIGN KEY (`mokki_id`) REFERENCES `mokki` (`mokki_id`),
  CONSTRAINT `varaus_ibfk` FOREIGN KEY (`asiakas_id`) REFERENCES `asiakas` (`asiakas_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `varaus`
--

LOCK TABLES `varaus` WRITE;
/*!40000 ALTER TABLE `varaus` DISABLE KEYS */;
INSERT INTO `varaus` VALUES (1,2,1,'2026-11-10 13:10:03','2026-11-10 13:10:03','2026-11-19 00:00:00','2026-11-21 00:00:00'),(2,3,2,'2026-11-10 13:12:40','2026-11-10 13:12:40','2026-12-24 00:00:00','2026-12-27 00:00:00');
/*!40000 ALTER TABLE `varaus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'vn'
--

--
-- Dumping routines for database 'vn'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-11-10 13:39:29
