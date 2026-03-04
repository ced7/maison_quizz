namespace maison_quizz.Models;

/// <summary>
/// Représente une question du quiz avec ses options et les maisons associées
/// </summary>
public class Question
{
    public int Id { get; set; }
    public string Texte { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new();
    
    /// <summary>
    /// Dictionnaire associant l'index de chaque option (0-3) à une maison
    /// </summary>
    public Dictionary<int, Maison> MaisonParOption { get; set; } = new();
}
