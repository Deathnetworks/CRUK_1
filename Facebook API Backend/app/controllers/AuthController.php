<?php

final class AuthController extends BaseController {
  public function index() {
    $facebook = new Facebook(array(
      'appId'  => FacebookConstants::APP_ID,
      'secret' => FacebookConstants::APP_SECRET,
    ));
    
    $id = $facebook->getUser();

    if ($id) {
      $user = $facebook->api('/me');
      $friends = $facebook->api('/me/friends');
      
      $model = $this->load->model('UserModel');
      $model->updateUser(
        $user['id'], 
        $user['first_name'], 
        $user['last_name'], 
        $facebook->getAccessToken()
      );
      
      // BUGS, BUGS EVERYWHERE.
      // $model->updateUserFriends($user['id'], $friends);
      
      $crukweb_messy_generated_crap = file_get_contents(
        FRAMEWORK_BASEDIR . '/webroot/crukweb.html'
      );        
      
      // return '<a href="' . $facebook->getLogoutUrl() . '">Logout</a>';
      return $crukweb_messy_generated_crap;
    } else {
      return '<a target="_top" href="' . $facebook->getLoginUrl(
        array(
          'scope' => 'publish_actions'
        )
      ) . '">Login</a>';
    }
  }
} 
