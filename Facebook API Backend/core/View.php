<?php

class View {
  private $view_vars = array();
  
  public function assign($key, $value = null) {
    if($value) {
      $this->view_vars[$key] = $value;
    } else {
      foreach($key as $k => $v) {
        if(is_int($k)) {
          $this->view_vars[] = $v;
        } else {
          $this->view_vars[$k] = $v;
        }
      }
    }
  }

  public function get($filename, $view_vars = null) {
    return $this->_view(
      FRAMEWORK_BASEDIR . 'app' . DS . 'views' . DS . "{$filename}.php", 
      $view_vars
    );
  }
  
  public function display($filename, $view_vars = null) {
    echo $this->get($filename, $view_vars);
  }

  private function _view($filepath, $view_vars) {
    if(!file_exists($filepath)) {
      throw new Exception("Unknown file '{$filepath}'");
    }

    extract($this->view_vars);
    if (isset($view_vars)) {
      extract($view_vars);
    }

    include($filepath);
  }
}
