<?php

class Load {
  public function model($model_name, $model_alias = null, $file_name = null,
    $pool_name = null) {
    
    if(!isset($model_alias)) {
      $model_alias = $model_name;
    }
    
    if(!isset($file_name)) {
      $file_name = strtolower($model_name) . '.php';
    }
    
    if(empty($model_alias)) {
      throw new Exception('Model alias ' . $model_alias . ' is reserved');
    }
    
    return new $model_name;
  }
  
  public function library($lib_name, $alias = null, $file_name = null) {
    if (!isset($alias)) {
      $alias = $lib_name;
    }

    if(empty($alias)) {
      throw new Exception("Library name cannot be empty");
    }

    if(!preg_match('!^[a-zA-Z][a-zA-Z_]+$', $alias)) {
      throw new Exception("Library name '{$alias}' is an invalid name");
    }

    if (method_exists($this, $alias)) {
      throw new Exception("Library name '{$alias}' is a reserved name");
    }

    $controller = Atlas::instance(null, 'controller');
    
    //TODO: log this
    if (isset($controller->$alias)) {
      return true;
    }
    
    $class_name = "Library{$lib_name}";
    $cotroller->$alias = new $class_name;
    return true;
  }

  public function database($poolname = null) {
    static $dbs = array();

    include(FRAMEWORK_BASEDIR . 'config' . DS . 'database.php');
     
    if(!$poolname) {
      $poolname = 'default';
    }        
    
    if ($poolname && isset($dbs[$poolname])) {
      return $dbs[$poolname];
    }
    
    if ($poolname && isset($config[$poolname]) && 
      !empty($config[$poolname]['plugin'])) {
      
      $dbs[$poolname] = 
        new $config[$poolname]['plugin']($config[$poolname]);
      return $dbs[$poolname];
    }
  }  
}
