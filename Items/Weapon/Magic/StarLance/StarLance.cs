using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Effects.Stargoop;
using SpiritMod.Dusts;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Weapon.Magic.StarLance
{
	public class StarLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Lance");
		}

		public override void SetDefaults()
		{
			item.damage = 65;
			item.magic = true;
			item.mana = 18;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<StarLanceProj>();
			item.shootSpeed = 0.25f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
				float angle = Main.rand.NextFloat(6.28f);
				Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * Main.rand.Next(70);
				if (Collision.CanHit(position, 0, 0, position + spawnPlace, 0, 0)) {
					position += spawnPlace;
				}

				Vector2 velocity = Vector2.Normalize(Main.MouseWorld - position) * item.shootSpeed;
				int p = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockBack, 0, 0.0f, 0.0f);
				for (int i = 0; i < 5; i++)
					Dust.NewDustPerfect(new Vector2(position.X,position.Y) - (velocity * 200), ModContent.DustType<FriendlyStargoopDust>(), Main.rand.NextFloat(6.28f).ToRotationVector2() * 3, Scale: Main.rand.NextFloat(1.4f, 1.8f));
			return false;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Material.Starjinx>(), 12);
            recipe.AddIngredient(ItemID.SpellTome, 1);
            recipe.AddTile(TileID.Bookcases);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
	public class StarLanceProj : ModProjectile, IGalaxySprite
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Lance");
			Main.projFrames[projectile.type] = 11;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.friendly = true;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 20;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.frame = Main.rand.Next(11);
		}
		int enemyID;
		bool initialized = false;
		bool stuck = false;
		Vector2 offset = Vector2.Zero;
		float shrinkCounter = 0.25f;
		public override void AI()
		{
			if (!initialized)
			{
				if (projectile.frame == 10 && Main.rand.Next(5) != 1)
					projectile.frame = Main.rand.Next(10);
				initialized = true;
				SpiritMod.Metaballs.FriendlyLayer.Sprites.Add(this);
			}
			if (stuck)
            {
				NPC target = Main.npc[enemyID];
				if (!target.active)
                {
					if (projectile.timeLeft > 40)
						projectile.timeLeft = 40;
					projectile.velocity = Vector2.Zero;
                }
				else
				{
					projectile.position = target.position + offset;
				}
				if (projectile.timeLeft < 40)
				{
					shrinkCounter+= 0.1f;
					projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (projectile.scale < 0.3f)
					{
						projectile.active = false;
						for (int i = 0; i < 6; i++)
						{
							Dust.NewDustPerfect(projectile.Center, ModContent.DustType<FriendlyStargoopDust>(), Main.rand.NextFloat(6.28f).ToRotationVector2() * 3, Scale: Main.rand.NextFloat(1.4f, 1.8f));
						}

                        Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 105, 0.5f, 0.5f);

                        target.StrikeNPC((int)(projectile.damage * Main.rand.NextFloat(0.85f,1.15f)), 0f, 0);
					}
					if (projectile.scale > 1)
						projectile.scale = ((projectile.scale - 1) / 2f) + 1;
				}
            }
			else
            {
				if (projectile.velocity.Length() < 40)
					projectile.velocity *= 1.07f;
				projectile.rotation = projectile.velocity.ToRotation();
            }
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate++;
			if (!stuck && target.life > 0)
			{
				stuck = true;
				projectile.friendly = false;
				projectile.tileCollide = false;
				enemyID = target.whoAmI;
				offset = projectile.position - target.position;
				offset -= projectile.velocity;
				//projectile.aiStyle = 2;
                projectile.timeLeft = 200;
			}
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 3);
			Vector2 vector9 = projectile.position;
			Vector2 value19 = projectile.rotation.ToRotationVector2();
			vector9 += value19 * 16f;
			for (int num257 = 0; num257 < 20; num257++)
			{
				int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, ModContent.DustType<FriendlyStargoopDust>(), 0f, 0f, 0, default(Color), 1f);
				Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
				Main.dust[newDust].velocity += value19 * 2f;
				Main.dust[newDust].velocity *= 0.5f;
				Main.dust[newDust].noGravity = true;
				vector9 -= value19 * 8f;
			}
			SpiritMod.Metaballs.FriendlyLayer.Sprites.Remove(this);
		}
		public override bool PreDraw(SpriteBatch sb, Color color) => false;
		public void DrawGalaxyMappedSprite(SpriteBatch sB)
		{
			if (projectile.type == ModContent.ProjectileType<StarLanceProj>() && projectile.active)
			{
				Texture2D tex = Main.projectileTexture[projectile.type];
				int frameHeight = (tex.Height / 11);
				sB.Draw(tex, (projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY)) / 2, new Rectangle(0,projectile.frame * frameHeight,tex.Width, frameHeight), Color.White, projectile.rotation, new Vector2(tex.Width - (projectile.width / 2), (tex.Height / 2) / 11), projectile.scale * 0.5f, SpriteEffects.None, 0);
			}
		}
	}
}
