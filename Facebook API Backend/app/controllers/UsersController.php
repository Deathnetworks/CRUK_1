<?php

final class UsersController extends BaseController {
  private 
    $facebook = null;
  
  public function __construct() {
    session_start();
    parent::__construct();
  }

  private function facebook() {
    return 
      $this->facebook ?: 
      $this->facebook = new Facebook(array(
        'appId' => FacebookConstants::APP_ID,
        'secret' => FacebookConstants::APP_SECRET,
      ));
  }

  public function index() {
    header('Content-type: application/json');
    $model = $this->load->model('UserModel');

    $id = idx($_GET, 'id');

    if ($id) {
      return json_encode($model->getUser($id));
    }

    $limit = idx($_GET, 'limit');
    $index = idx($_GET, 'index');

    if ($limit && $index) {
      return json_encode($model->getUsers($limit, $index));
    } else if ($limit) {
      return json_encode($model->getUsers($limit));
    }

    return json_encode($model->getUsers());
  }
  
  public function current() {
    header('Content-type: application/json');
    $access_token = idx($_GET, 'access_token');

    if ($access_token) {
      $this->facebook()->setAccessToken($access_token);
      return json_encode($this->facebook()->api('/me')); 
    }

    return json_encode('Access token missing');
  }
  
  public function picture() {
    $id = idx($_GET, 'id');

    if ($id) {
      header('Content-type: image/jpg');
      return file_get_contents(
        'http://graph.facebook.com/'.$id.'/picture?width=200&height=200'
      );
    }

    // TODO: Add standard image
  }
  
  /*
  public function friends() {
    header('Content-type: application/json');
    $id = idx($_GET, 'id');

    if ($id) {
      $model = $this->load->model('UserModel');
      return json_encode($model->getFriendsOf($id));
    } 

    return json_encode(array());
  }
   */

  public function friends() {
    header('Content-type: application/json');
    $access_token = idx($_GET, 'access_token');
    $playing = idx($_GET, 'playing');

    if ($access_token && !$playing) {
      $this->facebook()->setAccessToken($access_token);
      return json_encode($this->facebook()->api('/me/friends')); 
    } else if ($access_token && $playing) {
      $this->facebook()->setAccessToken($access_token);
      
      $all_friends = $this->facebook()->api('/me/friends');
      $all_friend_ids = array_map(
        function($friend) {
          return $friend['id'];
        },
        $all_friends['data']
      );

      $model = $this->load->model('UserModel');
      return json_encode(
        $model->getPlayingFriends(implode(',', $all_friend_ids))
      );
    }

    return json_encode('Access token missing');
  }
  
  public function score() {

  }
}
