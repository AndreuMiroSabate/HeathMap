<?php
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

// Executar la consulta per obtenir les dades agrupades
$sql = "SELECT 
            ROUND(Z + (SELECT ABS(MIN(ROUND(Z)) + 1) FROM player_positions)) * 1000 + 
            ROUND(X + (SELECT ABS(MIN(ROUND(X)) + 1) FROM player_positions)) AS HeathID,
            ROUND(X) AS X, 
            ROUND(Z) AS Z, 
            COUNT(*) AS Heath 
        FROM andreums.player_positions
        GROUP BY 1";
$result = $conn->query($sql);

$data = [];
if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $data[] = [
            'x' => (float)$row['X'],
            'z' => (float)$row['Z'],
            'frequency' => (int)$row['Heath']
        ];
    }
}

// Enviar les dades com a JSON
header('Content-Type: application/json');
echo json_encode($data);

// Tancar la connexió
$conn->close();
?>
