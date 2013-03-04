<?php
// compile PHP with --enable-pdo (default with PHP 5.1+)

if(!defined('FRAMEWORK_SQL_NONE')) {
  define('FRAMEWORK_SQL_NONE', 0);
}

if(!defined('FRAMEWORK_SQL_INIT')) {
  define('FRAMEWORK_SQL_INIT', 1);
}

if(!defined('FRAMEWORK_SQL_ALL')) {
  define('FRAMEWORK_SQL_ALL', 2);
}
 
final class PDODatabase {
  public 
    $pdo = null,
    $result = null,
    $fetch_mode = PDO::FETCH_ASSOC,
    $query_params = array('select' => '*'),
    $last_query = null,
    $last_query_type = null;
  
  public function __construct($config) {
    if (!class_exists('PDO',false)) {
      throw new Exception("PHP PDO package is required.");
    }

    if (empty($config)) {
        throw new Exception("Database definitions required.");
    }

    if(empty($config['charset'])) {
        $config['charset'] = 'UTF8';
    }

    try {    
      $this->pdo = new PDO(
        "{$config['type']}:host={$config['host']};".
        "dbname={$config['name']};".
        "charset={$config['charset']}",
        $config['user'],
        $config['pass'],
        array(
          PDO::ATTR_PERSISTENT => 
          !empty($config['persistent']) ?: false
        )
      );
      $this->pdo->exec("SET CHARACTER SET {$config['charset']}"); 
    } catch (PDOException $e) {
      throw new Exception(sprintf(
        "Can't connect to PDO database '{$config['type']}'".
        ". Error: %s",$e->getMessage()
      ));
    }

    $this->pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION); 
  }

  public function select($clause) {
    $this->query_params['select'] = $clause;
    return $this;
  }  

  public function from($clause) {
    $this->query_params['from'] = $clause;
    return $this;
  }  


  public function where($clause, $args) {
    if(empty($clause)) {
        throw new Exception(sprintf("where cannot be empty"));
    }

    if(!preg_match('![=<>]!', $clause)) {
      $clause .= '=';  
    }

    if(strpos($clause,'?') === false) {
        $clause .= '?';
    }
    
    $this->_where($clause, (array)$args, 'AND');    
    return $this;
  }  

  public function orWhere($clause, $args) {
    $this->_where($clause, $args, 'OR');    
    return $this;
  }  
  
  private function _where($clause, $args=array(), $prefix='AND') {    
    if(empty($clause)) {
      return false;
    }
        
    if(($count = substr_count($clause,'?')) && (count($args) != $count)) {
      throw new Exception(sprintf(
        "Number of where clause args don't match number of ?: '%s'", 
        $clause
      ));
    }  
    
    if(!isset($this->query_params['where'])) {
      $this->query_params['where'] = array();
    }
      
    $this->query_params['where'][] = array(
      'clause' => $clause,
      'args'=>$args,
      'prefix'=>$prefix
    );
    
    return $this->query_params['where'];    
  }  

  public function join($join_table, $join_on, $join_type = null) {
    $clause = "JOIN {$join_table} ON {$join_on}";
    
    if(!empty($join_type)) {
      $clause = $join_type . ' ' . $clause;
    }
    
    if(!isset($this->query_params['join'])) {
      $this->query_params['join'] = array();
    }
    
    $this->query_params['join'][] = $clause;
    return $this;
  }  

  public function in($field, $elements, $list = false) {
    $this->_in($field, $elements, $list, 'AND');
    return $this;
  }

  public function orIn($field, $elements, $list = false) {
    $this->_in($field, $elements, $list,'OR');
    return $this;
  }

  private function _in($field, $elements, $list = false, $prefix = 'AND') { 
    if(!$list) {
      if(!is_array($elements)) {
        $elements = explode(',',$elements);
      }

      foreach ($elements as $idx => $element) {
        $elements[$idx] = $this->pdo->quote($element);
      }

      $clause = sprintf("{$field} IN (%s)", implode(',', $elements));
    } else {
      $clause = sprintf("{$field} IN (%s)", $elements);
    }

    $this->_where($clause, array(), $prefix);
  }  
  
  public function orderBy($clause) {    
    $this->_setClause('orderby', $clause);
    return $this;
  }  

  public function groupBy($clause) {    
    $this->_setClause('groupby', $clause);
    return $this;
  }  
  
  public function limit($limit, $offset=0) {    
    if(!empty($offset)) {
      $this->_setClause('limit', sprintf('%d,%d', (int)$offset, (int)$limit));
    } else {
        $this->_setClause('limit', sprintf('%d', (int)$limit));
    }

    return $this;
  }  
  
  private function _setClause($type, $clause, $args=array()) {    
    if(empty($type) || empty($clause)) {
        return false;
    }
  
    $this->query_params[$type] = array('clause' => $clause);

    if(isset($args)) {
        $this->query_params[$type]['args'] = $args; 
    }
  }  
  
  private function _queryAssemble(&$params, $fetch_mode=null) {
    if(empty($this->query_params['from'])) {
        throw new Exception("Unable to get(), set from() first");
        return false;
    }

    $query = array();
    $query[] = "SELECT {$this->query_params['select']}";
    $query[] = "FROM {$this->query_params['from']}";

    if(!empty($this->query_params['join'])) {
        foreach($this->query_params['join'] as $cjoin) {
          $query[] = $cjoin;
        }
    }

    if($where = $this->_assembleWhere($where_string, $params)) {
      $query[] = $where_string;
    }

    if(!empty($this->query_params['groupby'])) {
      $query[] = "GROUP BY {$this->query_params['groupby']['clause']}";
    }

    if(!empty($this->query_params['orderby'])) {
      $query[] = "ORDER BY {$this->query_params['orderby']['clause']}";
    }

    if(!empty($this->query_params['limit'])) {
        $query[] = "LIMIT {$this->query_params['limit']['clause']}";
    }

    $query_string = implode(' ',$query);
    $this->last_query = $query_string;

    $this->query_params = array('select' => '*');

    return $query_string;
  }  
  
  private function _assembleWhere(&$where, &$params) {
    if(!empty($this->queryParams['where'])) {
      $where_init = false;
      $where_parts = array();
      $params = array();
      foreach($this->queryParams['where'] as $cwhere) {
        $prefix = !$where_init ? 'WHERE' : $cwhere['prefix'];
        $where_parts[] = "{$prefix} {$cwhere['clause']}";
        $params = array_merge($params,(array) $cwhere['args']);
        $where_init = true;
      }
      
      $where = implode(' ',$where_parts);      
      return true;
    }
    
    return false;
  }  
  
  public function query($query = null, $params = null, $fetch_mode = null) {
    if(!isset($query)) {
      $query = $this->_queryAssemble($params, $fetch_mode);
    }

    $query = $this->_query($query, $params, FRAMEWORK_SQL_NONE, $fetch_mode);

    if ($query) {
      return $this;
    } else {
      throw new PDODatabaseException('Couldn\'t execute query');
    }
  }  

  public function queryAll($query = null, $params = null, $fetch_mode = null) {
    if(!isset($query)) {
      $query = $this->_queryAssemble($params,$fetch_mode);
    }

    return $this->_query($query, $params, FRAMEWORK_SQL_ALL, $fetch_mode);
  }  

  public function queryOne($query = null, $params = null, $fetch_mode = null) {
    if(!isset($query)) {
      $this->limit(1);
      $query = $this->_queryAssemble($params, $fetch_mode);
    }

    return $this->_query($query, $params, FRAMEWORK_SQL_INIT, $fetch_mode);
  }  
  
  public function _query($query, $params = null, 
    $return_type = FRAMEWORK_SQL_NONE, $fetch_mode = null) {
    
    if(!isset($fetch_mode)) {
      $fetch_mode = PDO::FETCH_ASSOC;  
    }
    
    try {
      $this->result = $this->pdo->prepare($query);
    } catch (PDOException $e) {
      throw new Exception(sprintf(
        "PDO Error: %s Query: %s", $e->getMessage(), $query
      ));
      
      return false;
    }      

    try {
      $this->result->execute($params);  
    } catch (PDOException $e) {
      throw new Exception(sprintf(
        "PDO Error: %s Query: %s", $e->getMessage(), $query
      ));
      
      return false;
    }      

    $this->result->setFetchMode($fetch_mode);  

    switch($return_type) {
      case FRAMEWORK_SQL_INIT:
        return $this->result->fetch();
        break;
      case FRAMEWORK_SQL_ALL:
        return $this->result->fetchAll();
        break;
      case FRAMEWORK_SQL_NONE:
      default:
        return true;
        break;
    }
  }

  public function update($table, $columns) {
    if(empty($table)) {
      throw new Exception("Unable to update, table name required");
      return false;
    }

    if(empty($columns) || !is_array($columns)) {
      throw new Exception("Unable to update, at least one column required");
      return false;
    }

    $query = array("UPDATE {$table} SET");
    $fields = array();
    $params = array();
    
    foreach($columns as $cname => $cvalue) {
      if(!empty($cname)) {
        $fields[] = "{$cname}=?";
        $params[] = $cvalue;
      }
    }
    $query[] = implode(',',$fields);

    // assemble where clause
    if($this->_assembleWhere($where_string, $where_params)) {    
      $query[] = $where_string;
      $params = array_merge($params, $where_params);
    }

    $query = implode(' ',$query);

    $this->queryParams = array('select' => '*');

    return $this->_query($query, $params);
  }
  
  public function insert($table, $columns) {
    if(empty($table)) {
      throw new Exception("Unable to insert, table name required");
      return false;
    }

    if(empty($columns) || !is_array($columns)) {
      throw new Exception("Unable to insert, at least one column required");
      return false;
    }

    $column_names = array_keys($columns);

    $query = array(sprintf(
      "INSERT INTO `{$table}` (`%s`) VALUES", implode('`,`', $column_names)
    ));

    $fields = array();
    $params = array();
    foreach($columns as $cname => $cvalue) {
      if(!empty($cname)) {
        $fields[] = "?";
        $params[] = $cvalue;
      }
    }
    $query[] = '(' . implode(',',$fields) . ')';

    $query = implode(' ',$query);

    $this->_query($query,$params);
    return $this->lastInsertId();
  }

  public function delete($table) {
    if(empty($table)) {
      throw new Exception("Unable to delete, table name required");
      return false;
    }
    $query = array("DELETE FROM `{$table}`");
    $params = array();

    if($this->_assembleWhere($where_string, $where_params)) {    
      $query[] = $where_string;
      $params = array_merge($params, $where_params);
    }

    $query = implode(' ',$query);

    $this->queryParams = array('select' => '*');

    return $this->_query($query,$params);
  }
  
  public function next($fetch_mode = null) {
    if(isset($fetch_mode)) {
      $this->result->setFetchMode($fetch_mode);
    }
    
    return $this->result->fetch();
  }

  public function lastInsertId() {
    return $this->pdo->lastInsertId();
  }

  public function numRows() {
    return $this->result->rowCount();
  }

  public function affectedRows() {
    return $this->result->rowCount();
  }

  public function lastQuery() {
    return $this->last_query;
  }  

  public function quote($str) {
    return $this->pdo->quote($str);
  }

  public function get() {
    return $this->result->fetchAll();
  }

  public function __destruct() {
    $this->pdo = null;
  }
}

final class PDODatabaseException extends Exception {}
