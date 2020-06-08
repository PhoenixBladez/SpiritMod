using Terraria;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class CollapsingVoid : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Collapsing Void");
            Description.SetDefault("");
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            if(time >= 60) {
                MyPlayer modPlayer = player.GetSpiritPlayer();
                if(modPlayer.voidStacks < 3) {
                    modPlayer.voidStacks++;
                }

                Main.buffNoTimeDisplay[Type] = false;
            }

            return false;
        }

        public override void Update(Player player, ref int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            player.endurance += modPlayer.voidStacks * 0.05f;

            if(modPlayer.voidStacks > 1 && player.buffTime[buffIndex] <= 2) {
                modPlayer.voidStacks--;
                player.buffTime[buffIndex] = 299;
            }

            if(modPlayer.voidStacks <= 1) {
                Main.buffNoTimeDisplay[Type] = true;
            }

            if(player.whoAmI == Main.myPlayer && !Main.dedServ) {
                if(modPlayer.voidStacks == 0) {
                    Main.buffTexture[Type] = mod.GetTexture("CollapsingVoid");
                } else {
                    Main.buffTexture[Type] = mod.GetTexture("CollapsingVoid_" + modPlayer.voidStacks.ToString());
                }
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare) {
            MyPlayer modPlayer = Main.LocalPlayer.GetSpiritPlayer();
            tip = $"Damage taken is reduced by {modPlayer.voidStacks * 5}%";
            rare = modPlayer.voidStacks >> 1;
        }
    }
}
