using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.DuskingDrops
{
	public class ShadowflameSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Sword");
			Tooltip.SetDefault("Causes explosions of Shadowflames to appear when hitting enemies\nShoots out Shadow Embers that damage nearby foes");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/DuskingDrops/ShadowflameSword_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 42;
			Item.rare = ItemRarityID.LightPurple;
			Item.damage = 44;
			Item.knockBack = 6;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 20;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<ShadowPulse1>();
			Item.shootSpeed = 2;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/DuskingDrops/ShadowflameSword_Glow"),
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(5) == 0) {
				int dist = 80;
				Vector2 targetExplosionPos = target.Center;
				for (int i = 0; i < 200; ++i) {
					if (Main.npc[i].active && (Main.npc[i].Center - targetExplosionPos).Length() < dist) {
						Main.npc[i].HitEffect(0, damage);
					}
				}
				for (int i = 0; i < 15; ++i) {
					int newDust = Dust.NewDust(new Vector2(targetExplosionPos.X - (dist / 2), targetExplosionPos.Y - (dist / 2)), dist, dist, DustID.Shadowflame, 0f, 0f, 100, default, 2.5f);
					Main.dust[newDust].noGravity = true;
					Main.dust[newDust].velocity *= 5f;
					newDust = Dust.NewDust(new Vector2(targetExplosionPos.X - (dist / 2), targetExplosionPos.Y - (dist / 2)), dist, dist, DustID.Shadowflame, 0f, 0f, 100, default, 1.5f);
					Main.dust[newDust].velocity *= 3f;
				}
			}
			if (Main.rand.Next(4) == 0) {
				target.AddBuff(BuffID.ShadowFlame, 300, true);
			}
		}
	}
}