namespace maison_quizz.Models;

/// <summary>
/// Représente un quiz complet avec toutes ses questions
/// </summary>
public class Quiz
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Question> Questions { get; set; } = new();
}
