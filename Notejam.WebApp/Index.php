<!DOCTYPE html>
<html>
<head>
<style>
#customers {
  font-family: Arial, Helvetica, sans-serif;
  border-collapse: collapse;
  width: 100%;
}

#customers td, #customers th {
  border: 1px solid #ddd;
  padding: 8px;
}

#customers tr:nth-child(even){background-color: #f2f2f2;}

#customers tr:hover {background-color: #ddd;}

#customers th {
  padding-top: 12px;
  padding-bottom: 12px;
  text-align: left;
  background-color: #04AA6D;
  color: white;
}
</style>
</head>
<body>
<h1>
Notejam Web Application
</h1>
<h2>
All Notes (2)
</h2>
<?php
$json_string = file_get_contents("http://localhost:7071/api/notesmanagement/getnotes");
$array = json_decode($json_string, true);
// echo '<pre>'; print_r($array); echo '</pre>';
?>

<table id="customers">
    <thead>
    <tr>
    <th>Sq. Number</th>
    <th>Title</th>
    <th>Last Modified</th>
    <th>Action</th>
    </tr>
    
    </thead>
    <tbody>
    <?php 
    $numberOfNotes = 1;
    foreach($array as $key => $value): ?>
        <tr>
            <td><?php echo $numberOfNotes; ?></td>
            <td><?php echo $value['noteName']; ?></td>
            <td><?php echo $value['lastModifiedDate']; ?></td>
            <td> <a href="/Index.php">  <?php echo "View Details"; ?> </a>  </td>
        </tr>
    <?php
$numberOfNotes ++;
endforeach; ?>
    </tbody>
</table>



</body>
</html>
