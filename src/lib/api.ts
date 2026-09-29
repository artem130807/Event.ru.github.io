import { events as demoEvents } from '../data'
import type { EventDraft, EventItem, ResponseDraft } from '../types'
import { ensureSession, isSupabaseConfigured, supabase } from './supabase'

type EventRow = {
  id: number
  owner_id: string | null
  title: string
  category: EventItem['category']
  role: string
  description: string | null
  event_date: string
  start_time: string | null
  duration: string | null
  city: string
  place: string
  price: number
  tags: string[] | null
  organizer_name: string
  organizer_avatar: string
  accent_color: string
  is_urgent: boolean
  status: 'open' | 'closed' | 'draft'
  created_at: string
  response_count: number
}

const monthNames = ['ЯНВ', 'ФЕВ', 'МАР', 'АПР', 'МАЙ', 'ИЮН', 'ИЮЛ', 'АВГ', 'СЕН', 'ОКТ', 'НОЯ', 'ДЕК']

function fromRow(row: EventRow): EventItem {
  const date = new Date(`${row.event_date}T12:00:00`)
  return {
    id: row.id,
    ownerId: row.owner_id,
    title: row.title,
    category: row.category,
    date: new Intl.DateTimeFormat('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' }).format(date),
    day: String(date.getDate()).padStart(2, '0'),
    month: monthNames[date.getMonth()],
    city: row.city,
    place: row.place,
    price: row.price,
    role: row.role,
    tags: row.tags ?? [],
    author: row.organizer_name,
    avatar: row.organizer_avatar,
    responses: row.response_count ?? 0,
    urgent: row.is_urgent,
    color: row.accent_color,
    description: row.description ?? undefined,
    startTime: row.start_time ?? undefined,
    duration: row.duration ?? undefined,
    status: row.status,
    createdAt: row.created_at,
  }
}

async function requireClient() {
  if (!supabase) throw new Error('Supabase не настроен')
  const session = await ensureSession()
  if (!session) throw new Error('Не удалось создать сессию Supabase')
  return { client: supabase, userId: session.user.id }
}

export const eventsApi = {
  async list(): Promise<EventItem[]> {
    if (!isSupabaseConfigured || !supabase) return demoEvents
    const { data, error } = await supabase
      .from('events')
      .select('*')
      .eq('status', 'open')
      .order('event_date', { ascending: true })
    if (error) throw error
    return (data as EventRow[]).map(fromRow)
  },

  async create(draft: EventDraft): Promise<EventItem> {
    if (!isSupabaseConfigured) {
      const date = new Date(`${draft.eventDate}T12:00:00`)
      return {
        id: Date.now(), title: draft.title, category: draft.category, role: draft.role,
        description: draft.description, date: new Intl.DateTimeFormat('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' }).format(date),
        day: String(date.getDate()).padStart(2, '0'), month: monthNames[date.getMonth()], city: draft.city,
        place: draft.place, price: draft.price, tags: draft.tags, author: 'Евгений Соколов', avatar: 'ЕС',
        responses: 0, color: '#ff785a', status: 'open', createdAt: new Date().toISOString(),
      }
    }
    const { client, userId } = await requireClient()
    const { data, error } = await client.from('events').insert({
      owner_id: userId,
      title: draft.title,
      category: draft.category,
      role: draft.role,
      description: draft.description,
      event_date: draft.eventDate,
      city: draft.city,
      place: draft.place,
      price: draft.price,
      tags: draft.tags,
      organizer_name: 'Евгений Соколов',
      organizer_avatar: 'ЕС',
      status: 'open',
    }).select('*').single()
    if (error) throw error
    return fromRow(data as EventRow)
  },

  async update(id: number, patch: Partial<EventDraft>): Promise<EventItem> {
    const { client } = await requireClient()
    const payload = {
      ...(patch.title !== undefined && { title: patch.title }),
      ...(patch.category !== undefined && { category: patch.category }),
      ...(patch.role !== undefined && { role: patch.role }),
      ...(patch.description !== undefined && { description: patch.description }),
      ...(patch.eventDate !== undefined && { event_date: patch.eventDate }),
      ...(patch.city !== undefined && { city: patch.city }),
      ...(patch.place !== undefined && { place: patch.place }),
      ...(patch.price !== undefined && { price: patch.price }),
      ...(patch.tags !== undefined && { tags: patch.tags }),
    }
    const { data, error } = await client.from('events').update(payload).eq('id', id).select('*').single()
    if (error) throw error
    return fromRow(data as EventRow)
  },

  async remove(id: number) {
    const { client } = await requireClient()
    const { error } = await client.from('events').delete().eq('id', id)
    if (error) throw error
  },
}

export const responsesApi = {
  async listForEvent(eventId: number) {
    const { client } = await requireClient()
    const { data, error } = await client.from('responses').select('*').eq('event_id', eventId).order('created_at', { ascending: false })
    if (error) throw error
    return data
  },
  async create(draft: ResponseDraft) {
    if (!isSupabaseConfigured) return { id: Date.now(), ...draft, status: 'pending' }
    const { client, userId } = await requireClient()
    const { data, error } = await client.from('responses').insert({
      event_id: draft.eventId,
      responder_id: userId,
      responder_name: 'Евгений Соколов',
      responder_role: 'Специалист',
      message: draft.message,
      proposed_price: draft.proposedPrice,
      delivery_term: draft.deliveryTerm,
    }).select().single()
    if (error) throw error
    return data
  },
  async update(id: number, patch: { message?: string; proposedPrice?: number; deliveryTerm?: string }) {
    const { client } = await requireClient()
    const { data, error } = await client.from('responses').update({
      ...(patch.message !== undefined && { message: patch.message }),
      ...(patch.proposedPrice !== undefined && { proposed_price: patch.proposedPrice }),
      ...(patch.deliveryTerm !== undefined && { delivery_term: patch.deliveryTerm }),
    }).eq('id', id).select().single()
    if (error) throw error
    return data
  },
  async remove(id: number) {
    const { client } = await requireClient()
    const { error } = await client.from('responses').delete().eq('id', id)
    if (error) throw error
  },
}

export const favoritesApi = {
  async list(): Promise<number[]> {
    if (!isSupabaseConfigured || !supabase) return [3]
    const { client, userId } = await requireClient()
    const { data, error } = await client.from('favorites').select('event_id').eq('user_id', userId)
    if (error) throw error
    return data.map((row) => row.event_id)
  },
  async add(eventId: number) {
    const { client, userId } = await requireClient()
    const { error } = await client.from('favorites').insert({ user_id: userId, event_id: eventId })
    if (error) throw error
  },
  async remove(eventId: number) {
    const { client, userId } = await requireClient()
    const { error } = await client.from('favorites').delete().eq('user_id', userId).eq('event_id', eventId)
    if (error) throw error
  },
}

export const profilesApi = {
  async getOwn() {
    const { client, userId } = await requireClient()
    const { data, error } = await client.from('profiles').select('*').eq('id', userId).single()
    if (error) throw error
    return data
  },
  async update(patch: { display_name?: string; role?: string; city?: string; bio?: string }) {
    const { client, userId } = await requireClient()
    const { data, error } = await client.from('profiles').update(patch).eq('id', userId).select().single()
    if (error) throw error
    return data
  },
  async remove() {
    const { client, userId } = await requireClient()
    const { error } = await client.from('profiles').delete().eq('id', userId)
    if (error) throw error
  },
}
