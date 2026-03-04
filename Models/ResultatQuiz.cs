namespace maison_quizz.Models;

/// <summary>
/// Représente le résultat d'un quiz complété
/// </summary>
public class ResultatQuiz
{
    public int NombreQuestions { get; set; }
    public Maison Maison { get; set; }
    public string Nom { get; set; } = string.Empty;
    public DateTime DateCompletion { get; set; }
}
