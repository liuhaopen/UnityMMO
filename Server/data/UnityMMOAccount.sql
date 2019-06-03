/*
Navicat MySQL Data Transfer

Source Server         : UnityMMO
Source Server Version : 50640
Source Host           : 192.168.5.115:3306
Source Database       : UnityMMOAccount

Target Server Type    : MYSQL
Target Server Version : 50640
File Encoding         : 65001

Date: 2019-06-02 12:47:26
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for Account
-- ----------------------------
DROP TABLE IF EXISTS `Account`;
CREATE TABLE `Account` (
  `account_id` bigint(20) NOT NULL,
  `password` char(20) DEFAULT NULL,
  PRIMARY KEY (`account_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of Account
-- ----------------------------
INSERT INTO `Account` VALUES ('123', 'password');
INSERT INTO `Account` VALUES ('213', 'password');
INSERT INTO `Account` VALUES ('222', 'password');
INSERT INTO `Account` VALUES ('1234', 'password');
INSERT INTO `Account` VALUES ('2123', 'password');
INSERT INTO `Account` VALUES ('2134', 'password');
INSERT INTO `Account` VALUES ('12333', 'password');
INSERT INTO `Account` VALUES ('213213', 'password');
