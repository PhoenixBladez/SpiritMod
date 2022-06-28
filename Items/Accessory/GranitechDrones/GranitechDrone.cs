using System;
using Terraria;
using Terraria.GameContent;
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
            Main.projPet[Projectile.type] = true;
            Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 216000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (start)
			{
                timer = Projectile.ai[1] * 3f;
                start = false;
            }
            timer += 0.1f;

            if (tileTimer > 0)
                tileTimer--;

            if (shootTimer > 0)
                shootTimer--;

            Player owner = Main.player[Projectile.owner];
            MyPlayer modOwner = owner.GetModPlayer<MyPlayer>();

            if (owner.inventory[owner.selectedItem].pick > 0)
			{
                Projectile.ai[0] = 1f;
                currentMiningPower = owner.inventory[owner.selectedItem].pick;
            }
            else
                Projectile.ai[0] = 0f;

            if (Projectile.ai[0] == 0f) // ATTACKING
			{
                NPC target = Main.npc.Where(n => n.CanBeChasedBy(Projectile, false) && Vector2.Distance(n.Center, Projectile.Center) < attackRange).OrderBy(n => n.life / n.lifeMax).FirstOrDefault();

                if (target == default)
                    IdleMovement(owner);
                else
				{
                    var noise = GetNoise();

                    Vector2 targetPos = GetAnchorPoint(owner, new Vector2(-40, -36));
                    Vector2 dir = targetPos - Projectile.Center;
                    float length = dir.Length();
                    dir.Normalize();

                    Projectile.velocity *= 0.97f;
                    Projectile.velocity += dir * length * 0.01f;

                    Projectile.velocity += noise;

                    Projectile.spriteDirection = 1;
                    Projectile.rotation = (Projectile.rotation + (target.Center - Projectile.Center).ToRotation()) * 0.5f;

                    if (shootTimer <= 0)
                        Shoot(target);
                }
            }
            else if (Projectile.ai[0] == 1f) // MINING
			{
                if (Main.mouseLeft)
                {
                    var noise = GetNoise();

                    Vector2 targetPos = Main.MouseWorld - owner.Center;
                    targetPos.Normalize();
                    targetPos = -targetPos.RotatedBy((Projectile.ai[1] - 1f) * 0.7f);
                    targetPos = owner.Center + targetPos * 42f;

                    Vector2 dir = targetPos - Projectile.Center;
                    float length = dir.Length();
                    dir.Normalize();

                    Projectile.velocity *= 0.6f;
                    
                    Projectile.velocity += dir * (float)Math.Pow(length, 1.5) * 0.05f;

                    Projectile.velocity += noise * 0.5f;
                    Projectile.spriteDirection = 1;
                    Projectile.rotation = (Projectile.rotation + (Main.MouseWorld - Projectile.Center).ToRotation()) * 0.5f;

                    if (tileTimer <= 0)
                        Mine(owner);
                }
                else
                    IdleMovement(owner);
            }

            if (owner.dead)
                modOwner.granitechDrones = false;

            if (modOwner.granitechDrones)
                Projectile.timeLeft = 2;

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
            Vector2 dir = targetPos - Projectile.Center;

            float length = dir.Length();

            dir.Normalize();

            Projectile.velocity *= 0.97f;
            Projectile.velocity += dir * length * 0.01f;
            Projectile.velocity += noise * Math.Max(0, (80 - length)) / 80;
            Projectile.rotation = Projectile.velocity.ToRotation();
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

                        if (Main.tile[tile.X, tile.Y].HasTile)
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
            Vector2 delta = target.Center - Projectile.Center;
            float length = delta.Length();

            length -= Math.Min(target.width / 2, target.height / 2);

            delta.Normalize();
            delta *= length;

            Vector2 pos = Projectile.Center + delta;
            laserData.Add(new LaserData(pos, 8, AttackColor));

            shootTimer = 30 + Main.rand.Next(-5, 6);

            target.StrikeNPC(Projectile.damage, 0f, 0);
            //owner.ApplyDamageToNPC(target, projectile.damage, 0.1f, projectile.spriteDirection, false);
        }

		public override bool MinionContactDamage() => true;

		public override bool? CanCutTiles() => false;

		public override bool PreDraw(ref Color lightColor)
		{
            Color color = Color.White;

            if (Projectile.ai[0] == 0f) // ATTACK
                color = AttackColor;
            else if (Projectile.ai[0] == 1f) // MINING
                color = MiningColor;

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture), Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(16, 25), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SpiritMod/Items/Accessory/GranitechDrones/GranitechDroneModeGlowmask"), Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, new Vector2(16, 25), Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            
            foreach (var t in laserData)
			{
                Vector2 delta = t.endCoords - Projectile.Center;
                float length = delta.Length();
                float rotation = delta.ToRotation();
                Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 1, 1), t.color, rotation, new Vector2(0f, 1f), new Vector2(length, t.timeLeft), SpriteEffects.None, 0f);
            }
            return false;
		}
	}
}