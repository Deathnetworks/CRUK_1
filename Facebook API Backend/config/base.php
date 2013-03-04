<?php

/* URL routing, use preg_replace() compatible syntax */
$config['routing']['search'] =  array();
$config['routing']['replace'] = array();

/* set this to force controller and method instead of using URL params */
$config['root_controller'] = null;
$config['root_action'] = null;

/* name of default controller/method when none is given in the URL */
$config['default_controller'] = 'Auth';
$config['default_action'] = 'index';
