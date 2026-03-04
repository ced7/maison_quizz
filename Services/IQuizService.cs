using maison_quizz.Models;

namespace maison_quizz.Services;

/// <summary>
/// Interface du service de gestion du quiz
/// </summary>
public interface IQuizService
{
    /// <summary>
    /// Récupère les données du quiz (chargées depuis le fichier Markdown)
    /// </summary>
    Task<Quiz> GetQuizAsync();
    
    /// <summary>
    /// Calcule la maison de Poudlard selon les réponses de l'utilisateur
    /// </summary>
    /// <param name="questions">Liste des questions du quiz</param>
    /// <param name="reponses">Dictionnaire avec l'ID de la question et l'index de la réponse sélectionnée</param>
    Maison CalculerMaison(List<Question> questions, Dictionary<int, int> reponses);
    
    /// <summary>
    /// Retourne la description d'une maison
    /// </summary>
    string GetMaisonDescription(Maison maison);
}
