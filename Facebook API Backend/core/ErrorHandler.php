<?php

final class ErrorHandler {
  public static function get($errno, $errstr, $errfile, $errline) {
    if (error_reporting() === 0) {
      return;
    }

    if (error_reporting() & $errno) {   
      throw new ExceptionHandler($errstr, $errno, $errno, $errfile, $errline);
    }
  }
}
