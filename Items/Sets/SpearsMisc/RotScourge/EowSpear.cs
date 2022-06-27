using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpearsMisc.RotScourge
{
	public class EoWSpear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rot Scourge");
			Tooltip.SetDefault("Hitting foes may cause them to release multiple tiny, homing eaters");
		}


		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.width = 24;
			Item.height = 24;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useAnimation = 32;
			Item.useTime = 32;
			Item.shootSpeed = 5f;
			Item.knockBack = 3f;
			Item.damage = 20;
			Item.value = Item.sellPrice(0, 1, 15, 0);
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<EoWSpearProj>();
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

	}
}
