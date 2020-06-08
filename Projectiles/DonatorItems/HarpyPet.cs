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
using SpiritMod.Items.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
    class HarpyPet : ModProjectile
    {
        public static readonly int _type;

        private const float FOV = (float)System.Math.PI / 2;
        private const float Max_Range = 16 * 30;
        private const float Spread = (float)System.Math.PI / 9;
        private const int Damage = 15;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Mini Harpy");
            Main.projFrames[projectile.type] = 3;
            Main.projPet[projectile.type] = true;
        }

        public override void SetDefaults() {
            projectile.netImportant = true;
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = 26;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft *= 5;
            aiType = ProjectileID.BabyHornet;
        }

        private float Timer {
            get { return projectile.localAI[1]; }
            set { projectile.localAI[1] = value; }
        }

        private int animationCounter;
        private int frame;
        public override void AI() {
            if(++animationCounter >= 5) {
                animationCounter = 0;
                if(++frame >= Main.projFrames[_type])
                    frame = 0;
            }
            projectile.frameCounter = 0;
            projectile.frame = frame;

            var owner = Main.player[projectile.owner];
            if(owner.active && owner.HasBuff(HarpyPetBuff._type))
                projectile.timeLeft = 2;

            if(projectile.owner != Main.myPlayer)
                return;

            if(Timer > 0) {
                --Timer;
                return;
            }

            float direction;
            if(projectile.direction < 0)
                direction = FOVHelper.POS_X_DIR + projectile.rotation;
            else
                direction = FOVHelper.NEG_X_DIR - projectile.rotation;

            var origin = projectile.Center;
            var fov = new FOVHelper();
            fov.AdjustCone(origin, FOV, direction);
            float maxDistSquared = Max_Range * Max_Range;
            for(int i = 0; i < Main.maxNPCs; ++i) {
                NPC npc = Main.npc[i];
                Vector2 npcPos = npc.Center;
                if(npc.CanBeChasedBy() &&
                    fov.IsInCone(npcPos) &&
                    Vector2.DistanceSquared(origin, npcPos) < maxDistSquared &&
                    Collision.CanHitLine(origin, 0, 0, npc.position, npc.width, npc.height)) {
                    ShootFeathersAt(npcPos);
                    Timer = 140;
                    break;
                }
            }
        }

        private void ShootFeathersAt(Vector2 target) {
            var origin = projectile.Center;
            var direction = target - origin;
            direction = direction.SafeNormalize(Vector2.UnitX);
            direction *= 3f;
            Projectile.NewProjectile(origin, direction.RotatedBy(Spread), HarpyPetFeather._type, Damage, 0, projectile.owner);
            Projectile.NewProjectile(origin, direction.RotatedBy(-Spread), HarpyPetFeather._type, Damage, 0, projectile.owner);
            Projectile.NewProjectile(origin, direction, HarpyPetFeather._type, Damage, 0, projectile.owner);
        }
    }
}
