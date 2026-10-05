# EventRep backend

Минимальный каркас ASP.NET Core Web API на .NET 10 с разделением по Clean Architecture.

## Структура

| Проект | Назначение | Зависимости |
| --- | --- | --- |
| `EventRep.Domain` | Сущности и правила предметной области | Нет |
| `EventRep.Application` | Сценарии использования и абстракции | Domain |
| `EventRep.Infrastructure` | Реализации доступа к данным и внешним сервисам | Application, Domain |
| `EventRep.Api` | HTTP-контроллеры и композиция приложения | Application, Infrastructure |

В API пока есть только `GET /api/health`. Бизнес-эндпоинты, хранилище, авторизация и интеграция с фронтендом оставлены для дальнейшей реализации.

## Запуск

```bash
dotnet restore EventRep.sln
dotnet build EventRep.sln
dotnet run --project src/EventRep.Api/EventRep.Api.csproj
```

При запуске с профилем `http` API доступен по `http://localhost:5078/api/health`.
