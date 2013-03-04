<?php

/**
 * $this->load->library('uri');
 * // gets third segment from URI
 * $this->uri->segment(3);
 * // get key/val associative array starting with the third segment
 * $uri = $this->uri->uri_to_assoc(3);
 * // assign params to an indexed array, starting with third segment
 * $uri = $this->uri->uri_to_array(3);
**/ 

class BaseURI {
  private $path = null;

  public function __construct() {
    if(!empty($_SERVER['PATH_INFO'])) {
      $this->path = explode('/', $_SERVER['PATH_INFO']);
      $this->path = array_slice($this->path, 2);
    }
  }

  function segment($index) {
    if(!empty($this->path[$index-1])) {
        return $this->path[$index-1];
    } else { 
      return false;
    }
  }

  function uriToAssoc($index) {
    $assoc = array();
    for($x = count($this->path), $y = $index-1; $y < $x; $y += 2) {
      $assoc_idx = $this->path[$y];
      $assoc[$assoc_idx] = isset($this->path[$y+1]) ?: null;
    }
    return $assoc;
  }

  function uriToArray($index = 0) {
    if(is_array($this->path)) {
        return array_slice($this->path, $index);
    } else {
        return false;
    }
  }
}
