namespace GunMenuPlugin;

public enum WeaponType
{
    Primary = 0,
    Secondary = 1,
}

public class Weapon
{
    public string DisplayName { get; set; }
    public string GiveName { get; set; }
    public WeaponType Type { get; set; }

    public Weapon(string displayName, string giveName, WeaponType type = WeaponType.Primary)
    {
        DisplayName = displayName;
        GiveName = giveName;
        Type = type;
    }
}

public static class WeaponHelper
{
    private static readonly Dictionary<string, Weapon> _weaponsByDisplay;
    private static readonly Dictionary<string, Weapon> _weaponsByGiveName;

    static WeaponHelper()
    {
        var weapons = LoadWeapons();
        _weaponsByDisplay = weapons;
        _weaponsByGiveName = weapons.ToDictionary(x => x.Value.GiveName, x => x.Value, StringComparer.InvariantCultureIgnoreCase);
    }

    private static Dictionary<string, Weapon> LoadWeapons()
    {
        return new Dictionary<string, Weapon>(StringComparer.InvariantCultureIgnoreCase)
        {
            // ===== PRIMARY WEAPONS =====

            // Rifles
            { "AK-47", new("AK-47", "weapon_ak47") },
            { "M4A4", new("M4A4", "weapon_m4a1") },
            { "M4A1-S", new("M4A1-S", "weapon_m4a1_silencer") },
            { "AUG", new("AUG", "weapon_aug") },
            { "SG 553", new("SG 553", "weapon_sg553") },
            { "FAMAS", new("FAMAS", "weapon_famas") },
            { "Galil AR", new("Galil AR", "weapon_galilar") },

            // Sniper Rifles
            { "AWP", new("AWP", "weapon_awp") },
            { "SSG 08", new("SSG 08 (Scout)", "weapon_ssg08") },
            { "G3SG1", new("G3SG1", "weapon_g3sg1") },
            { "SCAR-20", new("SCAR-20", "weapon_scar20") },

            // SMGs
            { "MP5-SD", new("MP5-SD", "weapon_mp5sd") },
            { "MP7", new("MP7", "weapon_mp7") },
            { "MP9", new("MP9", "weapon_mp9") },
            { "MAC-10", new("MAC-10", "weapon_mac10") },
            { "PP-Bizon", new("PP-Bizon", "weapon_bizon") },
            { "P90", new("P90", "weapon_p90") },
            { "UMP-45", new("UMP-45", "weapon_ump45") },

            // Heavy
            { "Negev", new("Negev", "weapon_negev") },
            { "M249", new("M249", "weapon_m249") },
            { "Nova", new("Nova", "weapon_nova") },
            { "XM1014", new("XM1014", "weapon_xm1014") },
            { "MAG-7", new("MAG-7", "weapon_mag7") },
            { "Sawed-Off", new("Sawed-Off", "weapon_sawedoff") },

            // ===== SECONDARY WEAPONS =====

            // Pistols
            { "Glock-18", new("Glock-18", "weapon_glock", WeaponType.Secondary) },
            { "USP-S", new("USP-S", "weapon_usp_silencer", WeaponType.Secondary) },
            { "P2000", new("P2000", "weapon_hkp2000", WeaponType.Secondary) },
            { "P250", new("P250", "weapon_p250", WeaponType.Secondary) },
            { "Five-SeveN", new("Five-SeveN", "weapon_fiveseven", WeaponType.Secondary) },
            { "Tec-9", new("Tec-9", "weapon_tec9", WeaponType.Secondary) },
            { "CZ75-Auto", new("CZ75-Auto", "weapon_cz75a", WeaponType.Secondary) },
            { "Desert Eagle", new("Desert Eagle", "weapon_deagle", WeaponType.Secondary) },
            { "R8 Revolver", new("R8 Revolver", "weapon_revolver", WeaponType.Secondary) },
            { "Dual Berettas", new("Dual Berettas", "weapon_elite", WeaponType.Secondary) },
        };
    }

    public static List<Weapon> GetWeaponsByType(WeaponType type)
    {
        return _weaponsByDisplay.Values
            .Where(w => w.Type == type)
            .OrderBy(w => w.DisplayName)
            .ToList();
    }

    public static Weapon? GetWeaponByGiveName(string giveName)
    {
        _weaponsByGiveName.TryGetValue(giveName, out var weapon);
        return weapon;
    }

    public static Weapon? GetWeaponByDisplayName(string displayName)
    {
        _weaponsByDisplay.TryGetValue(displayName, out var weapon);
        return weapon;
    }
}
