<?php

abstract class Model {
  protected 
    $db;

  function __construct($poolname = null) {
    $this->db = Atlas::instance()->controller->load->database($poolname);
  }
}
