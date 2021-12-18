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
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 38;
			item.height = 38;
			item.useTime = 5;
			item.useAnimation = 30;
			item.reuseDelay = 50;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<Tome_of_the_Great_Scavenger_Projectile>();
			item.shootSpeed = 7f;
			item.knockBack = 4.4f;
			item.autoReuse = true;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item17;
			item.autoReuse = true;
			item.value = Item.sellPrice(gold: 2);
			item.useTurn = false;
			item.mana = 13;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.itemRotation = 0;
			for(int i = 0; i < 2; i++)
			{
				Vector2 vel = -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 4) * item.shootSpeed * Main.rand.NextFloat(0.7f, 1.3f);
				Projectile.NewProjectile(player.MountedCenter + (Vector2.UnitX * player.direction * item.width/2), vel + player.velocity, type, damage, knockBack, player.whoAmI, Main.rand.NextBool() ? -1 : 1);
			}
			return false;
		}
	}
}
