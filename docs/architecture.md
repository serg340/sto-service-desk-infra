# Infrastructure Architecture

## Servers

### GitLab Server

- GitLab CE
- GitLab Runner
- Docker Registry
- Monitoring stack

### App Server

- ASP.NET backend containers
- Docker Compose
- Nginx reverse proxy
- SSL termination

---

## CI/CD Flow

1. Developer pushes code to GitLab
2. GitLab Runner starts CI/CD pipeline
3. Docker image is built
4. Image pushed to private Docker Registry
5. Deployment executed via SSH
6. Docker Compose pulls new image
7. Containers recreated on target server
8. Healthchecks validate application status

---

## Monitoring

- Node Exporter (host metrics)
- cAdvisor (container metrics)
- Promtail (log shipping)
- Grafana (visualization)

---

## Technologies Used

- GitLab CE
- GitLab Runner
- Docker
- Docker Compose
- Nginx
- ASP.NET Core
- Linux
- SSH deployment
- Monitoring stack
- CI/CD automation

---

## Infrastructure Features

- Automated CI/CD pipelines
- Private Docker Registry
- Multi-stage Docker builds
- Reverse proxy with SSL
- Container healthchecks
- Automated deployments
- Environment separation (dev/main)
- Infrastructure documentation
- Monitoring and logging

---

## Deployment Strategy

### Development Environment

- Automatically deployed from `dev` branch
- Uses `latest` Docker image
- Fast testing and validation

### Production Environment

- Manual deployment from `main` branch
- Uses commit-based image tags
- Stable production releases

---

## Security

- SSH key authentication
- Private container registry
- SSL/TLS encryption
- Environment variables separation

---

## Repository Structure

```text
configs/
├── docker/
├── gitlab/
└── nginx/

docs/
├── architecture.md
└── info.md

screenshots/
