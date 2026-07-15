


export const currentPatient = {
  id: 101,
  fullName: 'Salma Ahmed',
  email: 'salma.ahmed@example.com',
  phone: '01012345678',
  bio: '',
  role: 'Patient',
}


export const currentDoctor = {
  id: 1,
  fullName: 'Dr. Ahmed Ali',
  email: 'doctor@tabiby.health',
  phone: '01098765432',
  specialty: 'Cardiology',
  role: 'Doctor',
}

export const currentPharmacist = {
  id: 201,
  fullName: 'Sara Khalil',
  email: 'pharm@tabiby.health',
  phone: '01122334455',
  bio: 'Managing the pharmacy inventory and order fulfillment.',
  role: 'Pharmacist',
}

export const currentNurse = {
  id: 301,
  fullName: 'Fatima Nasser',
  email: 'nurse@tabiby.health',
  phone: '01555667788',
  bio: 'Specialized in wound care and elderly home visits.',
  role: 'Nurse',
}

export const currentAdmin = {
  id: 1,
  fullName: 'Admin',
  email: 'admin@tabiby.health',
  role: 'Admin',
}


export const demoAccounts = [
  { label: 'Patient', email: 'patient@tabiby.health' },
  { label: 'Doctor', email: 'doctor@tabiby.health' },
  { label: 'Pharmacist', email: 'pharm@tabiby.health' },
  { label: 'Nurse', email: 'nurse@tabiby.health' },
  { label: 'Admin', email: 'admin@tabiby.health' },
]

export const specialties = [
  'Cardiology',
  'Dermatology',
  'Pediatrics',
  'Orthopedics',
  'Neurology',
  'Dentistry',
  'ENT',
  'General Medicine',
]


export const doctors = [
  {
    id: 1,
    name: 'Dr. Mona Khalil',
    specialty: 'Cardiology',
    location: 'Nasr City, Cairo',
    phone: '01012345678',
    rating: 4.9,
    reviews: 214,
    price: 350,
    nextSlot: 'Today, 4:30 PM',
  },
  {
    id: 2,
    name: 'Dr. Youssef Adel',
    specialty: 'Dermatology',
    location: 'Zamalek, Cairo',
    phone: '01098765432',
    rating: 4.8,
    reviews: 176,
    price: 300,
    nextSlot: 'Tomorrow, 11:00 AM',
  },
  {
    id: 3,
    name: 'Dr. Nourhan Samir',
    specialty: 'Pediatrics',
    location: 'Maadi, Cairo',
    phone: '01234567890',
    rating: 5.0,
    reviews: 302,
    price: 250,
    nextSlot: 'Today, 6:00 PM',
  },
  {
    id: 4,
    name: 'Dr. Karim Fathy',
    specialty: 'Orthopedics',
    location: 'Heliopolis, Cairo',
    phone: '01122334455',
    rating: 4.7,
    reviews: 98,
    price: 400,
    nextSlot: 'Mon, 9:00 AM',
  },
  {
    id: 5,
    name: 'Dr. Laila Hassan',
    specialty: 'Neurology',
    location: 'Dokki, Giza',
    phone: '01555667788',
    rating: 4.9,
    reviews: 143,
    price: 450,
    nextSlot: 'Wed, 1:00 PM',
  },
  {
    id: 6,
    name: 'Dr. Omar Reda',
    specialty: 'Dentistry',
    location: '6th of October, Giza',
    phone: '01099887766',
    rating: 4.6,
    reviews: 87,
    price: 280,
    nextSlot: 'Today, 8:00 PM',
  },
]


export const appointments = [
  {
    id: 1001,
    doctorId: 1,
    doctorName: 'Dr. Mona Khalil',
    specialty: 'Cardiology',
    appointmentDate: '2026-07-14',
    appointmentTime: '16:30',
    status: 'Confirmed',
    notes: 'Follow-up on blood pressure medication.',
  },
  {
    id: 1002,
    doctorId: 3,
    doctorName: 'Dr. Nourhan Samir',
    specialty: 'Pediatrics',
    appointmentDate: '2026-07-11',
    appointmentTime: '18:00',
    status: 'Pending',
    notes: 'Annual checkup.',
  },
  {
    id: 1003,
    doctorId: 2,
    doctorName: 'Dr. Youssef Adel',
    specialty: 'Dermatology',
    appointmentDate: '2026-06-30',
    appointmentTime: '11:00',
    status: 'Completed',
    notes: null,
  },
]


export const consultations = [
  {
    id: 501,
    doctorId: 1,
    doctorName: 'Dr. Mona Khalil',
    specialty: 'Cardiology',
    status: 'Active',
    lastMessage: "Let's check your readings again tomorrow morning.",
    lastMessageAt: '10 min ago',
    unread: 2,
  },
  {
    id: 502,
    doctorId: 5,
    doctorName: 'Dr. Laila Hassan',
    specialty: 'Neurology',
    status: 'Active',
    lastMessage: 'Please continue the medication for one more week.',
    lastMessageAt: '2 hr ago',
    unread: 0,
  },
  {
    id: 503,
    doctorId: 2,
    doctorName: 'Dr. Youssef Adel',
    specialty: 'Dermatology',
    status: 'Closed',
    lastMessage: 'Glad it cleared up! Take care.',
    lastMessageAt: '3 days ago',
    unread: 0,
  },
]


export const consultationMessages = [
  { id: 1, senderIsPatient: true, text: 'Hi doctor, my blood pressure reading this morning was 145/95.', time: '9:02 AM' },
  { id: 2, senderIsPatient: false, text: 'Thanks for sharing, Salma. Have you taken your medication today?', time: '9:05 AM' },
  { id: 3, senderIsPatient: true, text: 'Yes, right after breakfast as usual.', time: '9:06 AM' },
  { id: 4, senderIsPatient: false, text: "Let's check your readings again tomorrow morning.", time: '9:10 AM' },
]


export const medications = [
  { id: 1, name: 'Paracetamol 500mg', price: 15, stock: 240, isAvailable: true, pictureUrl: 'pills-hero', category: 'Pain Relief' },
  { id: 2, name: 'Amoxicillin 250mg Capsules', price: 42, stock: 120, isAvailable: true, pictureUrl: 'pills-capsules', category: 'Antibiotics' },
  { id: 3, name: 'Vitamin C Effervescent', price: 60, stock: 0, isAvailable: false, pictureUrl: 'pills-assorted', category: 'Vitamins' },
  { id: 4, name: 'Omeprazole 20mg', price: 35, stock: 80, isAvailable: true, pictureUrl: 'pills-hero', category: 'Digestive Health' },
  { id: 5, name: 'Cetirizine 10mg', price: 22, stock: 150, isAvailable: true, pictureUrl: 'pills-capsules', category: 'Allergy' },
  { id: 6, name: 'Ibuprofen 400mg', price: 18, stock: 200, isAvailable: true, pictureUrl: 'pills-assorted', category: 'Pain Relief' },
]


export const nursingRequests = [
  {
    id: 701,
    nurseId: 12,
    nurseName: 'Nurse Hoda Samir',
    specialization: 'Elderly Care',
    status: 'Assigned',
    requestedDate: '2026-07-12',
    address: '14 Al-Nasr St, Nasr City',
  },
  {
    id: 702,
    nurseId: null,
    nurseName: null,
    specialization: 'Wound Dressing',
    status: 'Pending',
    requestedDate: '2026-07-13',
    address: '3 Gamet El Dowal St, Mohandessin',
  },
]


export const notifications = [
  { id: 1, title: 'Appointment confirmed', message: 'Dr. Mona Khalil confirmed your appointment for Jul 14, 4:30 PM.', time: '5 min ago', read: false, type: 'appointment' },
  { id: 2, title: 'New message', message: 'Dr. Mona Khalil sent you a message.', time: '10 min ago', read: false, type: 'consultation' },
  { id: 3, title: 'Order shipped', message: 'Your order #4021 is on its way.', time: '1 hr ago', read: true, type: 'order' },
  { id: 4, title: 'Nurse assigned', message: 'Nurse Hoda Samir was assigned to your request.', time: 'Yesterday', read: true, type: 'nursing' },
]


export const basketItems = [
  { id: 1, medicationId: 1, name: 'Paracetamol 500mg', price: 15, quantity: 2, pictureUrl: 'pills-hero' },
  { id: 2, medicationId: 5, name: 'Cetirizine 10mg', price: 22, quantity: 1, pictureUrl: 'pills-capsules' },
]

export const orders = [
  { id: 4021, placedAt: '2026-07-09', status: 'Shipped', total: 52, items: 2 },
  { id: 3988, placedAt: '2026-06-28', status: 'Delivered', total: 108, items: 4 },
]

export const dashboardStats = {
  upcomingAppointments: 2,
  activeConsultations: 2,
  pendingOrders: 1,
  nursingRequests: 1,
}



export const doctorConsultationRequests = [
  { id: 9001, patientName: 'Layla Mahmoud', age: 34, complaint: 'Chest pain and shortness of breath', requestedAt: '2026-06-23' },
  { id: 9002, patientName: 'Karim Saleh', age: 28, complaint: 'Persistent headache for 3 days', requestedAt: '2026-06-23' },
]

export const doctorStats = {
  pendingRequests: 2,
  activeRequests: 1,
  completedRequests: 1,
  totalRequests: 4,
}


export const doctorScheduleSlots = [
  { id: 1, day: 'Sunday', hours: '09:00 - 13:00', available: true },
  { id: 2, day: 'Tuesday', hours: '14:00 - 18:00', available: true },
  { id: 3, day: 'Thursday', hours: '10:00 - 14:00', available: false },
]


export const doctorAppointments = [
  { id: 1001, patientName: 'Salma Ahmed', appointmentDate: '2026-07-14', appointmentTime: '16:30', status: 'Confirmed', notes: 'Follow-up on blood pressure medication.' },
  { id: 1004, patientName: 'Hana Adel', appointmentDate: '2026-07-15', appointmentTime: '10:00', status: 'Pending', notes: 'First-time visit, mild fatigue.' },
  { id: 1005, patientName: 'Tarek Nabil', appointmentDate: '2026-07-02', appointmentTime: '09:30', status: 'Completed', notes: null },
]



export const inventory = [
  { id: 1, name: 'Paracetamol 500mg', category: 'Analgesic', price: 15.5, stock: 200, status: 'Available' },
  { id: 2, name: 'Amoxicillin 250mg', category: 'Antibiotic', price: 45.0, stock: 150, status: 'Available' },
  { id: 3, name: 'Ibuprofen 400mg', category: 'Anti-inflammatory', price: 22.75, stock: 350, status: 'Available' },
  { id: 4, name: 'Omeprazole 20mg', category: 'Antacid', price: 38.0, stock: 0, status: 'Out of Stock' },
  { id: 5, name: 'Atorvastatin 10mg', category: 'Statin', price: 55.0, stock: 0, status: 'Out of Stock' },
  { id: 6, name: 'Lisinopril 10mg', category: 'Antihypertensive', price: 32.0, stock: 180, status: 'Available' },
  { id: 7, name: 'Cetirizine 10mg', category: 'Antihistamine', price: 18.0, stock: 500, status: 'Available' },
  { id: 8, name: 'Aspirin 100mg', category: 'Antiplatelet', price: 12.0, stock: 0, status: 'Out of Stock' },
]


export const pharmacyOrders = [
  { id: 55, patientName: 'Layla Mahmoud', items: 3, total: 68.75, date: '2026-06-20', status: 'Shipped' },
  { id: 56, patientName: 'Karim Saleh', items: 3, total: 150.0, date: '2026-06-18', status: 'Delivered' },
  { id: 57, patientName: 'Hana Adel', items: 2, total: 72.0, date: '2026-06-23', status: 'Pending' },
  { id: 58, patientName: 'Tarek Nabil', items: 1, total: 43.5, date: '2026-06-23', status: 'Processing' },
  { id: 59, patientName: 'Sara Hassan', items: 4, total: 89.25, date: '2026-06-22', status: 'Cancelled' },
]

export const orderStatusOptions = ['Pending', 'Processing', 'Shipped', 'Delivered', 'Cancelled']


export const nurseIncomingRequests = [
  {
    id: 801,
    patientName: 'Mostafa Adly',
    careType: 'Wound Care',
    address: '15 Tahrir St, Cairo, Egypt',
    requestedDate: '2026-07-13',
    status: 'Pending',
  },
  {
    id: 802,
    patientName: 'Nadia Fouad',
    careType: 'Elderly Care',
    address: '22 Nile Corniche, Giza, Egypt',
    requestedDate: '2026-07-14',
    status: 'Pending',
  },
]

export const nurseAssignedRequests = [
  {
    id: 701,
    patientName: 'Salma Ahmed',
    careType: 'Post-Op Care',
    address: '14 Al-Nasr St, Nasr City',
    requestedDate: '2026-07-12',
    status: 'Assigned',
  },
  {
    id: 703,
    patientName: 'Omar Sami',
    careType: 'IV Therapy',
    address: '5 Al-Azhar St, Cairo',
    requestedDate: '2026-07-09',
    status: 'Completed',
  },
]



export const professionalAccounts = [
  { id: 1, name: 'Dr. Ahmed Ali', email: 'doctor@tabiby.health', role: 'Doctor', licenseId: 'MD-2024-001', status: 'Active' },
  { id: 2, name: 'Sara Khalil', email: 'pharm@tabiby.health', role: 'Pharmacist', licenseId: 'PH-2024-005', status: 'Active' },
  { id: 3, name: 'Fatima Nasser', email: 'nurse@tabiby.health', role: 'Nurse', licenseId: 'RN-2024-012', status: 'Pending' },
  { id: 4, name: 'Dr. Rania Saeed', email: 'rania@tabiby.health', role: 'Doctor', licenseId: 'MD-2024-009', status: 'Active' },
]

export const professionalRoles = ['Doctor', 'Pharmacist', 'Nurse']
