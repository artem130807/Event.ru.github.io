import { useEffect, useMemo, useState } from 'react'
import {
  ArrowRight,
  Bell,
  Bookmark,
  BriefcaseBusiness,
  CalendarDays,
  Check,
  ChevronDown,
  Clock3,
  ListFilter,
  MapPin,
  Menu,
  MessageCircleMore,
  Plus,
  Search,
  Send,
  Sparkles,
  Users,
  WalletCards,
  X,
} from 'lucide-react'
import { categories, events as demoEvents } from './data'
import { eventsApi, favoritesApi, responsesApi } from './lib/api'
import { isSupabaseConfigured } from './lib/supabase'
import type { Category, EventDraft, EventItem, ResponseDraft } from './types'

type Modal = 'create' | 'details' | 'respond' | 'success' | null

const formatPrice = (value: number) => new Intl.NumberFormat('ru-RU').format(value) + ' ₽'

function App() {
  const [events, setEvents] = useState<EventItem[]>(demoEvents)
  const [loading, setLoading] = useState(isSupabaseConfigured)
  const [backendError, setBackendError] = useState<string | null>(null)
  const [activeCategory, setActiveCategory] = useState<(typeof categories)[number]>('Все события')
  const [query, setQuery] = useState('')
  const [city, setCity] = useState('Все города')
  const [price, setPrice] = useState('Любой бюджет')
  const [saved, setSaved] = useState<number[]>([3])
  const [selected, setSelected] = useState<EventItem>(events[0])
  const [modal, setModal] = useState<Modal>(null)
  const [menuOpen, setMenuOpen] = useState(false)
  const [submitted, setSubmitted] = useState<number[]>([])

  useEffect(() => {
    let active = true
    async function loadData() {
      try {
        const nextEvents = await eventsApi.list()
        if (!active) return
        setEvents(nextEvents)
        if (nextEvents[0]) setSelected(nextEvents[0])
        try {
          const nextFavorites = await favoritesApi.list()
          if (active) setSaved(nextFavorites)
        } catch (error) {
          if (active) setBackendError(error instanceof Error ? error.message : 'Авторизация Supabase недоступна')
        }
      } catch (error) {
        if (!active) return
        setBackendError(error instanceof Error ? error.message : 'Не удалось загрузить данные')
      } finally {
        if (active) setLoading(false)
      }
    }
    loadData()
    return () => { active = false }
  }, [])

  const filtered = useMemo(() => {
    return events.filter((event) => {
      const categoryMatch = activeCategory === 'Все события' || event.category === activeCategory
      const searchMatch = `${event.title} ${event.role} ${event.city}`.toLowerCase().includes(query.toLowerCase())
      const cityMatch = city === 'Все города' || event.city === city
      const priceMatch = price === 'Любой бюджет' || (price === 'до 40 000 ₽' ? event.price <= 40000 : event.price > 40000)
      return categoryMatch && searchMatch && cityMatch && priceMatch
    })
  }, [activeCategory, query, city, price])

  const openDetails = (event: EventItem) => {
    setSelected(event)
    setModal('details')
  }

  const openRespond = (event: EventItem) => {
    setSelected(event)
    setModal('respond')
  }

  const submitResponse = async (draft: ResponseDraft) => {
    await responsesApi.create(draft)
    setSubmitted((current) => [...new Set([...current, selected.id])])
    setEvents((current) => current.map((event) => event.id === selected.id ? { ...event, responses: event.responses + 1 } : event))
    setModal('success')
  }

  const toggleSaved = async (eventId: number) => {
    const wasSaved = saved.includes(eventId)
    setSaved((current) => wasSaved ? current.filter((id) => id !== eventId) : [...current, eventId])
    if (!isSupabaseConfigured) return
    try {
      if (wasSaved) await favoritesApi.remove(eventId)
      else await favoritesApi.add(eventId)
    } catch (error) {
      setSaved((current) => wasSaved ? [...current, eventId] : current.filter((id) => id !== eventId))
      setBackendError(error instanceof Error ? error.message : 'Не удалось изменить избранное')
    }
  }

  const createEvent = async (draft: EventDraft) => {
    const created = await eventsApi.create(draft)
    setEvents((current) => [created, ...current])
  }

  return (
    <div className="app-shell">
      <Header
        query={query}
        setQuery={setQuery}
        menuOpen={menuOpen}
        setMenuOpen={setMenuOpen}
        onCreate={() => setModal('create')}
      />

      <main>
        <div className={isSupabaseConfigured ? 'backend-status online' : 'backend-status demo'}>
          <span />{isSupabaseConfigured ? 'Supabase подключён' : 'Демо-режим · добавьте .env для Supabase'}
        </div>
        {backendError && <div className="backend-error"><span>{backendError}</span><button onClick={() => setBackendError(null)}><X size={15} /></button></div>}
        <section className="hero wrap">
          <div className="hero-copy">
            <div className="eyebrow"><span /> 312 новых задач за неделю</div>
            <h1>У важных событий<br />есть <em>свои люди.</em></h1>
            <p>Расскажите, что задумали, — специалисты сами предложат помощь. Или найдите событие, частью которого хочется стать.</p>
            <div className="hero-actions">
              <button className="button button-primary" onClick={() => setModal('create')}>
                Создать событие <Plus size={18} strokeWidth={2.5} />
              </button>
              <button className="button button-ghost" onClick={() => document.querySelector('#feed')?.scrollIntoView({ behavior: 'smooth' })}>
                Найти работу <ArrowRight size={18} />
              </button>
            </div>
            <div className="trust-row">
              <div className="avatar-stack"><i>АН</i><i>МД</i><i>КО</i><i>+2к</i></div>
              <span>Более 2 000 специалистов<br />уже находят здесь заказы</span>
            </div>
          </div>

          <div className="hero-board" aria-label="Пример карточки события">
            <div className="board-tape" />
            <div className="board-topline">
              <span className="board-label">ИЩУ СПЕЦИАЛИСТА</span>
              <span className="live-dot">активно</span>
            </div>
            <div className="board-icon"><span>♥</span></div>
            <h2>Фотограф на<br />летнюю свадьбу</h2>
            <div className="board-meta"><span>12 июля</span><span>•</span><span>Москва</span></div>
            <div className="board-footer">
              <div><small>БЮДЖЕТ</small><strong>до 70 000 ₽</strong></div>
              <div className="mini-responders"><b>АК</b><b>ИВ</b><b>+5</b></div>
            </div>
            <div className="board-sticker"><Sparkles size={15} /> 96% мэтч</div>
          </div>
        </section>

        <section className="feed-section" id="feed">
          <div className="wrap">
            <div className="section-heading">
              <div>
                <span className="kicker">ОТКРЫТЫЕ ЗАДАЧИ</span>
                <h2>События, которым<br />нужны вы</h2>
              </div>
              <p>Свежие предложения от людей и команд.<br />Выберите подходящее и расскажите о себе.</p>
            </div>

            <div className="category-scroll">
              {categories.map((category) => (
                <button
                  key={category}
                  className={activeCategory === category ? 'category active' : 'category'}
                  onClick={() => setActiveCategory(category)}
                >
                  {category}
                  {category === 'Свадьбы' && <span>18</span>}
                </button>
              ))}
            </div>

            <div className="filter-bar">
              <label className="inline-search">
                <Search size={19} />
                <input value={query} onChange={(e) => setQuery(e.target.value)} placeholder="Профессия или событие" />
              </label>
              <label className="select-wrap"><MapPin size={18} />
                <select value={city} onChange={(e) => setCity(e.target.value)}>
                  <option>Все города</option><option>Москва</option><option>Санкт-Петербург</option><option>Казань</option><option>Екатеринбург</option><option>Нижний Новгород</option>
                </select><ChevronDown size={16} />
              </label>
              <label className="select-wrap"><WalletCards size={18} />
                <select value={price} onChange={(e) => setPrice(e.target.value)}>
                  <option>Любой бюджет</option><option>до 40 000 ₽</option><option>от 40 000 ₽</option>
                </select><ChevronDown size={16} />
              </label>
              <button className="filter-button" onClick={() => { setCity('Все города'); setPrice('Любой бюджет'); setQuery('') }}><ListFilter size={18} /> Сбросить</button>
            </div>

            <div className="results-line"><b>{filtered.length}</b> событий найдено <span /> Обновлено только что</div>

            {loading ? <div className="loading-state"><span /><p>Загружаем события из Supabase…</p></div> : filtered.length ? (
              <div className="event-grid">
                {filtered.map((event, index) => (
                  <EventCard
                    key={event.id}
                    event={event}
                    index={index}
                    saved={saved.includes(event.id)}
                    submitted={submitted.includes(event.id)}
                    onSave={() => toggleSaved(event.id)}
                    onOpen={() => openDetails(event)}
                    onRespond={() => openRespond(event)}
                  />
                ))}
              </div>
            ) : (
              <div className="empty-state"><Search size={34} /><h3>Ничего не нашлось</h3><p>Попробуйте убрать часть фильтров или изменить запрос.</p></div>
            )}
          </div>
        </section>

        <section className="how wrap" id="how">
          <div className="how-heading"><span className="kicker">КАК ЭТО РАБОТАЕТ</span><h2>От идеи до команды —<br />всего три шага</h2></div>
          <div className="steps">
            <article><span className="step-no">01</span><div className="step-icon coral"><MessageCircleMore /></div><h3>Опишите событие</h3><p>Расскажите, кого ищете, укажите дату, место и комфортный бюджет.</p></article>
            <article><span className="step-no">02</span><div className="step-icon blue"><Users /></div><h3>Получите отклики</h3><p>Специалисты предложат свои условия и покажут портфолио.</p></article>
            <article><span className="step-no">03</span><div className="step-icon lime"><Sparkles /></div><h3>Выберите своего</h3><p>Сравните предложения, обсудите детали и соберите идеальную команду.</p></article>
          </div>
        </section>

        <section className="cta-wrap wrap">
          <div className="cta-copy"><span>ЕСТЬ ИДЕЯ?</span><h2>Давайте превратим<br />её в событие.</h2><p>Публикация бесплатна и займёт не больше трёх минут.</p></div>
          <button className="cta-button" onClick={() => setModal('create')}><span>Начать</span><ArrowRight /></button>
          <div className="cta-doodle">✦</div>
        </section>
      </main>

      <Footer />

      {modal && (
        <ModalShell onClose={() => setModal(null)} wide={modal === 'details'}>
          {modal === 'create' && <CreateEvent onClose={() => setModal(null)} onCreate={createEvent} />}
          {modal === 'details' && <EventDetails event={selected} onRespond={() => setModal('respond')} saved={saved.includes(selected.id)} onSave={() => toggleSaved(selected.id)} />}
          {modal === 'respond' && <RespondForm event={selected} onSubmit={submitResponse} />}
          {modal === 'success' && <SuccessState event={selected} onClose={() => setModal(null)} />}
        </ModalShell>
      )}
    </div>
  )
}

function Header({ query, setQuery, menuOpen, setMenuOpen, onCreate }: { query: string; setQuery: (v: string) => void; menuOpen: boolean; setMenuOpen: (v: boolean) => void; onCreate: () => void }) {
  return <header className="header"><div className="wrap nav">
    <a className="logo" href="#"><span className="logo-mark">С</span><span>событие<small>.рф</small></span></a>
    <nav className={menuOpen ? 'nav-links open' : 'nav-links'}>
      <a href="#feed" onClick={() => setMenuOpen(false)}>Найти событие</a><a href="#how" onClick={() => setMenuOpen(false)}>Как это работает</a><a href="#specialists" onClick={() => setMenuOpen(false)}>Специалисты</a>
    </nav>
    <div className="nav-actions">
      <label className="top-search"><Search size={18} /><input value={query} onChange={(e) => setQuery(e.target.value)} placeholder="Поиск" /></label>
      <button className="icon-button notification" aria-label="Уведомления"><Bell size={20} /><i /></button>
      <button className="profile-button"><span>ЕС</span><ChevronDown size={15} /></button>
      <button className="button nav-create" onClick={onCreate}><Plus size={17} /> Создать</button>
      <button className="mobile-menu" onClick={() => setMenuOpen(!menuOpen)}>{menuOpen ? <X /> : <Menu />}</button>
    </div>
  </div></header>
}

function EventCard({ event, index, saved, submitted, onSave, onOpen, onRespond }: { event: EventItem; index: number; saved: boolean; submitted: boolean; onSave: () => void; onOpen: () => void; onRespond: () => void }) {
  const match = [94, 89, 86, 82, 78, 91][index] ?? 84
  return <article className="event-card" style={{ '--accent': event.color } as React.CSSProperties}>
    <div className="event-card-top">
      <div className="date-block"><strong>{event.day}</strong><span>{event.month}</span></div>
      <div className="match-badge"><Sparkles size={13} /> {match}% вам подходит</div>
      <button className={saved ? 'save-button saved' : 'save-button'} onClick={onSave} aria-label="Сохранить"><Bookmark size={19} fill={saved ? 'currentColor' : 'none'} /></button>
    </div>
    <button className="card-click" onClick={onOpen}>
      <span className="event-category">{event.category}</span>
      <h3>{event.title}</h3>
      <div className="event-location"><MapPin size={16} /><span>{event.city} · {event.place}</span></div>
      <div className="need-line"><span>НУЖЕН</span><strong>{event.role}</strong></div>
      <div className="tag-list">{event.tags.map((tag) => <span key={tag}>{tag}</span>)}</div>
    </button>
    <div className="event-card-bottom">
      <div className="author"><span>{event.avatar}</span><div><small>ОРГАНИЗАТОР</small><strong>{event.author}</strong></div></div>
      <div className="price"><small>БЮДЖЕТ</small><strong>{formatPrice(event.price)}</strong></div>
    </div>
    <div className="card-actions"><span>{event.responses} откликов</span><button disabled={submitted} onClick={onRespond}>{submitted ? <><Check size={16} /> Отклик отправлен</> : <>Откликнуться <ArrowRight size={16} /></>}</button></div>
  </article>
}

function ModalShell({ children, onClose, wide = false }: { children: React.ReactNode; onClose: () => void; wide?: boolean }) {
  return <div className="modal-backdrop" onMouseDown={onClose}><div className={wide ? 'modal modal-wide' : 'modal'} onMouseDown={(e) => e.stopPropagation()}><button className="modal-close" onClick={onClose}><X size={22} /></button>{children}</div></div>
}

function CreateEvent({ onClose, onCreate }: { onClose: () => void; onCreate: (draft: EventDraft) => Promise<void> }) {
  const [step, setStep] = useState(1)
  const [title, setTitle] = useState('')
  const [category, setCategory] = useState<Category>('Свадьбы')
  const [role, setRole] = useState('Фотограф')
  const [description, setDescription] = useState('')
  const [eventDate, setEventDate] = useState('2026-10-18')
  const [city, setCity] = useState('Москва')
  const [place, setPlace] = useState('Место уточняется')
  const [price, setPrice] = useState(50000)
  const [published, setPublished] = useState(false)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const publish = async () => {
    setSaving(true)
    setError(null)
    try {
      await onCreate({ title, category, role, description, eventDate, city, place, price, tags: [] })
      setPublished(true)
    } catch (reason) {
      setError(reason instanceof Error ? reason.message : 'Не удалось опубликовать событие')
    } finally {
      setSaving(false)
    }
  }
  if (published) return <div className="success-state"><div className="success-icon"><Check /></div><span className="kicker">ГОТОВО</span><h2>Событие опубликовано!</h2><p>Мы уже ищем подходящих специалистов. Новые отклики появятся в вашем профиле.</p><button className="button button-primary" onClick={onClose}>Вернуться к событиям</button></div>
  return <div className="create-form">
    <div className="modal-head"><span className="kicker">НОВОЕ СОБЫТИЕ</span><h2>{step === 1 ? 'Что вы планируете?' : 'Кого и когда ищем?'}</h2><p>Шаг {step} из 2</p></div>
    <div className="progress"><i className="active" /><i className={step === 2 ? 'active' : ''} /></div>
    {step === 1 ? <>
      <label className="field"><span>Название события</span><input value={title} onChange={(e) => setTitle(e.target.value)} placeholder="Например, свадьба на летней веранде" autoFocus /></label>
      <div className="field"><span>Категория</span><div className="choice-grid">{(['Свадьбы', 'Дни рождения', 'Корпоративы', 'Концерты'] as Category[]).map((item) => <button key={item} className={category === item ? 'active' : ''} onClick={() => setCategory(item)}>{item}</button>)}</div></div>
      <label className="field"><span>Коротко о событии</span><textarea value={description} onChange={(e) => setDescription(e.target.value)} placeholder="Атмосфера, количество гостей и всё, что важно знать специалисту…" /></label>
      <button className="button button-primary full" disabled={!title.trim()} onClick={() => setStep(2)}>Продолжить <ArrowRight size={18} /></button>
    </> : <>
      <div className="two-fields"><label className="field"><span>Кого ищете</span><input value={role} onChange={(e) => setRole(e.target.value)} /></label><label className="field"><span>Бюджет, ₽</span><input type="number" value={price} onChange={(e) => setPrice(Number(e.target.value))} /></label></div>
      <div className="two-fields"><label className="field"><span>Дата</span><input type="date" value={eventDate} onChange={(e) => setEventDate(e.target.value)} /></label><label className="field"><span>Город</span><input value={city} onChange={(e) => setCity(e.target.value)} /></label></div>
      <label className="field"><span>Место</span><input value={place} onChange={(e) => setPlace(e.target.value)} /></label>
      {error && <p className="form-error">{error}</p>}
      <div className="form-bottom"><button className="back-link" onClick={() => setStep(1)}>Назад</button><button className="button button-primary" disabled={saving} onClick={publish}><Send size={17} /> {saving ? 'Публикуем…' : 'Опубликовать'}</button></div>
    </>}
  </div>
}

function EventDetails({ event, onRespond, saved, onSave }: { event: EventItem; onRespond: () => void; saved: boolean; onSave: () => void }) {
  return <div className="details">
    <div className="details-banner" style={{ '--accent': event.color } as React.CSSProperties}><span>{event.category}</span><div className="large-date"><strong>{event.day}</strong><small>{event.month}</small></div><div className="details-doodle">✦</div></div>
    <div className="details-content">
      <div className="details-main"><span className="kicker">НУЖЕН · {event.role.toUpperCase()}</span><h2>{event.title}</h2><div className="details-meta"><span><CalendarDays />{event.date}</span><span><MapPin />{event.city}, {event.place}</span><span><Clock3 />{event.startTime ? `Начало в ${event.startTime.slice(0, 5)}` : 'Время уточняется'}{event.duration ? ` · ${event.duration}` : ''}</span></div><h4>О задаче</h4><p>{event.description || 'Ищем человека, который почувствует атмосферу события и поможет сохранить её живой и настоящей. Нам важны лёгкость в общении, самостоятельность и внимание к деталям.'}</p><h4>Что особенно важно</h4><div className="detail-tags">{event.tags.map((tag) => <span key={tag}><Check size={14} />{tag}</span>)}</div></div>
      <aside className="details-aside"><div className="budget-card"><small>БЮДЖЕТ</small><strong>{formatPrice(event.price)}</strong><span>Финальная стоимость обсуждается</span></div><div className="organizer-card"><span>{event.avatar}</span><div><small>ОРГАНИЗАТОР</small><strong>{event.author}</strong><em><Check size={12} /> профиль подтверждён</em></div></div><button className="button button-primary full" onClick={onRespond}>Откликнуться <ArrowRight size={18} /></button><button className="button button-outline full" onClick={onSave}>{saved ? <><Bookmark fill="currentColor" size={17} /> Сохранено</> : <><Bookmark size={17} /> Сохранить</>}</button><p className="response-note"><Users size={15} /> Уже {event.responses} откликов</p></aside>
    </div>
  </div>
}

function RespondForm({ event, onSubmit }: { event: EventItem; onSubmit: (draft: ResponseDraft) => Promise<void> }) {
  const [message, setMessage] = useState('Здравствуйте! Буду рад помочь с вашим событием. У меня есть релевантный опыт и свободна указанная дата.')
  const [proposedPrice, setProposedPrice] = useState(event.price)
  const [deliveryTerm, setDeliveryTerm] = useState('14 дней')
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const send = async () => {
    setSaving(true); setError(null)
    try { await onSubmit({ eventId: event.id, message, proposedPrice, deliveryTerm }) }
    catch (reason) { setError(reason instanceof Error ? reason.message : 'Не удалось отправить отклик'); setSaving(false) }
  }
  return <div className="respond-form"><div className="modal-head"><span className="kicker">ОТКЛИК НА СОБЫТИЕ</span><h2>Расскажите о себе</h2><p>{event.title} · {event.role}</p></div><div className="profile-preview"><div className="profile-photo">ЕС</div><div><strong>Евгений Соколов</strong><span>{event.role} · Москва</span></div><button>Изменить профиль</button></div><label className="field"><span>Сопроводительное сообщение</span><textarea value={message} onChange={(e) => setMessage(e.target.value)} /></label><div className="two-fields"><label className="field"><span>Стоимость, ₽</span><input type="number" value={proposedPrice} onChange={(e) => setProposedPrice(Number(e.target.value))} /></label><label className="field"><span>Срок готовности</span><input value={deliveryTerm} onChange={(e) => setDeliveryTerm(e.target.value)} /></label></div><label className="attach-field"><BriefcaseBusiness size={20} /><span><strong>Добавить работу из портфолио</strong><small>Это заметно повышает шанс ответа</small></span><Plus size={18} /></label>{error && <p className="form-error">{error}</p>}<button className="button button-primary full" disabled={saving || message.length < 10} onClick={send}><Send size={17} /> {saving ? 'Отправляем…' : 'Отправить отклик'}</button></div>
}

function SuccessState({ event, onClose }: { event: EventItem; onClose: () => void }) {
  return <div className="success-state"><div className="success-icon"><Check /></div><span className="kicker">ОТКЛИК ОТПРАВЛЕН</span><h2>Теперь слово за<br />организатором</h2><p>{event.author} получит ваш отклик и сможет написать вам в чате. Мы пришлём уведомление.</p><div className="match-result"><Sparkles size={18} /><span><strong>Высокий шанс ответа</strong><small>Ваш профиль совпадает с 4 из 5 требований</small></span></div><button className="button button-primary full" onClick={onClose}>Продолжить смотреть события</button></div>
}

function Footer() {
  return <footer><div className="wrap footer-inner"><div><a className="logo light" href="#"><span className="logo-mark">С</span><span>событие<small>.рф</small></span></a><p>Место, где хорошие события<br />находят своих людей.</p></div><div className="footer-links"><div><strong>Сервис</strong><a href="#feed">Найти событие</a><a href="#">Найти специалиста</a><a href="#">Создать событие</a></div><div><strong>Помощь</strong><a href="#">Как это работает</a><a href="#">Безопасность</a><a href="#">Поддержка</a></div><div><strong>Мы рядом</strong><a href="#">Telegram</a><a href="#">ВКонтакте</a><a href="#">hello@sobytie.ru</a></div></div></div><div className="wrap footer-bottom"><span>© 2026 Событие.рф</span><span>Сделано для моментов, которые остаются ♥</span><span>Политика конфиденциальности</span></div></footer>
}

export default App
