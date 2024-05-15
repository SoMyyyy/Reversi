// using System.Text.Json;
// using System.Text.Json.Serialization;
//
// namespace ReversieISpelImplementatie.Model;
//
// /*
//  * The BordJSONConverter class you have is a custom JSON converter for the Kleur[,] type. It's used to control how a Kleur[,] is serialized to JSON and deserialized from JSON.
//  * This is useful when you're sending or receiving JSON in an API, for example.
//  */
//
// public class BordJSONConverter : JsonConverter<Kleur[,]>
// {
//     // Override the Read method to handle deserialization of Kleur[,]
//     public override Kleur[,] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         // Check if the JSON data starts with a start array token
//         if (reader.TokenType != JsonTokenType.StartArray)
//         {
//             throw new JsonException();
//         }
//
//         // Create a temporary list of lists to hold the data
//         List<List<Kleur>> tempBoard = new List<List<Kleur>>();
//
//         // Read through the JSON data
//         while (reader.Read())
//         {
//             // Break the loop if the end array token is encountered
//             if (reader.TokenType == JsonTokenType.EndArray)
//             {
//                 break;
//             }
//
//             // If a start array token is encountered, create a new list
//             if (reader.TokenType == JsonTokenType.StartArray)
//             {
//                 List<Kleur> row = new List<Kleur>();
//
//                 // Read through the inner array and add each color to the list
//                 while (reader.Read())
//                 {
//                     // Break the loop if the end array token is encountered
//                     if (reader.TokenType == JsonTokenType.EndArray)
//                     {
//                         break;
//                     }
//
//                     // Add the color to the list
//                     row.Add((Kleur)Enum.Parse(typeof(Kleur), reader.GetString()));
//                 }
//
//                 // Add the list to the outer list
//                 tempBoard.Add(row);
//             }
//         }
//
//         // Create a new Kleur[,] and fill it with the data from the list of lists
//         Kleur[,] board = new Kleur[tempBoard.Count, tempBoard[0].Count];
//
//         for (int i = 0; i < tempBoard.Count; i++)
//         {
//             for (int j = 0; j < tempBoard[0].Count; j++)
//             {
//                 board[i, j] = tempBoard[i][j];
//             }
//         }
//
//         // Return the Kleur[,]
//         return board;
//     }
//
//     // Override the Write method to handle serialization of Kleur[,]
//     public override void Write(Utf8JsonWriter writer, Kleur[,] value, JsonSerializerOptions options)
//     {
//         // Start writing an array
//         writer.WriteStartArray();
//
//         // Iterate over the rows of the Kleur[,]
//         for (int x = 0; x < value.GetLength(0); x++)
//         {
//             // Start writing an inner array
//             writer.WriteStartArray();
//
//             // Iterate over the columns of the Kleur[,]
//             for (int y = 0; y < value.GetLength(1); y++)
//             {
//                 // Write the color as a string
//                 writer.WriteStringValue(value[x, y].ToString());
//             }
//
//             // End writing the inner array
//             writer.WriteEndArray();
//         }
//
//         // End writing the array
//         writer.WriteEndArray();
//     }
// }