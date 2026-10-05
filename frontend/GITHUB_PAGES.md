# Публикация на GitHub Pages

Проект публикуется автоматически через `../.github/workflows/deploy-pages.yml` после
каждого push в ветку `main`. Также workflow можно запустить вручную на вкладке
**Actions → Deploy to GitHub Pages → Run workflow**.

Адрес сайта после первого успешного запуска:

`https://artem130807.github.io/Event.ru.github.io/`

## Однократная настройка репозитория

Откройте **Settings → Pages** и в поле **Source** выберите **GitHub Actions**.

Фронтенд собирается без переменных Supabase и не обращается к внешнему API.

## Как устроена сборка

- локально Vite использует `base: /`;
- в GitHub Actions задаётся `VITE_BASE_PATH=/Event.ru.github.io/`;
- `npm ci` в `frontend` устанавливает версии из `frontend/package-lock.json`;
- `npm run build` в `frontend` проверяет TypeScript и создаёт `frontend/dist`;
- `frontend/dist` публикуется официальными GitHub Pages Actions;
- `.nojekyll` отключает обработку статических файлов через Jekyll.
