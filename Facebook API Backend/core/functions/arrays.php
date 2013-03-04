<?php

function idx($array, $key, $default = null) {
  if ($array) {
    return array_key_exists($key, $array) ? $array[$key] : $default; 
  } 

  return $default;
}
