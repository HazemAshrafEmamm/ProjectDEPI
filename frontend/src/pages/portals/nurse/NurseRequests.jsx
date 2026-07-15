import { useEffect, useState } from 'react'
import { Inbox } from 'lucide-react'
import StatusBadge from '../../../components/common/StatusBadge'
import EmptyState from '../../../components/common/EmptyState'
import { useToast } from '../../../context/ToastContext'
import { nursingApi } from '../../../api/endpoints/nursing'

export default function NurseRequests() {
  const toast = useToast()
  const [requests, setRequests] = useState([])
  const [isLoading, setIsLoading] = useState(true)

  const loadRequests = () => {
    setIsLoading(true)
    nursingApi
      .myRequests()
      .then(setRequests)
      .catch((err) => toast.error(err.message || 'Could not load requests'))
      .finally(() => setIsLoading(false))
  }

  useEffect(() => {
    loadRequests()
  }, [])

  const handleUpdateStatus = async (requestId, status) => {
    try {
      await nursingApi.updateStatus(requestId, status)
      toast.success(`Request ${status.toLowerCase()} successfully`)
      loadRequests()
    } catch (err) {
      toast.error(err.message || `Failed to ${status.toLowerCase()} request`)
    }
  }

  const pending = requests.filter((r) => r.status === 'Pending')
  const assigned = requests.filter((r) => r.status !== 'Pending')

  return (
    <div className="space-y-8">
      <div>
        <h2 className="font-display text-2xl font-bold text-ink-900">Home Care Requests</h2>
        <p className="mt-1 text-sm text-slate-500">Review new requests and manage your assigned visits.</p>
      </div>

      <section>
        <h3 className="mb-4 font-display text-lg font-bold text-ink-900">Pending Requests</h3>
        {isLoading ? (
          <p className="text-sm text-slate-500">Loading…</p>
        ) : pending.length === 0 ? (
          <EmptyState icon={Inbox} title="No pending requests" message="New home care requests will appear here." />
        ) : (
          <div className="grid gap-4 sm:grid-cols-2">
            {pending.map((r, idx) => (
              <div key={`${r.patientId}-${idx}`} className="card flex flex-col justify-between gap-3 p-5">
                <div>
                  <p className="font-display text-sm font-semibold text-ink-900">Patient #{r.patientId}</p>
                  <p className="text-xs font-medium text-vital-600">{r.careType}</p>
                </div>
                <div className="flex items-center justify-between mt-2">
                  <StatusBadge status={r.status} />
                  <div className="flex gap-2">
                    <button 
                      onClick={() => handleUpdateStatus(r.id, 'Rejected')}
                      className="rounded-lg border border-mist-200 px-3 py-1 text-xs font-semibold text-slate-600 hover:bg-slate-50"
                    >
                      Reject
                    </button>
                    <button 
                      onClick={() => handleUpdateStatus(r.id, 'Accepted')}
                      className="rounded-lg bg-vital-500 px-3 py-1 text-xs font-semibold text-white hover:bg-vital-600"
                    >
                      Accept
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </section>

      <section>
        <h3 className="mb-4 font-display text-lg font-bold text-ink-900">My Assigned Visits</h3>
        <div className="space-y-3">
          {assigned.length === 0 && <p className="text-sm text-slate-500">No assigned visits.</p>}
          {assigned.map((r, idx) => (
            <div key={`${r.patientId}-${idx}`} className="card flex flex-col gap-3 p-5 sm:flex-row sm:items-center sm:justify-between">
              <div>
                <p className="font-display text-sm font-semibold text-ink-900">Patient #{r.patientId}</p>
                <p className="text-xs font-medium text-vital-600">{r.careType}</p>
              </div>
              <StatusBadge status={r.status} />
            </div>
          ))}
        </div>
      </section>
    </div>
  )
}
