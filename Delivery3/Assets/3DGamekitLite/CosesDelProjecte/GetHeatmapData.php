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

// Consulta per obtenir dades agrupades per posició
$sql = "SELECT x, y, z, COUNT(*) AS frequency FROM player_positions GROUP BY x, y, z";
$result = $conn->query($sql);

$data = [];
if ($result->num_rows > 0) {
    // Processar resultats
    while ($row = $result->fetch_assoc()) {
        $data[] = [
            'x' => (float)$row['x'],
            'y' => (float)$row['y'],
            'z' => (float)$row['z'],
            'frequency' => (int)$row['frequency']
        ];
    }
}

// Enviar les dades com a JSON
header('Content-Type: application/json');
echo json_encode($data);

// Tancar la connexió
$conn->close();
?>
