<?php

final class AchievementcardsController extends BaseController {
  public function index() {
    $this->load->library('uri');
    
    $achievement_id = $_GET['achievement_id'];
    if (!$achievement_id) {
      throw new Exception('No achievement ID provided');
    } else {
      $db = $this->load->model('AchievementsModel');
      $achievement = $db->getAchievement($achievement_id);

      $this->view->assign('description', $achievement[0]['description']);
      $this->view->assign('title', $achievement[0]['title']);
      $this->view->assign('image', $achievement[0]['image']);
      $this->view->assign('value', $achievement[0]['value']);
      $this->view->assign('achievement_id', $achievement[0]['achievement_id']);
      return $this->view->get('achievement');
    }
  }
}
