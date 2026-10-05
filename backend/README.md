# EventRep backend

Минимальный каркас ASP.NET Core Web API на .NET 10 с разделением по Clean Architecture.

## Структура

| Проект | Назначение | Зависимости |
| --- | --- | --- |
| `EventRep.Domain` | Сущности и правила предметной области | Нет |
| `EventRep.Application` | Сценарии использования и абстракции | Domain |
| `EventRep.Infrastructure` | Реализации доступа к данным и внешним сервисам | Application, Domain |
| `EventRep.Api` | HTTP-контроллеры и композиция приложения | Application, Infrastructure |

В API пока есть только `GET /api/health`. Каркас доступа к PostgreSQL уже зарегистрирован, а бизнес-эндпоинты, авторизация и интеграция с фронтендом оставлены для дальнейшей реализации.

## Базовые зависимости

- EF Core и Npgsql для PostgreSQL;
- FluentResults для результатов операций (`Result<T>`);
- FluentValidation с регистрацией валидаторов через DI;
- Swashbuckle для Swagger UI;
- локальный инструмент `dotnet-ef` для миграций.

Строка подключения задаётся ключом `ConnectionStrings__DefaultConnection`.

## Запуск

```bash
dotnet restore EventRep.sln
dotnet tool restore
dotnet build EventRep.sln
dotnet run --project src/EventRep.Api/EventRep.Api.csproj
```

При запуске с профилем `http` API доступен по `http://localhost:5078/api/health`, Swagger UI — по `http://localhost:5078/swagger/`.
