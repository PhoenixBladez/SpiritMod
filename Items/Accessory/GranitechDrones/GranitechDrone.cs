using System;
using Terraria;
using Terraria.ID;
using System.Linq;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Accessory.GranitechDrones
{
    class LaserData
	{
        public Vector2 endCoords;
        public int timeLeft;
        public Color color;

		public LaserData(Vector2 endCoords, int timeLeft, Color color)
		{
			this.endCoords = endCoords;
			this.timeLeft = timeLeft;
            this.color = color;
		}
	}

    public class GranitechDrone : ModProjectile
    {
        const float attackRange = 800;
        private readonly Color AttackColor = new Color(255, 34, 65);
        private readonly Color MiningColor = Color.Cyan;
		readonly List<LaserData> laserData = new List<LaserData>();

		float timer = 0;
        int tileTimer = 0;
        int shootTimer = 0;
        bool start = true;

        int currentMiningPower = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granitech Drone");
            Main.projPet[projectile.type] = true;
            Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 42;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 0;
            projectile.penetrate = -1;
            projectile.timeLeft = 216000;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (start)
			{
                timer = projectile.ai[1] * 3f;
                start = false;
            }
            timer += 0.1f;

            if (tileTimer > 0)
                tileTimer--;

            if (shootTimer > 0)
                shootTimer--;

            Player owner = Main.player[projectile.owner];
            MyPlayer modOwner = owner.GetModPlayer<MyPlayer>();

            if (owner.inventory[owner.selectedItem].pick > 0)
			{
                projectile.ai[0] = 1f;
                currentMiningPower = owner.inventory[owner.selectedItem].pick;
            }
            else
                projectile.ai[0] = 0f;

            if (projectile.ai[0] == 0f) // ATTACKING
			{
                NPC target = Main.npc.Where(n => n.CanBeChasedBy(projectile, false) && Vector2.Distance(n.Center, projectile.Center) < attackRange).OrderBy(n => n.life / n.lifeMax).FirstOrDefault();

                if (target == default)
                    IdleMovement(owner);
                else
				{
                    var noise = GetNoise();

                    Vector2 targetPos = GetAnchorPoint(owner, new Vector2(-40, -36));
                    Vector2 dir = targetPos - projectile.Center;
                    float length = dir.Length();
                    dir.Normalize();

                    projectile.velocity *= 0.97f;
                    projectile.velocity += dir * length * 0.01f;

                    projectile.velocity += noise;

                    projectile.spriteDirection = 1;
                    projectile.rotation = (projectile.rotation + (target.Center - projectile.Center).ToRotation()) * 0.5f;

                    if (shootTimer <= 0)
                        Shoot(target);
                }
            }
            else if (projectile.ai[0] == 1f) // MINING
			{
                if (Main.mouseLeft)
                {
                    var noise = GetNoise();

                    Vector2 targetPos = Main.MouseWorld - owner.Center;
                    targetPos.Normalize();
                    targetPos = -targetPos.RotatedBy((projectile.ai[1] - 1f) * 0.7f);
                    targetPos = owner.Center + targetPos * 42f;

                    Vector2 dir = targetPos - projectile.Center;
                    float length = dir.Length();
                    dir.Normalize();

                    projectile.velocity *= 0.6f;
                    
                    projectile.velocity += dir * (float)Math.Pow(length, 1.5) * 0.05f;

                    projectile.velocity += noise * 0.5f;
                    projectile.spriteDirection = 1;
                    projectile.rotation = (projectile.rotation + (Main.MouseWorld - projectile.Center).ToRotation()) * 0.5f;

                    if (tileTimer <= 0)
                        Mine(owner);
                }
                else
                    IdleMovement(owner);
            }

            if (owner.dead)
                modOwner.granitechDrones = false;

            if (modOwner.granitechDrones)
                projectile.timeLeft = 2;

            foreach (var t in laserData.ToList())
            {
                t.timeLeft--;

                if (t.timeLeft <= 0)
                    laserData.Remove(t);
            }
        }

		private Vector2 GetNoise() => new Vector2((float)Math.Cos(timer * 0.78f), (float)Math.Sin(timer * 1.06f)) * 0.1f;

		private void IdleMovement(Player owner)
		{
            var noise = GetNoise();

            Vector2 targetPos = GetAnchorPoint(owner, new Vector2(-40, -28));
            Vector2 dir = targetPos - projectile.Center;

            float length = dir.Length();

            dir.Normalize();

            projectile.velocity *= 0.97f;
            projectile.velocity += dir * length * 0.01f;
            projectile.velocity += noise * Math.Max(0, (80 - length)) / 80;
            projectile.rotation = projectile.velocity.ToRotation();
        }

        private Vector2 GetAnchorPoint(Player player, Vector2 offset)
		{
            Vector2 v = player.Center + new Vector2(0, offset.Y);
            v += new Vector2(offset.X, 0) * player.direction;
            return v;
		}

        private void Mine(Player owner)
		{
            var dir = Vector2.Normalize(Main.MouseWorld - owner.Center);

            var center = owner.Center;
            bool stop = false;

            for (int n = 0; n < 5; n++)
			{
                for (int k = -1; k < 2; k++)
				{
                    for(int l = -1; l < 2; l++)
					{
                        Point tile = new Point((int)center.X / 16 + k, (int)center.Y / 16 + l);

                        if (Main.tile[tile.X, tile.Y].active())
                        {
                            owner.PickTile(tile.X, tile.Y, currentMiningPower);

                            laserData.Add(new LaserData(tile.ToVector2() * 16, 6, MiningColor));
                            tileTimer = 25 + Main.rand.Next(-5, 6);

                            stop = true;
                            break;
                        }
                    }
                    if (stop) break;
                }
                if (stop) break;

                center += dir * 32f;
			}
        }

        private void Shoot(NPC target)
		{
            Vector2 delta = target.Center - projectile.Center;
            float length = delta.Length();

            length -= Math.Min(target.width / 2, target.height / 2);

            delta.Normalize();
            delta *= length;

            Vector2 pos = projectile.Center + delta;
            laserData.Add(new LaserData(pos, 8, AttackColor));

            shootTimer = 30 + Main.rand.Next(-5, 6);

            target.StrikeNPC(projectile.damage, 0f, 0);
            //owner.ApplyDamageToNPC(target, projectile.damage, 0.1f, projectile.spriteDirection, false);
        }

		public override bool MinionContactDamage() => true;

		public override bool? CanCutTiles() => false;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            Color color = Color.White;

            if (projectile.ai[0] == 0f) // ATTACK
                color = AttackColor;
            else if (projectile.ai[0] == 1f) // MINING
                color = MiningColor;

            spriteBatch.Draw(ModContent.GetTexture(Texture), projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(16, 25), projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(ModContent.GetTexture("SpiritMod/Items/Accessory/GranitechDrones/GranitechDroneModeGlowmask"), projectile.Center - Main.screenPosition, null, color, projectile.rotation, new Vector2(16, 25), projectile.scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            
            foreach (var t in laserData)
			{
                Vector2 delta = t.endCoords - projectile.Center;
                float length = delta.Length();
                float rotation = delta.ToRotation();
                Main.spriteBatch.Draw(Main.magicPixel, projectile.Center - Main.screenPosition, new Rectangle(0, 0, 1, 1), t.color, rotation, new Vector2(0f, 1f), new Vector2(length, t.timeLeft), SpriteEffects.None, 0f);
            }
            return false;
		}
	}
}