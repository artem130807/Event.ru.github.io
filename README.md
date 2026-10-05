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
docker compose up --build -d
```

Сайт доступен на `http://localhost/`, проверка API — `http://localhost/api/health`, Swagger UI — `http://localhost/swagger/`. Backend также опубликован на `http://localhost:8080`; порты можно изменить через переменные `FRONTEND_PORT` и `BACKEND_PORT`. Само React-приложение пока не отправляет запросы к API.

GitHub Pages по-прежнему публикует только `frontend/dist` через [workflow](.github/workflows/deploy-pages.yml).
