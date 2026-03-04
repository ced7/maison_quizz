using maison_quizz.Models;

namespace maison_quizz.Services;

/// <summary>
/// Service pour gérer l'état du résultat du quiz entre les pages
/// </summary>
public class ResultatService
{
    public ResultatQuiz? DernierResultat { get; set; }
}
