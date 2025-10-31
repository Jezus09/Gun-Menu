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
3. **CS2MenuManager** plugin

## 🔧 Telepítés

### 1. Metamod:Source telepítése
Töltsd le és telepítsd a Metamod:Source-t a CS2 szerveredre:
- https://www.sourcemm.net/downloads.php?branch=master

### 2. CounterStrikeSharp telepítése
Töltsd le és telepítsd a CounterStrikeSharp-ot:
- https://github.com/roflmuffin/CounterStrikeSharp/releases

### 3. CS2MenuManager telepítése
Töltsd le és telepítsd a CS2MenuManager-t:
- https://github.com/schwarper/CS2MenuManager/releases

Helyezd el a fájlokat:
```
csgo/addons/counterstrikesharp/
├── shared/
│   └── CS2MenuManager.dll
└── plugins/
    └── CS2MenuManager/
        └── CS2MenuManager.dll
```

### 4. Gun Menu Plugin telepítése

#### Opció A: Előre lefordított verzió
1. Töltsd le a legújabb release-t
2. Csomagold ki a `GunMenuPlugin` mappát ide:
   ```
   csgo/addons/counterstrikesharp/plugins/GunMenuPlugin/
   ```

#### Opció B: Fordítás forráskódból
1. Clone-old ezt a repository-t
2. Navigálj a `src` mappába
3. Futtasd:
   ```bash
   dotnet build -c Release
   ```
4. A lefordított DLL-t másold be:
   ```
   csgo/addons/counterstrikesharp/plugins/GunMenuPlugin/GunMenuPlugin.dll
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

MIT License

## 🤝 Közreműködés

Pull request-ek és issue-k várva várják!

## 📧 Kapcsolat

Ha kérdésed van, nyiss egy issue-t a GitHub-on.

---

**Készítette:** Claude
**Verzió:** 1.0.0
**Utolsó frissítés:** 2025
