<?php
// Configuració de la base de dades
$servername = "localhost";
$username = "andreums";
$password = "53872300k";
$dbname = "andreums";

// Crear connexió
$conn = new mysqli($servername, $username, $password, $dbname);

// Comprovar connexió
if ($conn->connect_error) {
    die("Connexió fallida: " . $conn->connect_error);
}

// Verificar si hi ha dades enviades via POST
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Recollir el tipus de dades enviades
    $data_type = $_POST['data_type'];

    // Gestionar segons el tipus de dades
    switch ($data_type) {
        case "player_position":
            handlePlayerPosition($conn, $_POST);
            break;
        default:
            echo "Tipus de dades no reconegut: $data_type";
            break;
    }
}

// Funció per gestionar posicions del jugador
function handlePlayerPosition($conn, $data)
{
    $action = $data['value0'];
    $x = $data['value1'];
    $y = $data['value2'];
    $z = $data['value3'];
    $timestamp = date("Y-m-d H:i:s");

    $sql = "INSERT INTO player_positions (action, x, y, z, timestamp)
            VALUES ('$action', $x, $y, $z, '$timestamp')";

    if ($conn->query($sql) === TRUE) {
        echo "Posició registrada correctament.";
    } else {
        echo "Error: " . $sql . "<br>" . $conn->error;
    }
}

// Tancar la connexió a la base de dades
$conn->close();
?>
