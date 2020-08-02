using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Jobs.LowLevel.Unsafe;
using MetadataTesting.Functions;
using MetadataTesting.Variables;

namespace MetadataTesting.Commands
{
    class ItemBreakdown : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "itembreakdown";

        public string Help => "Print all info about the item in the inventory to console";

        public string Syntax => "ib or itembreakdown";

        public List<string> Aliases => new List<string> { "ib" };

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            Items inventory = new Items(7);
            inventory.resize(8, 8);
            
            inventory.onItemAdded = (byte page, byte index, ItemJar jar) => {
                ItemJar item2 = jar;
                player.Player.inventory.forceAddItem(jar.item, true);
                inventory.clear();
                if (jar.item.metadata.Length == 0)
                {
                }
                else if(jar.item.metadata.Length == 18)
                { //Weapon
                    WeaponMetadata weapon = Unturned.getWeapon(jar.item.metadata);
                    string temp = "\n";
                    string name = Unturned.nameFromID(jar.item.id);
                    if (name == "Unknown")
                    {
                        temp += jar.item.id.ToString() + "\n";
                    }
                    else
                    {
                        temp += name + "\n";
                    }
                    if (weapon.sight.ID != 0)
                    {
                        temp += $"Sight:    {Unturned.nameFromID(weapon.sight.ID)} ({weapon.sight.ID}) Durability: {weapon.sight.durability}\n";
                    }
                    if(weapon.tactical.ID != 0)
                    {
                        temp += $"Tactical: {Unturned.nameFromID(weapon.tactical.ID)} ({weapon.tactical.ID}) Durability: {weapon.tactical.durability}\n";
                        temp += $"Enabled:  {weapon.tactical.enabled}\n";
                    }
                    if(weapon.grip.ID != 0)
                    {
                        temp += $"Grip:     {Unturned.nameFromID(weapon.grip.ID)} ({weapon.grip.ID}) Durability: {weapon.grip.durability}\n";
                    }
                    if(weapon.barrel.ID != 0)
                    {
                        temp += $"Barrel:   {Unturned.nameFromID(weapon.barrel.ID)} ({weapon.barrel.ID}) Durability: {weapon.barrel.durability}\n";
                    }
                    if(weapon.ammo.ID != 0)
                    {
                        temp += $"Ammo:     {Unturned.nameFromID(weapon.ammo.ID)} ({weapon.ammo.ID}) Durability: {weapon.ammo.durability}\n";
                        temp += $"Loaded:   {weapon.ammo.ammoCount} shots.\n";
                    }
                    temp += $"Firerate: {weapon.firerate}\n";
                    temp += $"RAW:      {Bytes.bytesToCDString(weapon.Metadata())}\n";
                    Logger.Log(temp, ConsoleColor.Blue);
                } 
                else if (Unturned.dictItemType(jar.item.id) == Unturned.ItemType.Water)
                {
                    WaterItemMetadata water = Unturned.getWaterContainer(jar.item.metadata);
                    string temp = "\n";
                    string name = Unturned.nameFromID(jar.item.id);
                    if (name == "Unknown")
                    {
                        temp += jar.item.id.ToString() + "\n";
                    }
                    else
                    {
                        temp += name + "\n";
                    }
                    temp += $"Water Type: {water.WaterType.ToString()}\n";
                    temp += $"RAW:        {Bytes.bytesToCDString(water.Metadata())}\n";
                    Logger.Log(temp, ConsoleColor.Blue);
                } 
                else if (Unturned.dictItemType(jar.item.id) == Unturned.ItemType.Gas)
                {
                    GasItemMetadata gas = Unturned.getGasContainer(jar.item.metadata);
                    string temp = "\n";
                    string name = Unturned.nameFromID(jar.item.id);
                    if (name == "Unknown")
                    {
                        temp += jar.item.id.ToString() + "\n";
                    }
                    else
                    {
                        temp += name + "\n";
                    }
                    temp += $"Filled to: {gas.gasVolume.ToString()}\n";
                    temp += " Gas can has 500, Industrial gas can has 2500. \n";
                    temp += $"RAW:       {Bytes.bytesToCDString(gas.Metadata())}\n";
                    Logger.Log(temp, ConsoleColor.Blue);
                } 
                else if (Unturned.dictItemType(jar.item.id) == Unturned.ItemType.Togglable)
                {
                    TogglableItemMetadata tg = Unturned.getTogglable(jar.item.metadata);
                    string temp = "\n";
                    string name = Unturned.nameFromID(jar.item.id);
                    if (name == "Unknown")
                    {
                        temp += jar.item.id.ToString() + "\n";
                    }
                    else
                    {
                        temp += name + "\n";
                    }
                    temp += $"Enabled: {tg.enabled.ToString()}\n";
                    temp += $"RAW: {Bytes.bytesToCDString(tg.Metadata())}\n";
                    Logger.Log(temp, ConsoleColor.Blue);
                } 
                else
                {
                    OtherMetadata other = Unturned.getOther(jar.item.metadata);
                    string temp = "\n";
                    string name = Unturned.nameFromID(jar.item.id);
                    if (name == "Unknown")
                    {
                        temp += jar.item.id.ToString() + "\n";
                    }
                    else
                    {
                        temp += name + "\n";
                    }
                    temp += $"RAW: {Bytes.bytesToCDString(other.Metadata())}\n";
                    Logger.Log(temp, ConsoleColor.Blue);
                }
            };
            player.Player.inventory.updateItems(7, inventory);
            player.Player.inventory.sendStorage();
        }
    }

    class giveItem : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "give item";

        public string Help => "give an item using a metadata string";

        public string Syntax => "gi [metadata]";

        public List<string> Aliases => new List<string> { "gi" };

        public List<string> Permissions => new List<string> { };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length == 0)
            {
                UnturnedChat.Say(caller, "Enter an id and optionally metadata.", UnityEngine.Color.red);
            }
            else if (command.Length == 1)
            {
                Item item2 = new Item(ushort.Parse(command[0]), 1, 100)
                {
                    amount = 1,
                    quality = 100,
                    durability = 100
                };
                try
                {
                    player.Inventory.forceAddItem(item2, true);
                    UnturnedChat.Say(caller, $"Gave item {Unturned.nameFromID(ushort.Parse(command[0]))} ({ushort.Parse(command[0])})", UnityEngine.Color.green);
                }
                catch
                {
                    UnturnedChat.Say(caller, $"{command[0]} not found.", UnityEngine.Color.red);
                    return;
                }

                return;
            }
            Item item = new Item(ushort.Parse(command[0]), 1, 100)
            {
                metadata = Bytes.stringToByte(command[1]),
                state = Bytes.stringToByte(command[1]),
                amount = 1,
                quality = 100,
                durability = 100
            };
            player.Inventory.forceAddItem(item, true);
        }

        class giveMag : IRocketCommand
        {
            public AllowedCaller AllowedCaller => AllowedCaller.Player;

            public string Name => "give item";

            public string Help => "give an item using a metadata string";

            public string Syntax => "gm [id] [ammo count]";

            public List<string> Aliases => new List<string> { "gm" };

            public List<string> Permissions => new List<string> { };

            public void Execute(IRocketPlayer caller, string[] command)
            {
                UnturnedPlayer player = (UnturnedPlayer)caller;
                if (command.Length == 0)
                {
                    UnturnedChat.Say(caller, "Enter an id and optionally metadata.", UnityEngine.Color.red);
                }
                else if (command.Length == 1)
                {
                    Item item2 = new Item(ushort.Parse(command[0]), 1, 100)
                    {
                        amount = 1,
                        quality = 100,
                        durability = 100
                    };
                    try
                    {
                        player.Inventory.forceAddItem(item2, true);
                        UnturnedChat.Say(caller, $"Gave item {Unturned.nameFromID(ushort.Parse(command[0]))} ({ushort.Parse(command[0])})", UnityEngine.Color.green);
                    }
                    catch
                    {
                        UnturnedChat.Say(caller, $"{command[0]} not found.", UnityEngine.Color.red);
                        return;
                    }

                    return;
                }
                Item item = new Item(ushort.Parse(command[0]), 1, 100)
                {
                    amount = byte.Parse(command[1]),
                    quality = 100,
                    durability = 100
                };
                player.Inventory.forceAddItem(item, true);
            }

        }
    }

    class edit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "edit";

        public string Help => "Add custom attachments to guns";

        public string Syntax => "/edit <sight|tactical|grip|barrel|ammo> <attemptToAddID> [ammoCount (IF AMMO, max of 255)]";

        public List<string> Aliases => new List<string> { "e" };

        public List<string> Permissions => new List<string> { "" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            Items inventory = new Items(7);
            inventory.resize(8, 8);
            inventory.onItemAdded = (byte page, byte index, ItemJar jar) =>
            {
                WeaponMetadata weapon = Unturned.getWeapon(jar.item.metadata);
                if(command[1] == "-1")
                {
                    ulong ammoT = weapon.ammo.ID;
                } else
                {
                    ulong ammoT = ushort.Parse(command[1]);
                }
                switch (command[0])
                {
                    case "sight":
                        weapon.sight = new Sight
                        {
                            ID = ushort.Parse(command[1]),
                            durability = 100
                        };
                        break;
                    case "tactical":
                        weapon.tactical = new Tactical
                        {
                            ID = ushort.Parse(command[1]),
                            durability = 100,
                            enabled = weapon.tactical.enabled
                        };
                        break;
                    case "grip":
                        weapon.grip = new Grip
                        {
                            ID = ushort.Parse(command[1]),
                            durability = 100
                        };
                        break;
                    case "barrel":
                        weapon.barrel = new Barrel
                        {
                            ID = ushort.Parse(command[1]),
                            durability = 100
                        };
                        break;
                    case "ammo":
                        weapon.ammo = new Ammo
                        {
                            ID = ushort.Parse(command[1]),
                            durability = 100,
                            ammoCount = byte.Parse(command[2])
                        };
                        break;
                    case "firerate":
                        weapon.firerate = (Unturned.FireRate)byte.Parse(command[1]);
                        break;

                }
                player.Inventory.forceAddItem(new Item(jar.item.id, jar.item.amount, jar.item.quality, weapon.Metadata()), true);
            };
            player.Player.inventory.updateItems(7, inventory);
            player.Player.inventory.sendStorage();
        }
    }
};
