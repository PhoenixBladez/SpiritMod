using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Prim;
using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.GreatswordSubclass
{
    public class RagingSunblade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helios");
            SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 180;
            item.width = 60;
            item.height = 60;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Red;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<RagingSunbladeProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
			{
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<RagingSunbladeProj>())
				{
					return false;
				}
			}
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => 
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, item, mod.GetTexture(Texture.Remove(0, "SpiritMod/".Length) + "_glow"), rotation, scale);
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentSolar, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class RagingSunbladeProj : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios");
        }

        public override void SetDefaults()
		{
			projectile.width = 45;
			projectile.height = 45;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.aiStyle = -1;
		}
        float growCounter = 20;
        double radians = 0;
        bool released = false;
		bool returned = false;
        bool primsCreated = false;

		int charge;
		float flickerTime = 0;
        SolarSwordPrimTrail trail;

		Projectile phantomProj;

		public Vector2 primCenter = Vector2.Zero;
        public override void AI()
        {
			projectile.timeLeft = 2;
            Player player = Main.player[projectile.owner];
            player.heldProj = projectile.whoAmI;
            if (!primsCreated)
            {
                trail = new SolarSwordPrimTrail(projectile);
                SpiritMod.primitives.CreateTrail(trail);
                primsCreated = true;
			}
			if (charge >= 60)
			{
				if (flickerTime == 0)
					Main.PlaySound(SoundID.NPCDeath7, projectile.Center);
				flickerTime += 0.05f;
				flickerTime = Math.Min(flickerTime, 1);
			}
			if (!released && player.channel)
			{
				primCenter = player.Center;
				charge++;
				//CheckCollision(player);

			}
			else if (!returned)
			{
				if (charge > 60)
				{
					if (phantomProj == null)
						phantomProj = Projectile.NewProjectileDirect(player.Center, player.DirectionTo(Main.MouseWorld) * 15, ModContent.ProjectileType<HeliosPhantomProj>(), projectile.damage, 0, player.whoAmI);
					primCenter = phantomProj.Center;
					if (!phantomProj.active)
						returned = true;
				}
				else
					returned = true;
				if (!released)
				{
					released = true;
				}
			}
			else
			{
				if (player.GetModPlayer<MyPlayer>().AnimeSword)
				{
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
					player.velocity = Vector2.Zero;
				}
				primCenter = player.Center;
			}
            Vector2 direction = Main.MouseWorld - player.position;
            direction.Normalize();
            projectile.scale = MathHelper.Clamp((growCounter - 20) / 30f, 0, 1);

            SpinBlade(0.3, 0.1);

			if (!released || returned)
			{
				player.itemAnimation -= 14;

				while (player.itemAnimation < 3)
				{
					Main.PlaySound(SoundID.Item, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 0.5f, 0.5f);
					player.itemAnimation += 320;
				}
			}
			else if (player.itemAnimation > 16)
			{
				player.itemAnimation -= 14;
			}
			else
				player.itemAnimation = 2;
			player.itemTime = player.itemAnimation;

			if (growCounter <= 0)
            {
                projectile.active = false;
                player.itemTime = player.itemAnimation = 2;
            }

            trail._direction = player.direction;
        }

        private void CheckCollision(Player player) //I'm sorry forthis
        {
            int pX = (int)(primCenter.X / 16f);
            int pY = (int)(primCenter.Y / 16f);

            List<(float, float)> collectedAngles = new List<(float, float)>(); //angles hit at

            for (int i = pX - 10; i < pX + 10; ++i)
            {
                for (int j = pY - 10; j < pY + 10; ++j)
                {
                    float dist = Vector2.Distance((new Vector2(i, j) * 16) + new Vector2(8), primCenter);
                    if (dist < 150 * (growCounter / 60f) && Framing.GetTileSafely(i, j).active() && Main.tileSolid[Framing.GetTileSafely(i, j).type])
                        collectedAngles.Add((Vector2.Normalize(primCenter - (new Vector2(i, j) * 16)).ToRotation(), dist)); //collects angles
                }
            }

            collectedAngles.Sort(); //sort angles

            if (collectedAngles.Count > 0) //oh no
            {
                float angle = 0f;
                float adjAngle = 0;
                int index = collectedAngles.Count - 1;
                do
                {
                    if (index < 0)
                        return; //return if there's no valid angle
                    angle = collectedAngles[index].Item1;
                    adjAngle = angle += (float)(angle >= 0 ? Math.PI : -Math.PI);
                    if (adjAngle > Math.PI)
                        adjAngle = -(adjAngle - (float)Math.PI);
                    if (adjAngle < Math.PI)
                        adjAngle = -(adjAngle + (float)Math.PI);
                    index--; //adjusts angles; checks if there's an angle similar to it but opposite and denies the movement if there is
                } while (collectedAngles.Any(x => x.Item1 > adjAngle - MathHelper.PiOver2 && x.Item1 < adjAngle + MathHelper.PiOver2)); //god help me

                player.velocity = new Vector2(1, 0).RotatedBy(angle + Math.PI) * 20; //set the player's position
            }
        }

        private void SpinBlade(double d1, double adder, bool grow = true)
        {
            Player player = Main.player[projectile.owner];
            for (double i = 0; i < d1; i+= adder)
            {
				if (player.direction == 1)
					radians += adder;
				else
					radians -= adder;

				if (radians > 6.28)
					radians -= 6.28;
				if (radians < -6.28)
					radians += 6.28;

				if (grow)
				{
					if (growCounter < 60 && !released)
						growCounter += (float)(adder / d1) * 1.5f;
					if (released && returned)
						growCounter -= (float)(adder / d1);
				}
				if (player.channel)
					projectile.Center = primCenter + ((float)radians + 3.14f).ToRotationVector2() * 110;
				else
					projectile.Center = primCenter + ((float)radians + 3.14f).ToRotationVector2() * 11 * growCounter / 6f;
				trail.Points.Add(projectile.Center - primCenter);

				if (grow)
				{
					if (Main.rand.Next(4) == 0)
					{
						Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(projectile.width / 4, projectile.height / 4), projectile.width / 2, projectile.height / 2, DustID.Fire, ((float)radians + 3.14f).ToRotationVector2().X * growCounter / 6f, ((float)radians + 3.14f).ToRotationVector2().Y * growCounter / 6f);
						dust.scale = 1.5f;
						dust.noGravity = true;
						dust = Dust.NewDustDirect(primCenter - (((float)radians + 3.14f).ToRotationVector2() * 10 * growCounter / 6f) - new Vector2(projectile.width / 4, projectile.height / 4), projectile.width / 2, projectile.height / 2, DustID.Fire, ((float)radians + 3.14f).ToRotationVector2().X * growCounter / -6f, ((float)radians + 3.14f).ToRotationVector2().Y * growCounter / -2f);
						dust.scale = 1.5f;
						dust.noGravity = true;
					}
				}
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockBack, ref bool crit, ref int hitDirection) => target.AddBuff(189, 180);

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
			float collisionPoint = 0f;
			for (float i = 0; i < 6.28f; i++)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), primCenter, primCenter + (((float)radians - i).ToRotationVector2() * 110), projectile.width, ref collisionPoint))
					return true;
			}
			return projHitbox.Intersects(targetHitbox);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = lightColor;
            Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], primCenter - Main.screenPosition, null, Color.White, (float)radians + 3.9f, new Vector2(Main.projectileTexture[projectile.type].Width / 2, Main.projectileTexture[projectile.type].Height / 2), projectile.scale, SpriteEffects.None, 0);
			if (charge >= 60)
			{
				float transparency = (float)Math.Pow(1 - flickerTime, 2);
				float scale = 1 + (flickerTime / 2.0f);
				Texture2D whiteTex = ModContent.GetTexture(Texture + "_White");
				Main.spriteBatch.Draw(whiteTex, primCenter - Main.screenPosition, null, Color.White * transparency, (float)radians + 3.9f, new Vector2(Main.projectileTexture[projectile.type].Width / 2, Main.projectileTexture[projectile.type].Height / 2), projectile.scale * scale, SpriteEffects.None, 0);
			}
			return false;
		}
    }
	public class HeliosPhantomProj : ModProjectile
	{

		int pauseTimer = 40;

		bool paused = false;

		bool pausedBefore = false;

		Vector2 oldVel = Vector2.Zero;

		Player player => Main.player[projectile.owner];

		private bool dashing = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = 3;
			projectile.friendly = false;
			projectile.melee = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 700;
			projectile.tileCollide = false;
			projectile.extraUpdates = 2;
			projectile.hide = true;
		}

		public override bool PreAI()
		{
			if (Main.mouseRight && !dashing && player == Main.LocalPlayer)
			{
				Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/slashdash").WithPitchVariance(0.4f).WithVolume(0.4f), player.Center);
				dashing = true;
				player.GetModPlayer<MyPlayer>().AnimeSword = true;
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<HeliosDash>(), projectile.damage * 3, 0, player.whoAmI);
			}
			if (dashing)
			{
				player.velocity = player.DirectionTo(projectile.Center) * 80;
				if (player.Distance(projectile.Center) < 100)
				{
					player.Center = projectile.Center;
					player.velocity = Vector2.Zero;
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
				}
			}
			if (!paused && projectile.velocity.Length() <= 3 && !pausedBefore)
			{
				paused = true;
				oldVel = projectile.velocity;
			}
			if (paused)
			{
				pausedBefore = true;
				projectile.velocity *= 0.99f;
				pauseTimer--;
				if (pauseTimer <= 0)
				{
					projectile.velocity = oldVel;
					paused = false;
				}
				return false;
			}
			return true;
		}
	}
	public class HeliosDash : ModProjectile
	{
		private Player player => Main.player[projectile.owner];
		public override void SetStaticDefaults() => DisplayName.SetDefault("Helios");
		public override void SetDefaults()
		{
			projectile.width = projectile.height = 180;
			projectile.hostile = false;
			projectile.melee = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = true;
		}
		public override void AI()
		{
			projectile.Center = player.Center;
			if (!player.GetModPlayer<MyPlayer>().AnimeSword)
				projectile.active = false;
			else
			{
				for (int i = 0; i < 3; i++)
				{
					ImpactLine line = new ImpactLine(projectile.Center + Main.rand.NextVector2Circular(90, 90), Vector2.Normalize(player.velocity) * 0.5f, Color.Lerp(Color.Orange, Color.OrangeRed, Main.rand.NextFloat()), new Vector2(0.25f, Main.rand.NextFloat(0.5f, 1.5f)) * 5, 60);
					line.TimeActive = 30;
					ParticleHandler.SpawnParticle(line);
				}
			}
		}
	}
}