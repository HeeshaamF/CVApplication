# CVApplication

# Description
CVApplication est une plateforme permettant d’analyser automatiquement des CV, de détecter les rubriques essentielles (formation, expérience, compétences, langues…), de calculer un score global, de comparer le CV avec une offre d’emploi et de générer un rapport PDF avec des recommandations personnalisées.

---

# Installation

Téléchargez localement les fichiers du projet à l'endroit désiré (git clone https://github.com/HeeshaamF/CVApplication)

## 1. Logiciels requis

### SQL Server 2025 Express
1. Téléchargez l’installeur : `SQL2025-SSEI-Expr.exe` (disponible dans le dossier [Logiciels](https://github.com/HeeshaamF/CVApplication/tree/master/Logiciels))  
2. Lancez l’installeur et choisissez **Installation de base**.  
3. Suivez les étapes jusqu’à la fin (cela installe le moteur SQL Server Express).  
4. Notez le nom de l’instance (par défaut : `SQLEXPRESS`). Vous en aurez besoin pour la connexion.

### SQL Server Management Studio (SSMS)
1. Téléchargez l’installeur : `vs_SSMS.exe` (disponible dans le dossier [Logiciels](https://github.com/HeeshaamF/CVApplication/tree/master/Logiciels))
2. Lancez l’installation et acceptez les options par défaut.  
3. Une fois installé, ouvrez SSMS.  
4. Connectez-vous à votre instance SQL Server (`localhost\SQLEXPRESS`).  
5. Vérifiez que la connexion fonctionne.

#### JetBrains Rider
1. Installez au préalable SDK .NET 8.0 (via ce lien https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.424/dotnet-sdk-8.0.424-win-x64.exe)
2. Téléchargez Rider depuis le site JetBrains.  
3. Installez-le avec les options par défaut.  
4. Ouvrez le projet `CVApplication` dans Rider.

---

## 2. Base de données (restauration du backup)

1. Téléchargez l'archive `CvApplicationDb.zip`
2. Récupérez le fichier `CvApplicationDb.bak` de l'archive dans un dossier accessible par SQL Server.  
3. Ouvrez **SQL Server Management Studio (SSMS)**.  
4. Connectez-vous à votre instance (`localhost\SQLEXPRESS`).  
5. Dans l’**Explorateur d’objets**, clic droit sur **Bases de données** → **Restaurer la base de données…**.  
6. Choisissez **Source : Dispositif** → cliquez sur **…** → ajoutez le fichier `CvApplicationDb.bak`.  
7. Donnez un nom à la base restaurée (par exemple `CVApplicationDb`).  
8. Cliquez sur **OK** pour lancer la restauration.  
9. Vérifiez que la base apparaît dans l’explorateur avec toutes les tables (`CVs`, `AnalysesCV`, `OffresEmploi`, `Recommandations`).

---

## 3. Configuration du projet

1. Dans Rider, ouvrez le fichier `appsettings.json`.  
2. Configurez la chaîne de connexion :  

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=CVApplicationDb;Trusted_Connection=True;"
   }
   ```
3. Dans le terminal, entrez ces lignes de commandes pour installer les packages utilisés dans le projet :

   ```
   dotnet add package Microsoft.EntityFrameworkCore --version 8.0.28
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.28
   dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.28
   dotnet tool install --global dotnet-ef --version 8.0.0
   dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
   dotnet add package System.IdentityModel.Tokens.Jwt
   dotnet add package Microsoft.AspNetCore.Http.Features
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.*
   dotnet add package DocumentFormat.OpenXml
   dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.28
   dotnet add package UglyToad.PdfPig --prerelease
   dotnet add package QuestPDF
   ```
---

## 4. Compilation et exécution

Lorsque nous compilons et exécutons le programme, nous arrivons sur cette page :

![accueil.jpg](images_ReadMe/accueil.JPG)

Si on clique sur le bouton "Inscription", nous arrivons sur cette page qui nous permet de nous inscrire : 

![inscription.jpg](images_ReadMe/inscription.JPG)

Si on clique sur le bouton "Connexion", nous arrivons sur cette page pour se connecter : 

![connexion_candidat.jpg](images_ReadMe/connexion_candidat.JPG)

Après s'être inscrit ou connecté, en tant que candidat, nous sommes redirigés vers le tableau de bord : 

![accueil_candidat.jpg](images_ReadMe/accueil_candidat.JPG)

Depuis ce tableau de bord, il peut consulter ses CV déposés avec, pour chaque CV, la date d'upload, le nom de CV, le score global et les actions possibles : 

![liste_cv_1.jpg](images_ReadMe/liste_cv_1.JPG)
![liste_cv_2.jpg](images_ReadMe/liste_cv_1.JPG)

En cliquant sur le bouton "Télécharger", nous pouvons voir le CV déposé dans un nouvel onglet : 

![consultation_cv.jpg](images_ReadMe/consultation_cv.JPG)

En cliquant sur le bouton "Voir l'analyse", nous pouvons voir les scores composant le score global du CV: 

![analyse_cv.jpg](images_ReadMe/analyse_cv.JPG)

En cliquant sur le bouton "Comparer", nous pouvons comparer le CV à une offre d'emploi : 

![comparaison_cv.jpg](images_Readme/comparaison_cv.JPG)
![comparaison_cv1.jpg](images_ReadMe/comparaison_cv1.JPG)

Après la sélection de l'offre, nous avons le résultat du score de matching :

![resultat_comparaison.jpg](images_ReadMe/resultat_comparaison.JPG)

En cliquant sur le bouton "Rapport PDF" depuis la liste des CV ou sur le bouton "Télécharger le rapport PDF" depuis la page d'analyse, un rapport PDF est généré en récapitulant les scores et en listant des recommandations personnalisées selon les scores : 

![rapport_bon_score.jpg](images_ReadMe/rapport_bon_score.JPG)
![rapport_mauvais_score.jpg](images_ReadMe/rapport_mauvais_score.JPG)

En cliquant sur le bouton "Comparaisons", nous pouvons consulter la liste des comparaisons pour chaque CV :

![liste_comparaisons.jpg](images_ReadMe/liste_comparaisons.JPG)

Sur le tableau de bord ou sur la barre de navigation, en cliquant sur le bouton "Déposer un CV", nous pouvons upload un CV au format PDF ou DOCX : 

![upload_cv.jpg](images_ReadMe/upload_cv.JPG)

Sur le tableau de bord ou sur la barre de navigation, en cliquant sur le bouton "Mes Analyses", nous pouvons consulter l'historique des analyses avec, pour chaque analyse, la date d'upload de CV, le nom du CV et les scores de structure, de compétences et de lisibilité  : 

![liste_analyses_1.jpg](images_ReadMe/liste_analyses_1.JPG)
![liste_analyses_2.jpg](images_ReadMe/liste_analyses_2.JPG)

Sur la barre de navigation, en cliquant sur le bouton "Déconnexion", nous sommes redirigés vers la page de connexion.

Nous pouvons également nous connecter en tant qu'administrateur : 

![connexion_admin.jpg](images_ReadMe/connexion_admin.JPG)

Après la connexion, nous sommes redirigés vers ce tableau de bord : 

![accueil_admin.jpg](images_ReadMe/accueil_admin.JPG)

En cliquant sur le bouton "Offres d'emploi", nous pouvons consulter la liste des offres d'emploi avec, pour chaque offre, le titre, la description et les compétences attendues : 

![liste_offres.jpg](images_ReadMe/liste_offres.JPG)

En cliquant sur le bouton "Ajouter une offre", nous pouvons ajouter une offre d'emploi :

![ajouter_offre.jpg](images_ReadMe/ajouter_offre.JPG)

En cliquant sur le bouton "Modifier l'offre", nous pouvons modifier l'offre sélectionnée (titre, description ou compétences) : 

![modifier_offre.jpg](images_ReadMe/modifier_offre.JPG)

En cliquant sur le bouton "Supprimer l'offre", nous pouvons supprimer l'offre sélectionnée : 

![supprimer_offre.jpg](images_ReadMe/supprimer_offre.JPG)
