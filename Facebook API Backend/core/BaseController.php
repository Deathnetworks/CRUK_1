<?php

abstract class BaseController {
  protected 
    $view;

  public
    $load;

  public function __construct(){
    Atlas::instance($this, 'controller');
    $this->load = new Load();
    $this->view = new View();
  }

  abstract function index();

  public function __call($function, $args) {
    throw new Exception(
      'Unknown controller method ' . $function
    );
  }
}
