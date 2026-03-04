using maison_quizz.Models;
using System.Net.Http.Json;

namespace maison_quizz.Services;

/// <summary>
/// Service de gestion du quiz Harry Potter chargeant les données depuis un fichier Markdown
/// </summary>
public class QuizService : IQuizService
{
    private readonly HttpClient _httpClient;
    private Quiz? _cachedQuiz;

    public QuizService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Quiz> GetQuizAsync()
    {
        if (_cachedQuiz == null)
        {
            _cachedQuiz = await ChargerQuizDepuisMarkdown();
        }

        // Créer une copie du quiz avec questions et réponses mélangées
        return new Quiz
        {
            Id = _cachedQuiz.Id,
            Titre = _cachedQuiz.Titre,
            Description = _cachedQuiz.Description,
            Questions = MelangerQuestions(_cachedQuiz.Questions)
        };
    }

    private async Task<Quiz> ChargerQuizDepuisMarkdown()
    {
        try
        {
            var content = await _httpClient.GetStringAsync("data/quiz.md");
            return ParserMarkdown(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors du chargement du quiz : {ex.Message}");
            // Retourner un quiz par défaut en cas d'erreur pour éviter le crash
            return new Quiz { Titre = "Erreur", Description = "Impossible de charger le quiz." };
        }
    }

    private Quiz ParserMarkdown(string content)
    {
        var lines = content.Split('\n');
        var quiz = new Quiz { Id = 1, Questions = new List<Question>() };
        Question? currentQuestion = null;
        int questionId = 1;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine)) continue;

            if (trimmedLine.StartsWith("# "))
            {
                quiz.Titre = trimmedLine.Substring(2).Trim();
            }
            else if (trimmedLine.StartsWith("## "))
            {
                currentQuestion = new Question
                {
                    Id = questionId++,
                    Texte = trimmedLine.Substring(3).Trim(),
                    Options = new List<string>(),
                    MaisonParOption = new Dictionary<int, Maison>()
                };
                quiz.Questions.Add(currentQuestion);
            }
            else if (trimmedLine.StartsWith("- ") && currentQuestion != null)
            {
                var parts = trimmedLine.Substring(2).Split('|');
                if (parts.Length >= 2)
                {
                    var optionText = parts[0].Trim();
                    var maisonName = parts[1].Trim();
                    
                    int index = currentQuestion.Options.Count;
                    currentQuestion.Options.Add(optionText);
                    currentQuestion.MaisonParOption[index] = MapperMaison(maisonName);
                }
            }
            else if (quiz.Titre != null && quiz.Description == null && !trimmedLine.StartsWith("#"))
            {
                quiz.Description = trimmedLine;
            }
        }

        return quiz;
    }

    private Maison MapperMaison(string name)
    {
        return name.ToLower() switch
        {
            "gryffondor" => Maison.Gryffindor,
            "serpentard" => Maison.Slytherin,
            "serdaigle" => Maison.Ravenclaw,
            "poufsouffle" => Maison.Hufflepuff,
            _ => Maison.Gryffindor // Valeur par défaut
        };
    }

    private List<Question> MelangerQuestions(List<Question> questions)
    {
        var random = new Random();
        // Créer des copies profondes pour ne pas modifier le cache
        var copies = questions.Select(q => new Question 
        { 
            Id = q.Id, 
            Texte = q.Texte, 
            Options = new List<string>(q.Options),
            MaisonParOption = new Dictionary<int, Maison>(q.MaisonParOption)
        }).ToList();

        var questionsMelangees = copies.OrderBy(x => random.Next()).ToList();
        
        foreach (var question in questionsMelangees)
        {
            MelangerReponses(question);
        }
        
        return questionsMelangees;
    }
    
    private void MelangerReponses(Question question)
    {
        var random = new Random();
        
        var optionsAvecMaisons = question.Options
            .Select((option, index) => new { Option = option, Maison = question.MaisonParOption[index] })
            .OrderBy(x => random.Next())
            .ToList();
        
        question.Options = optionsAvecMaisons.Select(x => x.Option).ToList();
        question.MaisonParOption = optionsAvecMaisons
            .Select((x, index) => new { Index = index, x.Maison })
            .ToDictionary(x => x.Index, x => x.Maison);
    }

    public Maison CalculerMaison(List<Question> questions, Dictionary<int, int> reponses)
    {
        var pointsParMaison = new Dictionary<Maison, int>
        {
            { Maison.Gryffindor, 0 },
            { Maison.Slytherin, 0 },
            { Maison.Ravenclaw, 0 },
            { Maison.Hufflepuff, 0 }
        };

        foreach (var reponse in reponses)
        {
            var questionId = reponse.Key;
            var optionIndex = reponse.Value;
            
            var question = questions.FirstOrDefault(q => q.Id == questionId);
            if (question != null && question.MaisonParOption.ContainsKey(optionIndex))
            {
                var maison = question.MaisonParOption[optionIndex];
                pointsParMaison[maison]++;
            }
        }

        return pointsParMaison.OrderByDescending(x => x.Value).First().Key;
    }

    public string GetMaisonDescription(Maison maison)
    {
        return maison switch
        {
            Maison.Gryffindor => "Courage, bravoure et détermination sont tes qualités principales. Tu es un vrai Gryffondor !",
            Maison.Slytherin => "Ambition, ruse et leadership te caractérisent. Bienvenue à Serpentard !",
            Maison.Ravenclaw => "Intelligence, sagesse et créativité sont tes forces. Tu appartiens à Serdaigle !",
            Maison.Hufflepuff => "Loyauté, patience et travail acharné te définissent. Tu es un Poufsouffle !",
            _ => "Maison inconnue"
        };
    }
}
