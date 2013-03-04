<?php
error_reporting(E_ALL);

define('FRAMEWORK_ERROR_HANDLING', 1);

if (!defined('DS')) {
  define('DS', DIRECTORY_SEPARATOR);
}

if (!defined('FRAMEWORK_BASEDIR')) {
  define('FRAMEWORK_BASEDIR', dirname(__FILE__) . DS);
}

if(!defined('FRAMEWORK_VERSION')) {
  define('FRAMEWORK_VERSION', '0.1');
}

require_once(FRAMEWORK_BASEDIR . 'core' . DS . 'functions' . DS . 'arrays.php');
require_once(FRAMEWORK_BASEDIR . 'core' . DS . 'Atlas.php');

$atlas = new Atlas();
$atlas->getResponse();
