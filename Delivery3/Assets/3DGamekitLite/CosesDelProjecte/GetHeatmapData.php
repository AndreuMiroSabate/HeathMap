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
$sql = "SELECT round(Z+(select ABS(min(round(Z))+1) from player_positions)) *1000 + round(X+(select ABS(min(round(X))+1) from player_positions)) as HeathID,
round(X) AS X, round(Z) AS Z, count(*) as Heath , action
FROM player_positions
group by 1";
$result = $conn->query($sql);

$data = [];
if ($result->num_rows > 0) {
    // Processar resultats
    while ($row = $result->fetch_assoc()) {
        $data[] = [
            'x' => (float)$row['X'],
            'z' => (float)$row['Z'],
            'Heath' => (int)$row['Heath'],
            'action' => (string)$row['action']
        ];
    }
}

// Enviar les dades com a JSON
header('Content-Type: application/json');
echo json_encode($data);

// Tancar la connexió
$conn->close();
?>
