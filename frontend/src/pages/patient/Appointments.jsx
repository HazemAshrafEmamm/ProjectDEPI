import { useEffect, useState } from 'react'
import { CalendarX2 } from 'lucide-react'
import AppointmentCard from '../../components/appointments/AppointmentCard'
import EmptyState from '../../components/common/EmptyState'
import Modal from '../../components/common/Modal'
import { useToast } from '../../context/ToastContext'
import { appointmentsApi } from '../../api/endpoints/appointments'
import { doctorsApi } from '../../api/endpoints/doctors'

const TABS = ['Upcoming', 'Past']

export default function Appointments() {
  const toast = useToast()
  const [tab, setTab] = useState('Upcoming')
  const [appointments, setAppointments] = useState([])
  const [doctorsById, setDoctorsById] = useState({})
  const [isLoading, setIsLoading] = useState(true)
  const [cancelTarget, setCancelTarget] = useState(null)

  const load = () => {
    setIsLoading(true)
    Promise.all([appointmentsApi.myAppointments(), doctorsApi.search({ pageSize: 200 })])
      .then(([appts, doctors]) => {
        setAppointments(appts || [])
        setDoctorsById(Object.fromEntries((doctors || []).map((d) => [d.id, d])))
      })
      .catch((err) => toast.error(err.message || 'Could not load appointments'))
      .finally(() => setIsLoading(false))
  }

  useEffect(load, [])

  const isPast = (a) => a.statusText === 'Completed' || a.statusText === 'Cancelled'
  const list = appointments
    .filter((a) => (tab === 'Upcoming' ? !isPast(a) : isPast(a)))
    .map((a) => ({
      id: a.id,
      doctorName: a.doctorName,
      specialty: doctorsById[a.doctorId]?.specialty || '',
      appointmentDate: a.appointmentDate,
      appointmentTime: a.appointmentTime?.slice(0, 5),
      status: a.statusText,
      notes: a.notes,
    }))

  const confirmCancel = async () => {
    try {
      await appointmentsApi.cancel(cancelTarget)
      toast.success('Appointment cancelled')
      setCancelTarget(null)
      load()
    } catch (err) {
      toast.error(err.message || 'Could not cancel this appointment')
    }
  }

  return (
    <div className="space-y-6">
      <div>
        <h2 className="font-display text-2xl font-bold text-ink-900">My Appointments</h2>
        <p className="mt-1 text-sm text-slate-500">Track, manage, and review your visits.</p>
      </div>

      <div className="flex gap-2 border-b border-mist-200">
        {TABS.map((t) => (
          <button
            key={t}
            onClick={() => setTab(t)}
            className={`px-4 py-2.5 text-sm font-semibold transition-colors ${
              tab === t ? 'border-b-2 border-vital-500 text-vital-700' : 'text-slate-500 hover:text-ink-900'
            }`}
          >
            {t}
          </button>
        ))}
      </div>

      {isLoading ? (
        <p className="text-sm text-slate-500">Loading…</p>
      ) : list.length === 0 ? (
        <EmptyState icon={CalendarX2} title={`No ${tab.toLowerCase()} appointments`} message="Book a new appointment to see it here." />
      ) : (
        <div className="space-y-3">
          {list.map((a) => (
            <AppointmentCard
              key={a.id}
              appointment={a}
              onCancel={tab === 'Upcoming' ? () => setCancelTarget(a.id) : undefined}
            />
          ))}
        </div>
      )}

      {cancelTarget && (
        <Modal onClose={() => setCancelTarget(null)} title="Cancel appointment?">
          <p className="text-sm text-slate-600">This will cancel your appointment. This can't be undone.</p>
          <div className="mt-5 flex justify-end gap-3">
            <button className="btn-secondary" onClick={() => setCancelTarget(null)}>Keep it</button>
            <button className="btn-danger" onClick={confirmCancel}>Cancel Appointment</button>
          </div>
        </Modal>
      )}
    </div>
  )
}
