/*
Navicat MySQL Data Transfer

Source Server         : UnityMMO
Source Server Version : 50640
Source Host           : 192.168.5.132:3306
Source Database       : UnityMMOAccount

Target Server Type    : MYSQL
Target Server Version : 50640
File Encoding         : 65001

Date: 2019-08-24 16:33:50
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
