-- MySQL dump 10.13  Distrib 8.0.34, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: sendkey
-- ------------------------------------------------------
-- Server version	8.0.35

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `sendkeys`
--

DROP TABLE IF EXISTS `sendkeys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sendkeys` (
  `KEY` varchar(45) NOT NULL,
  `CODE` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sendkeys`
--

LOCK TABLES `sendkeys` WRITE;
/*!40000 ALTER TABLE `sendkeys` DISABLE KEYS */;
INSERT INTO `sendkeys` VALUES ('A','A'),('B','B'),('C','C'),('D','D'),('E','E'),('F','F'),('G','G'),('H','H'),('I','I'),('J','J'),('K','K'),('L','L'),('M','M'),('N','N'),('O','O'),('P','P'),('Q','Q'),('R','R'),('S','S'),('T','T'),('U','U'),('V','V'),('W','W'),('X','X'),('Y','Y'),('Z','Z'),('Escape','{ESC}'),('F1','{F1}'),('F2','{F2}'),('F3','{F3}'),('F4','{F4}'),('F5','{F5}'),('F6','{F6}'),('F7','{F7}'),('F8','{F8}'),('F9','{F9}'),('F10','{F10}'),('F11','{F11}'),('F12','{F12}'),('Oemtilde','`'),('D1','1'),('D2','2'),('D3','3'),('D4','4'),('D5','5'),('D6','6'),('D7','7'),('D8','8'),('D9','9'),('D0','0'),('OemMinus','-'),('Oemplus','='),('Back','{BACKSPACE}'),('Insert ','{INSERT}'),('Home','{HOME}'),('PageUp','{PGUP}'),('Delete','{DELETE}'),('End','{END}'),('Next','{PGDN}'),('Capital','{CAPSLOCK}'),('LWin','{LWIN}'),('NumPad0','{NUMPAD0}'),('NumPad1','{NUMPAD1}'),('NumPad2','{NUMPAD2}'),('NumPad3','{NUMPAD3}'),('NumPad4','{NUMPAD4}'),('NumPad5','{NUMPAD5}'),('NumPad6','{NUMPAD6}'),('NumPad7','{NUMPAD7}'),('NumPad8','{NUMPAD8}'),('NumPad9','{NUMPAD9}'),('NumLock','{NUMLOCK}'),('Decimal','{NUMPADDEC}'),('Add','{NUMPADADD}'),('Subtract','{NUMPADSUB}'),('Multiply','{NUMPADMULT}'),('Divide','{NUMPADDIV}'),('Ctrl + A','^a'),('Ctrl + C','^c'),('Ctrl + V','^v'),('Ctrl + Z','^z'),('Ctrl + F','^f'),('Ctrl + W','^w'),('Ctrl + S','^s'),('Ctrl + T','^t'),('Ctrl + P','^p'),('Ctrl + Y','^y'),('Shift + Delete','+{DELETE}'),('Shift + F10','+{F10}'),('Alt + Return','!{ENTER}'),('Alt + F4','!{F4}'),('Ctrl + L','^l');
/*!40000 ALTER TABLE `sendkeys` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-02-15 19:46:45
