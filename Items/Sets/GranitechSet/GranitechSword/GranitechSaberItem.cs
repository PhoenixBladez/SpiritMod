using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechSword
{
	public class GranitechSaberItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Technobrand");
			Tooltip.SetDefault("Swinging creates additional holographic blades");
		}

		public override void SetDefaults()
		{
			item.melee = true;
			item.damage = 60;
			item.Size = new Vector2(88, 92);
			item.useTime = item.useAnimation = 18;
			item.reuseDelay = 20;
			item.knockBack = 5f;
			item.shootSpeed = 1;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.channel = true;
			item.value = Item.sellPrice(0, 1, 32, 0);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.PurificationPowder;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(SpiritMod.Instance.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/SwordSlash1").WithPitchVariance(0.6f).WithVolume(0.8f), player.Center);

			type = ModContent.ProjectileType<GranitechSaberProjectile>();
			GranitechSaberPlayer modplayer = player.GetModPlayer<GranitechSaberPlayer>();

			Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, modplayer.SwingDirection);
			modplayer.SwingDirection *= -1;

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, ModContent.GetTexture(Texture + "_glow"), rotation, scale);
	}

	//Storing info on items is buggy iirc with usage from inventory, so just using a modplayer instead
	internal class GranitechSaberPlayer : ModPlayer
	{
		public int SwingDirection { get; set; } = 1;

		public override void PostUpdate()
		{
			if (player.HeldItem.type != ModContent.ItemType<GranitechSaberItem>()) //Reset when held item changes
				SwingDirection = 1;
		}
	}
}