# Conception et architecture de système — Réponse

## Améliorations nécessaires pour rendre l'application  utilisable en production

- Authentification et autorisation  
  L'auth actuelle est une version de démonstration.  
  À faire : utiliser un vrai fournisseur d'identité (interne ou OIDC), définir des rôles clairs et protéger les API.

- Secrets et configuration  
  Les secrets ne doivent pas être stockés dans les fichiers.  
  À faire : utiliser un coffre à secrets et séparer les configurations par environnement (dev, préprod, prod).

- Sécurité API  
  Les protections HTTP de base ne sont pas en place.  
  À faire : ajouter les headers de sécurité standard, restreindre CORS, limiter les appels sur les endpoints publics.

- Validation et robustesse  
  Les validators existent déjà, mais uniquement sur le périmètre du test.  
  À faire : étendre la validation à tous les cas métier et uniformiser les messages d'erreur.

- Logs et supervision  
  Il n'y pas de logs 
  À faire : ajouter des logs structurés, tracer les erreurs et suivre quelques métriques simples (avec Serilog par exemple)

- Base de données et migrations  
  Le schéma n'est pas versionné.  
  À faire : introduire un outil de migration et ajouter les contraintes nécessaires.

- Tests et qualité  
  Les tests unitaires sur handlers sont déjà présents.  
  À faire : ajouter des tests d'intégration (API + DB) et un test E2E pour les parcours critiques.

- CI/CD  
  Aucun pipeline n'est défini.  
  À faire : mettre en place un pipeline simple (build → tests → déploiement) avec gestion des secrets.

## Fonctionnalité : auto‑inscription après achat

Objectif : permettre à un client d’inscrire un participant sans créer de compte, via un lien sécurisé envoyé après l’achat.

Flux utilisateur :

- Le commercial enregistre la vente (client, session, nombre de places).  
- Le système génère un lien unique d’inscription.  
- Le lien est envoyé par email au client.  
- Le client ouvre la page dédiée et saisit les informations du participant.  
- Le serveur vérifie : validité du lien, expiration, nombre de places restantes.  
- Si tout est correct, l’inscription est enregistrée et confirmée.

Sécurité (sans login) :

- Token unique, non devinable, associé à l’achat.  
- Expiration du lien.  
- Quota basé sur le nombre de places achetées.  
- Vérifications côté serveur (capacité, session, usage du token).  
- Rate limiting sur les endpoints publics.  
- Journalisation des tentatives invalides.  
- Possibilité de révoquer le lien en cas de problème.