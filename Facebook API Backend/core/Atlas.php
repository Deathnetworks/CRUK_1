<?php
// In Greek mythology, Atlas (/ˈætləs/; Ancient Greek: Ἄτλας) was the 
// primordial Titan who held up the celestial sphere.

spl_autoload_register(function($class) {
  $paths = array(
    FRAMEWORK_BASEDIR . 'core' . DS . $class . '.php',
    FRAMEWORK_BASEDIR . 'app' . DS . 'models' . DS . $class . '.php',
    FRAMEWORK_BASEDIR . 'app' . DS . 'controllers' . DS . $class . '.php',
    FRAMEWORK_BASEDIR . 'config' . DS . $class . '.php',
    FRAMEWORK_BASEDIR . 'lib' . DS . 'vendor' . DS . 'facebook' . DS . strtolower($class) . '.php',
  );

  foreach ($paths as $path) {
    if (file_exists($path)) {
      require_once($path);
    }
  }
});

final class Atlas {
  private 
    $config,
    $action,
    $pathInfo,
    $URLSegments;

  public 
    $controller; 

  public function __construct($id = 'default') {
    self::instance($this, $id);
  }

  public function getResponse() {
    $start_time = microtime();

    $this->pathInfo = idx($_SERVER, 'PATH_INFO');
    
    include(FRAMEWORK_BASEDIR . 'config' . DS . 'base.php');
    $this->config = $config;

    $this->setupErrorHandling();
    $this->setupSegments();
    $this->setupController();
    $this->setupAction();
    $this->setupAutoloaders();
    
    echo $this->controller->{$this->action}();
  }

  protected function setupErrorHandling() {
    if(defined('FRAMEWORK_ERROR_HANDLING') && FRAMEWORK_ERROR_HANDLING) {
      set_exception_handler(array('ExceptionHandler', 'handleException'));
      require_once(FRAMEWORK_BASEDIR . 'core' . DS . 'ExceptionHandler.php');
      set_error_handler('ErrorHandler::get');
    }
  }

  protected function setupRouting() {
    if($this->config['routing']['search'] &&
       $this->config['routing']['replace']) {
      $this->pathInfo = str_replace(
        $this->config['routing']['search'],
        $this->config['routing']['replace'],
        $this->pathInfo
      );
    }
  }
  
  protected function setupSegments() {
    $this->URLSegments = 
      $this->pathInfo ? array_filter(explode('/', $this->pathInfo)) : null;
  }

  protected function setupController() {
    $controller_name = 
      $this->URLSegments[1] ? 
      ucfirst(preg_replace('!\W!', '', $this->URLSegments[1])) . 'Controller' :
      ucfirst($this->config['default_controller']) . 'Controller';

    $this->controller = new $controller_name();
  }

  protected function setupAction() {
    if($this->config['root_action']) {
      $this->action = $this->config['root_action'];
    } else {
      $this->action = 
        idx($this->URLSegments, 2) ? 
         $this->URLSegments[2] : 
         $this->config['default_action'];
    }
    
    if(substr($this->action, 0, 1) === '_') {
      throw new Exception(
        'Action name cannot start with an underscore '.
        $this->action
      );
    }
  }

  protected function setupAutoloaders() {
    include(FRAMEWORK_BASEDIR . 'config' . DS . 'autoload.php');
    if($config['libraries']) {
      foreach($config['libraries'] as $library) {
        if(is_array($library)) {
          $this->controller->load->library($library[0], $library[1]); 
        } else {
          $this->controller->load->library($library);
        }
      }
    }
  }

  public static function &instance($new_instance = null, 
    $id = 'default') {
    
    static $instance = array();
    if(isset($new_instance) && is_object($new_instance)) {
      $instance[$id] = $new_instance;
    }

    return $instance[$id];
  }
}
