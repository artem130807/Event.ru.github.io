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

В проекте `EventRep.Application` подключён global using для FluentResults, поэтому
результаты можно объявлять без дополнительного импорта:

```csharp
Result<string> result = Result.Ok("Готово");
```

Слой `EventRep.Domain` намеренно не зависит от NuGet-пакетов. Если тип нужен в
другом слое, добавьте `using FluentResults;` и прямую ссылку на пакет в проект.

## Запуск

```bash
dotnet restore EventRep.sln
dotnet tool restore
dotnet build EventRep.sln
dotnet run --project src/EventRep.Api/EventRep.Api.csproj
```

При запуске с профилем `http` API доступен по `http://localhost:5078/api/health`, Swagger UI — по `http://localhost:5078/swagger/`.

## Миграции EF Core

`EventRepDbContextFactory` позволяет выполнять команды EF Core напрямую через
проект Infrastructure, без запуска API:

```bash
dotnet tool run dotnet-ef migrations add InitialCreate \
  --project src/EventRep.Infrastructure/EventRep.Infrastructure.csproj \
  --startup-project src/EventRep.Infrastructure/EventRep.Infrastructure.csproj
```

Фабрика получает строку подключения из аргумента `--connection`, переменной
окружения `ConnectionStrings__DefaultConnection` или файла
`src/EventRep.Api/appsettings.json` — в указанном порядке.

## CQRS

Слой Application разделён на команды и запросы через `IMediator`:

- каждая команда и каждый запрос — отдельный record, реализующий
  `IRequest<Result<T>>`;
- каждый обработчик напрямую реализует
  `IRequestHandler<TRequest, Result<T>>`;
- команды изменяют агрегаты через доменные репозитории и `IUnitOfWork`;
- запросы используют отдельные read-репозитории, возвращающие DTO без загрузки
  доменных сущностей;
- `ValidationBehavior` запускает FluentValidation до обработчика;
- `UnitOfWorkBehavior` вызывает один `SaveChangesAsync` для обычной команды и
  управляет явной транзакцией для транзакционной команды;
- запросы выполняются без транзакции и без change tracking.

Реализованный срез `Events` содержит создание события, отклик исполнителя,
назначение исполнителя, получение события и пагинированный список с фильтрами.
