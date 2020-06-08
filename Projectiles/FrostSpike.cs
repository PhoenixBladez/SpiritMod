using Microsoft.Xna.Framework;
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
using Terraria;
using Terraria.ModLoader;

using static SpiritMod.Items.Glyphs.FrostGlyph;

namespace SpiritMod.Projectiles
{
    class FrostSpike : ModProjectile
    {
        public static int _type;


        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ice Spike");
        }

        public override void SetDefaults() {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.height = 12;
            projectile.width = 12;
            projectile.tileCollide = false;
        }

        public float Offset {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public Vector2 Target =>
            new Vector2(-projectile.ai[0], -projectile.ai[1]);

        public override void AI() {
            Player player = Main.player[projectile.owner];
            if(player.active && Offset >= 0) {
                projectile.penetrate = 1;
                MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
                if(player.whoAmI == Main.myPlayer && modPlayer.glyph != GlyphType.Frost) {
                    projectile.Kill();
                    return;
                }

                projectile.timeLeft = 300;
                modPlayer.frostTally++;
                int count = modPlayer.frostCount;
                float sector = MathHelper.TwoPi / (count > 0 ? count : 1);
                float rotation = modPlayer.frostRotation + Offset * sector;
                if(rotation > MathHelper.TwoPi)
                    rotation -= MathHelper.TwoPi;
                float delta = projectile.rotation;
                if(delta > MathHelper.Pi)
                    delta -= MathHelper.TwoPi;
                else if(delta < -MathHelper.Pi)
                    delta += MathHelper.TwoPi;
                delta = rotation - delta;
                if(delta > MathHelper.Pi)
                    delta -= MathHelper.TwoPi;
                else if(delta < -MathHelper.Pi)
                    delta += MathHelper.TwoPi;
                if(delta > 1.5 * TURNRATE)
                    projectile.rotation += 1.5f * TURNRATE;
                else if(delta < .5 * TURNRATE)
                    projectile.rotation += 0.5f * TURNRATE;
                else
                    projectile.rotation = rotation;
                projectile.Center = player.MountedCenter + new Vector2(0, -OFFSET).RotatedBy(projectile.rotation);
                return;
            }
            //else if (Offset < 0)
            //{
            //	if (!projectile.velocity.Nearing(Target - projectile.Center))
            //	{
            //		projectile.position = Target;
            //		projectile.Kill();
            //		return;
            //	}
            //}

            if(projectile.localAI[1] == 0) {
                projectile.localAI[1] = 1;
                ProjectileExtras.LookAlongVelocity(this);
                projectile.penetrate = -1;
                projectile.extraUpdates = 1;
                projectile.tileCollide = true;
            }
        }

        public override void Kill(int timeLeft) {
            Player player = Main.player[projectile.owner];
            if(player.active && Offset >= 0)
                player.GetModPlayer<MyPlayer>().frostUpdate = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            Player player = Main.player[projectile.owner];
            if(player.active && Offset >= 0)
                hitDirection = target.position.X + (target.width >> 1) - player.position.X - (player.width >> 1) > 0 ? 1 : -1;
        }
    }
}
