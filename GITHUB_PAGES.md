# Публикация на GitHub Pages

Проект публикуется автоматически через `.github/workflows/deploy-pages.yml` после
каждого push в ветку `main`. Также workflow можно запустить вручную на вкладке
**Actions → Deploy to GitHub Pages → Run workflow**.

Адрес сайта после первого успешного запуска:

`https://artem130807.github.io/Event.ru.github.io/`

## Однократная настройка репозитория

Откройте **Settings → Pages** и в поле **Source** выберите **GitHub Actions**.

Supabase URL и publishable key уже имеют безопасные публичные fallback-значения в
workflow. При желании их можно переопределить без изменения кода через
**Settings → Secrets and variables → Actions → Variables**:

- `VITE_SUPABASE_URL`
- `VITE_SUPABASE_PUBLISHABLE_KEY`

`service_role` key нельзя добавлять ни в Variables, ни в исходный код фронтенда.

## Как устроена сборка

- локально Vite использует `base: /`;
- в GitHub Actions задаётся `VITE_BASE_PATH=/Event.ru.github.io/`;
- `npm ci` устанавливает версии из `package-lock.json`;
- `npm run build` проверяет TypeScript и создаёт `dist`;
- `dist` публикуется официальными GitHub Pages Actions;
- `.nojekyll` отключает обработку статических файлов через Jekyll.
