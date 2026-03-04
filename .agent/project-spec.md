# Spécification du Projet - Maison Quizz

## 1. Vue d'ensemble de l'application

### Objectif de l'application
**Quoi** : Une application web de quizz interactive permettant aux utilisateurs de déterminer leur appartenance à une maison de Poudlard.

**Pour qui** : 
- Fans de Harry Potter souhaitant s'amuser
- Enfants / adolescents

**Pourquoi** : 
- Offrir une expérience de quiz moderne et engageante
- Permettre de déterminer de manière ludique selon les réponses à un quiz à choix multiples à quelle maison de Poudlard ils appartiennent correspondante à leur personnalité
- Créer une plateforme facile à utiliser sans inscription. Il faut juste un bouton pour commencer le quiz et un bouton pour recommencer et un champ pour entrer son nom. Le nom sera affiché sur la page de résultat.

## 2. Fonctionnalités principales

### Phase 1 - MVP (Minimum Viable Product)
1. **Affichage des questions**
   - Présentation claire des questions une par une, comme une sorte de sondage. Une seule question à la fois. On clique sur une réponse et on passe à la suivante. Il y a un bouton suivant pour passer à la question suivante.
   - Support de questions à choix multiples (QCM), avec des radio buttons. A chaque question, il y a 4 réponses possibles.
   - **Ordre aléatoire des questions** : L'ordre des questions doit être mélangé à chaque nouvelle session de quiz
   - **Ordre aléatoire des réponses** : L'ordre des 4 réponses proposées doit être mélangé pour chaque question

2. **Système de réponses**
   - Sélection d'une réponse
   - Bouton suivant pour passer à la question suivante

3. **Suivi de progression**
   - Compteur de questions (ex: 3/10)
   - Résumé final avec décision de la maison

4. **Interface utilisateur**
   - Design moderne et responsive
   - Navigation intuitive

### Phase 2 - Fonctionnalités avancées (futures)
- [x] Ordre aléatoire des questions et des réponses
- [x] **Externalisation des données** : Charger les questions depuis un fichier Markdown externe
- [ ] **Sauvegarde de l'historique** : Sauvegarder les résultats dans le localStorage et afficher l'historique

## 3. Spécifications techniques

### Technologies
- **Framework** : Blazor WebAssembly (.NET 10)
- **Langage** : C# 14
- **Styling** : PicoCSS le plus possible + CSS personnalisé uniquement si nécessaire
- **Hébergement** : Statique (GitHub Pages, Azure Static Web Apps, ou similaire)

### Architecture
- **Type** : Blazor WebAssembly Standalone (pas de backend pour le MVP)
- **Navigateurs cibles** : Navigateurs modernes (Chrome, Firefox, Safari, Edge)
- **Support mobile** : Oui, design responsive
- **Authentification** : Non requise pour le MVP
- **API externe** : Non requise pour le MVP, mais utilisation d'un fichier statique local pour les données (`wwwroot/data/quiz.md`)

### Structure des données

#### Modèle Question
```csharp
public class Question
{
    public int Id { get; set; }
    public string Texte { get; set; }
    public List<string> Options { get; set; }
    public string Maison { get; set; }
}
```

#### Modèle Quiz
```csharp
public class Quiz
{
    public int Id { get; set; }
    public string Titre { get; set; }
    public string Description { get; set; }
    public List<Question> Questions { get; set; }
}
```

#### Modèle Résultat
```csharp
public class ResultatQuiz
{
    public int NombreQuestions { get; set; }
    public string Maison { get; set; }
    public string Nom { get; set; }
    public DateTime DateCompletion { get; set; }
}
```

## 4. Composants principaux

### Pages
1. **Accueil** (`/`) 
   - Présentation de l'application
   - Bouton "Commencer le quiz"

2. **Quiz** (`/quiz/{id}`)
   - Affichage de la question actuelle
   - Options de réponse
   - Boutons de navigation
   - Barre de progression

3. **Résultats** (`/resultats`)
   - Score final
   - Récapitulatif des réponses
   - Bouton pour recommencer

### Composants réutilisables
1. **QuestionCard** - Affiche une question et ses options
2. **ProgressBar** - Barre de progression visuelle
3. **NameDisplay** - Affichage du nom de l'utilisateur actuel
4. **ResultCard** - Carte de résultat final
5. **QuizButton** - Bouton stylisé pour les actions

## 5. Gestion d'état

### Approche
- **Service QuizService** : Gestion des questions et de la logique du quiz
- **Service StateService** : Gestion de l'état global (score, progression)
- **LocalStorage** : Persistance de l'historique des résultats (Liste de `ResultatQuiz`)

### État à gérer
- Question actuelle (index)
- Réponses de l'utilisateur
- Question actuelle (index)
- Réponses de l'utilisateur
- Historique des résultats : Liste de nom + maison + date

## 6. Design et expérience utilisateur

### Principes de design
- **Minimaliste** : Interface épurée, focus sur le contenu
- **Coloré** : Utilisation des couleurs de Poudlard
- **Responsive** : Fonctionne parfaitement sur mobile et desktop
- **Accessible** : Contrastes suffisants

### Animations
- Pas d'animations pour le MVP

## 7. Fonctionnalité d'Historique

### Stockage (LocalStorage)
- Clé : `maison_quizz_historique`
- Format : JSON (Liste de `ResultatQuiz`)
- Données conservées : Nom, Maison, Date, Score

### Affichage
- **Page d'accueil** : Liste des derniers résultats sous le bouton "Commencer".
- **Page de résultats** : Liste des précédents résultats.
- **Tri** : Ordre antéchronologique (le plus récent en premier).
- **Design** : Carte simple ou liste stylisée "PicoCSS".

## 8. Données de test (MVP)

### Quiz exemple : "Culture Générale"
```
Questions :
1. Quelle est la capitale de la France ?
   - Lyon
   - Marseille
   - Paris
   - Nice

2. Combien de continents y a-t-il sur Terre ?
   - 5
   - 6
   - 7
   - 8

3. Qui a peint la Joconde ?
   - Picasso
   - Van Gogh
   - Léonard de Vinci
   - Monet

4. Quel est le plus grand océan du monde ?
   - Atlantique
   - Pacifique
   - Indien
   - Arctique

5. En quelle année l'homme a-t-il marché sur la Lune ?
   - 1965
   - 1967
   - 1969
   - 1971
```

## 8. Format des données (Markdown)

Les questions seront stockées dans un fichier `wwwroot/data/quiz.md` selon le format suivant :

```markdown
# Titre du Quiz
Description détaillée du quiz.

## Quelle qualité admires-tu le plus chez les autres ?
- Le courage | Gryffondor
- L'ambition | Serpentard
- L'intelligence | Serdaigle
- La loyauté | Poufsouffle

## Comment préfères-tu passer ton temps libre ?
- Vivre des aventures excitantes | Gryffondor
...
```

La logique de parsing devra :
1. Lire le titre (H1) et la description.
2. Extraire chaque question (H2).
3. Extraire chaque option (liste à puces) et son association avec une maison via le séparateur `|`.
4. Faire la correspondance entre les noms français (Gryffondor, Serpentard, Serdaigle, Poufsouffle) et l'énumération interne.

## 9. Critères de succès

### Fonctionnels
- ✅ L'utilisateur peut entrer son nom
- ✅ L'utilisateur peut commencer le quiz
- ✅ L'utilisateur peut répondre à toutes les questions
- ✅ L'utilisateur peut voir le résultat
- ✅ L'utilisateur peut recommencer le quiz

### Non-fonctionnels
- ✅ Temps de chargement < 10 secondes
- ✅ Fonctionne sur mobile et desktop
- ✅ Interface intuitive (pas de formation nécessaire)
- ✅ Design moderne et attrayant

## 9. Prochaines étapes

### Étape 1 : Mise en place
- [x] Créer le projet Blazor WebAssembly
- [x] Ajouter PicoCSS
- [ ] Créer la structure de dossiers

### Étape 2 : Modèles et services
- [ ] Créer les modèles de données (Question, Quiz, Resultat)
- [ ] Implémenter QuizService
- [ ] Créer les données de test

### Étape 3 : Composants
- [ ] Créer le layout principal
- [ ] Développer NameDisplay
- [ ] Développer QuestionCard
- [ ] Développer ProgressBar
- [ ] Développer ScoreDisplay
- [ ] Développer QuizButton

### Étape 4 : Pages
- [ ] Page d'accueil
- [ ] Page de quiz
- [ ] Page de résultats

### Étape 5 : Styling et polish
- [ ] Appliquer le design
- [ ] Optimiser pour mobile
- [ ] Tests utilisateur

### Étape 6 : Externalisation des données
- [ ] Créer le fichier `wwwroot/data/quiz.md`
- [ ] Modifier `QuizService` pour charger et parser le fichier Markdown
- [ ] Gérer le chargement asynchrone des données
- [ ] Mettre à jour les modèles si nécessaire pour supporter le parsing

## 10. Notes et idées

### Idées futures
- Mode sombre/clair
- Déploiement
- Build de production
- Déploiement sur hébergement statique
- Tests en production

### Questions ouvertes
- Combien de questions par quiz ? (10-15 pour le MVP)
- Faut-il permettre de revenir en arrière ? (Non pour le MVP)

---

**Date de création** : 16 janvier 2026
**Dernière mise à jour** : 16 janvier 2026
**Statut** : En développement - Phase MVP
