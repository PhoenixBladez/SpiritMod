using SpiritMod.Items;
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
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
    public class gProj : GlobalProjectile
    {
        private static bool hasNext = false;
        private static int nextType;
        private static GlyphType nextGlyph;

        private GlyphType glyph;
        public GlyphType Glyph => glyph;

        public override bool InstancePerEntity => true;


        public override void SetDefaults(Projectile projectile) {
            bool send = true;
            if(MyPlayer.swingingCheck && MyPlayer.swingingItem != null)
                glyph = MyPlayer.swingingItem.GetGlobalItem<GItem>().Glyph;
            else if(Main.ProjectileUpdateLoopIndex >= 0) {
                Projectile source = Main.projectile[Main.ProjectileUpdateLoopIndex];
                if(source.active && source.owner == Main.myPlayer)
                    glyph = source.GetGlobalProjectile<gProj>().glyph;
            } else if(hasNext) {
                send = false;
                hasNext = false;
                if(projectile.type == nextType)
                    glyph = nextGlyph;
            } else
                glyph = 0;

            if(send && glyph != 0 && Main.netMode != 0) {
                ModPacket packet = SpiritMod.instance.GetPacket(MessageType.ProjectileData, 3);
                packet.Write((short)projectile.type);
                packet.Write((byte)glyph);
                packet.Send();
            }
        }

        internal static void ReceiveProjectileData(BinaryReader reader, int sender) {
            hasNext = true;
            nextType = reader.ReadInt16();
            nextGlyph = (GlyphType)reader.ReadByte();
            if(Main.netMode != 2)
                return;

            ModPacket packet = SpiritMod.instance.GetPacket(MessageType.ProjectileData, 3);
            packet.Write((short)nextType);
            packet.Write((byte)nextGlyph);
            packet.Send(-1, sender);
        }


        public override bool? CanHitNPC(Projectile projectile, NPC target) {
            if(projectile.aiStyle == 88 && ((projectile.knockBack == .5f || projectile.knockBack == .4f) || (projectile.knockBack >= .4f && projectile.knockBack < .5f)) && target.immune[projectile.owner] > 0) {
                return false;
            }
            return null;
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            Player player = Main.player[projectile.owner];
            MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
            if(glyph == GlyphType.Unholy)
                Items.Glyphs.UnholyGlyph.PlagueEffects(target, projectile.owner, ref damage, crit);
            else if(glyph == GlyphType.Phase)
                Items.Glyphs.PhaseGlyph.PhaseEffects(player, ref damage, crit);
            else if(glyph == GlyphType.Daze)
                Items.Glyphs.DazeGlyph.Daze(target, ref damage);
            else if(glyph == GlyphType.Radiant)
                Items.Glyphs.RadiantGlyph.DivineStrike(player, ref damage);

            if(modPlayer.reachSet && target.life <= target.life / 2) {
                if(projectile.thrown && crit)
                    damage = (int)((double)damage * 2.25f);
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
            Player player = Main.player[projectile.owner];

            switch(glyph) {
                case GlyphType.Frost:
                    Items.Glyphs.FrostGlyph.CreateIceSpikes(player, target, crit);
                    break;
                case GlyphType.Sanguine:
                    Items.Glyphs.SanguineGlyph.BloodCorruption(Main.player[projectile.owner], target, damage);
                    break;
                case GlyphType.Blaze:
                    Items.Glyphs.BlazeGlyph.Rage(player, target);
                    break;
                case GlyphType.Bee:
                    if(projectile.type != ProjectileID.Bee && projectile.type != ProjectileID.GiantBee)
                        Items.Glyphs.BeeGlyph.ReleaseBees(player, target, damage);
                    break;
            }

            if(projectile.aiStyle == 88 && (projectile.knockBack >= .2f && projectile.knockBack <= .5f)) {
                target.immune[projectile.owner] = 6;
            }
            if(projectile.friendly && projectile.thrown && Main.rand.Next(8) == 1 && player.GetSpiritPlayer().geodeSet == true) {
                target.AddBuff(24, 150);
            }
            if(projectile.friendly && projectile.thrown && Main.rand.Next(8) == 1 && player.GetSpiritPlayer().geodeSet == true) {
                target.AddBuff(44, 150);
            }
        }

        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit) {
            Player player = Main.player[projectile.owner];
            if(glyph == GlyphType.Phase)
                Items.Glyphs.PhaseGlyph.PhaseEffects(player, ref damage, crit);
            else if(glyph == GlyphType.Daze)
                Items.Glyphs.DazeGlyph.Daze(target, ref damage);
            else if(glyph == GlyphType.Radiant)
                Items.Glyphs.RadiantGlyph.DivineStrike(player, ref damage);
        }

        public override void OnHitPvp(Projectile projectile, Player target, int damage, bool crit) {
            Player player = Main.player[projectile.owner];
            switch(glyph) {
                case GlyphType.Sanguine:
                    Items.Glyphs.SanguineGlyph.BloodCorruption(Main.player[projectile.owner], target, damage);
                    break;
                case GlyphType.Blaze:
                    Items.Glyphs.BlazeGlyph.Rage(player);
                    break;
            }
        }


        public override void AI(Projectile projectile) {//todo - forking lightning in Kill(), kill projectile when far from player in AI(), homing in OnHitNPC()
            if(projectile.aiStyle == 88 && projectile.knockBack == .5f || (projectile.knockBack >= .2f && projectile.knockBack < .5f)) {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.magic = true;
                projectile.penetrate = -1;
            }
        }

    }
}
