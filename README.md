# GSB Gestion Stock

> Application Windows Forms de gestion de prescriptions médicales — Projet BTS SIO

---

## Table des matières

1. [Présentation](#présentation)
2. [Fonctionnalités](#fonctionnalités)
3. [Technologies utilisées](#technologies-utilisées)
4. [Architecture](#architecture)
5. [Base de données](#base-de-données)
6. [Prérequis et installation](#prérequis-et-installation)
7. [Configuration](#configuration)
8. [Utilisation](#utilisation)
9. [Sécurité](#sécurité)
10. [Structure du projet](#structure-du-projet)

---

## Présentation

**GSB Gestion Stock** est une application de bureau développée en **C# / .NET 8.0** avec Windows Forms. Elle permet de centraliser la gestion des prescriptions médicales, des patients et du catalogue de médicaments d'un établissement de santé.

### Objectifs

| Objectif | Description |
|----------|-------------|
| Centralisation | Toutes les données dans une base unique MySQL |
| Traçabilité | Historique complet des prescriptions (qui, quand, quoi) |
| Productivité | Création et export PDF rapides des prescriptions |
| Sécurité | Authentification et gestion des rôles Admin / Utilisateur |

---

## Fonctionnalités

### Rôle Administrateur
- Gestion complète des utilisateurs (CRUD)
- Gestion du catalogue de médicaments
- Consultation en lecture seule des patients et prescriptions

### Rôle Utilisateur
- Gestion des patients (création, modification, suppression)
- Gestion des prescriptions avec plusieurs médicaments
- Consultation du catalogue de médicaments
- **Export PDF** professionnel des prescriptions (ordonnance)

### Commun
- Authentification par email / mot de passe
- Recherche en temps réel dans toutes les entités

---

## Technologies utilisées

| Technologie | Version | Rôle |
|-------------|---------|------|
| C# / .NET | 8.0 | Langage & framework principal |
| Windows Forms | — | Interface graphique |
| MySQL | 8.0 | Base de données relationnelle |
| QuestPDF | 2024.3.10 | Génération de PDFs |
| MySql.Data | — | Connecteur MySQL pour .NET |
| SHA-256 | — | Hachage des mots de passe |

### Justifications des choix

- **Windows Forms** : Rapidité de développement, simplicité, compatible .NET 8.0
- **MySQL** : SGBD open-source robuste et largement utilisé
- **Pattern DAO** : Séparation claire des responsabilités, maintenabilité, testabilité
- **QuestPDF** : Licence Community gratuite, API fluide, génération rapide

---

## Architecture

L'application suit une **architecture 3 couches** :

```
┌─────────────────────────────────────────────────────────┐
│                    COUCHE PRÉSENTATION                   │
│                  (Windows Forms - UI)                    │
│  LoginForm, AdminViewForm, UserForm, AddEditXxxForm      │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                    COUCHE MÉTIER                        │
│              (Models + Services + Utils)                 │
│  User, Patient, Medicine, Prescription, Appartient      │
│  PrescriptionPDFExporter, PasswordHasher, CurrentUser   │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│              COUCHE ACCÈS AUX DONNÉES                   │
│                    (DAO Pattern)                        │
│  Database, UserDAO, PatientDAO, MedicineDAO, etc.       │
└────────────────────┬────────────────────────────────────┘
                     │
                     ▼
┌─────────────────────────────────────────────────────────┐
│                  BASE DE DONNÉES                        │
│                      MySQL 8.0                          │
│         GSB_Gestion_Stock (5 tables)                    │
└─────────────────────────────────────────────────────────┘
```

### Principes appliqués

- **SRP** : Chaque classe a une responsabilité unique (Forms = UI, DAO = données, Services = logique métier)
- **DRY** : Connexion centralisée dans `Database.cs`, hachage dans `PasswordHasher`
- **Paramètres SQL** : Protection systématique contre les injections SQL

---

## Base de données

### Schéma simplifié (MCD)

```
Users (id_users, name, firstname, role, email, password)
  │
  ├── Patients (id_patients, id_users, name, firstname, age, gender)
  │
  ├── Medicine (id_medicine, id_users, name, dosage, description, molecule)
  │
  └── Prescription (id_prescription, id_users, id_patients, validity)
            │
            └── Appartient (id_prescription, id_medicine, quantity)
```

### Tables

| Table | Description | ID |
|-------|-------------|-----|
| `Users` | Utilisateurs de l'application | Manuel (MAX+1) |
| `Patients` | Patients suivis | Manuel (MAX+1) |
| `Medicine` | Catalogue de médicaments | Manuel (MAX+1) |
| `Prescription` | Prescriptions émises | AUTO_INCREMENT |
| `Appartient` | Relation N-N médicaments/prescriptions | Clé composite |

### Contraintes référentielles principales

- `Patients.id_users` → `Users` (RESTRICT)
- `Prescription.id_patients` → `Patients` (RESTRICT)
- `Appartient.id_prescription` → `Prescription` (**CASCADE**)

---

## Prérequis et installation

### Prérequis

- [.NET 8.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (ou SDK pour compiler)
- [MySQL Server 8.0](https://dev.mysql.com/downloads/mysql/)
- Windows 10 / 11

### Installation

1. **Cloner le dépôt**

```bash
git clone https://github.com/<votre-repo>/GSB_Gestion_Stock.git
cd GSB_Gestion_Stock
```

2. **Créer la base de données MySQL**

Exécuter le script SQL fourni pour créer la base `GSB_Gestion_Stock` et ses tables.

3. **Configurer la connexion** (voir section [Configuration](#configuration))

4. **Compiler et lancer**

```bash
cd GSB_Gestion_Stock
dotnet run
```

Ou ouvrir la solution `GSB_Gestion_Stock.sln` dans Visual Studio et lancer avec `F5`.

---

## Configuration

Le fichier `appsettings.json` contient la chaîne de connexion MySQL :

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;database=GSB_Gestion_Stock;uid=root;pwd=votre_mot_de_passe;"
  }
}
```

> **Note** : Ne pas committer ce fichier avec des identifiants réels en production.

---

## Utilisation

### Connexion

L'application démarre sur l'écran de connexion. Saisir un **email** et un **mot de passe** valides.

Selon le rôle de l'utilisateur, l'interface Admin ou Utilisateur s'ouvre automatiquement.

#### Comptes de démonstration

**Administrateur**

| Email | Mot de passe | Rôle |
|-------|--------------|------|
| `ADMIN` | `123` | Administrateur |

**Utilisateurs**

| Email | Mot de passe | Nom |
|-------|--------------|-----|
| `hugo@clinic.fr` | `123` | Hugo Dagreat |
| `walter.white@clinic.fr` | `123` | Walter White |
| `alice.martin@clinic.fr` | `password` | Alice Martin |
| `hugo.durand@clinic.fr` | `password` | Hugo Durand |
| `lea.petit@pharma.fr` | `password` | Léa Petit |

> **Note** : Les mots de passe `password` sont stockés en SHA-256 en base. Les mots de passe `123` sont stockés en clair (comptes de test uniquement).

### Export PDF

Depuis l'interface Utilisateur, sélectionner une prescription puis cliquer sur **Exporter PDF**. Un fichier PDF est généré contenant :

1. En-tête "ORDONNANCE MÉDICALE"
2. Informations patient (Nom, Prénom, Âge)
3. Date de validité
4. Tableau des médicaments (Nom, Dosage, Quantité, Molécule)
5. Date et heure de génération

---

## Sécurité

| Mesure | Statut | Détail |
|--------|--------|--------|
| Hachage des mots de passe | ✅ | SHA-256 (64 caractères hex) |
| Protection injections SQL | ✅ | Paramètres SQL sur tous les DAO |
| Gestion des rôles | ✅ | Admin / Utilisateur avec permissions distinctes |
| Rétrocompatibilité mots de passe | ✅ | Supporte anciens mots de passe en clair |
| Limitation des tentatives | ⚠️ | Non implémenté |
| Chiffrement des données en base | ⚠️ | Non implémenté |
| Expiration de session | ⚠️ | Non implémenté |

> Pour la production, il est recommandé de migrer vers **bcrypt** ou **Argon2** et de chiffrer la chaîne de connexion.

---

## Structure du projet

```
GSB_Gestion_Stock/
├── Forms/                    # Couche présentation (UI)
│   ├── LoginForm.cs
│   ├── AdminViewForm.cs
│   ├── UserForm.cs
│   └── AddEdit*.cs
│
├── Models/                   # Entités métier
│   ├── User.cs
│   ├── Patient.cs
│   ├── Medicine.cs
│   ├── Prescription.cs
│   ├── Appartient.cs
│   └── CurrentUser.cs        # Gestion de session
│
├── DAO/                      # Accès aux données
│   ├── Database.cs           # Connexion centralisée
│   ├── UserDAO.cs
│   ├── PatientDAO.cs
│   ├── MedicineDAO.cs
│   ├── PrescriptionDAO.cs
│   └── AppartientDAO.cs
│
├── Services/
│   └── PrescriptionPDFExporter.cs
│
├── Utils/
│   └── PasswordHasher.cs
│
├── appsettings.json          # Configuration (connexion DB)
└── Program.cs                # Point d'entrée
```

---

*Projet BTS SIO — GSB Gestion Stock*
