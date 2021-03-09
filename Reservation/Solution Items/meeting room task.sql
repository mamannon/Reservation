/*
SQLyog Community v12.2.2 (64 bit)
MySQL - 5.7.12-log : Database - meeting_rooms
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`meeting_rooms` /*!40100 DEFAULT CHARACTER SET utf8 COLLATE utf8_swedish_ci */;

USE `meeting_rooms`;

/*Table structure for table `meeting_room` */

DROP TABLE IF EXISTS `meeting_room`;

CREATE TABLE `meeting_room` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8_swedish_ci DEFAULT NULL,
  `location` varchar(255) COLLATE utf8_swedish_ci DEFAULT NULL,
  `seats` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_swedish_ci;

/*Data for the table `meeting_room` */

/*Table structure for table `reservation` */

DROP TABLE IF EXISTS `reservation`;

CREATE TABLE `reservation` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `meeting_room_id` int(11) DEFAULT NULL,
  `host` varchar(255) COLLATE utf8_swedish_ci DEFAULT NULL COMMENT 'Person who made the reservation',
  `time_from` datetime DEFAULT NULL,
  `time_to` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_meeting_room_id` (`meeting_room_id`),
  CONSTRAINT `fk_meeting_room_id` FOREIGN KEY (`meeting_room_id`) REFERENCES `meeting_room` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_swedish_ci;

/*Data for the table `reservation` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
