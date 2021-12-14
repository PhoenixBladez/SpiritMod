using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using SpiritMod.Dusts;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Sets.MagicMisc.MagicDeck
{
	public class MagicDeck : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Magic Deck");

		public override void SetDefaults()
		{
			item.damage = 45;
			item.magic = true;
			item.mana = 9;
			item.width = 40;
			item.height = 40;
			item.useTime = 6;
			item.useAnimation = 18;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<MagicDeckProj>();
			item.shootSpeed = 15;
			item.noUseGraphic = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 direction = new Vector2(speedX, speedY);
			direction = direction.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
			speedX = direction.X;
			speedY = direction.Y;
			return true;
		}
	}

	public class MagicDeckProj : ModProjectile
	{
		private const int NUMBEROFXFRAMES = 4;

		private int xFrame = 0;

		int enemyID;
		bool stuck = false;
		Vector2 offset = Vector2.Zero;

		public Color SuitColor
		{
			get
			{
				switch (projectile.frame)
				{
					case 0:
						return new Color(93, 13, 184);
					case 1:
						return new Color(204, 10, 20);
					case 2:
						return new Color(93, 13, 184);
					case 3:
						return new Color(204, 10, 20);
					default:
						return Color.White;
				}
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Card");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.width = projectile.height = 14;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.frame = Main.rand.Next(4);
		}

		int counter;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			counter++;
			if (counter > 15)
				projectile.alpha += 25;
			if (projectile.alpha > 255)
				projectile.active = false;

			if (stuck)
			{
				NPC target = Main.npc[enemyID];

				if (!target.active)
				{

				}
				else
					projectile.position = target.position + offset;
				return;
			}

			projectile.frameCounter++;
			if (projectile.frameCounter % 2 == 0)
				xFrame++;
			xFrame %= NUMBEROFXFRAMES;

			var target2 = Main.npc.Where(n => n.active && n.CanBeChasedBy(projectile) && !n.townNPC && Vector2.Distance(n.Center, projectile.Center) < 200).OrderBy(n => Vector2.Distance(n.Center, projectile.Center)).FirstOrDefault();
			if (target2 != default)
			{
				Vector2 direction = target2.Center - projectile.Center;
				direction.Normalize();
				direction *= 15;
				projectile.velocity = Vector2.Lerp(projectile.velocity, direction, 0.05f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate++;
			if (!stuck && target.life > 0)
			{
				enemyID = target.whoAmI;
				counter = 16;
				stuck = true;
				projectile.friendly = false;
				projectile.tileCollide = false;
				offset = projectile.position - target.position;
				offset -= projectile.velocity;
				projectile.timeLeft = 200;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D tex2 = ModContent.GetTexture(Texture + "_White");
			Texture2D tex3 = ModContent.GetTexture(Texture + "_Glow");
			int frameWidth = tex.Width / NUMBEROFXFRAMES;
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(frameWidth * xFrame, frameHeight * projectile.frame, frameWidth, frameHeight);
			Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);
			for (int k = projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = projectile.oldPos[k] + (new Vector2(projectile.width, projectile.height) / 2);
				Color color = lightColor * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)) * (1 - (projectile.alpha / 255f));
				Color fadeColor = SuitColor * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)) * (projectile.alpha / 255f) * (1 - (projectile.alpha / 255f));
				Color glowColor = Color.White * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)) * (1 - (projectile.alpha / 255f));
				spriteBatch.Draw(tex, drawPos - Main.screenPosition, frame, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex2, drawPos - Main.screenPosition, frame, fadeColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex3, drawPos - Main.screenPosition, frame, glowColor, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}