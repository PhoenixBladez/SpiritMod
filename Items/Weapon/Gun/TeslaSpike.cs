using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class TeslaSpike : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Spike");
			Tooltip.SetDefault("'Electriiiiiiiic'");
		}


		public override void SetDefaults()
		{
			item.damage = 65;
			item.magic = true;
			item.mana = 10;
			item.width = 52;
			item.height = 24;
			item.useTime = 19;
			item.useAnimation = 19;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<TeslaSpikeProjectile>();
			item.shootSpeed = 20f;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}
