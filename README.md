# 🧱 Kiến trúc hệ thống News2025 (WinForms App)

## 📦 Cấu trúc thư mục


---

## 🏗️ Kiến trúc lớp tổng thể

```text
┌────────────────────────────┐
│         UI Layer           │
│ (frmMain, frmTroiTinTuc...)│
└────────────────────────────┘
              │
              ▼
┌────────────────────────────┐
│      Service Layer         │
│ (KarismaModel, CG3Model)   │
└────────────────────────────┘
              │
              ▼
┌────────────────────────────┐
│ External CG SDKs (KAEngine)│
└────────────────────────────┘
