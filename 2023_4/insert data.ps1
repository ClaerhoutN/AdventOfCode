$filePath = "C:\\Users\\nclaerhout\\OneDrive - Deloitte (O365D)\\Documents\\AOC\\2023_4\\input.txt";
$connectionString = "Data Source=127.0.0.1;Initial Catalog=AOC_2023_4;User id=sa;Password=Google@123;";

$connection = new-object System.Data.SqlClient.SQLConnection($connectionString);

$playerNumberQuery = $("INSERT INTO PlayerNumbers (CardId, Number) VALUES (@cardId, @number);");
$cmdPlayerNumbers = new-object System.Data.SqlClient.SqlCommand($playerNumberQuery, $connection);
$cmdPlayerNumbers.CommandTimeout = 0;

$winningNumberQuery = $("INSERT INTO WinningNumbers (CardId, Number) VALUES (@cardId, @number);");
$cmdWinningNumbers = new-object System.Data.SqlClient.SqlCommand($winningNumberQuery, $connection);
$cmdWinningNumbers.CommandTimeout = 0;

$connection.Open();
$input = [System.IO.File]::ReadAllLines($filePath);
$cardId = 1;
foreach ($inputLine in $input)
{
    $winningNumbers = $inputLine.substring(10, 30);
    $playerNumbers = $inputLine.substring(41, 75);

    foreach ($number in ($winningNumbers -split "[\s]+"))
    {
        if($number -eq "") { continue; }
        $cmdWinningNumbers.Parameters.Clear();
        $cmdWinningNumbers.Parameters.AddWithValue("@cardId", $cardId);
        $cmdWinningNumbers.Parameters.AddWithValue("@number", $number);
        $cmdWinningNumbers.ExecuteNonQuery();
    }
    foreach ($number in ($playerNumbers -split "[\s]+"))
    {
        if($number -eq "") { continue; }
        $cmdPlayerNumbers.Parameters.Clear();
        $cmdPlayerNumbers.Parameters.AddWithValue("@cardId", $cardId);
        $cmdPlayerNumbers.Parameters.AddWithValue("@number", $number);
        $cmdPlayerNumbers.ExecuteNonQuery();
    }
    ++$cardId;
}
$connection.Close();