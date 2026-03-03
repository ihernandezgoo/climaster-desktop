# 🌦️ CliMaster Widget Diseinatzailea

> **Android-erako eguraldi widget pertsonalizatuak sortzeko tresna bisuala**

![alt text](<Captura de pantalla 2026-03-03 110326.png>)

---

## 📌 Zer da?

`CliMaster` Windows mahaigaineko aplikazioa da, Android gailuetarako eguraldi widget-ak diseinatzeko.
Interfaze bisual batekin widget-aren itxura eta edukia kontrolatu ditzakezu, eta ondoren QR edo JSON bidez esportatu.

### ✨ Ezaugarri nagusiak

- 🧩 **Diseinu bisuala**: aurrebista denbora errealean
- 📲 **QR esportazioa**: sortu, eskaneatu eta inportatu azkar
- 🎨 **Pertsonalizazio osoa**: koloreak, tamainak, ertzak eta elementuak
- 🧊 **Glassmorphism estiloa**: itxura moderno eta garbia
- 🌍 **Euskarazko interfazea**: erabilera erraza eta argia

---

## 🛠️ Instalazioa

### Betekizunak

- **Windows 10/11**
- **.NET 8.0 Runtime**

### Pausoak

```bash
git clone https://github.com/ihernandezgoo/climaster-desktop.git
cd climaster-desktop/Climaster-feature
dotnet restore
dotnet run
```

---

## 🚀 Erabilera azkarra

### 1️⃣ Tamaina hautatu

Hautatu widget-aren tamaina (gomendatua: **4x2 Ertaina**).

### 2️⃣ Itxura pertsonalizatu

- **Oinarrizko kolorea**: adib. `#FFFFFF`
- **Gardentasuna**: `%0` – `%100`
- **Ertz-erradioa**: `0` – `50px`

### 3️⃣ Elementuak gehitu

| Elementua | Tamaina | Deskribapena |
|---|---|---|
| 📍 **Kokapena** | 18px | Herri edo hiri izena |
| 🌡️ **Tenperatura** | 48px | Uneko tenperatura |
| ☁️ **Baldintza** | 20px | Eguraldiaren deskribapena |
| 💧 **Hezetasuna** | 16px | Hezetasun portzentajea |
| 💨 **Haizea** | 16px | Haizearen abiadura |
| ➖ **Zatitzailea** | - | Lerro bereizlea |
| 📅 **Eguneko iragarpena** | - | 1-7 eguneko informazioa |

### 4️⃣ Esportatu

#### 📷 QR kodea (gomendatua)

1. Sakatu **"📲 QR Sortu JSON-rekin"**
2. Eskaneatu Android gailutik
3. Prest ✅

#### 📄 JSON fitxategia

1. Sakatu **"💾 JSON Gorde"**
2. Transferitu fitxategia Android-era
3. Inportatu widget-a

---

## 📐 Widget tamainak

| Tamaina | Erabilera | Edukia |
|---|---|---|
| **2x1** | Oso txikia | Tenperatura bakarrik |
| **4x2** | Ertaina ⭐ | Informazio orekatua (gomendatua) |
| **4x3** | Handia | + Eguneko iragarpena |
| **4x4** | Oso handia | Informazio zabala |

---

## 🧾 JSON adibidea

```json
{
  "widgetId": "custom_widget_v1",
  "size": {
    "gridWidth": 4,
    "gridHeight": 2,
    "horizontalAlignment": "center",
    "verticalAlignment": "center"
  },
  "background": {
    "type": "glassmorphism",
    "color": "#33FFFFFF",
    "cornerRadius": 24,
    "padding": 16
  },
  "layout": [
    {
      "type": "location_name",
      "fontSize": 18,
      "alignment": "center"
    },
    {
      "type": "current_temp",
      "fontSize": 48,
      "alignment": "center"
    },
    {
      "type": "current_condition_text",
      "fontSize": 20,
      "alignment": "center"
    }
  ]
}
```

---

## 🧯 Arazo arruntak

### ❌ "Ezin da elementua gehitu"

**Konponbidea**: hautatu tamaina handiagoa edo kendu elementu batzuk.

### 📍 Widget-a ezkerrean agertzen da Android-en

**Konponbidea**: egiaztatu JSON-an `"alignment": "center"` ezarrita dagoela.

### 📦 QR kodea handiegia da

**Konponbidea**: murriztu elementu kopurua edo edukia sinplifikatu.

---

## 🧪 Teknologiak

- **.NET 8.0** — framework nagusia
- **WPF** — interfaze grafikoa
- **MVVM** — arkitektura eredua
- **QRCoder** — QR kodeen sorrera
- **System.Text.Json** — JSON serializazioa

---

## 🤝 Ekarpenak

Ekarpenak ongi etorriak dira!

```bash
git checkout -b feature/nire-ezaugarria
git commit -m "feat: ezaugarri berria"
git push origin feature/nire-ezaugarria
```

---

## 📬 Laguntza

- 🐞 Arazoak: [GitHub Issues](https://github.com/ihernandezgoo/climaster-desktop/issues)

---

## 📄 Lizentzia

MIT License — CC BY
Ibai Garrido eta Ivan Hernandez
