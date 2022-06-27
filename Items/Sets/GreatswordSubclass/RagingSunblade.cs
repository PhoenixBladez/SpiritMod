using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Enums;
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
			Tooltip.SetDefault("Hold and release to throw \nRight click to dash to it, destroying everything in your path");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 180;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 8;
            Item.useTurn = false;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<RagingSunbladeProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override bool CanUseItem(Player player)
		{
			for (int i = 0; i < 1000; ++i)
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<RagingSunbladeProj>())
					return false;
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => 
            GlowmaskUtils.DrawItemGlowMaskWorld(Main.spriteBatch, Item, Mod.Assets.Request<Texture2D>(Texture.Remove(0, "SpiritMod/".Length) + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentSolar, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
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
			Projectile.width = 45;
			Projectile.height = 45;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.aiStyle = -1;
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
			Projectile.timeLeft = 2;
            Player player = Main.player[Projectile.owner];
            player.heldProj = Projectile.whoAmI;
            if (!primsCreated)
            {
                trail = new SolarSwordPrimTrail(Projectile);
                SpiritMod.primitives.CreateTrail(trail);
                primsCreated = true;
			}
			if (charge >= 60)
			{
				if (flickerTime == 0)
					SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);
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
						phantomProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), player.Center, player.DirectionTo(Main.MouseWorld) * 15, ModContent.ProjectileType<HeliosPhantomProj>(), Projectile.damage, 0, player.whoAmI);
					primCenter = phantomProj.Center;
					if (!phantomProj.active || !(phantomProj.ModProjectile is HeliosPhantomProj))
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
            Projectile.scale = MathHelper.Clamp((growCounter - 20) / 30f, 0, 1);

            SpinBlade(0.3, 0.1);

			if (!released || returned)
			{
				player.itemAnimation -= 14;

				while (player.itemAnimation < 3)
				{
					SoundEngine.PlaySound(SoundID.Item, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 0.5f, 0.5f);
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
                Projectile.active = false;
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
                    if (dist < 150 * (growCounter / 60f) && Framing.GetTileSafely(i, j).HasTile && Main.tileSolid[Framing.GetTileSafely(i, j).TileType])
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
            Player player = Main.player[Projectile.owner];
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
					Projectile.Center = primCenter + ((float)radians + 3.14f).ToRotationVector2() * 110;
				else
					Projectile.Center = primCenter + ((float)radians + 3.14f).ToRotationVector2() * 11 * growCounter / 6f;
				trail.Points.Add(Projectile.Center - primCenter);

				if (grow)
				{
					if (Main.rand.Next(4) == 0)
					{
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(Projectile.width / 4, Projectile.height / 4), Projectile.width / 2, Projectile.height / 2, DustID.Torch, ((float)radians + 3.14f).ToRotationVector2().X * growCounter / 6f, ((float)radians + 3.14f).ToRotationVector2().Y * growCounter / 6f);
						dust.scale = 1.5f;
						dust.noGravity = true;
						dust = Dust.NewDustDirect(primCenter - (((float)radians + 3.14f).ToRotationVector2() * 10 * growCounter / 6f) - new Vector2(Projectile.width / 4, Projectile.height / 4), Projectile.width / 2, Projectile.height / 2, DustID.Torch, ((float)radians + 3.14f).ToRotationVector2().X * growCounter / -6f, ((float)radians + 3.14f).ToRotationVector2().Y * growCounter / -2f);
						dust.scale = 1.5f;
						dust.noGravity = true;
					}
				}
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.AddBuff(189, 180);
			hitDirection = Math.Sign(target.Center.X - primCenter.X);
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[Projectile.owner];
			float collisionPoint = 0f;
			for (float i = 0; i < 6.28f; i++)
			{
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), primCenter, primCenter + (((float)radians - i).ToRotationVector2() * 110), Projectile.width, ref collisionPoint))
				{
					Vector2 position = Vector2.Lerp(primCenter, primCenter + (((float)radians - i).ToRotationVector2() * 110), collisionPoint / 110.0f);
					SoundEngine.PlaySound(SoundID.Item14, position);
					Projectile.NewProjectile(Projectile.GetSource_FromThis("TargetlessOnHit"), position, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, 0, 0, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
					return true;
				}
			}
			if (projHitbox.Intersects(targetHitbox))
			{
				Vector2 position = Projectile.Center;
				SoundEngine.PlaySound(SoundID.Item14, position);
				Projectile.NewProjectile(Projectile.GetSource_FromThis("TargetlessOnHit"), position, Vector2.Zero, ProjectileID.SolarWhipSwordExplosion, 0, 0, Projectile.owner, 0f, 0.85f + Main.rand.NextFloat() * 1.15f);
				return true;
			}
			return false;
        }

		public override bool? CanCutTiles() => true;

		// Plot a line from the start of the Solar Eruption to the end of it, to change the tile-cutting collision logic. (Don't change this.)
		public override void CutTiles()
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			for (float i = 0; i < 6.28f; i++)
			{
				Utils.PlotTileLine(primCenter, primCenter + (((float)radians - i).ToRotationVector2() * 110), Projectile.width, DelegateMethods.CutTiles);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Color color = lightColor;
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, primCenter - Main.screenPosition, null, Color.White, (float)radians + 3.9f, new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2, TextureAssets.Projectile[Projectile.type].Value.Height / 2), Projectile.scale, SpriteEffects.None, 0);
			if (charge >= 60)
			{
				float transparency = (float)Math.Pow(1 - flickerTime, 2);
				float scale = 1 + (flickerTime / 2.0f);
				Texture2D whiteTex = ModContent.Request<Texture2D>(Texture + "_White", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				Main.spriteBatch.Draw(whiteTex, primCenter - Main.screenPosition, null, Color.White * transparency, (float)radians + 3.9f, new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width / 2, TextureAssets.Projectile[Projectile.type].Value.Height / 2), Projectile.scale * scale, SpriteEffects.None, 0);
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

		Player player => Main.player[Projectile.owner];

		private bool dashing = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = 3;
			Projectile.friendly = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 700;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.hide = true;
		}

		public override bool PreAI()
		{
			if (Main.mouseRight && !dashing && player == Main.LocalPlayer)
			{
				SoundEngine.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/slashdash").WithPitchVariance(0.4f).WithVolume(0.4f), player.Center);
				dashing = true;
				player.GetModPlayer<MyPlayer>().AnimeSword = true;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<HeliosDash>(), Projectile.damage * 3, 0, player.whoAmI);
			}

			if (dashing)
			{
				player.velocity = player.DirectionTo(Projectile.Center) * 80;
				if (player.Distance(Projectile.Center) < 100 && ClearPath(Projectile.Center, player.Center))
				{
					player.Center = Projectile.Center;
					player.velocity = Vector2.Zero;
					player.GetModPlayer<MyPlayer>().AnimeSword = false;
				}
			}

			if (!paused && Projectile.velocity.Length() <= 3 && !pausedBefore)
			{
				paused = true;
				oldVel = Projectile.velocity;
			}

			if (paused)
			{
				pausedBefore = true;
				Projectile.velocity *= 0.99f;
				pauseTimer--;

				if (pauseTimer <= 0)
				{
					Projectile.velocity = oldVel;
					paused = false;
				}
				return false;
			}
			return true;
		}

		private static bool ClearPath(Vector2 point1, Vector2 point2)
		{
			Vector2 distance = point1 - point2;
			for (float i = 0; i < 1; i+= 8.0f / (distance.Length()))
			{
				Vector2 positionToCheck = point2 + (distance * i);
				Point tPos = positionToCheck.ToTileCoordinates();
				if (WorldGen.InWorld(tPos.X, tPos.Y, 2) && Framing.GetTileSafely(tPos.X, tPos.Y).HasTile && Main.tileSolid[Framing.GetTileSafely((int)(positionToCheck.X / 16f), (int)(positionToCheck.Y / 16f)).TileType])
					return false;
			}
			return true;
		}
	}

	public class HeliosDash : ModProjectile
	{
		private Player player => Main.player[Projectile.owner];

		public override void SetStaticDefaults() => DisplayName.SetDefault("Helios");

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 180;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Projectile.Center = player.Center;

			if (!player.GetModPlayer<MyPlayer>().AnimeSword)
				Projectile.active = false;
			else
			{
				for (int i = 0; i < 3; i++)
				{
					ImpactLine line = new ImpactLine(Projectile.Center + Main.rand.NextVector2Circular(90, 90), Vector2.Normalize(player.velocity) * 0.5f, Color.Lerp(Color.Orange, Color.OrangeRed, Main.rand.NextFloat()), new Vector2(0.25f, Main.rand.NextFloat(0.5f, 1.5f)) * 5, 60);
					line.TimeActive = 30;
					ParticleHandler.SpawnParticle(line);
				}
			}
		}
	}
}