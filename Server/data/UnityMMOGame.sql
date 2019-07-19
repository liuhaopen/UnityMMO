/*
Navicat MySQL Data Transfer

Source Server         : UnityMMO
Source Server Version : 50640
Source Host           : 192.168.5.132:3306
Source Database       : UnityMMOGame

Target Server Type    : MYSQL
Target Server Version : 50640
File Encoding         : 65001

Date: 2019-07-19 19:35:19
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for Bag
-- ----------------------------
DROP TABLE IF EXISTS `Bag`;
CREATE TABLE `Bag` (
  `goods_uid` bigint(20) unsigned NOT NULL,
  `goods_type_id` int(10) unsigned NOT NULL,
  `role_id` bigint(20) unsigned NOT NULL,
  `pos` tinyint(10) unsigned NOT NULL,
  `cell` smallint(10) unsigned NOT NULL,
  `num` int(10) unsigned NOT NULL,
  PRIMARY KEY (`goods_uid`),
  KEY `role_id` (`role_id`) USING BTREE,
  KEY `pos` (`pos`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for BagInfo
-- ----------------------------
DROP TABLE IF EXISTS `BagInfo`;
CREATE TABLE `BagInfo` (
  `role_id` bigint(20) unsigned zerofill NOT NULL,
  `pos` varchar(255) DEFAULT NULL,
  `cell_num` mediumint(10) unsigned zerofill NOT NULL,
  KEY `role_id` (`role_id`,`pos`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `role_id` bigint(20) unsigned NOT NULL,
  `name` varchar(18) NOT NULL,
  `career` tinyint(4) NOT NULL,
  `level` smallint(5) unsigned DEFAULT '0',
  `scene_id` int(11) DEFAULT NULL,
  `pos_x` int(11) DEFAULT NULL,
  `pos_y` int(11) DEFAULT NULL,
  `pos_z` int(11) DEFAULT NULL,
  `coin` int(11) unsigned zerofill DEFAULT NULL,
  `diamond` int(11) unsigned zerofill DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for RoleList
-- ----------------------------
DROP TABLE IF EXISTS `RoleList`;
CREATE TABLE `RoleList` (
  `account_id` bigint(20) unsigned NOT NULL,
  `role_id` bigint(20) unsigned NOT NULL,
  `create_time` bigint(20) unsigned NOT NULL,
  KEY `account_id` (`account_id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

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
  `contentID` int(8) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `role_id` (`roleID`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=latin1;
