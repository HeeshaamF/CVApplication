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
