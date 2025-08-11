# **QuickCourt – Local Sports Booking Platform**

> **Book. Play. Connect.** – A seamless way for sports enthusiasts to find and book local sports facilities, and for facility owners to manage courts, schedules, and earnings.

---

## Team: **ThinkOutside4**

**Team Leader:** Nency Parmar

| Name             | Responsibility                          | 🔧 Contribution Highlights                                                                                                      |
| ------------------- | ------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------- |
| **Nency Parmar**    | Frontend Developer (UI/UX), Team Leader | Built responsive, accessible UI using **Next.js**, **Tailwind CSS**, and **shadcn/ui** while leading overall project execution. |
| **Mansi Nimavat**   | API Integration (Frontend)              | Connected backend APIs using **Fetch**, **Axios**, and **React Hooks** in **Next.js** for seamless data flow.                   |
| **Darshi Kathrani** | Database Designer                       | Designed database structures, tested core features, resolved UI bugs, and supported basic AI utilities using **NPM**.           |
| **Vishal Baraiya**  | Backend Developer                       | Designed and developed secure RESTful APIs using **ASP.NET Core** and integrated **SQL Server** for robust data management.     |

---

## Overview

**QuickCourt** is a full-stack web application enabling users to **book local sports venues** (badminton courts, turf grounds, tennis tables, etc.), create/join matches, and connect with the community.
Facility owners can manage facilities, set availability, view bookings, and track earnings, while admins manage approvals, users, and global stats.

---

## Tech Stack

**Frontend**

* React.js
* Tailwind CSS
* Framer Motion (animations)
* Chart.js / Recharts (analytics)

**Backend**

* ASP.NET Core Web API
* JWT Authentication
* Entity Framework Core

**Database**

* SQL Server

**Other**

* OTP Verification
* Simulated Payment Gateway
* Role-Based Access Control (User, Facility Owner, Admin)

---

## Features

### **User**

* Signup/Login with OTP verification.
* Search and filter venues by sport type, price, rating.
* View venue details, photos, amenities, reviews.
* Book courts with date/time selection.
* View & manage bookings (cancel, track status).
* Edit personal profile.

### **Facility Owner**

* Dashboard with KPIs: total bookings, active courts, earnings.
* Add/Edit/Delete facilities & courts.
* Set pricing/hour & operating hours.
* Block/unblock time slots for maintenance.
* View upcoming/past bookings with user details.
* Manage profile.

### **Admin**

* Dashboard with global stats (total users, bookings, active courts).
* Approve/Reject facility registration requests.
* Manage users (search, filter, ban/unban).
* View booking history.
* Reports & moderation (optional).

---

## Analytics & Charts

* Daily/Weekly/Monthly Booking Trends.
* Earnings Summary.
* Peak Booking Hours.
* Facility Approval Trends.
* Most Active Sports.

---

## 🗄 Database Schema (Overview)

**Tables**

* `Users` (User, Owner, Admin roles)
* `Facilities`
* `Courts`
* `Bookings`
* `Amenities`
* `Reviews`
* `TimeSlots`
* `Payments`
* `AdminActions`

(Relationships defined via foreign keys for Users → Facilities → Courts → Bookings)
---

## 👨‍💻 Contributors

* **You** – Full-stack Development
* Team Members (if any)

---
