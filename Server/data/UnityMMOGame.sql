/*
Navicat MySQL Data Transfer

Source Server         : UnityMMO
Source Server Version : 50640
Source Host           : 192.168.5.115:3306
Source Database       : UnityMMOGame

Target Server Type    : MYSQL
Target Server Version : 50640
File Encoding         : 65001

Date: 2019-06-02 12:46:41
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
-- Records of Account
-- ----------------------------
INSERT INTO `Account` VALUES ('123', '1100585369600', '1100585369601', null);
INSERT INTO `Account` VALUES ('213', '1100585369602', null, null);

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
-- Records of RoleBaseInfo
-- ----------------------------
INSERT INTO `RoleBaseInfo` VALUES ('1100585369600', '开朗的赤穗', '2', '0', '1001', '69158', '16377', '114931');
INSERT INTO `RoleBaseInfo` VALUES ('1100585369601', '可爱哒圭介', '1', '0', '1001', '76711', '16433', '114771');
INSERT INTO `RoleBaseInfo` VALUES ('1100585369602', '可爱哒恭平', '1', '0', '1001', '84934', '16420', '128311');

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
-- Records of RoleLooksInfo
-- ----------------------------
INSERT INTO `RoleLooksInfo` VALUES ('1100585369600', '1', '1', null, null);
INSERT INTO `RoleLooksInfo` VALUES ('1100585369601', '0', '0', null, null);
INSERT INTO `RoleLooksInfo` VALUES ('1100585369602', '1', '1', null, null);

-- ----------------------------
-- Table structure for Task
-- ----------------------------
DROP TABLE IF EXISTS `Task`;
CREATE TABLE `Task` (
  `role_id` bigint(50) NOT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records of Task
-- ----------------------------
