# Gun Menu Plugin for CS2

Counter-Strike 2 gun menu plugin **CounterStrikeSharp**-pal és **WASD vezérléssel** (CenterHTML megjelenítés).

## 🎯 Funkciók

- ✅ **WASD vezérlés** - Navigálás W/S gombokkal, választás A/D-vel
- ✅ **CenterHTML menü** - Középen jelenik meg szép HTML formázással
- ✅ **Automatikus megjelenés** - Minden round elején automatikusan megnyílik
- ✅ **AWP limit** - Maximum 2 AWP csapatonként
- ✅ **Minden fegyver** - Rifles, SMGs, Shotguns, Snipers, Pistols
- ✅ **Manual commands** - !guns, !rifles, !pistols parancsok

## 📋 Követelmények

1. **Metamod:Source** (CS2 verzió)
2. **CounterStrikeSharp** (legújabb verzió)
3. **CS2MenuManager** (submodule-ként benne van a projektben)

## 🔧 Telepítés

### 1. Metamod:Source telepítése
Töltsd le és telepítsd a Metamod:Source-t a CS2 szerveredre:
- https://www.sourcemm.net/downloads.php?branch=master

### 2. CounterStrikeSharp telepítése
Töltsd le és telepítsd a CounterStrikeSharp-ot:
- https://github.com/roflmuffin/CounterStrikeSharp/releases

### 3. Repository klónozása submodule-okkal

```bash
# Clone a repository és a submodule-ok
git clone --recurse-submodules https://github.com/Jezus09/Gun-Menu.git

# VAGY ha már klónoztad, inicializáld a submodule-okat:
git clone https://github.com/Jezus09/Gun-Menu.git
cd Gun-Menu
git submodule update --init --recursive
```

### 4. Fordítás forráskódból

```bash
cd Gun-Menu

# Fordítsd le a CS2MenuManager-t először
cd dependencies/CS2MenuManager/CS2MenuManager
dotnet build -c Release

# Majd a Gun Menu plugint
cd ../../../src
dotnet build -c Release
```

### 5. Telepítés a szerverre

A lefordított fájlokat másold be a szerver megfelelő helyére:

```
csgo/addons/counterstrikesharp/
├── shared/
│   └── CS2MenuManager.dll  (dependencies/CS2MenuManager/CS2MenuManager/bin/Release/net8.0/)
└── plugins/
    ├── CS2MenuManager/
    │   └── CS2MenuManager.dll
    └── GunMenuPlugin/
        └── GunMenuPlugin.dll  (src/bin/Release/net8.0/)
```

**Egyszerűbb telepítési script:**
```bash
# Lépj be a projekt gyökér mappájába
cd Gun-Menu

# Másold be a fájlokat (cseréld ki a CS2_SERVER_PATH-t a szervered elérési útjára)
CS2_SERVER_PATH="/path/to/your/cs2/server"

# CS2MenuManager
cp dependencies/CS2MenuManager/CS2MenuManager/bin/Release/net8.0/CS2MenuManager.dll \
   "$CS2_SERVER_PATH/game/csgo/addons/counterstrikesharp/shared/"

mkdir -p "$CS2_SERVER_PATH/game/csgo/addons/counterstrikesharp/plugins/CS2MenuManager"
cp dependencies/CS2MenuManager/CS2MenuManager/bin/Release/net8.0/CS2MenuManager.dll \
   "$CS2_SERVER_PATH/game/csgo/addons/counterstrikesharp/plugins/CS2MenuManager/"

# Gun Menu Plugin
mkdir -p "$CS2_SERVER_PATH/game/csgo/addons/counterstrikesharp/plugins/GunMenuPlugin"
cp src/bin/Release/net8.0/GunMenuPlugin.dll \
   "$CS2_SERVER_PATH/game/csgo/addons/counterstrikesharp/plugins/GunMenuPlugin/"
```

## 🎮 Használat

### Automatikus használat
- A menü **automatikusan megjelenik** minden round elején minden élő játékosnak

### Manual parancsok
- `!guns` vagy `!gunmenu` - Gun menü megnyitása
- `!rifles` vagy `!primary` - Csak primary fegyverek
- `!pistols` vagy `!secondary` - Csak secondary fegyverek

### WASD vezérlés
- **W** - Fel görgetés
- **S** - Le görgetés
- **A** vagy **SPACE** - Választás
- **D** - Vissza / Kilépés

## ⚙️ Konfiguráció

### AWP limit módosítása
A `GunMenuPlugin.cs` fájlban módosítsd ezt a sort:
```csharp
private const int MaxAwpPerTeam = 2; // Változtasd meg a számot
```

### Round start menü kikapcsolása
Ha nem szeretnéd hogy automatikusan megjelenjen, kommenteld ki vagy töröld ezt a részt:
```csharp
RegisterEventHandler<EventRoundStart>(OnRoundStart);
```

## 📝 Fegyver lista

### Primary Weapons
- **Rifles:** AK-47, M4A4, M4A1-S, AUG, SG 553, FAMAS, Galil AR
- **Sniper Rifles:** AWP, SSG 08, G3SG1, SCAR-20
- **SMGs:** MP5-SD, MP7, MP9, MAC-10, PP-Bizon, P90, UMP-45
- **Heavy:** Negev, M249, Nova, XM1014, MAG-7, Sawed-Off

### Secondary Weapons
- **Pistols:** Glock-18, USP-S, P2000, P250, Five-SeveN, Tec-9, CZ75-Auto, Desert Eagle, R8 Revolver, Dual Berettas

## 🐛 Hibaelhárítás

### A menü nem jelenik meg
1. Ellenőrizd hogy a **CS2MenuManager** telepítve van
2. Ellenőrizd a szerver konzolt hibákért
3. Bizonyosodj meg róla hogy **CounterStrikeSharp** megfelelően fut

### AWP limit nem működik
- A plugin automatikusan követi az AWP-ket
- Ha valaki meghal AWP-vel, a számláló csökken
- Round végén reset-elődik

### Fordítási hiba
- Bizonyosodj meg hogy **.NET 8 SDK** telepítve van
- Ellenőrizd hogy a **CS2MenuManager.dll** elérhető

## 📄 Licensz

Ez a projekt **GPL-3.0** licensz alatt érhető el (lásd [LICENSE](LICENSE) fájl), mivel a CS2MenuManager dependency-t használja, ami szintén GPL-3.0 licensz alatt van.

### Külső komponensek

- **CS2MenuManager** - [schwarper/CS2MenuManager](https://github.com/schwarper/CS2MenuManager) (GPL-3.0)
- **CounterStrikeSharp** - [roflmuffin/CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp)

## 📁 Projekt struktúra

```
Gun-Menu/
├── src/                          # Gun Menu plugin forráskód
│   ├── GunMenuPlugin.cs
│   ├── WeaponHelper.cs
│   └── GunMenuPlugin.csproj
├── dependencies/
│   └── CS2MenuManager/           # Git submodule
└── README.md
```

## 🤝 Közreműködés

Pull request-ek és issue-k várva várják!

## 📧 Kapcsolat

Ha kérdésed van, nyiss egy issue-t a GitHub-on.

## 🙏 Köszönetnyilvánítás

- **schwarper** - CS2MenuManager készítője
- **roflmuffin** - CounterStrikeSharp framework
- **Constummer** - cs2-simple-guns-menu inspirációért

---

**Készítette:** Claude
**Verzió:** 1.0.0
**Utolsó frissítés:** 2025
