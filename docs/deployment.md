# Deployment Guide

## Overview

The project uses automated CI/CD deployment with GitLab Runner, Docker Registry and Docker Compose.

Deployment is performed through SSH automation and container recreation.

---

## Staging Environment

### Description

The staging environment is used for development testing and validation.

Deployment is automatically triggered from the `dev` branch.

---

### Deployment Flow

1. Developer pushes code to `dev`
2. GitLab CI/CD pipeline starts
3. Docker image is built
4. Image pushed to GitLab Container Registry
5. SSH deploy executed on staging server
6. Docker Compose updates containers

---

### Main Commands

Pull latest image:

```bash
docker compose pull
```

Update containers:

```bash
docker compose up -d
```

---

## Production Environment

### Description

The production environment is deployed manually from the `main` branch.

Production uses commit-based Docker image tags for stable releases.

---

### Deployment Flow

1. Developer pushes code to `main`
2. GitLab pipeline starts
3. Docker image is built
4. Image pushed to Container Registry
5. Manual deployment approval
6. SSH deploy executed on production server
7. Containers recreated with updated image

---

### Main Commands

Pull image:

```bash
docker compose pull
```

Recreate containers:

```bash
docker compose up -d --force-recreate
```

---

## Docker Infrastructure

### Docker Compose

The infrastructure uses Docker Compose for:

- Backend container deployment
- Environment separation
- Healthchecks
- Restart policies
- Image updates

---

### Healthchecks

Container health is validated using Docker healthchecks.

Example:

```bash
docker ps
```

Expected result:

```text
(healthy)
```

---

## Monitoring & Logs

### View running containers

```bash
docker ps
```

---

### View container logs

```bash
docker logs backend-main
```

---

### Inspect container health

```bash
docker inspect backend-main
```

---

## Deployment Features

- Automated CI/CD pipelines
- GitLab Runner automation
- Docker Registry integration
- SSH deployment
- Environment separation
- Manual production approval
- Container healthchecks
- Docker Compose orchestration

---

## Security

### Authentication

- SSH key-based authentication
- GitLab CI/CD Variables
- Private Docker Registry authentication

---

### Secrets Management

Sensitive data is not stored in the repository.

Environment variables are separated through:

```text
.env
```

and GitLab CI/CD Variables.

---

## Repository Structure

```text
configs/
├── docker/
├── gitlab/
└── nginx/

docs/
├── architecture.md
├── deployment.md
└── monitoring.md

screenshots/
```
