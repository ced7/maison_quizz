using Blazored.LocalStorage;
using maison_quizz.Models;

namespace maison_quizz.Services;

/// <summary>
/// Service pour gérer l'historique des résultats du quiz avec persistance locale
/// </summary>
public class HistoryService
{
    private readonly ILocalStorageService _localStorage;
    private const string STORAGE_KEY = "maison_quizz_historique";
    private const int MAX_HISTORY_ITEMS = 12;

    public HistoryService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    /// <summary>
    /// Récupère l'historique des résultats trié par date décroissante
    /// </summary>
    public async Task<List<ResultatQuiz>> GetHistoryAsync()
    {
        try
        {
            var history = await _localStorage.GetItemAsync<List<ResultatQuiz>>(STORAGE_KEY);
            return history ?? new List<ResultatQuiz>();
        }
        catch
        {
            // En cas d'erreur de lecture, retourner une liste vide
            return new List<ResultatQuiz>();
        }
    }

    /// <summary>
    /// Ajoute un résultat à l'historique en conservant uniquement les 12 plus récents
    /// </summary>
    public async Task AddResultAsync(ResultatQuiz result)
    {
        var history = await GetHistoryAsync();
        
        // Ajouter le nouveau résultat au début de la liste
        history.Insert(0, result);
        
        // Garder uniquement les 12 derniers éléments
        if (history.Count > MAX_HISTORY_ITEMS)
        {
            history = history.Take(MAX_HISTORY_ITEMS).ToList();
        }
        
        await _localStorage.SetItemAsync(STORAGE_KEY, history);
    }
}
