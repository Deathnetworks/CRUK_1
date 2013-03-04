<?php

final class AchievementsModel extends Model {
  function getAchievements() {
    try {
      return $this->db
        ->select('*')
        ->from('achievements')
        ->query()
        ->get();
    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }
  }

  function getAchievement($id) {
    try {
      return $this->db
        ->select('*')
        ->from('achievements')
        ->where('achievement_id = ?', array($id))
        ->query()
        ->get();
    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }
  }

  private function getError(Exception $e) {
    return array(
      'error' => $e->getMessage()
    );
  }
}
