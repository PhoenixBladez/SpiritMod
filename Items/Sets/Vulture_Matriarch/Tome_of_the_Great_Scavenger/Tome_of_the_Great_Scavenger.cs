using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch.Tome_of_the_Great_Scavenger
{
	public class Tome_of_the_Great_Scavenger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Great Scavenger");
			Tooltip.SetDefault("Casts a torrent of sharp, armor penetrating feathers\nHit enemies may drop more gold");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.noMelee = true;
			Item.noUseGraphic = false;
			Item.DamageType = DamageClass.Magic;
			Item.width = 38;
			Item.height = 38;
			Item.useTime = 5;
			Item.useAnimation = 30;
			Item.reuseDelay = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Tome_of_the_Great_Scavenger_Projectile>();
			Item.shootSpeed = 7f;
			Item.knockBack = 4.4f;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item17;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(gold: 2);
			Item.useTurn = false;
			Item.mana = 13;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow"), rotation, scale);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.itemRotation = 0;
			for(int i = 0; i < 2; i++)
			{
				Vector2 vel = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 4) * Item.shootSpeed * Main.rand.NextFloat(0.7f, 1.3f);
				Projectile.NewProjectile(player.MountedCenter + (Vector2.UnitX * player.direction * Item.width/2), vel + player.velocity, type, damage, knockback, player.whoAmI, Main.rand.NextBool() ? -1 : 1);
			}
			return false;
		}
	}
}
