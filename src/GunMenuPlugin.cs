using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;
using CS2MenuManager.API.Menu;

namespace GunMenuPlugin;

public partial class GunMenuPlugin : BasePlugin
{
    public override string ModuleName => "Gun Menu with WASD";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Claude";
    public override string ModuleDescription => "Gun Menu with WASD controls and round start trigger";

    // AWP tracking per team
    private int _awpCountT = 0;
    private int _awpCountCT = 0;
    private const int MaxAwpPerTeam = 2;

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventRoundStart>(OnRoundStart);
        RegisterEventHandler<EventPlayerDeath>(OnPlayerDeath);
        Console.WriteLine($"[{ModuleName}] Plugin loaded successfully!");
    }

    [GameEventHandler]
    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        // Reset AWP counters at round start
        _awpCountT = 0;
        _awpCountCT = 0;

        // Show gun menu to all alive players
        Server.NextFrame(() =>
        {
            var players = Utilities.GetPlayers().Where(p =>
                p != null &&
                p.IsValid &&
                !p.IsBot &&
                p.PawnIsAlive &&
                p.Team is CsTeam.Terrorist or CsTeam.CounterTerrorist
            );

            foreach (var player in players)
            {
                ShowGunMenu(player);
            }
        });

        return HookResult.Continue;
    }

    [GameEventHandler]
    public HookResult OnPlayerDeath(EventPlayerDeath @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !player.IsValid)
            return HookResult.Continue;

        // Check if player had AWP and decrease counter
        var weapons = player.PlayerPawn?.Value?.WeaponServices?.MyWeapons;
        if (weapons != null)
        {
            foreach (var weapon in weapons)
            {
                if (weapon?.Value?.DesignerName == "weapon_awp")
                {
                    if (player.Team == CsTeam.Terrorist)
                        _awpCountT = Math.Max(0, _awpCountT - 1);
                    else if (player.Team == CsTeam.CounterTerrorist)
                        _awpCountCT = Math.Max(0, _awpCountCT - 1);
                    break;
                }
            }
        }

        return HookResult.Continue;
    }

    [ConsoleCommand("guns")]
    [ConsoleCommand("gunmenu")]
    public void Command_Guns(CCSPlayerController? player, CommandInfo info)
    {
        if (!ValidatePlayer(player))
            return;

        ShowGunMenu(player!);
    }

    [ConsoleCommand("pistols")]
    [ConsoleCommand("secondary")]
    public void Command_Pistols(CCSPlayerController? player, CommandInfo info)
    {
        if (!ValidatePlayer(player))
            return;

        ShowPistolsMenu(player!);
    }

    [ConsoleCommand("rifles")]
    [ConsoleCommand("primary")]
    public void Command_Rifles(CCSPlayerController? player, CommandInfo info)
    {
        if (!ValidatePlayer(player))
            return;

        ShowRiflesMenu(player!);
    }

    private void ShowGunMenu(CCSPlayerController player)
    {
        var menu = new WasdMenu("Gun Menu", this);

        // Add Primary Weapons submenu
        menu.AddItem("Primary Weapons", (p, option) =>
        {
            ShowRiflesMenu(p);
        });

        // Add Secondary Weapons submenu
        menu.AddItem("Secondary Weapons", (p, option) =>
        {
            ShowPistolsMenu(p);
        });

        menu.Display(player, 0);
    }

    private void ShowRiflesMenu(CCSPlayerController player)
    {
        var menu = new WasdMenu("Primary Weapons", this);

        var primaryWeapons = WeaponHelper.GetWeaponsByType(WeaponType.Primary);

        foreach (var weapon in primaryWeapons)
        {
            // Check AWP limit
            if (weapon.GiveName == "weapon_awp")
            {
                int currentAwpCount = player.Team == CsTeam.Terrorist ? _awpCountT : _awpCountCT;
                if (currentAwpCount >= MaxAwpPerTeam)
                {
                    menu.AddItem($"{weapon.DisplayName} [LIMIT REACHED {currentAwpCount}/{MaxAwpPerTeam}]", (p, option) =>
                    {
                        p.PrintToChat($" \x02[Gun Menu]\x01 AWP limit reached ({MaxAwpPerTeam} per team)!");
                    });
                    continue;
                }
            }

            menu.AddItem(weapon.DisplayName, (p, option) =>
            {
                GiveWeapon(p, weapon);
            });
        }

        menu.Display(player, 0);
    }

    private void ShowPistolsMenu(CCSPlayerController player)
    {
        var menu = new WasdMenu("Secondary Weapons", this);

        var secondaryWeapons = WeaponHelper.GetWeaponsByType(WeaponType.Secondary);

        foreach (var weapon in secondaryWeapons)
        {
            menu.AddItem(weapon.DisplayName, (p, option) =>
            {
                GiveWeapon(p, weapon);
            });
        }

        menu.Display(player, 0);
    }

    private void GiveWeapon(CCSPlayerController player, Weapon weapon)
    {
        if (player?.PlayerPawn?.Value?.WeaponServices == null)
            return;

        // Check AWP limit before giving
        if (weapon.GiveName == "weapon_awp")
        {
            int currentAwpCount = player.Team == CsTeam.Terrorist ? _awpCountT : _awpCountCT;
            if (currentAwpCount >= MaxAwpPerTeam)
            {
                player.PrintToChat($" \x02[Gun Menu]\x01 AWP limit reached ({MaxAwpPerTeam} per team)!");
                return;
            }
        }

        // Remove current weapon of same type
        RemoveCurrentWeapon(player, weapon.Type);

        // Give new weapon
        player.GiveNamedItem(weapon.GiveName);

        // Increment AWP counter if AWP was given
        if (weapon.GiveName == "weapon_awp")
        {
            if (player.Team == CsTeam.Terrorist)
                _awpCountT++;
            else if (player.Team == CsTeam.CounterTerrorist)
                _awpCountCT++;
        }

        player.PrintToChat($" \x04[Gun Menu]\x01 You received: \x06{weapon.DisplayName}");
    }

    private void RemoveCurrentWeapon(CCSPlayerController player, WeaponType type)
    {
        var weapons = player.PlayerPawn?.Value?.WeaponServices?.MyWeapons;
        if (weapons == null)
            return;

        foreach (var weapon in weapons)
        {
            if (weapon?.Value?.DesignerName == null)
                continue;

            var currentWeapon = WeaponHelper.GetWeaponByGiveName(weapon.Value.DesignerName);
            if (currentWeapon != null && currentWeapon.Type == type)
            {
                // Decrease AWP counter if removing AWP
                if (weapon.Value.DesignerName == "weapon_awp")
                {
                    if (player.Team == CsTeam.Terrorist)
                        _awpCountT = Math.Max(0, _awpCountT - 1);
                    else if (player.Team == CsTeam.CounterTerrorist)
                        _awpCountCT = Math.Max(0, _awpCountCT - 1);
                }

                weapon.Value.Remove();
                break;
            }
        }
    }

    private bool ValidatePlayer(CCSPlayerController? player)
    {
        if (player == null || !player.IsValid || player.IsBot)
            return false;

        if (!player.PawnIsAlive)
        {
            player.PrintToChat(" \x02[Gun Menu]\x01 You must be alive to use this command!");
            return false;
        }

        if (player.Team is not (CsTeam.Terrorist or CsTeam.CounterTerrorist))
        {
            player.PrintToChat(" \x02[Gun Menu]\x01 You must be on a team to use this command!");
            return false;
        }

        return true;
    }
}
