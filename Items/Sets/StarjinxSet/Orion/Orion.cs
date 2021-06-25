using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using Terraria.Graphics.Shaders;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.Orion
{
	public class Orion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sagittarius");
			Tooltip.SetDefault("Creates a constellation behind you as you shoot\nConstellation stars will fire additional astral arrows towards the cursor");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			item.damage = 100;
			item.noMelee = true;
			item.ranged = true;
			item.width = 40;
			item.height = 78;
			item.useTime = 70;
			item.useAnimation = 70;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.knockBack = 3;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast");
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(gold: 2);
			item.autoReuse = true;
			item.shootSpeed = 22f;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = ModContent.ProjectileType<OrionArrow>();
			Vector2 shootDir = Vector2.Normalize(new Vector2(speedX, speedY));
			Projectile.NewProjectileDirect(player.MountedCenter - shootDir.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(70, 150), 
				Vector2.Zero, ModContent.ProjectileType<OrionConstellation>(), (int)(damage * 0.75f), knockBack, player.whoAmI, 4, -1).netUpdate = true;
			return true;
		}
	}
}