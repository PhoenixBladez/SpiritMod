using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Effects.Stargoop;
using SpiritMod.Items.Material;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.StarLance
{
	public class StarLance : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Star Lance");

		public override void SetDefaults()
		{
			Item.damage = 65;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 9;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<StarLanceProj>();
			Item.shootSpeed = 0.25f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			float angle = Main.rand.NextFloat(6.28f);
			Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * Main.rand.Next(70);
			if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0))
				position += spawnPlace;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, 0, 0.0f, 0.0f);
			for (int i = 0; i < 5; i++)
				Dust.NewDustPerfect(position - (velocity * 200), ModContent.DustType<FriendlyStargoopDust>(), Main.rand.NextFloat(6.28f).ToRotationVector2() * 3, Scale: Main.rand.NextFloat(1.4f, 1.8f));
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Starjinx>(), 12);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.Bookcases);
			recipe.Register();
		}
	}

	public class StarLanceProj : ModProjectile, IGalaxySprite
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Lance");
			Main.projFrames[Projectile.type] = 11;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.width = Projectile.height = 20;
			Projectile.frame = Main.rand.Next(11);
		}

		int enemyID;
		bool initialized = false;
		bool stuck = false;
		Vector2 offset = Vector2.Zero;
		float shrinkCounter = 0.25f;

		public override void AI()
		{
			Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.054F * 3, 0.027F * 3, 0.135F * 3);

			if (!initialized)
			{
				if (Projectile.frame == 10 && Main.rand.Next(5) != 1)
					Projectile.frame = Main.rand.Next(10);

				initialized = true;
				SpiritMod.Metaballs.FriendlyLayer.Sprites.Add(this);
			}

			if (stuck)
			{
				NPC target = Main.npc[enemyID];

				if (!target.active)
				{
					if (Projectile.timeLeft > 40)
						Projectile.timeLeft = 40;
					Projectile.velocity = Vector2.Zero;
				}
				else
					Projectile.position = target.position + offset;

				if (Projectile.timeLeft < 40)
				{
					shrinkCounter += 0.1f;
					Projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (Projectile.scale < 0.3f)
					{
						Projectile.active = false;

						for (int i = 0; i < 6; i++)
							Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<FriendlyStargoopDust>(), Main.rand.NextFloat(6.28f).ToRotationVector2() * 3, Scale: Main.rand.NextFloat(1.4f, 1.8f));

						SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 105, 0.5f, 0.5f);
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.SmallExplosion>(), (int)(Projectile.damage * Main.rand.NextFloat(0.85f, 1.15f)), 0, Projectile.owner);
					}

					if (Projectile.scale > 1)
						Projectile.scale = ((Projectile.scale - 1) / 2f) + 1;
				}
			}
			else
			{
				if (Projectile.velocity.Length() < 40)
					Projectile.velocity *= 1.07f;
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.penetrate++;
			if (!stuck && target.life > 0)
			{
				stuck = true;
				Projectile.friendly = false;
				Projectile.tileCollide = false;
				enemyID = target.whoAmI;
				offset = Projectile.position - target.position;
				offset -= Projectile.velocity;
				Projectile.timeLeft = 200;
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
			}
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCHit, (int)Projectile.position.X, (int)Projectile.position.Y, 3);
			Vector2 vector9 = Projectile.position;
			Vector2 value19 = Projectile.rotation.ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, Projectile.width, Projectile.height, ModContent.DustType<FriendlyStargoopDust>(), 0f, 0f, 0, default, 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + Projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			SpiritMod.Metaballs.FriendlyLayer.Sprites.Remove(this);
		}

		public override bool PreDraw(ref Color lightColor) => false;

		public void DrawGalaxyMappedSprite(SpriteBatch sB)
		{
			if (Projectile.type == ModContent.ProjectileType<StarLanceProj>() && Projectile.active)
			{
				Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
				int frameHeight = (tex.Height / 11);
				sB.Draw(tex, (Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY)) / 2, new Rectangle(0, Projectile.frame * frameHeight, tex.Width, frameHeight), Color.White, Projectile.rotation, new Vector2(tex.Width - (Projectile.width / 2), (tex.Height / 2) / 11), Projectile.scale * 0.5f, SpriteEffects.None, 0);
			}
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(stuck);
			writer.WriteVector2(offset);
			writer.Write(enemyID);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			stuck = reader.ReadBoolean();
			offset = reader.ReadVector2();
			enemyID = reader.ReadInt32();
		}
	}
}