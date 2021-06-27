using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using Terraria.Graphics.Shaders;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.OlympiumSet.ArtemisHunt
{
	public class ArtemisHunt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis's Hunt");
			Tooltip.SetDefault("Converts wooden arrows into Celestial Arrows\nCelestial Arrows create damaging crescents when hitting enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 43;
			item.noMelee = true;
			item.ranged = true;
			item.width = 20;
			item.height = 38;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.Shuriken;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.UseSound = SoundID.Item5;
			item.rare = 4;
			item.value = Item.sellPrice(gold: 2);
			item.autoReuse = true;
			item.shootSpeed = 12f;


		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Lighting.AddLight(new Vector2(item.Center.X, item.Center.Y), 0.075f, 0.255f, 0.193f);
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Sets/OlympiumSet/ArtemisHunt/ArtemisHunt_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White * .6f,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly) 
			{
				type = ModContent.ProjectileType<ArtemisHuntArrow>();
			}
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, 0);
		}
	}
	public class ArtemisHuntArrow : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Arrow");
		    ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.extraUpdates = 1;
			projectile.timeLeft = 600;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			aiType = ProjectileID.WoodenArrowFriendly;
		}
		public override void AI()
		{
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y), 0.075f * .75f, 0.255f * .75f, 0.193f * .75f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int proj = Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<ArtemisCrescent>(), (int)(projectile.damage * 0.75f), 0, projectile.owner);
			float offset = Main.rand.NextFloat(0.5f) * Main.rand.NextBool().ToDirectionInt();
			if (Main.projectile[proj].modProjectile is ArtemisCrescent modProj)
			{
				switch (Main.rand.Next(2))
				{
					case 0:
						modProj.start = projectile.Center - (new Vector2( 60, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c2 = projectile.Center - (new Vector2(-38, -60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c1 = projectile.Center - (new Vector2(-38, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.end = projectile.Center - (new Vector2( 60,-60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.reverse = false;
						break;
					case 1:
						modProj.end = projectile.Center - (new Vector2( 60, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c2 = projectile.Center - (new Vector2(-38, 60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.c1 = projectile.Center - (new Vector2(-38, -60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.start = projectile.Center - (new Vector2( 60,-60).RotatedBy(projectile.velocity.ToRotation() + offset));
						modProj.reverse = true;
						break;
				}
			}
		}
		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(projectile, new GradientTrail(new Color(108, 215, 245), new Color(48, 255, 176)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 50f, new DefaultShader());
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y, 1);
			Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 3);
            Vector2 vector9 = projectile.position;
            Vector2 value19 = (projectile.rotation - 1.57079637f).ToRotationVector2();
            vector9 += value19 * 12f;
            for (int num257 = 0; num257 < 18; num257++)
            {
                int newDust = Dust.NewDust(vector9, projectile.width, projectile.height, 163, 0f, 0f, 0, Color.White, 1f);
                Main.dust[newDust].position = (Main.dust[newDust].position + projectile.Center) / 2f;
                Main.dust[newDust].velocity += value19 * 2f;
                Main.dust[newDust].velocity *= 0.5f;
                Main.dust[newDust].noGravity = true;
				Main.dust[newDust].shader = GameShaders.Armor.GetSecondaryShader(22, Main.LocalPlayer);
                vector9 -= value19 * 8f;
            }
            DustHelper.DrawDustImage(projectile.Center, 89, 0.125f, "SpiritMod/Effects/DustImages/MoonSigil", 1f);
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
        {
			for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Color color = new Color(77, 255, 193) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = projectile.scale;
                Texture2D tex = ModContent.GetTexture("SpiritMod/Items/Weapon/Bow/ArtemisHunt/ArtemisHuntArrow_Glow");

                spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
            }
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
            {
                Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color * .6f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f); ;
                }
            }
			return false;
        }
	}
	public class ArtemisCrescent : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Artemis Crescent");
		}
		public Vector2 start = Vector2.Zero;
		public Vector2 c1 = Vector2.Zero;
		public Vector2 c2 = Vector2.Zero;
		public Vector2 end = Vector2.Zero;
		public bool reverse;
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
			projectile.light = 0;
			projectile.extraUpdates = 7;
			projectile.tileCollide = false;
			projectile.alpha = 255;
		}
		private bool primsCreated;
		public override void AI()
		{
			if (projectile.timeLeft <= 60)
			{
				Lighting.AddLight(projectile.Center, 0.08f, .28f, .38f);
				if (!primsCreated)
				{
					primsCreated = true;
					SpiritMod.primitives.CreateTrail(new ArtemisPrimTrail(projectile, start, c1, c2, end, reverse));
				}
				projectile.Center = Helpers.TraverseBezier(end, start, c1, c2, projectile.timeLeft / 60f);
				projectile.friendly = true;
			}
		}
	}
}