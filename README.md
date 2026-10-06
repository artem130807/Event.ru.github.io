# EventRep

Репозиторий разделён на два независимых приложения:

- [`frontend/`](frontend/) — React + TypeScript + Vite интерфейс с локальными демонстрационными данными;
- [`backend/`](backend/) — минимальное решение ASP.NET Core Web API на .NET 10 с Clean Architecture.

## Локальный запуск

Фронтенд:

```bash
cd frontend
npm ci
npm run dev
```

Backend:

```bash
cd backend
dotnet run --project src/EventRep.Api/EventRep.Api.csproj
```

Фронтенд не отправляет запросы к API или Supabase. Созданные в интерфейсе события и отклики хранятся только до перезагрузки страницы.

## Docker

```bash
cp .env.example .env
# Замените POSTGRES_PASSWORD в .env на стойкий пароль.
docker compose up --build -d
```

Контейнер frontend доступен локально на `http://localhost:18080`, backend — на `http://localhost:8080`, PostgreSQL — на `localhost:15432`. Порты можно изменить через переменные `FRONTEND_PORT`, `BACKEND_PORT` и `POSTGRES_PORT`; по умолчанию они привязаны только к `127.0.0.1`. Данные PostgreSQL сохраняются в именованном volume `postgres_data`, а миграции EF Core применяются при запуске API.

На VPS системный Nginx использует конфигурацию [`deploy/nginx/eventrep.conf`](deploy/nginx/eventrep.conf): публичный сайт доступен на `http://185.246.65.59/`, проверка API — на `http://185.246.65.59/api/health`, Swagger UI — на `http://185.246.65.59/swagger/`. Само React-приложение пока не отправляет запросы к API.

GitHub Pages по-прежнему публикует только `frontend/dist` через [workflow](.github/workflows/deploy-pages.yml).

Push в ветку `main` также запускает [VPS deployment workflow](.github/workflows/deploy-vps.yml). Для него в настройках GitHub Actions должен быть задан секрет `VPS_SSH_KEY` с приватным ключом отдельного пользователя/ключа деплоя.
