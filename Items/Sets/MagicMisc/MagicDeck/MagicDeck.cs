using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace SpiritMod.Items.Sets.MagicMisc.MagicDeck
{
	public class MagicDeck : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Magic Deck");

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 9;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 6;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<MagicDeckProj>();
			Item.shootSpeed = 15;
			Item.noUseGraphic = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
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
				switch (Projectile.frame)
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
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.width = Projectile.height = 14;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.frame = Main.rand.Next(4);
		}

		int counter;
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			counter++;
			if (counter > 15)
				Projectile.alpha += 25;
			if (Projectile.alpha > 255)
				Projectile.active = false;

			if (stuck)
			{
				NPC target = Main.npc[enemyID];

				if (!target.active)
				{

				}
				else
					Projectile.position = target.position + offset;
				return;
			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter % 2 == 0)
				xFrame++;
			xFrame %= NUMBEROFXFRAMES;

			var target2 = Main.npc.Where(n => n.active && n.CanBeChasedBy(Projectile) && !n.townNPC && Vector2.Distance(n.Center, Projectile.Center) < 200).OrderBy(n => Vector2.Distance(n.Center, Projectile.Center)).FirstOrDefault();
			if (target2 != default)
			{
				Vector2 direction = target2.Center - Projectile.Center;
				direction.Normalize();
				direction *= 15;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction, 0.05f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.penetrate++;
			if (!stuck && target.life > 0)
			{
				enemyID = target.whoAmI;
				counter = 16;
				stuck = true;
				Projectile.friendly = false;
				Projectile.tileCollide = false;
				offset = Projectile.position - target.position;
				offset -= Projectile.velocity;
				Projectile.timeLeft = 200;
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Texture2D tex2 = ModContent.Request<Texture2D>(Texture + "_White");
			Texture2D tex3 = ModContent.Request<Texture2D>(Texture + "_Glow");
			int frameWidth = tex.Width / NUMBEROFXFRAMES;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(frameWidth * xFrame, frameHeight * Projectile.frame, frameWidth, frameHeight);
			Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (new Vector2(Projectile.width, Projectile.height) / 2);
				Color color = lightColor * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)) * (1 - (Projectile.alpha / 255f));
				Color fadeColor = SuitColor * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)) * (Projectile.alpha / 255f) * (1 - (Projectile.alpha / 255f));
				Color glowColor = Color.White * (float)(((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)) * (1 - (Projectile.alpha / 255f));
				spriteBatch.Draw(tex, drawPos - Main.screenPosition, frame, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex2, drawPos - Main.screenPosition, frame, fadeColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex3, drawPos - Main.screenPosition, frame, glowColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(stuck);
			writer.Write(counter);
			writer.WriteVector2(offset);
			writer.Write(enemyID);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			stuck = reader.ReadBoolean();
			counter = reader.ReadInt32();
			offset = reader.ReadVector2();
			enemyID = reader.ReadInt32();
		}
	}
}