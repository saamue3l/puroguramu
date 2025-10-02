# Puroguramu

[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-512BD4)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-6.0-blueviolet)](https://docs.microsoft.com/aspnet/core)

Plateforme d'apprentissage C# développée en 2024 dans le cadre d'un projet de groupe de bloc 2 axé sur le framework .NET Core. Permet aux étudiants de progresser à travers des leçons structurées et aux enseignants de gérer le contenu pédagogique.

## Contexte du projet

Projet académique en binôme réalisé en 2024 pour l'apprentissage du framework .NET Core (ASP.NET Core, Entity Framework Core, Identity). Conçu pour faciliter l'enseignement de C# avec un système de leçons progressives et d'exercices auto-corrigés.

## Fonctionnalités

### Espace Étudiant
- Consultation et progression à travers les leçons C#
- Éditeur de code intégré avec compilation en temps réel via Roslyn
- Tests automatisés des solutions soumises
- Système de difficulté (Facile, Moyen, Difficile)
- Suivi de progression personnalisé (À faire, En cours, Résolu, Abandonné)
- Gestion de profil utilisateur

### Espace Enseignant
- CRUD complet sur les leçons (création, modification, suppression)
- CRUD complet sur les exercices (énoncé, modèle de code, solution, tests)
- Gestion de l'ordre d'affichage des leçons et exercices
- Contrôle de visibilité (masquer/publier du contenu)
- Attribution des niveaux de difficulté
- Gestion des profils étudiants et groupes

## Technologies

- **.NET 6.0** - Framework principal
- **ASP.NET Core Razor Pages** - Interface web
- **Entity Framework Core** - ORM avec migrations
- **SQLite** (développement) / **SQL Server** (production)
- **ASP.NET Identity** - Authentification et autorisation
- **Roslyn** - Compilation et évaluation dynamique de code C#

## Prérequis

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- IDE compatible (Visual Studio 2022, JetBrains Rider, VS Code)

## Installation

### 1. Cloner le repository
git clone https://github.com/saamue31/puroguramu.git
cd puroguramu/Puroguramu.App

### 2. Installer l'outil EF Core
dotnet tool install --global dotnet-ef --version 6.0.36

### 3. Créer la base de données
mkdir Data
dotnet ef database update --context PuroguramuDbContext

### 4. Lancer l'application
dotnet run

L'application sera accessible sur `https://localhost:7291`.

## Comptes par défaut

Deux comptes de démonstration sont créés automatiquement au premier lancement :

| Rôle | Email | Mot de passe |
|------|-------|--------------|
| Enseignant | `enseignant@example.com` | `Enseignant123!` |
| Étudiant | `etudiant@example.com` | `Etudiant123!` |

## Structure du projet

puroguramu/
├── Puroguramu.App/               # Application web (Razor Pages)
├── Puroguramu.Domains/           # Modèles métier et interfaces
├── Puroguramu.Infrastructures/   # Implémentations (DbContext, Repositories)
│   ├── DbContexts/               # Contextes EF Core (SQLite/SQL Server)
│   ├── Migrations/               # Migrations de base de données
│   ├── Repositories/             # Accès aux données
│   └── Roslyn/                   # Compilateur C# dynamique

Projet académique - HELMO (2024)
