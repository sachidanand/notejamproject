<!DOCTYPE html>

<?php header('Access-Control-Allow-Origin: *'); ?>
<html lang="en">
  <head>
    <title>Ajax Api call by jQuery</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
 
  </head>
  <body>
 <?php
$response = file_get_contents('http://localhost:7071/api/notesmanagement/getnotes');
echo $response ;
 ?>
 <?php
$json_string = file_get_contents("http://localhost:7071/api/notesmanagement/getnotes");
$array = json_decode($json_string, true);
echo '<pre>'; print_r($array); echo '</pre>';
?>

<table border="1" cellpadding="10">
    <thead>
    <tr>
    <th>Pad</th>
    <th>Note Title</th>
    </tr>
    
    </thead>
    <tbody>
    <?php foreach($array as $key => $value): ?>
        <tr>
            <td><?php echo $value['noteName']; ?></td>
            <td><?php echo $value['noteText']; ?></td>
        </tr>
    <?php endforeach; ?>
    </tbody>
</table>
  
  </body>
</html>
