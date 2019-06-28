/*
Navicat MySQL Data Transfer

Source Server         : UnityMMO
Source Server Version : 50640
Source Host           : 192.168.5.132:3306
Source Database       : UnityMMOGame

Target Server Type    : MYSQL
Target Server Version : 50640
File Encoding         : 65001

Date: 2019-06-28 17:40:31
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for Account
-- ----------------------------
DROP TABLE IF EXISTS `Account`;
CREATE TABLE `Account` (
  `account_id` bigint(20) NOT NULL,
  `role_id_1` bigint(20) DEFAULT NULL,
  `role_id_2` bigint(20) DEFAULT NULL,
  `role_id_3` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`account_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for Property
-- ----------------------------
DROP TABLE IF EXISTS `Property`;
CREATE TABLE `Property` (
  `role_id` bigint(60) NOT NULL,
  `att` int(255) DEFAULT NULL,
  `hp` int(255) DEFAULT NULL,
  `def` int(255) DEFAULT NULL,
  `hit` int(255) DEFAULT NULL,
  `dodge` int(255) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for RoleBaseInfo
-- ----------------------------
DROP TABLE IF EXISTS `RoleBaseInfo`;
CREATE TABLE `RoleBaseInfo` (
  `role_id` bigint(20) NOT NULL,
  `name` varchar(18) NOT NULL,
  `career` tinyint(4) NOT NULL,
  `level` smallint(5) unsigned NOT NULL DEFAULT '0',
  `scene_id` int(11) DEFAULT NULL,
  `pos_x` int(11) DEFAULT NULL,
  `pos_y` int(11) DEFAULT NULL,
  `pos_z` int(11) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for RoleLooksInfo
-- ----------------------------
DROP TABLE IF EXISTS `RoleLooksInfo`;
CREATE TABLE `RoleLooksInfo` (
  `role_id` bigint(64) NOT NULL,
  `body` int(10) DEFAULT NULL,
  `hair` int(10) DEFAULT NULL,
  `weapon` int(10) DEFAULT NULL,
  `wing` int(10) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for TaskInfo
-- ----------------------------
DROP TABLE IF EXISTS `TaskInfo`;
CREATE TABLE `TaskInfo` (
  `roleID` bigint(50) NOT NULL,
  `mainTaskID` int(11) DEFAULT NULL,
  PRIMARY KEY (`roleID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for TaskList
-- ----------------------------
DROP TABLE IF EXISTS `TaskList`;
CREATE TABLE `TaskList` (
  `id` bigint(60) NOT NULL AUTO_INCREMENT,
  `roleID` bigint(60) NOT NULL,
  `taskID` int(11) NOT NULL,
  `status` tinyint(5) DEFAULT NULL,
  `subTaskIndex` tinyint(10) unsigned zerofill DEFAULT NULL,
  `subType` int(10) DEFAULT NULL,
  `curProgress` int(8) unsigned zerofill DEFAULT NULL,
  `maxProgress` int(8) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `role_id` (`roleID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=latin1;
