<?php

final class AchievementsController extends BaseController {
  private 
    $facebook = null,
    $model = null;
  
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

    // OMFG no time, this is ugly.
    $this->model = $this->load->model('AchievementsModel'); 
    $user_model = $this->load->model('UserModel');

    if (!$_POST) {
      return json_encode($this->model->getAchievements());
    } else {
      $achievement_id = idx($_POST, 'achievement_id');
      $user_id = idx($_POST, 'user_id');

      if (!$achievement_id || !$user_id) {
        return json_encode(array('You need to POST an achievement_id and an user_id'));
      } else {
        $achievement = $this->model->getAchievement($achievement_id);
        
        if (!$achievement) {
          throw new Exception('Wrong achievement ID');
        }
        
        $achievement_result = json_encode(array());        
        if ($user_model->setAchievement($user_id, $achievement_id)) {
          $achievement_url = 
            'https://graph.facebook.com/' . $user_id . '/achievements';

          $achievement_result = $this->httpsPost(
            $achievement_url,
            'achievement=' . urlencode('http://crukhack.eu01.aws.af.cm/achievementcards/?' .
            'achievement_id=' . $achievement_id) .
            '&' . $this->getAppAccessToken()
          ); 
        }

        return $achievement_result;  
      }
    }
  }
  
  public function earned() {
    header('Content-type: application/json');
    $user_id = idx($_GET, 'user_id');

    $model = $this->load->model('UserModel');

    if ($user_id) {
      return json_encode($model->getAchievement($user_id));
    }

    return json_encode(array('User ID missing.'));
  }

  public function register() {
    header('Content-type: application/json');
    $this->model = $this->load->model('AchievementsModel'); 

    if ($_GET) {
      $achievement_id = idx($_GET, 'achievement_id');

      if (!$achievement_id) {
        return json_encode(array('You need to POST the achievement_id'));
      }

      $achievement = $this->model->getAchievement($achievement_id);
      
      if (!$achievement) {
        return json_encode(array('Wrong achievement ID'));
      }
      
      $achievement_result = $this->facebook()->api(
        FacebookConstants::APP_ID . '/achievements',
        'POST',
        array( 
          'achievement' => 
            urlencode(
              'http://crukhack.eu01.aws.af.cm/achievementcards/?achievement_id=' . 
              $achievement_id
            ), 
          'display_order' => $achievement_id
        )
      );

      return $achievement_result;
    }

    return json_encode(array('POST the achievement id to register'));
  }

  private function getAppAccessToken() {
    $token_url =    
      "https://graph.facebook.com/oauth/access_token?" .
      "client_id=" . FacebookConstants::APP_ID .
      "&client_secret=" . FacebookConstants::APP_SECRET .
      "&grant_type=client_credentials";

    return file_get_contents($token_url);
  }

  private function httpsPost($uri, $postdata) {
    $ch = curl_init($uri);
    curl_setopt($ch, CURLOPT_POST, true);
    curl_setopt($ch, CURLOPT_POSTFIELDS, $postdata);
    $result = curl_exec($ch);
    curl_close($ch);

    return $result;
  }
}
