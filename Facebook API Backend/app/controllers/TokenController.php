<?php

final class TokenController extends BaseController {
  public function index() {
    $facebook = new Facebook(array(
      'appId' => FacebookConstants::APP_ID,
      'secret' => FacebookConstants::APP_SECRET,
    ));
    
    return $facebook->getAccessToken();
  }
}
