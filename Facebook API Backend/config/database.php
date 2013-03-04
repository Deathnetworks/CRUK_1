<?php

$services =  json_decode(getenv('VCAP_SERVICES'), true);
$credentials = $services['mysql-5.1'][0]['credentials'];

$config['default']['plugin'] = 'PDODatabase'; 
$config['default']['type'] = 'mysql';      
$config['default']['host'] = $credentials['hostname']; 
$config['default']['name'] = $credentials['name'];     
$config['default']['user'] = $credentials['username'];
$config['default']['pass'] = $credentials['password'];    
$config['default']['charset'] = '';     
$config['default']['persistent'] = true;  
