<?php

final class UserModel extends Model {
  function getUsers($limit = 100, $index = 0) {
    try {
      return $this->db
        ->select('*')
        ->from('users')
        ->limit($limit, $index)
        ->query()
        ->get();
    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }
  }
  
  function getUser($fbid) {
    try {
      return $this->db->queryAll(
        'SELECT * FROM users WHERE user_fb_id = '. $this->db->quote($fbid)
      );

    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }
  }
  
  function getFriendsOf($fbid) {
    try {
      $friends = $this->db->queryAll(
        'SELECT friend_fb_id FROM friends WHERE '.
        'user_fb_id = ' . $this->db->quote($fbid)
      );

      if (!$friends) {
        return array();
      }

      // <3 PHP
      $pdo = $this->db;
      $friend_ids = array_map(
        function($row) use ($pdo) {
          return $pdo->quote($row['friend_fb_id']);
        },
        $friends
      );

      return $this->db->queryAll(
        'SELECT * FROM users WHERE user_fb_id IN ' .
        '(' . implode(',', $friend_ids) . ')'
      );
    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }
  }
  
  function updateUser($id, $first_name, $last_name, $token) {
    try {
      $user = $this->getUser($id);
      if (!$user) {
        $query =
          'INSERT INTO users (user_fb_id, first_name, last_name, fb_access_token) '.
          'VALUES ('. (int) $id .', '. $this->db->quote($first_name) .
          ', '. $this->db->quote($last_name) .', '. $this->db->quote($token) .')';

        $this->db->queryAll($query);
      } 
      
      /* else if ($user[0]['fb_access_token'] !== $token) {
        $query =
          'UPDATE users SET fb_access_token = '. $this->db->quote($token) .
          ' WHERE user_fb_id = '. (int) $id;

        $this->db->queryAll($query);
      }*/
    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }  
  }
  
  function updateUserFriends($id, $friends) {
    try {
      $query_pairs = array_map(
        function($friend) use ($id) {
          return '('. (int) $id .', '. (int) $friend['id'] .')';
        },
        $friends['data']
      );

      $query_pairs = implode(', ', $query_pairs);
      $query = 
        'INSERT INTO friends (user_fb_id, friend_fb_id) VALUES '. $query_pairs;

      $this->db->queryAll($query);
    } catch (PDODatabaseException $e) {
      return $this->getError($e);
    }
  }
  
  function getPlayingFriends($ids) {
    $query = 
      'SELECT user_fb_id, first_name, last_name FROM users '.
      'WHERE user_fb_id IN ('.$ids.')';
    return $this->db->queryAll($query);
  }

  function getAchievement($user_id, $achievement_id = null) {
    $query = 
      'SELECT * FROM achievements_earned WHERE '.
      'user_fb_id = '. (int) $user_id;

    if ($achievement_id) {
      $query .= ' AND achievement_id = '. (int) $achievement_id;
    }

    return $this->db->queryAll($query);
  }

  function setAchievement($user_id, $achievement_id) {
    try {
      if (!$this->getAchievement($user_id, $achievement_id)) {
        $query = 
          'INSERT INTO achievements_earned (achievement_id, user_fb_id) '.
          'VALUES ('.(int) $user_id .', '.(int) $achievement_id.')';

        $this->db->queryAll($query);
        return true;
      }

      return false;
    } catch (Exception $e) {
      return false;
    }
  }

  private function getError(Exception $e) {
    return array(
      'error' => $e->getMessage()
    );
  }
}
