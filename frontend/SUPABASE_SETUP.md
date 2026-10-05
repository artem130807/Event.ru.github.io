# Подключение Supabase

## 1. Создайте проект

Создайте проект на [supabase.com](https://supabase.com/) и дождитесь запуска базы данных.

## 2. Создайте таблицы и политики

Откройте **SQL Editor**, вставьте содержимое файла
`frontend/supabase/migrations/202609290001_initial_schema.sql` и нажмите **Run**.

Миграция создаёт:

- `profiles` — профили пользователей;
- `events` — мероприятия;
- `responses` — отклики специалистов;
- `favorites` — сохранённые мероприятия;
- индексы, триггеры, демо-данные и RLS-политики.

## 3. Включите анонимные сессии

В Supabase Dashboard откройте **Authentication → Sign In / Providers → Anonymous**
и включите **Allow anonymous sign-ins**.

Приложение создаёт анонимного пользователя при первом открытии. Поэтому RLS может
безопасно определять владельца события, отклика и избранного даже без формы входа.

## 4. Настройте переменные окружения

Скопируйте `.env.example` в `.env` и заполните значения из **Project Settings → API**:

```env
VITE_SUPABASE_URL=https://YOUR_PROJECT_REF.supabase.co
VITE_SUPABASE_PUBLISHABLE_KEY=YOUR_PUBLISHABLE_KEY
```

Используйте только publishable/anon key. Никогда не добавляйте `service_role` в Vite-приложение.

## 5. Запустите приложение

```bash
cd frontend
npm install
npm run dev
```

Без `.env` интерфейс продолжает работать в демонстрационном режиме.
