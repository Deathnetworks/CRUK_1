<?php
class URI {
  private $path = null;

  function __construct() {
    if (!empty($_SERVER['PATH_INFO'])) {
      $this->path = explode('/', $_SERVER['PATH_INFO']);
      $this->path = array_slice($this->path, 2);
    }
  }

  function edge($index) {
    if (!empty($this->path[$index-1])) {
      return $this->path[$index-1];
    } else {
      return false;
    }
  }

  function uriToAssoc($index) {
    $assoc = array();

    $count = count($this->path);
    $i = $index-1;
    for ($count, $i; $i < $count; $i += 2) {
      $assoc_idx = $this->path[$y];
      $assoc[$assoc_idx] = 
        isset($this->path[$i+1]) ? $this->path[i+1] : null;
    }

    return $assoc;
  }

  function uriToArray($index = 0) {
    if (is_array($this->path)) {
      return array_slice($this->path, $index);
    } else {
      return false;
    }
  }
} 
