export type Category = 'Свадьбы' | 'Дни рождения' | 'Корпоративы' | 'Концерты' | 'Другое'

export type EventItem = {
  id: number
  title: string
  category: Category
  date: string
  day: string
  month: string
  city: string
  place: string
  price: number
  role: string
  tags: string[]
  author: string
  avatar: string
  responses: number
  urgent?: boolean
  color: string
  ownerId?: string | null
  description?: string
  startTime?: string
  duration?: string
  status?: 'open' | 'closed' | 'draft'
  createdAt?: string
}

export type EventDraft = {
  title: string
  category: Category
  role: string
  description: string
  eventDate: string
  city: string
  place: string
  price: number
  tags: string[]
}

export type ResponseDraft = {
  eventId: number
  message: string
  proposedPrice: number
  deliveryTerm: string
}
